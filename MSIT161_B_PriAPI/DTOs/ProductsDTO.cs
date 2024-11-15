using MSIT161_B_PriAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MSIT161_B_PriAPI.DTOs
{
	public class ProductsDTO
	{
		//會員ID
		public int FUserId { get; set; }
		//商品ID
		public int FProductId { get; set; }
		//商品名稱
		public string? FProductName { get; set; }
		//折舊狀況
		public string? FConditionsName { get; set; }
		//商品類別
		public string? FTagName { get; set; }
		//商品部位
		public string? FPartName { get; set; }
		//商品價格
		public decimal FOriginPrice { get; set; }
		//商品描述
		public string? FProductIllustrate { get; set; }
		//是否上架
		public bool? Fstate { get; set; }
		//因為一個產品會有多張圖所以要關聯其他DTO
		public List<ProductImageDTO>? FProductImages { get; set; }
		//一個商品多的商品明細
		public List<ProductDetailsDTO> FProductDetails { get; set; }

	}
}

