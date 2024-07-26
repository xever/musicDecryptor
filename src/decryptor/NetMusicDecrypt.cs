using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATL.AudioData;
using ATL;
using System.IO;
using log4net.Core;
using log4net;

namespace Tomusic
{
    /// <summary>
    /// 网易云 下载的歌曲ncm文件解密
    /// </summary>
    public class NetMusicDecrypt : BaseCacheDecrypt
    {
    	
        private ILog _logger = LogManager.GetLogger(typeof(NetMusicDecrypt));
    	
        
			// 创建 NeteaseCrypt 类的实例
       readonly NCMDumper _neteaseCrypt = new NCMDumper();

            
        public override string AcceptableExtension
        {
            get
            {
                return ".ncm";
            }
        }

        
		public override byte[] Decrypt(byte[] cacheFileData)
		{
			_logger.Info("unsupport");
			
			return null;
		}


		/// <summary>
		/// 实际的解密工作
		/// </summary>
		/// <param name="cacheFile"></param>
		/// <returns></returns>
		///
		
		public override byte[] Decrypt(string cacheFile)
        {
			
//			_logger.Info("即将处理--"+cacheFile);
			
            _neteaseCrypt.Convert(cacheFile);
            
            // lib 内部写入了mp3 再返回给主程序
            string mpPath = cacheFile.Replace(".ncm",".mp3");
			
//			_logger.Info("处理完成--"+mpPath);
			
            return File.ReadAllBytes(mpPath);
        }


    }
}
