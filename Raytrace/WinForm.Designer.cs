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
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
    private Button button7;
    private Button button8;
    private CheckBox checkBox1;
    private Button butAbort;

    private Scene scene;
    private Bitmap bm;

    private bool abort;

    #region Constructor

    public WinForm()
    {
      InitializeComponent();

      var tt = new ToolTip();

      tt.SetToolTip(button1, b1hint);
      tt.SetToolTip(button2, b2hint);
      tt.SetToolTip(button3, b3hint);
      tt.SetToolTip(button4, b4hint);
      tt.SetToolTip(button5, b5hint);
      tt.SetToolTip(button6, b6hint);
      tt.SetToolTip(button7, b7hint);
      tt.SetToolTip(button8, b8hint);
      tt.SetToolTip(button9, b9hint);
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
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.button6 = new System.Windows.Forms.Button();
      this.button7 = new System.Windows.Forms.Button();
      this.button8 = new System.Windows.Forms.Button();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.butAbort = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.buttSave = new System.Windows.Forms.Button();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.button9 = new System.Windows.Forms.Button();
      this.button10 = new System.Windows.Forms.Button();
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
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 7);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(32, 24);
      this.button1.TabIndex = 1;
      this.button1.Tag = "1";
      this.button1.Text = "1";
      this.button1.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(50, 7);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(32, 24);
      this.button2.TabIndex = 2;
      this.button2.Tag = "2";
      this.button2.Text = "2";
      this.button2.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(88, 7);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(32, 24);
      this.button3.TabIndex = 3;
      this.button3.Tag = "3";
      this.button3.Text = "3";
      this.button3.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(126, 7);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(32, 24);
      this.button4.TabIndex = 4;
      this.button4.Tag = "4";
      this.button4.Text = "4";
      this.button4.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(164, 7);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(32, 24);
      this.button5.TabIndex = 5;
      this.button5.Tag = "5";
      this.button5.Text = "5";
      this.button5.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(202, 7);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(32, 24);
      this.button6.TabIndex = 8;
      this.button6.Tag = "6";
      this.button6.Text = "6";
      this.button6.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button7
      // 
      this.button7.Location = new System.Drawing.Point(240, 7);
      this.button7.Name = "button7";
      this.button7.Size = new System.Drawing.Size(32, 24);
      this.button7.TabIndex = 9;
      this.button7.Tag = "7";
      this.button7.Text = "7";
      this.button7.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button8
      // 
      this.button8.Location = new System.Drawing.Point(278, 7);
      this.button8.Name = "button8";
      this.button8.Size = new System.Drawing.Size(32, 24);
      this.button8.TabIndex = 10;
      this.button8.Tag = "8";
      this.button8.Text = "8";
      this.button8.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Checked = true;
      this.checkBox1.CheckState = System.Windows.Forms.CheckState.Indeterminate;
      this.checkBox1.Location = new System.Drawing.Point(12, 35);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(65, 17);
      this.checkBox1.TabIndex = 6;
      this.checkBox1.Text = "Antialias";
      this.checkBox1.ThreeState = true;
      this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // butAbort
      // 
      this.butAbort.Location = new System.Drawing.Point(412, 14);
      this.butAbort.Name = "butAbort";
      this.butAbort.Size = new System.Drawing.Size(52, 24);
      this.butAbort.TabIndex = 7;
      this.butAbort.Text = "Abort";
      this.butAbort.Click += new System.EventHandler(this.butAbort_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(540, 2);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(0, 11);
      this.label1.TabIndex = 11;
      // 
      // buttSave
      // 
      this.buttSave.Enabled = false;
      this.buttSave.Location = new System.Drawing.Point(470, 14);
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
      this.label2.Location = new System.Drawing.Point(540, 20);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(0, 11);
      this.label2.TabIndex = 13;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(540, 38);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(0, 11);
      this.label3.TabIndex = 14;
      // 
      // button9
      // 
      this.button9.Location = new System.Drawing.Point(316, 7);
      this.button9.Name = "button9";
      this.button9.Size = new System.Drawing.Size(32, 24);
      this.button9.TabIndex = 15;
      this.button9.Tag = "9";
      this.button9.Text = "9";
      this.button9.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // button10
      // 
      this.button10.Location = new System.Drawing.Point(316, 7);
      this.button10.Name = "button10";
      this.button10.Size = new System.Drawing.Size(32, 24);
      this.button10.TabIndex = 15;
      this.button10.Tag = "8";
      this.button10.Text = "9";
      this.button10.Click += new System.EventHandler(this.buttGo_Click);
      // 
      // WinForm
      // 
      this.ClientSize = new System.Drawing.Size(721, 693);
      this.Controls.Add(this.button9);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.buttSave);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button6);
      this.Controls.Add(this.button7);
      this.Controls.Add(this.button8);
      this.Controls.Add(this.butAbort);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.picture);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "WinForm";
      this.Text = "Aurora Raytracer V2       AJB 2004";
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
    private Button button9;
  }


}