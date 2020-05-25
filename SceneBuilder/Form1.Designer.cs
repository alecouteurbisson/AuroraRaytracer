namespace SceneBuilder
{
  partial class Form1
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.txtbOut = new System.Windows.Forms.TextBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.menuFile = new System.Windows.Forms.ToolStripDropDownButton();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuEdit = new System.Windows.Forms.ToolStripDropDownButton();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.buttCompile = new System.Windows.Forms.ToolStripButton();
      this.buttRender = new System.Windows.Forms.ToolStripButton();
      this.buttAbort = new System.Windows.Forms.ToolStripButton();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.scintilla = new ScintillaNet.Scintilla();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.scintilla)).BeginInit();
      this.SuspendLayout();
      // 
      // txtbOut
      // 
      this.txtbOut.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtbOut.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtbOut.Location = new System.Drawing.Point(0, 0);
      this.txtbOut.Multiline = true;
      this.txtbOut.Name = "txtbOut";
      this.txtbOut.ReadOnly = true;
      this.txtbOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtbOut.Size = new System.Drawing.Size(792, 113);
      this.txtbOut.TabIndex = 0;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.buttCompile,
            this.buttRender,
            this.buttAbort});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(792, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // menuFile
      // 
      this.menuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
      this.menuFile.Name = "menuFile";
      this.menuFile.Size = new System.Drawing.Size(38, 22);
      this.menuFile.Text = "File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.NewDocument;
      this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.newToolStripMenuItem.Text = "New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.FileNew);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Open;
      this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.openToolStripMenuItem.Text = "Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.FileOpen);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Save;
      this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.FileSave);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Save;
      this.saveAsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.saveAsToolStripMenuItem.Text = "Save As...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.FileSaveAs);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 6);
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.FileQuit);
      // 
      // menuEdit
      // 
      this.menuEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.toolStripMenuItem2,
            this.searchToolStripMenuItem});
      this.menuEdit.Image = ((System.Drawing.Image)(resources.GetObject("menuEdit.Image")));
      this.menuEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.menuEdit.Name = "menuEdit";
      this.menuEdit.Size = new System.Drawing.Size(40, 22);
      this.menuEdit.Text = "Edit";
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Copy;
      this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.copyToolStripMenuItem.Text = "Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.EditCopy);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Cut;
      this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.cutToolStripMenuItem.Text = "Cut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.EditCut);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Paste;
      this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.pasteToolStripMenuItem.Text = "Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.EditPaste);
      // 
      // selectAllToolStripMenuItem
      // 
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.EditSelectAll);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
      // 
      // searchToolStripMenuItem
      // 
      this.searchToolStripMenuItem.Image = global::SceneBuilder.Properties.Resources.Search;
      this.searchToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
      this.searchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.searchToolStripMenuItem.Text = "Find/Replace...";
      this.searchToolStripMenuItem.Click += new System.EventHandler(this.EditFindReplace);
      // 
      // buttCompile
      // 
      this.buttCompile.Image = global::SceneBuilder.Properties.Resources.CheckGrammar;
      this.buttCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttCompile.Name = "buttCompile";
      this.buttCompile.Size = new System.Drawing.Size(72, 22);
      this.buttCompile.Text = "Compile";
      this.buttCompile.Click += new System.EventHandler(this.Compile);
      // 
      // buttRender
      // 
      this.buttRender.Image = global::SceneBuilder.Properties.Resources.Play;
      this.buttRender.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttRender.Name = "buttRender";
      this.buttRender.Size = new System.Drawing.Size(64, 22);
      this.buttRender.Text = "Render";
      this.buttRender.Click += new System.EventHandler(this.Render);
      // 
      // buttAbort
      // 
      this.buttAbort.Enabled = false;
      this.buttAbort.Image = global::SceneBuilder.Properties.Resources.Stop;
      this.buttAbort.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.buttAbort.Name = "buttAbort";
      this.buttAbort.Size = new System.Drawing.Size(57, 22);
      this.buttAbort.Text = "Abort";
      this.buttAbort.Click += new System.EventHandler(this.Abort);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 25);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.scintilla);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.txtbOut);
      this.splitContainer1.Size = new System.Drawing.Size(792, 482);
      this.splitContainer1.SplitterDistance = 365;
      this.splitContainer1.TabIndex = 2;
      // 
      // scintilla
      // 
      this.scintilla.ConfigurationManager.Language = "cs";
      this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scintilla.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.scintilla.Indentation.SmartIndentType = ScintillaNet.SmartIndent.CPP;
      this.scintilla.Indentation.TabWidth = 2;
      this.scintilla.Indentation.UseTabs = false;
      this.scintilla.IsBraceMatching = true;
      this.scintilla.Lexing.Lexer = ScintillaNet.Lexer.Cpp;
      this.scintilla.Lexing.LexerName = "cpp";
      this.scintilla.Lexing.LineCommentPrefix = "";
      this.scintilla.Lexing.StreamCommentPrefix = "";
      this.scintilla.Lexing.StreamCommentSufix = "";
      this.scintilla.Location = new System.Drawing.Point(0, 0);
      this.scintilla.Name = "scintilla";
      this.scintilla.Size = new System.Drawing.Size(792, 365);
      this.scintilla.Styles.BraceBad.FontName = "Verdana";
      this.scintilla.Styles.BraceBad.ForeColor = System.Drawing.Color.Red;
      this.scintilla.Styles.BraceLight.FontName = "Verdana";
      this.scintilla.Styles.ControlChar.FontName = "Verdana";
      this.scintilla.Styles.Default.FontName = "Verdana";
      this.scintilla.Styles.IndentGuide.FontName = "Verdana";
      this.scintilla.Styles.LastPredefined.FontName = "Verdana";
      this.scintilla.Styles.LineNumber.FontName = "Verdana";
      this.scintilla.Styles.Max.FontName = "Verdana";
      this.scintilla.TabIndex = 0;
      this.scintilla.DocumentChange += new System.EventHandler<ScintillaNet.NativeScintillaEventArgs>(this.SourceChanged);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.DefaultExt = "scn";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "Scene file|*.scn";
      this.openFileDialog1.Title = "Open Scene file";
      // 
      // saveFileDialog1
      // 
      this.saveFileDialog1.DefaultExt = "scn";
      this.saveFileDialog1.Filter = "Scene file|*.scn";
      this.saveFileDialog1.Title = "Save Scene file";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(792, 507);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "Form1";
      this.Text = "Aurora Raytracer";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.scintilla)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtbOut;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton buttRender;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ToolStripButton buttCompile;
    private ScintillaNet.Scintilla scintilla;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.ToolStripButton buttAbort;
    private System.Windows.Forms.ToolStripDropDownButton menuFile;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.ToolStripDropDownButton menuEdit;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
  }
}

