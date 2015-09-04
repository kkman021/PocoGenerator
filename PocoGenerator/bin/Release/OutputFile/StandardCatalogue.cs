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
	/// StandardCatalogue類別。
	/// </summary>
	public class StandardCatalogueModel
	{
		///<summary>
		///Id
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "Id")]
		public int Id { get; set; }

		///<summary>
		///ParentId
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "ParentId")]
		public int ParentId { get; set; }

		///<summary>
		///CatalogueId
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[StringLength(10, ErrorMessage = "{0}長度不可超過10。")]
		[Display(Name = "CatalogueId")]
		public string CatalogueId { get; set; }

		///<summary>
		///EngName
		///<summary>
		[StringLength(50, ErrorMessage = "{0}長度不可超過50。")]
		[Display(Name = "EngName")]
		public string EngName { get; set; }

		///<summary>
		///ChtName
		///<summary>
		[StringLength(50, ErrorMessage = "{0}長度不可超過50。")]
		[Display(Name = "ChtName")]
		public string ChtName { get; set; }

		///<summary>
		///Sort
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "Sort")]
		public int Sort { get; set; }

		///<summary>
		///IsDelete
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "IsDelete")]
		public bool IsDelete { get; set; }

		///<summary>
		///CreatedBy
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "CreatedBy")]
		public int CreatedBy { get; set; }

		///<summary>
		///CreateOnUtc
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "CreateOnUtc")]
		public DateTime CreateOnUtc { get; set; }

		///<summary>
		///ModifiedBy
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "ModifiedBy")]
		public int ModifiedBy { get; set; }

		///<summary>
		///ModifiedOnUtc
		///<summary>
		[Required(ErrorMessage = "{0}欄位必填")]
		[Display(Name = "ModifiedOnUtc")]
		public DateTime ModifiedOnUtc { get; set; }


	}
}