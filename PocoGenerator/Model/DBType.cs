using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.Model
{
    public class DBType
    {
        /// <summary>
        /// 連線類別名稱
        /// </summary>
        public string Name { get; set; }

    }

    public class DBTypes : List<DBType>
    {
    }
}
