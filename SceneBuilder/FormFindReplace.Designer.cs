namespace SceneBuilder
{
  partial class formFindReplace
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.txtbFind = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtbReplace = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.chkbMatchCase = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // txtbFind
      // 
      this.txtbFind.Location = new System.Drawing.Point(66, 12);
      this.txtbFind.Name = "txtbFind";
      this.txtbFind.Size = new System.Drawing.Size(206, 20);
      this.txtbFind.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(27, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Find";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(47, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Replace";
      // 
      // txtbReplace
      // 
      this.txtbReplace.Location = new System.Drawing.Point(66, 38);
      this.txtbReplace.Name = "txtbReplace";
      this.txtbReplace.Size = new System.Drawing.Size(206, 20);
      this.txtbReplace.TabIndex = 3;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 74);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "Find";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.Find);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(104, 74);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "Replace";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.Replace);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(196, 74);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(75, 23);
      this.button3.TabIndex = 6;
      this.button3.Text = "Replace All";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.ReplaceAll);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(197, 103);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(75, 23);
      this.button4.TabIndex = 7;
      this.button4.Text = "Close";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.CloseForm);
      // 
      // chkbMatchCase
      // 
      this.chkbMatchCase.AutoSize = true;
      this.chkbMatchCase.Location = new System.Drawing.Point(15, 108);
      this.chkbMatchCase.Name = "chkbMatchCase";
      this.chkbMatchCase.Size = new System.Drawing.Size(83, 17);
      this.chkbMatchCase.TabIndex = 8;
      this.chkbMatchCase.Text = "Match Case";
      this.chkbMatchCase.UseVisualStyleBackColor = true;
      // 
      // formFindReplace
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 134);
      this.Controls.Add(this.chkbMatchCase);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.txtbReplace);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtbFind);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "formFindReplace";
      this.Text = "Search/Replace";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtbFind;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtbReplace;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.CheckBox chkbMatchCase;
  }
}