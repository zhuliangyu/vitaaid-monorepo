using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDB.DBPO
{
  public static class EnvHelper
  {
    public const string BASE_DIR = @"/ProductImages/";

    public const string PRODUCT_DIR = BASE_DIR + @"product/";
    public const string PRODUCT_SHEET_DIR = BASE_DIR + @"product/datasheet/";
    public const string BLOG_DIR = BASE_DIR + @"blog/";
    public const string WEBINAR_DIR = BASE_DIR + @"webinar/";
    public const string MEMBER_DIR = BASE_DIR + @"member/";
    public const string PROTOCOL_DIR = BASE_DIR + @"protocol/";

    public const string UPLOAD_DIR = BASE_DIR + @"upload/";
    public const string THUMB_DIR = BASE_DIR + @"thumb/";
    public const string CACHE_DIR = BASE_DIR + @"cache/";

    public static int MAX_UPLOAD_SIZE = 40 * 1024 * 1024;// 40MB 40960000;
    public static string[] ALLOW_FILE_EXT = { ".pdf", ".png", ".jpg", ".jpeg", ".gif", ".avi", ".mp3" };
  }
}
