using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Statics
{
    public static class PathTools
    {
        #region User

        public static readonly string DefaultUserAvatar = "DefaultAvatar.png";

        public static readonly string UserAvatarServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/user/");
        public static readonly string UserAvatarPath = "/content/user/";

        /// <summary>
        /// استخراج نام فایل از مسیر کامل یا URL
        /// </summary>
        /// <param name="filePath">مسیر کامل یا URL</param>
        /// <returns>نام فایل</returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
        #endregion

        #region Site

        public static readonly string SiteAddress = "https://localhost:44308";

        #endregion

        #region Ckeditor

        public static readonly string EditorImageServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/ckeditor/");
        public static readonly string EditorImagePath = "/content/ckeditor/";

        #endregion
    }


}
