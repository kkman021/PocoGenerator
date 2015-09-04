using PocoGenerator.Factory;
using PocoGenerator.Interface;
using PocoGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.PocoHelper
{
    /// <summary>
    /// PocoHelper 類別
    /// </summary>
    public class PocoHelpers
    {
        private PocoHelperFactory _factory;

        public PocoHelpers()
        {
            _factory = new PocoHelperFactory();
        }

        public IPocoHelper GetPocoHelper(string dbType, DBInfo dbInfo)
        {
            IPocoHelper helper;
            helper = _factory.GetPocoHelper(dbType, dbInfo);
            return helper;
        }

        public IPocoHelper GetPocoHelper(string dbType)
        {
            IPocoHelper helper;
            helper = _factory.GetPocoHelper(dbType);
            return helper;
        }
    }
}
