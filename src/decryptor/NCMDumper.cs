using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using TagLib;

namespace Tomusic
{
    public class NCMDumper
    {
        private readonly int vectorSize = 32; // Vector256<byte>.Count;
        private readonly byte[] coreKey = { 0x68, 0x7A, 0x48, 0x52, 0x41, 0x6D, 0x73, 0x6F, 0x35, 0x6B, 0x49, 0x6E, 0x62, 0x61, 0x78, 0x57 };
        private readonly byte[] metaKey = { 0x23, 0x31, 0x34, 0x6C, 0x6A, 0x6B, 0x5F, 0x21, 0x5C, 0x5D, 0x26, 0x30, 0x55, 0x3C, 0x27, 0x28 };

        private bool VerifyHeader(ref MemoryStream ms)
        {
            // Header Should be "CTENFDAM"
            byte[] header = new byte[8];
            ms.Read(header, 0, 8);
            long header_num = BitConverter.ToInt64(header, 0);
            return header_num == 0x4d4144464e455443;
        }

        private byte[] ReadRC4Key(ref MemoryStream ms)
        {
            // read keybox length
            uint KeyboxLength = ReadUint32(ref ms);

            // read raw keybox data
            byte[] buffer = new byte[KeyboxLength];
            ms.Read(buffer, 0, buffer.Length);

            // SIMD XOR 0x64
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= 0x64;
            }

            // decrypt keybox data
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = coreKey;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] cleanText = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    return cleanText.Skip(17).ToArray();
                }
            }
        }

        private MetaInfo ReadMeta(ref MemoryStream ms)
        {
            // read meta length
            uint MetaLength = ReadUint32(ref ms);
            byte[] buffer = new byte[MetaLength];
            ms.Read(buffer, 0, buffer.Length);

            // SIMD XOR 0x63
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] ^= 0x63;
            }

            buffer = System.Convert.FromBase64String(Encoding.ASCII.GetString(buffer.Skip(22).ToArray()));

            // decrypt meta data which is a json contains info of the song
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = metaKey;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] cleanText = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    string MetaJsonString = Encoding.UTF8.GetString(cleanText, 6, cleanText.Length - 6);
                    MetaInfo metainfo = JsonConvert.DeserializeObject<MetaInfo>(MetaJsonString);
                    return metainfo;
                }
            }
        }

        private async Task<byte[]> ReadAudioData(MemoryStream ms, byte[] Key)
        {
            using (RC4_NCM_Stream rc4s = new RC4_NCM_Stream(ms, Key))
            {
                byte[] data = new byte[ms.Length - ms.Position];
                await rc4s.ReadAsync(data, 0, data.Length);
                return data;
            }
        }

        private void AddTag(string fileName, byte[] ImgData, MetaInfo metainfo)
        {
            TagLib.File tagfile = TagLib.File.Create(fileName);

            // Use Embedded Picture
            if (ImgData != null)
            {
            	
                TagLib.Picture PicEmbedded = new Picture(new ByteVector(ImgData));
                tagfile.Tag.Pictures = new Picture[] { PicEmbedded };
            }
            // Use Internet Picture
            else if (!string.IsNullOrEmpty(metainfo.albumPic))
            {
                byte[] NetImgData = FetchUrl(new Uri(metainfo.albumPic));
                if (NetImgData != null)
                {
                    var PicFromNet = new Picture(new ByteVector(NetImgData));
                    tagfile.Tag.Pictures = new Picture[] { PicFromNet };
                }
            }

            // Add more information
            tagfile.Tag.Title = metainfo.musicName;
//            tagfile.Tag.Performers = metainfo.artist.Select(x => x[0]).ToArray();
            tagfile.Tag.Album = metainfo.album;
            tagfile.Tag.Subtitle = string.Join(";", metainfo.alias);
            tagfile.Save();
        }

        private byte[] FetchUrl(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.GetAsync(uri).Result;
                    Console.WriteLine(response.StatusCode);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var memStream = new MemoryStream())
                        {
                            response.Content.ReadAsStreamAsync().Result.CopyTo(memStream);
                            memStream.Position = 0;
                            Console.WriteLine("album picture Load OK : remote returned {0}", response.StatusCode);
                            return memStream.ToArray();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to download album picture: remote returned {0}", response.StatusCode);
                        return null;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return null;
                }
            }
        }

        private uint ReadUint32(ref MemoryStream ms)
        {
            byte[] buffer = new byte[4];
            ms.Read(buffer, 0, 4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public bool Convert(string path)
        {
            return Task.Run(() => ConvertAsync(path)).Result;
        }

        public async Task<bool> ConvertAsync(string path)
        {
            if (!System.IO.File.Exists(path))
            {
//                Console.WriteLine($"File {path} Not Exist!");
                return false;
            }

            // Read all bytes to ram.
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(fileBytes);

            // Verify Header
            if (!VerifyHeader(ref ms))
            {
//                Console.WriteLine($"{path} is not a NCM File");
                return false;
            }

            // skip 2 bytes
            ms.Seek(2, SeekOrigin.Current);

            // Make Keybox
            byte[] RC4Key = ReadRC4Key(ref ms);

            // Read Meta Info
            MetaInfo metainfo = ReadMeta(ref ms);

            // CRC32 Check
            uint crc32 = ReadUint32(ref ms);

            // skip 5 character,
            ms.Seek(5, SeekOrigin.Current);

            // read image length
            uint ImageLength = ReadUint32(ref ms);
            byte[] ImageData;
            if (ImageLength != 0)
            {
                // read image data
                ImageData = new byte[ImageLength];
                ms.Read(ImageData, 0, ImageData.Length);
            }
            else
            {
                ImageData = null;
            }

            // Read Audio Data
            byte[] AudioData = await ReadAudioData(ms, RC4Key);

            // Flush Audio Data to disk drive
            string OutputPath = path.Substring(0, path.LastIndexOf('.'));

            string format = metainfo.format;
            if (string.IsNullOrEmpty(format)) format = "mp3";
            System.IO.File.WriteAllBytes(OutputPath+"."+format, AudioData);

            // Add tag and cover
            AddTag(OutputPath+"."+format, ImageData, metainfo);
            ms.Dispose();
            return true;
        }
    }

}