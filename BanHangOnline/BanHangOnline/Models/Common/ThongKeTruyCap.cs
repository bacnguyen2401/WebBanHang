using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace BanHangOnline.Models.Common
{
    public class ThongKeTruyCap
    {
        private static string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public static ThongKeViewModel ThongKe()
        {
            using(var connect = new SqlConnection(connection))
            {
                var item = connect.QueryFirstOrDefault<ThongKeViewModel>("sp_ThongKe", commandType: CommandType.StoredProcedure);
                return item;
            }
        }
    }
}