using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Galleri.Model
{
    public class Gallery
    {
        private static readonly Regex ApprovedExtensions;
        private static string PhysicalUploadedImagesPath;
        private static readonly Regex SantizePath;

        static Gallery()
        {
            ApprovedExtensions = new Regex(".*.(gif|jpg|png|GIF|JPG|PNG)$");
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            

            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "pics");
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            SantizePath.Replace(invalidChars, "");
        }

        public IEnumerable<Images> GetImageNames()
        {
            var dir = new DirectoryInfo(Path.Combine(PhysicalUploadedImagesPath, "thumbs"));
            return (from fi in dir.GetFiles()
                    where ApprovedExtensions.IsMatch(fi.Name)
                    select new Images
                    {
                        name = fi.Name,
                        thumbPath = Path.Combine("/?img=", fi.Name),
                        thumbnailUrl = Path.Combine("pics/thumbs/", fi.Name)

                    }).OrderBy(fi => fi.name).ToList();
        }

        public bool ImageExists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }

        private bool IsValidImage(Image image)
        {
            return image.RawFormat.Guid == ImageFormat.Gif.Guid || image.RawFormat.Guid == ImageFormat.Png.Guid || image.RawFormat.Guid == ImageFormat.Jpeg.Guid;
        }

        public string SaveImage(Stream stream, string fileName)
        {
            int makeUnique = 1;
            fileName = SantizePath.Replace(fileName, "");

            var image = System.Drawing.Image.FromStream(stream);

            if (!IsValidImage(image))
            {
                throw new ArgumentException();
            }

            if (ImageExists(fileName))
            {
                var extension = Path.GetExtension(fileName);
                var name = Path.GetFileNameWithoutExtension(fileName);

                while(ImageExists(fileName))
                {
                    fileName = String.Format("{0}{1}{2}", name, makeUnique, extension);
                    makeUnique++;
                }
            }
            

            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            image.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));
            thumbnail.Save(Path.Combine(PhysicalUploadedImagesPath, "thumbs", fileName));

            return fileName;
        }
    }
}