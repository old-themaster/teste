// Decompiled with JetBrains decompiler
// Type: Tank.Request.ValidateCode
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 763179ED-1C51-45AB-99EE-9908DC4C4D6A
// Assembly location: C:\WONDERTANK vReZero\Request\bin\Tank.Request.dll

using Bussiness;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Tank.Request
{
  public class ValidateCode : Page
  {
    public static Color[] colors = new Color[4]
    {
      Color.Blue,
      Color.DarkRed,
      Color.Green,
      Color.Gold
    };
    protected HtmlForm form1;
    protected Button Button1;

    protected void Page_Load(object sender, EventArgs e)
    {
      byte[] image = CheckCode.CreateImage(CheckCode.GenerateCheckCode());
      this.Response.ClearContent();
      this.Response.ContentType = "image/Gif";
      this.Response.BinaryWrite(image);
    }

    protected void Button1_Click(object sender, EventArgs e) => this.CreateCheckCodeImage(this.GenerateCheckCode());

    private string GenerateCheckCode()
    {
      string empty = string.Empty;
      Random random = new Random();
      for (int index = 0; index < 4; ++index)
      {
        char ch = (char) (65U + (uint) (ushort) (random.Next() % 26));
        empty += ch.ToString();
      }
      return empty;
    }

    private void CreateCheckCodeImage(string checkCode)
    {
      if (checkCode == null || checkCode.Trim() == string.Empty)
        return;
      Bitmap bitmap = new Bitmap((int) Math.Ceiling((double) checkCode.Length * 40.5), 44);
      Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap);
      try
      {
        Random random = new Random();
        Color color = ValidateCode.colors[random.Next(ValidateCode.colors.Length)];
        graphics.Clear(Color.Transparent);
        for (int index = 0; index < 2; ++index)
        {
          int num1 = random.Next(bitmap.Width);
          random.Next(bitmap.Width);
          int num2 = random.Next(bitmap.Height);
          random.Next(bitmap.Height);
          graphics.DrawArc(new Pen(color, 2f), -num1, -num2, bitmap.Width * 2, bitmap.Height, 45, 100);
        }
        Font font = new Font("Arial", 24f, FontStyle.Bold | FontStyle.Italic);
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), color, color, 1.2f, true);
        graphics.DrawString(checkCode, font, (Brush) linearGradientBrush, 2f, 2f);
        int num = 40;
        Math.Sin(Math.PI * (double) num / 180.0);
        Math.Cos(Math.PI * (double) num / 180.0);
        Math.Atan(Math.PI * (double) num / 180.0);
        if (num > 0)
        {
          int width = bitmap.Width;
        }
        new TextureBrush((System.Drawing.Image) bitmap).RotateTransform(30f);
        bitmap.Save("c:\\1.jpg", ImageFormat.Png);
        MemoryStream memoryStream = new MemoryStream();
        bitmap.Save((Stream) memoryStream, ImageFormat.Png);
        this.Response.ClearContent();
        this.Response.ContentType = "image/Gif";
        this.Response.BinaryWrite(memoryStream.ToArray());
      }
      finally
      {
        graphics.Dispose();
        bitmap.Dispose();
      }
    }

    public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
    {
      int width = bmp.Width + 2;
      int height = bmp.Height + 2;
      PixelFormat format = !(bkColor == Color.Transparent) ? bmp.PixelFormat : PixelFormat.Format32bppArgb;
      Bitmap bitmap1 = new Bitmap(width, height, format);
      Graphics graphics1 = Graphics.FromImage((System.Drawing.Image) bitmap1);
      graphics1.Clear(bkColor);
      graphics1.DrawImageUnscaled((System.Drawing.Image) bmp, 1, 1);
      graphics1.Dispose();
      GraphicsPath graphicsPath = new GraphicsPath();
      graphicsPath.AddRectangle(new RectangleF(0.0f, 0.0f, (float) width, (float) height));
      Matrix matrix = new Matrix();
      matrix.Rotate(angle);
      RectangleF bounds = graphicsPath.GetBounds(matrix);
      Bitmap bitmap2 = new Bitmap((int) bounds.Width, (int) bounds.Height, format);
      Graphics graphics2 = Graphics.FromImage((System.Drawing.Image) bitmap2);
      graphics2.Clear(bkColor);
      graphics2.TranslateTransform(-bounds.X, -bounds.Y);
      graphics2.RotateTransform(angle);
      graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
      graphics2.DrawImageUnscaled((System.Drawing.Image) bitmap1, 0, 0);
      graphics2.Dispose();
      bitmap1.Dispose();
      return bitmap2;
    }
  }
}
