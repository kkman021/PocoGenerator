Poco 產生器
功能：
	●依據DB中針對欄位型態、描述等產生出Class

參數說明：
	●單一Table
		勾選時，當點選Table，自動帶ClassName、Column List
	●驗證Attribute
		勾選時，會產生（必填＆最大長度）的驗證Attribute
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