using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: BinaryToImage.exe [BitTextFile] [ImageFilename]");
                return;
            }

            var outputImage = args[1];

            var bitsLines = File.ReadAllLines(args[0])
                .Select(x => x.Replace(" ", ""))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var black = new SolidBrush(Color.Black);
            var white = new SolidBrush(Color.White);

            var width = bitsLines.Max(x => x.Length) * 8;
            var height = bitsLines.Count(x => !string.IsNullOrWhiteSpace(x))*8;
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);

            for (var y = 0; y < bitsLines.Count; y++)
            {
                var line = bitsLines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    var color = c == '0' ? black : white;
                    g.FillRectangle(color, x * 8, y * 8, 8f, 8f);
                }
            }

            ImageFormat format;

            switch (Path.GetExtension(outputImage).ToLower())
            {
                case "jpeg":
                case "jpg":
                    format = ImageFormat.Jpeg;
                    break;
                default:
                    format = ImageFormat.Png;
                    break;

            }

            bmp.Save(outputImage, format);
        }
    }
}
