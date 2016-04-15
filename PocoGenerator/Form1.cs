using PocoGenerator.Extension;
using PocoGenerator.Model;
using PocoGenerator.PocoHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocoGenerator
{
    public partial class PocoFrom : Form
    {
        public PocoFrom()
        {
            InitializeComponent();

            var dbTypes = new DBTypes();
            dbTypes.Add(new DBType() { Name = "MSSQL" });
            dbTypes.Add(new DBType() { Name = "Oracle" });

            this.ddlDbType.DataSource = dbTypes;
            this.ddlDbType.DisplayMember = "Name";
            this.ddlDbType.ValueMember = "Name";
            this.ddlDbType.SelectedIndex = 0;

            //設定預設值
            tbxHost.Text = ConfigHelper.HostIp;
            tbxPort.Text = ConfigHelper.Port;
            tbxUserName.Text = ConfigHelper.UserId;
            tbxPwd.Text = ConfigHelper.UserPwd;
            tbxDBName.Text = ConfigHelper.DbName;
            tbxDBOwner.Text = ConfigHelper.DbOwner;
            cbxValidateByWindow.Checked = ConfigHelper.ValidateByWindow;
            ddlDbType.SelectedIndex = ConfigHelper.DbType;

            this.tbxPort.Enabled = false;
            this.tbxDBOwner.Enabled = false;
            this.cbxOriColumnName.Checked = true;
            this.rblReadMe.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReadMe.txt"), RichTextBoxStreamType.PlainText);
        }

        #region 控件連動事件

        private void ddlDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDbType.Text == "MSSQL")
            {
                tbxPort.Text = string.Empty;
                tbxPort.Enabled = false;
                tbxDBOwner.Text = string.Empty;
                tbxDBOwner.Enabled = false;
                cbxValidateByWindow.Enabled = true;
                lblDBName.Text = "DB名稱:";
            }
            else
            {
                tbxPort.Enabled = true;
                tbxDBOwner.Enabled = true;
                lblDBName.Text = "ServiceName:";
                cbxValidateByWindow.Checked = false;
                cbxValidateByWindow.Enabled = false;
            }
        }

        private void cbxValidateByWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxValidateByWindow.Checked)
            {
                tbxUserName.Text = tbxPwd.Text = string.Empty;
                tbxUserName.Enabled = tbxPwd.Enabled = false;
            }
            else
                tbxUserName.Enabled = tbxPwd.Enabled = true;
        }

        private void cbxSingleTable_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbxSingleTable.Checked)
            {
                this.cbxValidateAttr.Checked = true;
                this.cbxFormatErrorMsg.Checked = true;
            }
        }

        private void cbxFormatErrorMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxFormatErrorMsg.Checked)
                cbxValidateAttr.Checked = true;
        }

        private void cbxDTOMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxDTOMode.Checked)
            {
                this.cbxValidateAttr.Checked = false;
                this.cbxFormatErrorMsg.Checked = false;
                this.cbxValidateAttr.Enabled = false;
                this.cbxFormatErrorMsg.Enabled = false;
            }
            else
            {
                this.cbxValidateAttr.Checked = true;
                this.cbxFormatErrorMsg.Checked = true;
                this.cbxValidateAttr.Enabled = true;
                this.cbxFormatErrorMsg.Enabled = true;
            }
        }

        #endregion 控件連動事件

        #region 開啟連線

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                var dbInfo = GetDBInfo();
                var helper = new PocoHelpers().GetPocoHelper(ddlDbType.Text, dbInfo);
                gvTable.DataSource = helper.GetTableList();

                ConfigHelper.UpdateDbInfo(dbInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion 開啟連線

        #region Grid事件

        private void gvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var helper = new PocoHelpers().GetPocoHelper(ddlDbType.Text, GetDBInfo());

                string tableID = this.gvTable.Rows[e.RowIndex].Cells["QueryKey"].Value.ToString();
                gvColumn.DataSource = helper.GetColumnList(tableID);

                if (cbxSingleTable.Checked)
                {
                    tbxClassName.Text = this.gvTable.Rows[e.RowIndex].Cells["TableName"].Value.ToString();
                    btnAllDown_Click(sender, e);
                }
            }
        }

        private void gvColumn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                // 選取欄變成選取資料列 
                this.gvColumn.Rows[e.RowIndex].Selected = true;
                // 開始加入資料列 
                this.btnAdd_Click(sender, e);
            }
        }

        private void gvSelect_CellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                // 選取欄變成選取資料列 
                this.gvSelect.Rows[e.RowIndex].Selected = true;
                // 開始加入資料列 
                this.btnRemove_Click(sender, e);
            }
        }

        #endregion Grid事件

        #region Grid選取功能

        /// <summary>
        /// 單筆加入 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 選取欄變成選取資料列 
            foreach (DataGridViewCell gvcCell in this.gvColumn.SelectedCells)
            {
                this.gvColumn.Rows[gvcCell.RowIndex].Selected = true;
            }
            // 開始加入資料列 
            if (this.gvColumn.SelectedRows.Count > 0)
            {
                // 加入資料列 
                foreach (DataGridViewRow gvrRow in this.gvColumn.SelectedRows)
                {
                    // 加入選取的資料列資料 
                    this.gvSelect.Rows.Insert(0, new object[] {
                        gvrRow.Cells["ColumnName"].Value,
                        gvrRow.Cells["DataType"].Value,
                        gvrRow.Cells["Length"].Value,
                        gvrRow.Cells["IsNullable"].Value,
                        gvrRow.Cells["ISPrimaryKey"].Value,
                        gvrRow.Cells["ColumnDes"].Value,
                        gvrRow.Cells["Scale"].Value,
                        gvrRow.Cells["Prec"].Value,
                        gvrRow.Cells["IsIdentity"].Value
                    });
                }
            }
        }

        /// <summary>
        /// 整批加入 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllDown_Click(object sender, EventArgs e)
        {
            if (cbxSingleTable.Checked)
                btnAllRemove_Click(sender, e);

            // 選取欄變成選取資料列 
            foreach (DataGridViewRow gvrRow in this.gvColumn.Rows)
            {
                gvrRow.Selected = true;
            }
            // 開始加入資料列 
            this.btnAdd_Click(sender, e);
        }

        /// <summary>
        /// 單筆移除 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // 選取欄變成選取資料列 
            foreach (DataGridViewCell gvcCell in this.gvSelect.SelectedCells)
            {
                this.gvSelect.Rows[gvcCell.RowIndex].Selected = true;
            }
            // 開始加入資料列 
            if (this.gvSelect.SelectedRows.Count > 0)
            {
                for (int i = this.gvSelect.SelectedRows.Count; i > 0; i--)
                {
                    DataGridViewRow gvrRow = this.gvSelect.SelectedRows[i - 1];
                    // 移除加入的欄位 
                    this.gvSelect.Rows.Remove(gvrRow);
                }
            }
        }

        private void btnAllRemove_Click(object sender, EventArgs e)
        {
            this.gvSelect.Rows.Clear();
        }

        #endregion Grid選取功能

        #region 程式碼產生

        private void btnGenerator_Click(object sender, EventArgs e)
        {
            #region 資料驗證

            if (this.gvSelect.Rows.Count == 0)
            {
                MessageBox.Show("欄位列表最少需有一筆資料", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.tbxClassName.Text))
            {
                MessageBox.Show("請輸入ClassName", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion 資料驗證

            var pocoHelper = new PocoHelpers().GetPocoHelper(ddlDbType.Text);

            StringBuilder sb = new StringBuilder();

            string requiredTipStr = ConfigHelper.RequiredStr;
            string maxLengthTipStr = ConfigHelper.MaxLengthStr;

            if (cbxFormatErrorMsg.Checked)
            {
                requiredTipStr = "{0}" + requiredTipStr;
                maxLengthTipStr = "{0}" + maxLengthTipStr;
            }

            //載入Template
            this.rtbTemplete.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", "Template.Model.txt"), RichTextBoxStreamType.PlainText);

            #region Property 文字產生

            foreach (DataGridViewRow rowItem in this.gvSelect.Rows)
            {
                var dbColumnName = Convert.ToString(rowItem.Cells[0].Value);
                var dbType = Convert.ToString(rowItem.Cells[1].Value);
                var dbMaxLength = Convert.ToString(rowItem.Cells[2].Value);
                var dbNuable = Convert.ToString(rowItem.Cells[3].Value) == "Y" || Convert.ToString(rowItem.Cells[3].Value).ToUpper() == "TRUE";
                var dbDesc = Convert.ToString(rowItem.Cells[5].Value);
                var dbScale = Convert.ToString(rowItem.Cells[6].Value);
                var dbPrecision = Convert.ToString(rowItem.Cells[7].Value);
                var dbIsIdentity = Convert.ToBoolean(rowItem.Cells[8].Value);

                // 轉換資料型態對應 
                string dataType = pocoHelper.GetDataType(dbType, dbScale, dbPrecision);

                // Nullable 
                if (dbNuable && !new List<string>() { "object", "string", "byte[]" }.Contains(dataType))
                    dataType = String.Format("{0}?", dataType);

                //Summary
                sb.AppendLine("\t\t///<summary>");
                sb.AppendLine(string.Format("\t\t///{0}", (string.IsNullOrWhiteSpace(dbDesc) ? dbColumnName : dbDesc)));
                sb.AppendLine("\t\t///</summary>");

                //DB自動編號產生Remarks
                if (dbIsIdentity)
                    sb.AppendLine("\t\t///<remarks>Identity Specification Is Identity</remarks>");

                // Validate Attribute 
                if (this.cbxValidateAttr.Checked)
                {
                    if (dbColumnName.ToLower().Contains("url"))
                        sb.AppendLine("\t\t[Url]");

                    if (dbColumnName.ToLower().Contains("email"))
                        sb.AppendLine("\t\t[EmailAddress]");

                    if (dbColumnName.ToLower().Contains("datetime") || dbType.ToLower().Contains("datetime"))
                        sb.AppendLine("\t\t[DisplayFormat(DataFormatString = \"{0:yyyy/MM/dd HH:mm}\", ApplyFormatInEditMode = true)]");
                    else if (dbColumnName.ToLower().Contains("date") || dbType.ToLower().Contains("date"))
                        sb.AppendLine("\t\t[DisplayFormat(DataFormatString = \"{0:yyyy/MM/dd}\", ApplyFormatInEditMode = true)]");

                    if (!dbNuable && !dbIsIdentity)
                        sb.AppendLine(string.Format("\t\t[Required(ErrorMessage = \"{0}\")]", requiredTipStr));

                    if (!string.IsNullOrWhiteSpace(dbMaxLength) && dbMaxLength != "-1")
                        sb.AppendLine(string.Format("\t\t[StringLength({0}, ErrorMessage = \"{1}{0}。\")]",
                                                        dbMaxLength,
                                                        maxLengthTipStr));
                }

                //Property
                sb.AppendLine(string.Format("\t\tpublic {0} {1} {{ get; set; }}",
                                                dataType,
                                                (cbxOriColumnName.Checked ? dbColumnName : dbColumnName.FormatColumnName())
                                            ));

                sb.AppendLine("");
            }

            #endregion Property 文字產生

            this.rtbTemplete.Text = this.rtbTemplete.Text.Replace("<<className>>", tbxClassName.Text);
            this.rtbTemplete.Text = this.rtbTemplete.Text.Replace("<<propertyBlock>>", sb.ToString());

            #region 產出檔案

            if (cbxGenFile.Checked)
            {
                var basePath = string.IsNullOrWhiteSpace(tbxoutputPath.Text) ?
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OutputFile") :
                                tbxoutputPath.Text.Trim();

                var path = Path.Combine(basePath, tbxClassName.Text + ".cs");
                using (StreamWriter outfile = new StreamWriter(path))
                {
                    outfile.Write(this.rtbTemplete.Text);
                }
            }

            #endregion 產出檔案

            this.tabControl1.SelectedIndex = 1;
        }

        #endregion 程式碼產生

        #region 取DB物件

        private DBInfo GetDBInfo()
        {
            var dbInfo = new DBInfo()
            {
                DBType = ddlDbType.SelectedIndex,
                HostIP = tbxHost.Text.Trim(),
                Port = tbxPort.Text.Trim(),
                UserID = tbxUserName.Text.Trim(),
                UserPwd = tbxPwd.Text.Trim(),
                DBName = tbxDBName.Text.Trim(),
                ValidateByWindow = cbxValidateByWindow.Checked,
                DBOwner = tbxDBOwner.Text.Trim()
            };

            return dbInfo;
        }

        #endregion 取DB物件
    }
}