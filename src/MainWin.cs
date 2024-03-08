using log4net.Appender;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net.Core;
using log4net;

namespace Tomusic
{
    public partial class MainWin : Form, IAppender
    {
        private  ILog _logger = LogManager.GetLogger(typeof(MainWin));
        private string extension;
        private string path;
        private string outPath = "./已转换";
        private string currentMp3 = "";
        public MainWin()
        {
            InitializeComponent();
            this.linkLabel1.Click += LinkLabel1_Click;
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            Process.Start(this.currentMp3);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;//设置拖动操作
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            outPath = inputPath.Text;//输入保存路径给paths
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            path = (((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());//获取拖动的文件路径
            _logger.Debug(path);
            
            this.outPath = preparePath(this.outPath);

            new Thread(() =>
            {
                Decryptor.Instance.AutoRename = true;
                Decryptor.Instance.TargetDirectory = this.outPath;
                int success = Decryptor.Instance.Process(path);
                _logger.Debug("成功转换" + success + "个文件");
            }).Start();
           
        }
        
        private string preparePath(string path){
        	if(path !=null && path.StartsWith("./")){
        		path = System.Environment.CurrentDirectory+"\\"+path.Remove(0,2);
        		if (!Directory.Exists(path)) {
				    // 文件夹不存在时执行的逻辑，创建文件夹
				    DirectoryInfo directoryInfo = new DirectoryInfo(path);
				    directoryInfo.Create();
				}     		
			}
        	return path;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.inputPath.Text = folderBrowserDialog1.SelectedPath;
                outPath = inputPath.Text;
            }
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            logConsole.BeginInvoke((Action)(() =>
            {
                logConsole.AppendText(loggingEvent.MessageObject.ToString() + Environment.NewLine);
            }));
           
        }
    }
}

