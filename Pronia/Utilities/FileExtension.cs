using static NuGet.Packaging.PackagingConstants;

namespace Pronia.Utilities
{
    public static class FileExtension
    {
        //public static bool CheckFileType(this IFormFile ImageFile, string type)
        //{
        //    //return ImageFile.Contenttype(type);
        //}
        //public static bool CheckFileLength(this IFormFile ImageFile, int length)
        //{
        //    return ImageFile.Length > length;
        //}
        //public static string UploadFile(this IFormFile file, string envPath, string folderName)
        //{
        //    string filename = Guid.NewGuid().ToString() + file.FileName;

        //    string path = Path.Combine(root, folder, filename);

        //    using (FileStream stream = new FileStream(path, FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //    }

        //    return filename;
        //}

        public static void DeleteFile(this string image, string root, string folder)
        {
            string path = Path.Combine(root, folder, image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
