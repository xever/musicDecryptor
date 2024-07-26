using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tomusic
{
	public class RC4_NCM_Stream : Stream
	{
		private readonly Stream innerStream;
		private readonly RC4_NCM rc4;

		public RC4_NCM_Stream(Stream innerStream, byte[] key)
		{
			this.innerStream = innerStream;
			rc4 = new RC4_NCM(key);
		}

		public override bool CanRead {
			get { return innerStream.CanRead; }
		}

		public override bool CanSeek {
			get { return innerStream.CanSeek; }
		}

		public override bool CanWrite {
			get { return innerStream.CanWrite; }
		}

		public override long Length {
			get { return innerStream.Length; }
		}
		public override long Position {
			get {
				return innerStream.Position;
			}
			set {
				innerStream.Position = value;
			}
		}
        
		public override void Flush()
		{
			innerStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			byte[] tempBuffer = new byte[count];
			innerStream.Seek(offset, SeekOrigin.Current);
			int bytesRead = innerStream.Read(tempBuffer, 0, count);
//            rc4.Encrypt(tempBuffer, 0, bytesRead);
			rc4.Encrypt(tempBuffer);
			Array.Copy(tempBuffer, 0, buffer, offset, bytesRead);
			return bytesRead;
		}

		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			innerStream.Seek(offset, SeekOrigin.Current);
			byte[] tempBuffer = new byte[count];
			int bytesRead = await innerStream.ReadAsync(tempBuffer, 0, count, cancellationToken);
//            rc4.Encrypt(tempBuffer, 0, bytesRead);
			rc4.Encrypt(tempBuffer);
			Array.Copy(tempBuffer, 0, buffer, offset, bytesRead);
			return bytesRead;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return innerStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			innerStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			byte[] tempBuffer = new byte[count];
			Array.Copy(buffer, offset, tempBuffer, 0, count);
			rc4.Encrypt(tempBuffer);
			innerStream.Write(tempBuffer, 0, count);
		}

		public override async Task WriteAsync(byte[] data, int offset, int count, CancellationToken cancellationToken)
		{
			byte[] tempBuffer = new byte[count];
			Array.Copy(data, offset, tempBuffer, 0, count);
			rc4.Encrypt(tempBuffer);
			await innerStream.WriteAsync(tempBuffer, 0, count, cancellationToken);
		}
	}

}