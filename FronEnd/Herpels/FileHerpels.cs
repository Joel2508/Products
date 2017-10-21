using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FronEnd.Herpels
{
    public class FileHerpels
    {
        public static string FileUpload(HttpPostedFileBase file, string folder)
        {
            var path = string.Empty;
            var pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(path);
            }
            return pic;
        }
    }
}