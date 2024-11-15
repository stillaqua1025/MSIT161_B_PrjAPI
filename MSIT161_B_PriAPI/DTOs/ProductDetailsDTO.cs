namespace MSIT161_B_PriAPI.DTOs
{
	public class ProductDetailsDTO
	{
		public int FProductId { get; set; }
		public int FProductDetailId { get; set; }
		public string FColorName { get; set; }
		public string FSizeName { get; set; }
		public int FQTY { get; set; }
		//因為一個產品會有多張圖所以要關聯其他DTO
		public string? firstProductDetailImage { get; set; }
	}
}