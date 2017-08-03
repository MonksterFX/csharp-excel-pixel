
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace BMPConverter
{
    [ComVisible(true)]
    [Guid("6B59EC0A-AF3F-419D-B116-B616B1BCEF15")]
    public interface IImageConverter
    {
        bool LoadImage(string url);
        byte[] ConvertToByteArray();
        RGBPixel[] GetPixels();

        int GetVSize();
        int GetHSize();

    }
    [ComVisible(true)]
    [Guid("7D2E80E7-8780-4E0B-B5E0-5E23589354FA"),
        ClassInterface(ClassInterfaceType.None)]
    public class BMPConverter : IImageConverter
    {

        Image img;
        byte[] buffer;
        UInt32 offset;
        UInt32 hSize;
        UInt32 vSize;
        UInt32 compression;

        public byte[] ConvertToByteArray()
        {
            using (var ms = new MemoryStream())
            {
                this.img.Save(ms, ImageFormat.Bmp);

                this.buffer = ms.ToArray();
                return this.buffer;
            }
        }

        public bool LoadImage(string url)
        {
            bool result = false;
            if (File.Exists(url) && (Path.GetExtension(url).ToLower() == ".bmp"))
            {
                img = Image.FromFile(url);
                //if (img.PixelFormat != PixelFormat.Format24bppRgb)
                //{
                img = ConvertTo24bpp(img);
                //}
                result = true;
            }
            return result;
        }

        public RGBPixel[] GetPixels()
        {
            RGBPixel[] pixelArray;
            ConvertToByteArray();

            // Read Head
            offset = BitConverter.ToUInt32(this.buffer, 10);
            hSize = BitConverter.ToUInt32(this.buffer, 18);
            vSize = BitConverter.ToUInt32(this.buffer, 22);
            compression = BitConverter.ToUInt32(this.buffer, 30) == (uint)0 ? (uint)0 : (uint)2;
            
            // Prepare Array
            UInt32 pixels = (UInt32)(hSize * vSize);
            pixelArray = new RGBPixel[pixels];

            UInt32 pixel = 0;
       
               for (UInt32 vPos = vSize; vPos > 0; vPos--)
            {
            for (UInt32 hPos = 0; hPos < hSize; hPos++)
                {
                    UInt32 vOffset = (vPos -1) * ((hSize * 3) + compression);
                    UInt32 hOffset = (hPos - 1) * 3;
                    UInt32 totalOffset = offset + vOffset + hOffset;

                    pixelArray[pixel] = new RGBPixel(this.buffer[totalOffset+2], this.buffer[totalOffset + 1], this.buffer[totalOffset + 0]);
                    pixel++;
                }
            }

            return pixelArray;

        }

        public static Bitmap ConvertTo24bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        public int GetVSize()
        {
            return (int)vSize;
        }

        public int GetHSize()
        {
            return (int)hSize;
        }
    }
}
