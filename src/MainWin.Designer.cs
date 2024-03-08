namespace Tomusic
{
    partial class MainWin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        	this.inputPath = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        	this.pathTip = new System.Windows.Forms.Label();
        	this.btnSelectPath = new System.Windows.Forms.Button();
        	this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        	this.cachePanel = new System.Windows.Forms.Panel();
        	this.logConsole = new System.Windows.Forms.RichTextBox();
        	this.cachePanel.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// inputPath
        	// 
        	this.inputPath.AllowDrop = true;
        	this.inputPath.Location = new System.Drawing.Point(97, 15);
        	this.inputPath.Margin = new System.Windows.Forms.Padding(4);
        	this.inputPath.Name = "inputPath";
        	this.inputPath.ReadOnly = true;
        	this.inputPath.Size = new System.Drawing.Size(400, 25);
        	this.inputPath.TabIndex = 0;
        	this.inputPath.Text = "./已转换";
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        	this.label1.Location = new System.Drawing.Point(144, 45);
        	this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(275, 19);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "将缓存文件夹或者缓存文件拖入";
        	// 
        	// linkLabel1
        	// 
        	this.linkLabel1.AutoSize = true;
        	this.linkLabel1.Location = new System.Drawing.Point(43, 132);
        	this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        	this.linkLabel1.Name = "linkLabel1";
        	this.linkLabel1.Size = new System.Drawing.Size(0, 15);
        	this.linkLabel1.TabIndex = 2;
        	// 
        	// pathTip
        	// 
        	this.pathTip.AutoSize = true;
        	this.pathTip.Location = new System.Drawing.Point(20, 19);
        	this.pathTip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        	this.pathTip.Name = "pathTip";
        	this.pathTip.Size = new System.Drawing.Size(67, 15);
        	this.pathTip.TabIndex = 3;
        	this.pathTip.Text = "输出目录";
        	this.pathTip.Click += new System.EventHandler(this.label2_Click);
        	// 
        	// btnSelectPath
        	// 
        	this.btnSelectPath.Location = new System.Drawing.Point(507, 14);
        	this.btnSelectPath.Margin = new System.Windows.Forms.Padding(4);
        	this.btnSelectPath.Name = "btnSelectPath";
        	this.btnSelectPath.Size = new System.Drawing.Size(100, 29);
        	this.btnSelectPath.TabIndex = 4;
        	this.btnSelectPath.Text = "选择";
        	this.btnSelectPath.UseVisualStyleBackColor = true;
        	this.btnSelectPath.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// cachePanel
        	// 
        	this.cachePanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        	this.cachePanel.Controls.Add(this.label1);
        	this.cachePanel.Location = new System.Drawing.Point(23, 49);
        	this.cachePanel.Margin = new System.Windows.Forms.Padding(4);
        	this.cachePanel.Name = "cachePanel";
        	this.cachePanel.Size = new System.Drawing.Size(584, 98);
        	this.cachePanel.TabIndex = 5;
        	// 
        	// logConsole
        	// 
        	this.logConsole.Location = new System.Drawing.Point(23, 155);
        	this.logConsole.Margin = new System.Windows.Forms.Padding(4);
        	this.logConsole.Name = "logConsole";
        	this.logConsole.ReadOnly = true;
        	this.logConsole.Size = new System.Drawing.Size(583, 210);
        	this.logConsole.TabIndex = 6;
        	this.logConsole.Text = "";
        	// 
        	// MainWin
        	// 
        	this.AllowDrop = true;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoSize = true;
        	this.ClientSize = new System.Drawing.Size(621, 369);
        	this.Controls.Add(this.logConsole);
        	this.Controls.Add(this.cachePanel);
        	this.Controls.Add(this.btnSelectPath);
        	this.Controls.Add(this.pathTip);
        	this.Controls.Add(this.linkLabel1);
        	this.Controls.Add(this.inputPath);
        	this.Margin = new System.Windows.Forms.Padding(4);
        	this.MaximizeBox = false;
        	this.Name = "MainWin";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "酷狗&网易音乐缓存文件转MP3";
        	this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
        	this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
        	this.cachePanel.ResumeLayout(false);
        	this.cachePanel.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label pathTip;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel cachePanel;
        private System.Windows.Forms.RichTextBox logConsole;
    }
}

