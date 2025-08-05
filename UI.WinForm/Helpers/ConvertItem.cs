using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UI.WinForm
{
    abstract class ConvertItem
    {

        public static Image binaryToImage(byte[] imageBytes)
        {
            using (MemoryStream tempStream = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] array  to Image
                tempStream.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(tempStream, true);
                return image;
            }
        }

        public static byte[] imageToBinary(Image image)
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                // Convert Image to byte[] array
                image.Save(tempStream, ImageFormat.Png);
                tempStream.Seek(0, SeekOrigin.Begin);
                byte[] imageBytes = tempStream.ToArray();
                return imageBytes;
            }
        }
    }
}
