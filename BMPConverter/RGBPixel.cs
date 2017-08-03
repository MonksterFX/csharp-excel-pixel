using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace BMPConverter
{

    [ComVisible(true)]
    [Guid("9C971472-125E-42AA-9028-F6CD7ECCD781")]
    public interface IPixel
    {
        byte Red{ get; set; }
        byte Blue { get; set; }
        byte Green { get; set; }
    }

    [ComVisible(true)]
    [Guid("05029BD1-6A9C-4A8B-B2CA-0310B021B235"), 
        ClassInterface(ClassInterfaceType.None)]
    public class RGBPixel : IPixel
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public RGBPixel(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
    }
}
