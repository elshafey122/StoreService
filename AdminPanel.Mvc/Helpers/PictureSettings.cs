using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace AdminDashboard.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
            //1-Get Folder Path
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",FolderName);
            //2-Set FileName Unique
            var fileName = Guid.NewGuid() + file.FileName;
            //3-GetFilePath
            var filePath = Path.Combine(FolderPath, fileName);
            //4-Save File As stream
            var fs = new FileStream(filePath, FileMode.Create);
            //5-copy file into stream
            file.CopyTo(fs);
            //6-return fileName
            return Path.Combine($"{FolderName}", fileName); //   products/123.child.jpg
		}

        public static void DeleteFile(string imageurl) //images/foldername // filenamem
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", imageurl);
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
