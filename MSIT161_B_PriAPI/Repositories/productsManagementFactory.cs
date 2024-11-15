using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using System.Drawing.Printing;

namespace MSIT161_B_PriAPI.Repositories
{
	public class productsManagementFactory
	{
		private readonly dbMSTI161_B_ProjectContext _context;
		private readonly JwtService _jwtService;

		public productsManagementFactory(dbMSTI161_B_ProjectContext context, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
		}
		//商品首頁搜尋所有商品,查到商品是多筆,要用List
		//只有上架的會顯示
		public async Task<List<ProductsDTO>> getAllProducts()
		{
			//// 查詢商品資料並轉換為 DTO
			//var products = await _context.TRtwproducts
			//	.Include(t => t.FConditions)
			//	.Include(t => t.FPart)
			//	.Include(t => t.FTag)
			//	.Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d => d.FProductImage)
			//	.Where(p => p.FUserId == userId)
			//	.Select(p => new ProductsDTO
			//	{
			//		FUserId = p.FUserId,
			//		FProductId = p.FProductId,
			//		FProductName = p.FProductName,
			//		FConditionsName = p.FConditions.FConditionsName,
			//		FPartName = p.FPart.FPartName,
			//		FTagName = p.FTag.FTagName,
			//		FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			var products = await _context.TRtwproducts
			.Include(t => t.FConditions)
			.Include(t => t.FPart)
			.Include(t => t.FTag)
			.Include(t => t.FUser)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FColor)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FSize)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FProductImage)
			.Where(t=>t.Fstate==true)
			.Select(p => new ProductsDTO
			{
				FUserId = p.FUserId,
				FProductId = p.FProductId,
				FProductName = p.FProductName,
				FConditionsName = p.FConditions.FConditionsName,
				FPartName = p.FPart.FPartName,
				FTagName = p.FTag.FTagName,
				FOriginPrice = p.FOriginPrice,
				Fstate = p.Fstate,
				FProductIllustrate = p.FProductIllustrate,
				FProductDetails = p.TRtwproductDetails
				.Select(details => new ProductDetailsDTO // 映射所有的詳細資料
				{
					firstProductDetailImage = details.FProductImage.FProductImage,
					FProductId = p.FProductId,
					FProductDetailId = details.FProductDetailId,
					FColorName = details.FColor.FColorName, // 產品顏色名稱
					FSizeName = details.FSize.FSizeName, // 產品尺寸名稱
					FQTY = details.FQty
				}).ToList() // 把詳細資料轉換成列表
			}).ToListAsync(); // 使用 FirstOrDefaultAsync 返回單一商品


			// 查詢產品圖片資料
			var productImages = await _context.TRtwproductImages.ToListAsync();

			// 將圖片資料與產品關聯
			foreach (var product in products)
			{
				product.FProductImages = productImages
					.Where(img => img.FProductId == product.FProductId)
					.Select(img => new ProductImageDTO
					{
						FProductImageId = img.FProductImageId,
						FProductId = img.FProductId,
						FProductImage = img.FProductImage
					}).ToList();
			}

			return products;
		}
		//商品首頁所有商品並且只顯示前20筆,要用List
		//只有上架的會顯示
		public async Task<List<ProductsDTO>> getPageProducts(int page)
		{
			var products = await _context.TRtwproducts
			.Include(t => t.FConditions)
			.Include(t => t.FPart)
			.Include(t => t.FTag)
			.Include(t => t.FUser)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FColor)
			.Include(t => t.TRtwproductDetails)
				.ThenInclude(t => t.FSize)
			.Include(t => t.TRtwproductDetails)
				.ThenInclude(t => t.FProductImage)
			.Where(t => t.Fstate == true)
			.OrderByDescending(t => t.FProductId) // 根據 FProductId 由大到小排序
			.Skip((page-1)*8)                     // 跳過幾筆資料
			.Take(8)                              // 取得跳過之後的幾筆資料
			.Select(p => new ProductsDTO
			{
				FUserId = p.FUserId,
				FProductId = p.FProductId,
				FProductName = p.FProductName,
				FConditionsName = p.FConditions.FConditionsName,
				FPartName = p.FPart.FPartName,
				FTagName = p.FTag.FTagName,
				FOriginPrice = p.FOriginPrice,
				Fstate = p.Fstate,
				FProductIllustrate = p.FProductIllustrate,
				FProductDetails = p.TRtwproductDetails
				.Select(details => new ProductDetailsDTO // 映射所有的詳細資料
				{
					firstProductDetailImage = details.FProductImage.FProductImage,
					FProductId = p.FProductId,
					FProductDetailId = details.FProductDetailId,
					FColorName = details.FColor.FColorName, // 產品顏色名稱
					FSizeName = details.FSize.FSizeName, // 產品尺寸名稱
					FQTY = details.FQty
				}).ToList() // 把詳細資料轉換成列表
			}).ToListAsync(); // 使用 FirstOrDefaultAsync 返回單一商品
			

			// 查詢產品圖片資料
			var productImages = await _context.TRtwproductImages.ToListAsync();

			// 將圖片資料與產品關聯
			foreach (var product in products)
			{
				product.FProductImages = productImages
					.Where(img => img.FProductId == product.FProductId)
					.Select(img => new ProductImageDTO
					{
						FProductImageId = img.FProductImageId,
						FProductId = img.FProductId,
						FProductImage = img.FProductImage
					}).ToList();
			}

			return products;
		}
		//根據會員ID搜尋,查到商品是多筆,要用List
		public async Task<List<ProductsDTO>> getProducts(int userId)
		{
			//// 查詢商品資料並轉換為 DTO
			//var products = await _context.TRtwproducts
			//	.Include(t => t.FConditions)
			//	.Include(t => t.FPart)
			//	.Include(t => t.FTag)
			//	.Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d => d.FProductImage)
			//	.Where(p => p.FUserId == userId)
			//	.Select(p => new ProductsDTO
			//	{
			//		FUserId = p.FUserId,
			//		FProductId = p.FProductId,
			//		FProductName = p.FProductName,
			//		FConditionsName = p.FConditions.FConditionsName,
			//		FPartName = p.FPart.FPartName,
			//		FTagName = p.FTag.FTagName,
			//		FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			var products = await _context.TRtwproducts
			.Include(t => t.FConditions)
			.Include(t => t.FPart)
			.Include(t => t.FTag)
			.Include(t => t.FUser)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FColor)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FSize)
			.Include(t => t.TRtwproductDetails)
			.ThenInclude(t => t.FProductImage)
			.Where(p => p.FUserId == userId)
			.Select(p => new ProductsDTO
			{
				FUserId = p.FUserId,
				FProductId = p.FProductId,
				FProductName = p.FProductName,
				FConditionsName = p.FConditions.FConditionsName,
				FPartName = p.FPart.FPartName,
				FTagName = p.FTag.FTagName,
				FOriginPrice = p.FOriginPrice,
				Fstate = p.Fstate,
				FProductIllustrate = p.FProductIllustrate,
				FProductDetails = p.TRtwproductDetails
				.Select(details => new ProductDetailsDTO // 映射所有的詳細資料
				{
					firstProductDetailImage = details.FProductImage.FProductImage,
					FProductId = p.FProductId,
					FProductDetailId = details.FProductDetailId,
					FColorName = details.FColor.FColorName, // 產品顏色名稱
					FSizeName = details.FSize.FSizeName, // 產品尺寸名稱
					FQTY = details.FQty
				}).ToList() // 把詳細資料轉換成列表
			}).ToListAsync(); // 使用 FirstOrDefaultAsync 返回單一商品


			// 查詢產品圖片資料
			var productImages = await _context.TRtwproductImages.ToListAsync();

			// 將圖片資料與產品關聯
			foreach (var product in products)
			{
				product.FProductImages = productImages
					.Where(img => img.FProductId == product.FProductId)
					.Select(img => new ProductImageDTO
					{
						FProductImageId = img.FProductImageId,
						FProductId = img.FProductId,
						FProductImage = img.FProductImage
					}).ToList();
			}

			return products;
		}
		//根據會員ID搜尋,查到商品只會有一筆.不要用List
		public async Task<ProductsDTO> getProduct(int userId, int productId)
		{
			// 查詢商品資料並轉換為 DTO
		
			var product = await _context.TRtwproducts
				.Include(t => t.FConditions)
				.Include(t => t.FPart)
				.Include(t => t.FTag)
				.Include(t => t.FUser)
				.Include(t => t.TRtwproductDetails)
				.ThenInclude(t => t.FColor)
				.Include(t => t.TRtwproductDetails)
				.ThenInclude(t => t.FSize)
				.Include(t => t.TRtwproductDetails)
				.ThenInclude(t => t.FProductImage)
				.Where(p => p.FProductId== productId)
                //p.FUserId == userId&&
                .Select(p => new ProductsDTO
				{
					FUserId = p.FUserId,
					FProductId = p.FProductId,
					FProductName = p.FProductName,
					FConditionsName = p.FConditions.FConditionsName,
					FPartName = p.FPart.FPartName,
					FTagName = p.FTag.FTagName,
					FOriginPrice = p.FOriginPrice,
					FProductIllustrate = p.FProductIllustrate,
					FProductDetails = p.TRtwproductDetails
					.Select(details => new ProductDetailsDTO // 映射所有的詳細資料
					{
						firstProductDetailImage = details.FProductImage.FProductImage,
						FProductId = p.FProductId,
						FProductDetailId = details.FProductDetailId,
						FColorName = details.FColor.FColorName, // 產品顏色名稱
						FSizeName = details.FSize.FSizeName, // 產品尺寸名稱
						FQTY = details.FQty
					}).ToList() // 把詳細資料轉換成列表
				}).FirstOrDefaultAsync(); // 使用 FirstOrDefaultAsync 返回單一商品
			if (product == null)
			{
				return null; // 如果未找到商品則返回 null
			}
			// 查詢產品圖片資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			// 將圖片資料與產品關聯
			
				product.FProductImages = await _context.TRtwproductImages
					.Where(img => img.FProductId == productId)
					.Select(img => new ProductImageDTO
					{
						FProductImageId = img.FProductImageId,
						FProductId = img.FProductId,
						FProductImage = img.FProductImage
					}).ToListAsync();

			return product;
		}
		//搜尋商品
		public async Task<List<ProductsDTO>> searchProduct(int userId, string? name, int? id)
		{
			var query = _context.TRtwproducts.AsQueryable();  //如果不加AsQueryable,資料表示無法使用LINQ查詢的

			//當id不等於空值時,將關聯表都加進來
			if (id.HasValue)
			{
				query = _context.TRtwproducts.Where(p => p.FUserId == userId && p.FProductId == id.Value)
			   .Include(t => t.FConditions)
			   .Include(t => t.FPart)
			   .Include(t => t.FTag)
			   .Include(t => t.FUser);
			}
			//當name不等於空值時,將關聯表都加進來
			else if (!string.IsNullOrEmpty(name))
			{
				query = query.Where(p => p.FUserId == userId && p.FProductName.Contains(name))
				.Include(t => t.FConditions)
				.Include(t => t.FPart)
				.Include(t => t.FTag)
				.Include(t => t.FUser);
			}
			//將查詢出的結果注入到ProductsDTO並映射欄位
			var products = await query.Select(p => new ProductsDTO
			{
				FUserId = p.FUserId,
				FProductId = p.FProductId,
				FProductName = p.FProductName,
				FConditionsName = p.FConditions.FConditionsName,
				FPartName = p.FPart.FPartName,
				FTagName = p.FTag.FTagName,
				FOriginPrice = p.FOriginPrice
			}).ToListAsync();
			var productImages = await _context.TRtwproductImages.ToListAsync();

			// 將圖片資料與產品關聯
			// 因為一個商品為多張圖片,所以必須把查出來的多個商品用foreach將多張圖片給塞進去單個商品
			foreach (var product in products)
			{
				product.FProductImages = productImages
					.Where(img => img.FProductId == product.FProductId)
					.Select(img => new ProductImageDTO
					{
						FProductImageId = img.FProductImageId,
						FProductId = img.FProductId,
						FProductImage = img.FProductImage
					}).ToList();
			}

			//// 查詢商品資料並轉換為 DTO
			//var products = await _context.TRtwproducts
			//.Include(t => t.FConditions)
			//.Include(t => t.FPart)
			//.Include(t => t.FTag)
			//.Include(t => t.FUser)
			//.Include(t => t.TRtwproductDetails)
			//.ThenInclude(d => d.FProductImage)
			//.Where(p => p.FUserId == userId && p.FProductName.Contains(name))
			//.Select(p => new ProductsDTO
			//{
			//	FUserId = p.FUserId,
			//	FProductId = p.FProductId,
			//	FProductName = p.FProductName,
			//	FConditionsName = p.FConditions.FConditionsName,
			//	FPartName = p.FPart.FPartName,
			//	FTagName = p.FTag.FTagName,
			//	FOriginPrice = p.FOriginPrice
			//}).ToListAsync();

			//// 查詢產品圖片資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			//// 將圖片資料與產品關聯
			//foreach (var product in products)
			//{
			//	product.FProductImages = productImages
			//		.Where(img => img.FProductId == product.FProductId)
			//		.Select(img => new ProductImageDTO
			//		{
			//			FProductImageId = img.FProductImageId,
			//			FProductId = img.FProductId,
			//			FProductImage = img.FProductImage
			//		}).ToList();
			//}
			//return products;


			return products;
		}
	}

}
