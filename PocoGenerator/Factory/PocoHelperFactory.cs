using PocoGenerator.Interface;
using PocoGenerator.Model;
using PocoGenerator.PocoHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.Factory
{
    /// <summary>
    /// PocoHelper 工廠類別
    /// </summary>
    public class PocoHelperFactory
    {
        public IPocoHelper GetPocoHelper(string dbType, DBInfo dbinfo)
        {
            IPocoHelper helper = null;

            switch (dbType)
            {
                case "MSSQL":
                    helper = new MSSQLPocoHelper(dbinfo);
                    break;
                case "Oracle":
                    helper = new OraclePocoHelper(dbinfo);
                    break;
                default:
                    throw new NotSupportedException("Not Support" + dbType);
            }
            return helper;
        }

        public IPocoHelper GetPocoHelper(string dbType)
        {
            IPocoHelper helper = null;

            switch (dbType)
            {
                case "MSSQL":
                    helper = new MSSQLPocoHelper();
                    break;
                case "Oracle":
                    helper = new OraclePocoHelper();
                    break;
                default:
                    throw new NotSupportedException("Not Support" + dbType);
            }
            return helper;
        }
    }
}
