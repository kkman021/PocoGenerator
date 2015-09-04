using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model
{
	/// <summary>
	/// AC_ADJ_DETAIL類別。
	/// </summary>
	public class AC_ADJ_DETAILModel
	{
		///<summary>
		///調帳單號(站所代碼4碼 + YYMM + AJ＋4碼流水號)
		///<summary>
		[Display(Name = "調帳單號(站所代碼4碼 + YYMM + AJ＋4碼流水號)")]
		public string ADJ_NO { get; set; }

		///<summary>
		///客戶換號(0:未更換客戶／1:原客戶／2:新客戶)
		///<summary>
		[Display(Name = "客戶換號(0:未更換客戶／1:原客戶／2:新客戶)")]
		public string CUST_C { get; set; }

		///<summary>
		///調帳方式(0:單筆／1:整批)
		///<summary>
		[Display(Name = "調帳方式(0:單筆／1:整批)")]
		public string ADJ_KIND { get; set; }

		///<summary>
		///託運單號碼/其它收入(1.單筆調帳時,記錄託運單號bill_id/其它收入單號or_id,2.整批調帳時,為null,3.託運單號12碼,其它收入代號13碼)
		///<summary>
		[Display(Name = "託運單號碼/其它收入(1.單筆調帳時,記錄託運單號bill_id/其它收入單號or_id,2.整批調帳時,為null,3.託運單號12碼,其它收入代號13碼)")]
		public string BILL_ID { get; set; }

		///<summary>
		///調帳日期(可讓使用者修改.單筆調整時,須介於集貨日期~apldate.整批調整時,須介於2013/01/01(含)~apldate)
		///<summary>
		[Display(Name = "調帳日期(可讓使用者修改.單筆調整時,須介於集貨日期~apldate.整批調整時,須介於2013/01/01(含)~apldate)")]
		public DateTime ADJ_DATE { get; set; }

		///<summary>
		///原始集貨日期(一般單:集貨日,到付單:配完日,其他收入:入帳日期,整批:apldate)
		///<summary>
		[Display(Name = "原始集貨日期(一般單:集貨日,到付單:配完日,其他收入:入帳日期,整批:apldate)")]
		public DateTime? COL_DATE { get; set; }

		///<summary>
		///調帳單位(一般單:集貨所,到付單:配完所,其他收入:入帳所,整批:客代的[發票所])
		///<summary>
		[Display(Name = "調帳單位(一般單:集貨所,到付單:配完所,其他收入:入帳所,整批:客代的[發票所])")]
		public string ADJ_ORG_ID { get; set; }

		///<summary>
		///營業區單位代號(記錄營業區代號,一般單:集貨所營業區,到付單:配完所營業區,其他收入:入帳所營業區,整批:客代的[發票所]營業區)
		///<summary>
		[Display(Name = "營業區單位代號(記錄營業區代號,一般單:集貨所營業區,到付單:配完所營業區,其他收入:入帳所營業區,整批:客代的[發票所]營業區)")]
		public string ADJ_ZONE_ID { get; set; }

		///<summary>
		///G組單位代號(記錄G組代號,如226001,一般單:集貨所G組,到付單:配完所G組,其它收入:入帳所,整批:客代的[發票所])
		///<summary>
		[Display(Name = "G組單位代號(記錄G組代號,如226001,一般單:集貨所G組,到付單:配完所G組,其它收入:入帳所,整批:客代的[發票所])")]
		public string ADJ_G_ID { get; set; }

		///<summary>
		///集貨SD/調帳人員
		///<summary>
		[Display(Name = "集貨SD/調帳人員")]
		public string EMP_ID { get; set; }

		///<summary>
		///客戶代號(母客代)
		///<summary>
		[Display(Name = "客戶代號(母客代)")]
		public string CUST_NO { get; set; }

		///<summary>
		///調整運費
		///<summary>
		[Display(Name = "調整運費")]
		public double ADJ_TRAN_CHARGE { get; set; }

		///<summary>
		///原始運費
		///<summary>
		[Display(Name = "原始運費")]
		public double OLD_TRAN_CHARGE { get; set; }

		///<summary>
		///調整後運費
		///<summary>
		[Display(Name = "調整後運費")]
		public double NEW_TRAN_CHARGE { get; set; }

		///<summary>
		///調帳狀態(0:未處理的調帳／1:補開發票(意即開立發票，與銷項發票的補開意義不同)／2：折讓)
		///<summary>
		[Display(Name = "調帳狀態(0:未處理的調帳／1:補開發票(意即開立發票，與銷項發票的補開意義不同)／2：折讓)")]
		public string ADJ_STATUS { get; set; }

		///<summary>
		///調帳原因
		///<summary>
		[Display(Name = "調帳原因")]
		public string ADJ_DESC { get; set; }

		///<summary>
		///備註
		///<summary>
		[Display(Name = "備註")]
		public string ADJ_MEMO { get; set; }

		///<summary>
		///折讓單號
		///<summary>
		[Display(Name = "折讓單號")]
		public string REPAY_NO { get; set; }

		///<summary>
		///發票號碼
		///<summary>
		[Display(Name = "發票號碼")]
		public string INVOICE_NO { get; set; }

		///<summary>
		///結帳方式(Y:現結,N:月結)
		///<summary>
		[Display(Name = "結帳方式(Y:現結,N:月結)")]
		public string IS_CASH { get; set; }

		///<summary>
		///託運單類別(區分01.一般、02.到付、03.混單、04.客樂得)
		///<summary>
		[Display(Name = "託運單類別(區分01.一般、02.到付、03.混單、04.客樂得)")]
		public string BILL_TYPE { get; set; }

		///<summary>
		///應收金額
		///<summary>
		[Display(Name = "應收金額")]
		public double? COL_PRICE { get; set; }

		///<summary>
		///資料類型(1:一般單,2:到付單,3:其它收入,null:整批調整)
		///<summary>
		[Display(Name = "資料類型(1:一般單,2:到付單,3:其它收入,null:整批調整)")]
		public string DATA_KIND { get; set; }

		///<summary>
		///建立日期
		///<summary>
		[Display(Name = "建立日期")]
		public DateTime CREATE_DATETIME { get; set; }

		///<summary>
		///建立單位
		///<summary>
		[Display(Name = "建立單位")]
		public string CREATE_ORG { get; set; }

		///<summary>
		///建立人員
		///<summary>
		[Display(Name = "建立人員")]
		public string CREATE_EMP { get; set; }

		///<summary>
		///最後異動日期
		///<summary>
		[Display(Name = "最後異動日期")]
		public DateTime UPDATE_DATETIME { get; set; }

		///<summary>
		///異動單位
		///<summary>
		[Display(Name = "異動單位")]
		public string UPDATE_ORG { get; set; }

		///<summary>
		///異動人員
		///<summary>
		[Display(Name = "異動人員")]
		public string UPDATE_EMP { get; set; }


	}
}