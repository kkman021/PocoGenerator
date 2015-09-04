using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.Model
{
    public class DBInfo
    {
        /// <summary>
        /// 主機IP
        /// </summary>
        public string HostIP { get; set; }

        /// <summary>
        /// 主機Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// Oracle：SERVICE_NAME MSSQL：DBName
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// Oracle DB擁有者
        /// </summary>
        public string DBOwner { get; set; }

        /// <summary>
        /// Windows驗證
        /// </summary>
        public bool ValidateByWindow { get; set; }
    }
}
