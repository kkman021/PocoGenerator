using PocoGenerator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PocoGenerator.Extension
{
    public static class ConfigHelper
    {
        public static string RequiredStr => ConfigurationManager.AppSettings["RequiredStr"];
        public static string MaxLengthStr => ConfigurationManager.AppSettings["MaxLengthStr"];
        public static string HostIP => ConfigurationManager.AppSettings["HostIP"];
        public static string Port => ConfigurationManager.AppSettings["Port"];
        public static string UserID => ConfigurationManager.AppSettings["UserID"];
        public static string UserPwd => ConfigurationManager.AppSettings["UserPwd"];
        public static string DBName => ConfigurationManager.AppSettings["DBName"];
        public static string DBOwner => ConfigurationManager.AppSettings["DBOwner"];
        public static int DBType => Convert.ToInt32(ConfigurationManager.AppSettings["DBType"]);
        public static bool ValidateByWindow => Convert.ToBoolean(ConfigurationManager.AppSettings["ValidateByWindow"]);

        /// <summary>
        /// 更新App.Config 中AppSetting 
        /// </summary>
        /// <param name="key"> AppSetting Name </param>
        /// <param name="updateValue"> AppSetting Value </param>
        public static void UpdateSetting(string key, string updateValue)
        {
            Configuration configuration = ConfigurationManager.
                OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = updateValue;
            configuration.Save();
        }

        /// <summary>
        /// 更新DBInf設定設定App.Config 中AppSetting 
        /// </summary>
        /// <param name="dbInfo"> <see cref="DBInfo" /> </param>
        public static void UpdateDbInfo(DBInfo dbInfo)
        {
            foreach (var prop in dbInfo.GetType().GetProperties())
            {
                var val = prop.GetValue(dbInfo);
                var key = prop.Name;

                UpdateSetting(key, val.ToString());
            }
        }
    }
}