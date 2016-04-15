using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.Interface
{
    public interface IPocoHelper
    {
        /// <summary>
        /// 取得Table清單
        /// </summary>
        /// <returns></returns>
        DataView GetTableList();

        /// <summary>
        /// 取得欄位清單
        /// </summary>
        /// <param name="tableID">TableName</param>
        /// <returns></returns>

        DataView GetColumnList(string tableID);

        /// <summary>
        /// 資料庫型別轉換成C#型別
        /// </summary>
        /// <param name="dbColumnType">資料庫類型</param>
        /// <returns></returns>
        string GetDataType(string dbColumnType, string scale = "", string Prec = "");
    }
}
