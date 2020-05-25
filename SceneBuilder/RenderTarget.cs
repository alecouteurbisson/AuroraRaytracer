using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SceneBuilder
{
  public partial class RenderTarget : Form
  {
    Bitmap bm;
    bool closed;

    public RenderTarget(Size size)
    {
      InitializeComponent();
      closed = false;
      ClientSize = size;
      bm = new Bitmap(size.Width, size.Height);
    }

    public Size ImageSize
    {
      set { ClientSize = value; }
      get { return pictureBox1.Size; }
    }

    public Bitmap Bitmap
    {
      get { return bm; }
    }

    public bool Update(Rectangle rect)
    {
      if(closed)
        return false;

      pictureBox1.Invalidate(rect);
      Application.DoEvents();
      return true;
    }

    private void PaintPicture(object sender, PaintEventArgs e)
    {
      if(bm != null)
      {
        //lock(bm)
        //{
        e.Graphics.DrawImage(bm, 0, 0);
        //}
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      closed = true;
    }
  }
}
