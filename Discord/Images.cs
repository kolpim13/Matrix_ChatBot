using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot
{
    public class Image
    {
        private const int height = 32;
        private const int width = 64;

        private const int red_pos = 0;
        private const int green_pos = 1;
        private const int blue_pos = 2;

        private const int red_val = 1;
        private const int green_val = 2;
        private const int blue_val = 4;

        private const byte red_thresold = 127;
        private const byte green_thresold = 127;
        private const byte blue_thresold = 127;

        private static Dictionary<string, string> imagesList = new Dictionary<string, string>
        {
            { "strawberry", @"D:\Politechnika_semestr2\Seminarium_special\RGB\pictures\strawberry.png" },
            { "fox", @"D:\Politechnika_semestr2\Seminarium_special\RGB\pictures\fox.png" },
        };

        public int Height
        {
            get { return height; }
        }
        public int Width
        {
            get { return width; }
        }

        public bool ConvertToByteArray_BlackAndWhite(string pictureName, out byte[] picture)
        {
            // Check if picture is exist
            if (imagesList.TryGetValue(pictureName, out string path) == false)
            {
                picture = null;
                return false;
            }

            // Get bitmap from image on disk
            Bitmap im = (Bitmap)System.Drawing.Image.FromFile(path);

            // Create byte array and fill it
            //picture = new byte[height, width];
            picture = new byte[height * width];
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Color pixelColor = im.GetPixel(col, row);
                    // Fill
                    byte byteColor = 0;
                    if (pixelColor.R > red_thresold) { byteColor |= red_val; }
                    if (pixelColor.G > green_thresold) { byteColor |= green_val; }
                    if (pixelColor.B > blue_thresold) { byteColor |= blue_val; }

                    //picture[row, col] = byteColor;
                    picture[row * width + col] = byteColor;
                }
            }

            return true;
        }

        public bool ChangeColor(List<RGB_Color> colorsFrom, List<RGB_Color> colorsTo)
        {

            return false;
        }
    }

    public class Picture
    {
        private const int height = 32;
        private const int halfH = 16;
        private const int width = 64;
        public int Height
        {
            get { return height; }
        }
        public int Width
        {
            get { return width; }
        }

        private const string path_to_pictures = @"D:\Politechnika_semestr2\Seminarium_special\RGB\pictures\";

        private const int red_val = 1;
        private const int green_val = 2;
        private const int blue_val = 4;
        private const int second_pixel_shift = 3;

        private const byte red_thresold = 127;
        private const byte green_thresold = 127;
        private const byte blue_thresold = 127;

        public bool ConvertToByteArray_Colors8(string pictureName, out byte[] picture)
        {
            string path = path_to_pictures + pictureName;

            // Check if picture is exist
            if (File.Exists(path) == false)
            {
                picture = null;
                return false;
            }

            // Get bitmap from image on disk
            Bitmap im = (Bitmap)System.Drawing.Image.FromFile(path);

            // Create byte array and fill it
            //picture = new byte[height, width];
            picture = new byte[height * width / 2];
            for (int row = 0; row < halfH; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Color pixelColor = im.GetPixel(col, row);
                    Color pixelColor2 = im.GetPixel(col, row+halfH);

                    // Fill
                    byte byteColor = 0;
                    if (pixelColor.R > red_thresold) { byteColor |= red_val; }
                    if (pixelColor.G > green_thresold) { byteColor |= green_val; }
                    if (pixelColor.B > blue_thresold) { byteColor |= blue_val; }
                    if (pixelColor2.R > red_thresold) { byteColor |= (red_val << second_pixel_shift); }
                    if (pixelColor2.G > green_thresold) { byteColor |= (green_val << second_pixel_shift); }
                    if (pixelColor2.B > blue_thresold) { byteColor |= (blue_val << second_pixel_shift); }

                    picture[row * width + col] = byteColor;
                }
            }

            return true;
        }
    }
}
