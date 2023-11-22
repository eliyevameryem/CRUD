namespace Pronia.Utilities
{
    public static class FileExtension
    {
        public static bool CheckFileType(this IFormFile ImageFile, string type)
        {
            return ImageFile.Contenttype(type);
        }
        public static bool CheckFileLength(this IFormFile ImageFile, int length)
        {
            return ImageFile.Length > length;
        }

        
    }
}
