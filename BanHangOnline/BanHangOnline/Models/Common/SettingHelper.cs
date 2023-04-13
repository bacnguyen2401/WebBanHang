using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models.Common
{
    public class SettingHelper
    {
        private static ApplicationDbContext _context = new ApplicationDbContext();

        public static string GetValue(string key)
        {
            var item = _context.SystemSettings.SingleOrDefault(x=>x.SettingKet == key);
            if (item != null)
            {
                return item.SettingValue;
            }
            return "";
        }
    }
}