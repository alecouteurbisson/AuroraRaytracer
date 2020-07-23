using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Aurora
{
  public partial class WinForm : Form
  {
    private IContainer components = null;

    private PictureBox picture;
    private Button buttAbort;

    private Bitmap bm;

    private bool abort;

    #region Constructor

    public WinForm()
    {
      InitializeComponent();

    }

    #endregion

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
            this.picture = new System.Windows.Forms.PictureBox();
            this.buttAbort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.buttScript = new System.Windows.Forms.Button();
            this.txtScriptPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picture.Location = new System.Drawing.Point(0, 54);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(721, 639);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.Paint += new System.Windows.Forms.PaintEventHandler(this.picture_Paint);
            // 
            // buttAbort
            // 
            this.buttAbort.Location = new System.Drawing.Point(440, 15);
            this.buttAbort.Name = "buttAbort";
            this.buttAbort.Size = new System.Drawing.Size(52, 24);
            this.buttAbort.TabIndex = 7;
            this.buttAbort.Text = "Abort";
            this.buttAbort.Click += new System.EventHandler(this.butAbort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(560, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 11);
            this.label1.TabIndex = 11;
            // 
            // buttSave
            // 
            this.buttSave.Enabled = false;
            this.buttSave.Location = new System.Drawing.Point(498, 15);
            this.buttSave.Name = "buttSave";
            this.buttSave.Size = new System.Drawing.Size(52, 24);
            this.buttSave.TabIndex = 12;
            this.buttSave.Text = "Save";
            this.buttSave.Click += new System.EventHandler(this.SaveImage);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "jpg";
            this.saveFileDialog1.Filter = "Jpeg Image|*.jpg";
            this.saveFileDialog1.Title = "Save Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(560, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 11);
            this.label2.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(560, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 11);
            this.label3.TabIndex = 14;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(0, 0);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 0;
            // 
            // buttScript
            // 
            this.buttScript.Location = new System.Drawing.Point(12, 14);
            this.buttScript.Name = "buttScript";
            this.buttScript.Size = new System.Drawing.Size(52, 24);
            this.buttScript.TabIndex = 18;
            this.buttScript.Text = "Script";
            this.buttScript.Click += new System.EventHandler(this.OpenScript);
            // 
            // txtScriptPath
            // 
            this.txtScriptPath.Location = new System.Drawing.Point(70, 17);
            this.txtScriptPath.Name = "txtScriptPath";
            this.txtScriptPath.Size = new System.Drawing.Size(306, 20);
            this.txtScriptPath.TabIndex = 16;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Scene File|*.js";
            // 
            // buttStart
            // 
            this.buttStart.Enabled = false;
            this.buttStart.Location = new System.Drawing.Point(382, 15);
            this.buttStart.Name = "buttStart";
            this.buttStart.Size = new System.Drawing.Size(52, 24);
            this.buttStart.TabIndex = 17;
            this.buttStart.Text = "Start";
            this.buttStart.Click += new System.EventHandler(this.buttStart_Click);
            // 
            // WinForm
            // 
            this.ClientSize = new System.Drawing.Size(721, 693);
            this.Controls.Add(this.buttStart);
            this.Controls.Add(this.txtScriptPath);
            this.Controls.Add(this.buttScript);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttAbort);
            this.Controls.Add(this.picture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WinForm";
            this.Text = "Aurora Raytracer V2       AJB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    #endregion

    private Label label1;
    private Button buttSave;
    private SaveFileDialog saveFileDialog1;
    private Label label2;
    private Label label3;
    private Button button10;
        private Button buttScript;
        private TextBox txtScriptPath;
        private OpenFileDialog openFileDialog1;
        private Button buttStart;
    }


}