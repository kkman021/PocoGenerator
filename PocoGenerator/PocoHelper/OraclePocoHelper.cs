using Oracle.ManagedDataAccess.Client;
using PocoGenerator.Interface;
using PocoGenerator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PocoGenerator.PocoHelper
{
    public class OraclePocoHelper : IPocoHelper
    {
        #region Property

        public string _HostIP { get; private set; }
        public string _Port { get; private set; }
        public string _UserID { get; private set; }
        public string _UserPwd { get; private set; }
        public string _ServiceName { get; private set; }
        public string _OwnerName { get; set; }

        #endregion Property

        private const string connstrFormat = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {4}))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = {1})));User Id={2};Password={3};";
        private string connStr;
        private OracleConnection oConnection;

        public OraclePocoHelper()
        {
        }

        public OraclePocoHelper(DBInfo dbInfo)
        {
            #region 資料驗證

            if (dbInfo == null)
                throw new ArgumentNullException("DBInfo", "參數遺失");

            if (string.IsNullOrWhiteSpace(dbInfo.HostIP))
                throw new ArgumentNullException("主機名稱", "必填");

            if (string.IsNullOrWhiteSpace(dbInfo.DBName))
                throw new ArgumentNullException("Service Name", "必填");

            if (string.IsNullOrWhiteSpace(dbInfo.DBOwner))
                throw new ArgumentNullException("DB Owner Name", "必填");

            if (string.IsNullOrWhiteSpace(dbInfo.UserID))
                throw new ArgumentNullException("使用者名稱", "必填");

            if (string.IsNullOrWhiteSpace(dbInfo.UserPwd))
                throw new ArgumentNullException("密碼", "必填");

            #endregion 資料驗證

            _HostIP = dbInfo.HostIP;
            _Port = dbInfo.Port;
            _UserID = dbInfo.UserID;
            _UserPwd = dbInfo.UserPwd;
            _ServiceName = dbInfo.DBName;
            _OwnerName = dbInfo.DBOwner;
            connStr = string.Format(connstrFormat, _HostIP, _ServiceName, _UserID, _UserPwd, _Port);
        }

        #region 取得Table清單

        /// <summary>
        /// 取得Table清單 
        /// </summary>
        /// <returns></returns>
        public DataView GetTableList()
        {
            var query = "select AT.Table_Name AS ID,AT.Table_Name AS Name ,ATC.COMMENTS AS TableDesc,AT.Table_Name AS QueryKey " +
                        "   from ALL_TABLES AT" +
                        "   join ALL_TAB_COMMENTS ATC " +
                        "     on ATC.OWNER = AT.OWNER " +
                        "       AND ATC.TABLE_TYPE = 'TABLE' " +
                        "       AND ATC.TABLE_NAME = AT.TABLE_NAME ";
            if (!string.IsNullOrWhiteSpace(_OwnerName))
            {
                query += " where AT.OWNER = :OWNER";
            }

            query += " order by AT.Table_Name";

            OracleCommand oComm = new OracleCommand(query);
            oComm.Parameters.Add(new OracleParameter("OWNER", _OwnerName));

            return GetDataView(oComm);
        }

        #endregion 取得Table清單

        #region 取得欄位清單

        public DataView GetColumnList(string tableID)
        {
            #region Query

            var query = @"SELECT C.COLUMN_NAME AS ColumnName,
                              DATA_TYPE          AS DataType,
                              DATA_LENGTH        AS LENGTH,
                              DATA_PRECISION     AS Prec,
                              DATA_SCALE         AS Scale,
                              NULLABLE           AS IsNullable,
                              COMMENTS           AS ColumnDes,
                              CASE
                                WHEN F.COLUMN_NAME IS NULL
                                THEN 'N'
                                ELSE 'Y'
                              END AS ISPrimaryKey,
                              0 AS IsIdentity
                            FROM ALL_TAB_COLUMNS C
                              JOIN ALL_TABLES T
                                ON C.OWNER       = T.OWNER
                                AND C.TABLE_NAME = T.TABLE_NAME
                              LEFT JOIN ALL_COL_COMMENTS R
                                ON C.OWNER        = R.Owner
                                AND C.TABLE_NAME  = R.TABLE_NAME
                                AND C.COLUMN_NAME = R.COLUMN_NAME
                            LEFT JOIN
                                (SELECT C.OWNER,
                                  C.TABLE_NAME,
                                  D.POSITION,
                                  D.COLUMN_NAME
                                FROM ALL_CONSTRAINTS C
                                  JOIN ALL_CONS_COLUMNS D
                                   ON C.OWNER              = D.OWNER
                                   AND C.CONSTRAINT_NAME   = D.CONSTRAINT_NAME
                                WHERE C.CONSTRAINT_TYPE = 'P'
                              )F ON F.OWNER           = C.OWNER
                                  AND F.TABLE_NAME          = C.TABLE_NAME
                                  AND F.COLUMN_NAME         = C.COLUMN_NAME
                            WHERE  C.TABLE_NAME          = :TableName
                            ORDER BY C.TABLE_NAME,C.COLUMN_ID
                            ";

            #endregion Query

            OracleCommand oComm = new OracleCommand(query);
            oComm.Parameters.Add(new OracleParameter("TableName", tableID));

            return GetDataView(oComm);
        }

        #endregion 取得欄位清單

        #region 型別轉換

        /// <summary>
        /// 轉換資料型態Sql DataType =&gt; C# DataType 
        /// </summary>
        /// <param name="dbDataType"> Sql DataType </param>
        /// <param name="scale"> 小數點以後的位數 </param>
        /// <param name="precision"> 精準長度 </param>
        /// <returns></returns>
        public string GetDataType(string dbDataType, string scale = "", string precision = "")
        {
            var result = String.Empty;
            switch (dbDataType.ToUpper())
            {
                case "BFILE":
                case "BLOB":
                case "LONG RAW":
                case "RAW":
                    result = "byte[]";
                    break;

                case "CHAR":
                case "CLOB":
                case "NCHAR":
                case "NCLOB":
                case "NVARCHAR2":
                case "ROWID":
                case "VARCHAR2":
                    result = "string";
                    break;

                case "DATE":
                case "TIMESTAMP":
                    result = "DateTime";
                    break;

                case "FLOAT":
                case "REAL":
                    result = "double";
                    break;

                case "NUMBER":
                    if (!string.IsNullOrWhiteSpace(scale) && scale != "0")
                        result = "decimal";
                    else
                    {
                        var precN = Convert.ToInt32(precision);

                        if (precN < 9)
                            result = "int";
                        else
                            result = "long";
                    }
                    break;
            }

            return result;
        }

        #endregion 型別轉換

        #region 取得DataView

        private DataView GetDataView(OracleCommand oComm)
        {
            oConnection = new OracleConnection(connStr);
            oComm.Connection = oConnection;
            oComm.BindByName = true;
            using (OracleDataAdapter da = new OracleDataAdapter(oComm))
            {
                DataSet ds = new DataSet();
                try
                {
                    oComm.Connection = oConnection;
                    oConnection.Open();
                    da.Fill(ds);
                }
                catch (OracleException e)
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

        #endregion 取得DataView

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