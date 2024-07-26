using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomusic
{
    public abstract class BaseCacheDecrypt : ICacheDecrypt
    {

        protected string currentCacheFile;

        public abstract string AcceptableExtension
        {
            get;
        }

        public abstract byte[] Decrypt(byte[] cacheFileData);

        public virtual byte[] Decrypt(string cacheFile)
        {
            currentCacheFile = cacheFile;
            return Decrypt(File.ReadAllBytes(cacheFile));
        }

        public void Decrypt(string cacheFile, string decodedFile)
        {
        	byte[] audio = Decrypt(cacheFile);
        	if(audio.Length > 10){
            	File.WriteAllBytes(decodedFile, audio);
        	}
        }

        public  bool isAcceptable(string cacheFile)
        {
            return cacheFile.EndsWith(AcceptableExtension);
        }
    }
}
