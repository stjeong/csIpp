using IppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BmpShow
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IppDib _bitmap = IppDib.LoadFrom("lenna.bmp");
        Bitmap _img;
        private void Form1_Load(object sender, EventArgs e)
        {
            _img = _bitmap.ToBitmap();


           // Clipboard.SetImage(_img);
        // _img.Save("lenna2.bmp", ImageFormat.Bmp);
    }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(_img, new Point(0, 0));
        }
    }
}
