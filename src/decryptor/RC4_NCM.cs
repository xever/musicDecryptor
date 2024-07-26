/// <summary>
/// 解密算法，
/// 从源码转写过来 https://github.com/kingsznhone/NCMDump.NET 
/// 适配低版本的.net写法
/// </summary>
namespace Tomusic
{
    public class RC4_NCM
    {
        private readonly byte[] Keybox;
        private int i = 0, j = 0;

        public RC4_NCM(byte[] key)
        {
//            Keybox = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
           Keybox = new byte[256];

            int len = 256;
            for(int p=0; p<len ;p++){
            	Keybox[p] = (byte)p;
            }
            
            
            //Generate Keybox
            for (int x = 0, y = 0; x < 256; x++)
            {
                y = (y + Keybox[x] + key[x % key.Length]) & 0xFF;
                byte temp = Keybox[x];
                Keybox[x] = Keybox[y];
                Keybox[y] = temp;
                // 传统方法交换值
                //(Keybox[x], Keybox[y]) = (Keybox[y], Keybox[x]);
            }
        }

        public byte[] Encrypt(byte[] data)
        {
        	for (int m = 0; m < data.Length; m++)
            {
                i = (i + 1) & 0xFF;
                j = (i + Keybox[i]) & 0xFF;
                data[m] ^= Keybox[(Keybox[i] + Keybox[j]) & 0xFF];
            }
        	return data;
        }
        	
//        public byte[] Encrypt(byte[] data)
//        {
//            Span<byte> span = new Span(data);
//            Encrypt(span);
//            return span.ToArray();
//        }

//        public int Encrypt(ref Span<byte> data)
//        {
//            for (int m = 0; m < data.Length; m++)
//            {
//                i = (i + 1) & 0xFF;
//                j = (i + Keybox[i]) & 0xFF;
//                data[m] ^= Keybox[(Keybox[i] + Keybox[j]) & 0xFF];
//            }
//            return data.Length;
//        }

//        public int Encrypt(Memory<byte> data)
//        {
//            for (int m = 0; m < data.Length; m++)
//            {
//                i = (i + 1) & 0xFF;
//                j = (i + Keybox[i]) & 0xFF;
//                data.Span[m] ^= Keybox[(Keybox[i] + Keybox[j]) & 0xFF];
//            }
//            return data.Length;
//        }

//        public byte[] Decrypt(byte[] data)
//        {
//            return Encrypt(data);
//        }
//
//        public int Decrypt(Span<byte> data)
//        {
//            return Encrypt(ref data);
//        }
    }
}