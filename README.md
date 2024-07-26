# musicDecryptor
酷狗、网易等音乐缓存文件转mp3

以前写过[kgtemp文件转mp3工具](http://www.cnblogs.com/xiaoqi/p/8085563.html),正好当前又有网易云音乐缓存文件需求，因此就在原来小工具的基础上做了一点修改，增加了对网易云音乐的支持，并简单调整了下代码结构，方便后续增加其他音乐软件的支持。

```
基于原作者代码开发，
增加了自动生成已转换文件夹，增加了应用图标
增加了对网易ncm文件的支持
```

## 再次开发指南
之前都是写java的，用了个简易IDE进行C#开发，Visual Studio太庞大，可用 SharpDevelop进行本作品再开发
[SharpDevelop下载地址](https://sourceforge.net/projects/sharpdevelop/files/SharpDevelop%205.x/5.1/)

## 工具使用介绍

启动程序，
- 可直接将酷狗或者网易的缓存文件 or 整个文件夹，拖入到程序界面即可
- 默认在程序文件夹中新加一个'已转换'的文件夹，可选一个希望的文件夹。

打开转码结果目录，可以看到转码后的结果


## FAQ

### 网易云音乐的缓存目录
打开设置 -- 下载设置 - 缓存目录就是了


### 酷狗缓存目录
在设置--下载设置里

## 工具代码简要说明

### ICacheDecrypt
我们定义一个解码接口ICacheDecrypt，实现将缓存文件字节流转换为mp3字节流。


### BaseCacheDecrypt

然后，实现一个默认的抽象类BaseCacheDecrypt，实现一些公共的东西，具体的转码工作让子类去实现：


### NetMusicCacheDecrypt
然后，分别实现酷狗和网易云音乐的解码工作，酷狗的上次已经写了如何解码，这里只贴网易的，解码很简单，异或0xa3就可以了。网易音乐在测试时发现好多mp3没有ID3信息，经过观察发现缓存文件名里包含歌曲的id信息，因此可以根据这个id信息去抓取歌曲网页，解析出歌手和歌曲名称，然后写入到ID3里，这里ID3的读写采用了GitHub上的一个开源库


接着介绍核心的Decryptor，实现转码的调度，这里的思路就是将所有的解码器放到一个list里，当一个文件过来的时候，遍历所有解码器，如果accetbale，就处理，否则跳过。
两个主要工作：
-   加载所有的BaseCacheDecrypt
-   进行解码工作

### 加载所有的BaseCacheDecrypt

两种方法，一是自己实例化，一是使用反射，这里当然用反射了：）


Decryptor通过单例模式对外提供调用。

### 进行解码

判断拖入的是文件夹还是文件，文件夹的话遍历子文件，依次处理。解码方式就是钢说的，遍历decryptors，如果支持就解码。
解码完后，读取ID3信息，对文件进行重命名。

##合并dll到exe
借助ILMerge工具,把dll 和exe文件合并到一个大exe中
```
ILMerge.exe Tomusic.exe /out:Tomusic-one.exe ATL.dll log4net.dll netstandard.dll  Newtonsoft.Json.dll TagLibSharp.dll 
```

## changelog
#### 2024-07, v 1.1.1.0
-增加对网易ncm文件的支持，缝合了 [NCMDump.NET](https://github.com/kingsznhone/NCMDump.NET)

#### 2024-03，v 1.0.1.0
-可自动生成目标文件夹，默认在程序路径 ```/已转换```