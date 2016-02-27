Poco 產生器
功能：
	●依據DB中針對欄位型態、描述等產生出Class
選項說明：
	●單一Table
		勾選時，當點選Table，自動帶單一Table所有資訊
	●驗證Attribute
		勾選時，會產生以下驗證Attribute
			■ 必填			=> 非Nullable欄位型態
			■ 最大長度		=> 有長度限制欄位
			■ Url			=> 欄位名稱中有Url字樣
			■ EmailAddress	=> 欄位名稱中有Email字樣
			■ DisplayFormat => 欄位名稱中有Date、Datetime字樣產生DataFormatString
	●Format ErrorMsg
		勾選時，錯誤訊息會套用MVC Model驗證可以帶Property名稱的Format
	●原始欄位名稱
		勾選時，Property會使用原始名稱。
		預設格式為：
			1.第一個字大寫，其餘小寫
			2.移除【_】
			3.遇到【_】後第一個字轉大寫
		可自行調整：StringExtension.cs => FormatColumnName
	●輸出檔案
		將Class匯出成.cs檔，存放在專案的OutputFile資料夾中

V1.1 增加
	● 驗證Attribute
	● MSSQL增加顯示是否為DB自動編號
	● 加入記住最後一次連線資訊功能