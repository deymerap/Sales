
namespace Sales.Helpers
{
    using System.IO;
    public class FileHelper
    {
        public static byte[] ReadFully(Stream pvInput)
        {
            using (MemoryStream ms = new  MemoryStream())
            {
                pvInput.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
