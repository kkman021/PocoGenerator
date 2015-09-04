using PocoGenerator.Interface;
using PocoGenerator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator.PocoHelper
{
    public class MSSQLPocoHelper : IPocoHelper
    {
        #region Property
        public string _HostIP { get; private set; }
        public string _UserID { get; private set; }
        public string _UserPwd { get; private set; }
        public string _DBName { get; private set; }
        public bool _IsValidateWindow { get; set; }
        #endregion

        private const string connstrFormat = "Data Source={0};Initial Catalog={1};Connect TimeOut=30;User Id={2};Password={3};Persist Security Info=True;";
        private string connStr;
        private SqlConnection oConnection;

        public MSSQLPocoHelper()
        {

        }

        public MSSQLPocoHelper(DBInfo dbInfo)
        {
            #region 資料驗證
            if (dbInfo == null)
                throw new ArgumentNullException("DBInfo", "參數遺失");

            if (string.IsNullOrWhiteSpace(dbInfo.HostIP))
                throw new ArgumentNullException("主機名稱", "必填");

            if (string.IsNullOrWhiteSpace(dbInfo.DBName))
                throw new ArgumentNullException("資料庫名稱名稱", "必填");

            if (!dbInfo.ValidateByWindow)
            {
                if (string.IsNullOrWhiteSpace(dbInfo.UserID))
                    throw new ArgumentNullException("使用者名稱", "必填");
                if (string.IsNullOrWhiteSpace(dbInfo.UserPwd))
                    throw new ArgumentNullException("密碼", "必填");
            }
            #endregion

            _HostIP = dbInfo.HostIP;
            _UserID = dbInfo.UserID;
            _UserPwd = dbInfo.UserPwd;
            _DBName = dbInfo.DBName;
            _IsValidateWindow = dbInfo.ValidateByWindow;

            connStr = string.Format(connstrFormat, _HostIP, _DBName, _UserID, _UserPwd);
            if (_IsValidateWindow)
                this.connStr += "Integrated Security=True;";


        }

        #region 取得Table清單
        /// <summary>
        /// 取得Table清單
        /// </summary>
        /// <returns></returns>
        public DataView GetTableList()
        {
            var query = new System.Text.StringBuilder(271);
            query.AppendLine(@"SELECT");
            query.AppendLine(@"	A.id,");
            query.AppendLine(@"	A.Name,");
            query.AppendLine(@"	B.value AS TableDesc,");
            query.AppendLine(@"	A.id AS QueryKey");
            query.AppendLine(@"FROM SYSOBJECTS A WITH (NOLOCK)");
            query.AppendLine(@"LEFT OUTER JOIN sys.EXTENDED_PROPERTIES B WITH (NOLOCK)");
            query.AppendLine(@"	ON A.id = B.major_id");
            query.AppendLine(@"	AND B.minor_id = 0");
            query.AppendLine(@"	AND B.Name = 'MS_Description'");
            query.AppendLine(@"WHERE XType = 'U'");
            query.AppendLine(@"ORDER BY Name ASC");

            var oComm = new SqlCommand(query.ToString());
            return GetDataView(oComm);
        }
        #endregion

        #region 取得欄位清單
        public DataView GetColumnList(string tableID)
        {
            var query = new System.Text.StringBuilder();
            query.AppendLine(@"SELECT");
            query.AppendLine(@"	A.Name AS ColumnName,");
            query.AppendLine(@"	B.Name AS DataType,");
            query.AppendLine(@"	CASE");
            query.AppendLine(@"		WHEN A.[Collationid] = 0 THEN NULL");
            query.AppendLine(@"		WHEN A.[Length] < A.Prec THEN A.[Length]");
            query.AppendLine(@"		WHEN A.[Length] > A.Prec THEN A.Prec");
            query.AppendLine(@"		ELSE A.[Length]");
            query.AppendLine(@"	END AS [Length],");
            query.AppendLine(@"	A.[Prec],");
            query.AppendLine(@"	A.[Scale],");
            query.AppendLine(@"	CAST(A.IsNullable AS BIT) AS IsNullable,");
            query.AppendLine(@"	C.value AS [ColumnDes],");
            query.AppendLine(@"	CASE");
            query.AppendLine(@"		WHEN E.object_id IS NOT NULL THEN 'Y'");
            query.AppendLine(@"		ELSE 'N'");
            query.AppendLine(@"	END AS [ISPrimaryKey]");
            query.AppendLine(@"FROM SYSCOLUMNS A WITH (NOLOCK)");
            query.AppendLine(@"INNER JOIN SYSTYPES B WITH (NOLOCK)");
            query.AppendLine(@"	ON A.XType = B.XType");
            query.AppendLine(@"	AND A.XUserType = B.XUserType");
            query.AppendLine(@"LEFT OUTER JOIN sys.EXTENDED_PROPERTIES C WITH (NOLOCK)");
            query.AppendLine(@"	ON A.id = C.major_id");
            query.AppendLine(@"	AND A.ColOrder = C.minor_id");
            query.AppendLine(@"	AND C.Name = 'MS_Description'");
            query.AppendLine(@"LEFT OUTER JOIN sys.INDEXES D WITH (NOLOCK)");
            query.AppendLine(@"	ON A.id = D.object_id");
            query.AppendLine(@"	AND D.IS_PRIMARY_KEY = 1");
            query.AppendLine(@"LEFT OUTER JOIN sys.INDEX_COLUMNS E WITH (NOLOCK)");
            query.AppendLine(@"	ON D.object_id = E.object_id");
            query.AppendLine(@"	AND D.index_id = E.index_id");
            query.AppendLine(@"	AND A.COLID = E.column_id");
            query.AppendLine(@"WHERE id = @object_id");
            query.AppendLine(@"ORDER BY ColOrder ASC");

            var oComm = new SqlCommand(query.ToString());
            oComm.Parameters.Add(new SqlParameter("@OBJECT_ID", tableID));

            return GetDataView(oComm);

        }
        #endregion

        #region 資料型態轉換
        /// <summary>
        /// 轉換資料型態Sql DataType => C# DataType
        /// </summary>
        /// <param name="dbColumnType">Sql DataType</param>
        /// <returns></returns>
        public string GetDataType(string dbColumnType, string scale = "", string Prec = "")
        {
            var result = String.Empty;
            switch (dbColumnType.ToLower())
            {
                case "bigint":
                    result = "long";
                    break;
                case "int":
                    result = "int";
                    break;
                case "smallint":
                    result = "short";
                    break;
                case "binary":
                case "image":
                case "timestamp":
                case "varbinary":
                    result = "byte[]";
                    break;
                case "tinyint":
                    result = "byte";
                    break;
                case "bit":
                    result = "bool";
                    break;
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                    result = "string";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    result = "DateTime";
                    break;
                case "datetimeoffset":
                    result = "DateTimeOffset";
                    break;
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    result = "decimal";
                    break;
                case "float":
                    result = "double";
                    break;
                case "geography":
                case "geometry":
                case "hierarchyid":
                case "sql_variant":
                    result = "object";
                    break;
                case "real":
                    result = "Single";
                    break;
                case "time":
                    result = "TimeSpan";
                    break;
                case "uniqueidentifier":
                    result = "Guid";
                    break;
                case "xml":
                    result = "Xml";
                    break;
            }

            return result;
        }
        #endregion

        #region DataView
        private DataView GetDataView(SqlCommand oComm)
        {
            if (string.IsNullOrWhiteSpace(connStr))
                throw new ArgumentNullException("連線字串", "參數遺失");

            oConnection = new SqlConnection(connStr);
            oComm.Connection = oConnection;

            using (SqlDataAdapter da = new SqlDataAdapter(oComm))
            {
                DataSet ds = new DataSet();
                try
                {
                    oComm.Connection = oConnection;
                    oConnection.Open();
                    da.Fill(ds);
                }
                catch (Exception e)
                {
                    CloseConn();
                    throw e;
                }
                finally
                {
                    CloseConn();
                }
                return ds.Tables[0].DefaultView;
            }
        }
        #endregion

        /// <summary>
        /// 關閉連線
        /// </summary>
        private void CloseConn()
        {
            if (oConnection.State == ConnectionState.Open)
                oConnection.Close();
        }
    }
}
