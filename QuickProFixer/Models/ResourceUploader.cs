using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QuickProFixer.Models.UtilityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickProFixer.Models
{
    public static class ResourceUploader
    {        


        public  static async Task UploadFile(IFormFile FileUpload, string UniqueFileName, IWebHostEnvironment _environment, FileCategory fileCategory)
        {           
            
            var uploadsOtherFiles = Path.Combine(_environment.WebRootPath, fileCategory.ToString());

            var filePath = Path.Combine(uploadsOtherFiles, UniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await FileUpload.CopyToAsync(stream);
            }      
                    
        }

        public static string GetUniqueFileName(string fileName)
        {
            if (fileName!=null)
            {
                fileName = Path.GetFileName(fileName);
                return Path.GetFileNameWithoutExtension(fileName)
                    + "_"
                    + Guid.NewGuid().ToString().Substring(0, 4)
                    + Path.GetExtension(fileName);
            }
            return null;
           
        }

    }

    
}
