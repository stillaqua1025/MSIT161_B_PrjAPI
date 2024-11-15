namespace MSIT161_B_PriAPI.DTOs
{
	public class ProductImageDTO
	{
		//圖片編號
		public int FProductImageId { get; set; }
		//商品ID
		public int? FProductId { get; set; }
		//商品圖片路徑
		public string? FProductImage { get; set; }
	}
}