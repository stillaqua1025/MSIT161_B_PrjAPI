using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;
using Newtonsoft.Json;

using static System.Net.WebRequestMethods;

namespace MSIT161_B_PriAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]  // 確保所有操作需要通過 JWT 認證
	public class ProductsController : ControllerBase
	{
		private readonly dbMSTI161_B_ProjectContext _context;
		private readonly JwtService _jwtService;
		public ProductsController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
		}

		// GET: api/Products
		// 個人賣場商品列表
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TRtwproduct>>> GetTRtwproducts()
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			productsManagementFactory PMF = new productsManagementFactory(_context, _jwtService);
			var getProducts = await PMF.getProducts(fUserId);

			//var Products = await _context.TRtwproducts
			//             .Include(t => t.FConditions)
			//             .Include(t => t.FPart)
			//             .Include(t => t.FTag)
			//             .Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d=>d.FProductImage)
			//	.Where(p => p.FUserId == 1)
			//             .Select(p => new ProductsDTO
			//	{
			//                 FUserId = p.FUserId,
			//                 FProductId = p.FProductId,
			//                 FProductName = p.FProductName,
			//                 FConditionsName = p.FConditions.FConditionsName,
			//                 FPartName = p.FPart.FPartName,
			//                 FTagName = p.FTag.FTagName,
			//                 FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			//// 查詢tRTWProductImage的資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			//         // 根據某些條件 (如 FProductId) 合併 C 表的資料
			//         // 因為一個商品為多張圖片,所以必須把查出來的多個商品用foreach將多張圖片給塞進去單個商品
			//         foreach (var product in Products)
			//         {
			//             product.FProductImages = productImages
			//                 .Where(img => img.FProductId == product.FProductId) // 假設有 FProductId 來關聯
			//                 .Select(img => new ProductImageDTO
			//                 {
			//                     FProductImageId = img.FProductImageId,
			//                     FProductId = img.FProductId,
			//                     FProductImage = img.FProductImage
			//                 }).ToList();
			//         }

			return Ok(getProducts);
		}
		//首頁商品
		[HttpGet("Home")]
		public async Task<ActionResult<IEnumerable<TRtwproduct>>> GetHomeAllTRtwproducts()
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			productsManagementFactory PMF = new productsManagementFactory(_context, _jwtService);
			var getAllProducts = await PMF.getAllProducts();

			//var Products = await _context.TRtwproducts
			//             .Include(t => t.FConditions)
			//             .Include(t => t.FPart)
			//             .Include(t => t.FTag)
			//             .Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d=>d.FProductImage)
			//	.Where(p => p.FUserId == 1)
			//             .Select(p => new ProductsDTO
			//	{
			//                 FUserId = p.FUserId,
			//                 FProductId = p.FProductId,
			//                 FProductName = p.FProductName,
			//                 FConditionsName = p.FConditions.FConditionsName,
			//                 FPartName = p.FPart.FPartName,
			//                 FTagName = p.FTag.FTagName,
			//                 FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			//// 查詢tRTWProductImage的資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			//         // 根據某些條件 (如 FProductId) 合併 C 表的資料
			//         // 因為一個商品為多張圖片,所以必須把查出來的多個商品用foreach將多張圖片給塞進去單個商品
			//         foreach (var product in Products)
			//         {
			//             product.FProductImages = productImages
			//                 .Where(img => img.FProductId == product.FProductId) // 假設有 FProductId 來關聯
			//                 .Select(img => new ProductImageDTO
			//                 {
			//                     FProductImageId = img.FProductImageId,
			//                     FProductId = img.FProductId,
			//                     FProductImage = img.FProductImage
			//                 }).ToList();
			//         }

			return Ok(getAllProducts);
		}
		//首頁商品
		// 由於前端使用 HttpParams 傳遞查詢參數，後端建議使用 [FromQuery] 明確告知該參數來自查詢字符串。
		// 如果後端同時需要接收表單數據、路徑參數或文件等，使用 [FromQuery] 可以避免混淆，保證數據來源正確解析。
		[HttpGet("HomePage")]
		public async Task<ActionResult<IEnumerable<TRtwproduct>>> GetHomePageTRtwproducts([FromQuery] int pageNumber)
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			productsManagementFactory PMF = new productsManagementFactory(_context, _jwtService);
			var getAllProducts = await PMF.getPageProducts(pageNumber);

			if (getAllProducts.Count == 0)
			{
				return NoContent();
			}
			//var Products = await _context.TRtwproducts
			//             .Include(t => t.FConditions)
			//             .Include(t => t.FPart)
			//             .Include(t => t.FTag)
			//             .Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d=>d.FProductImage)
			//	.Where(p => p.FUserId == 1)
			//             .Select(p => new ProductsDTO
			//	{
			//                 FUserId = p.FUserId,
			//                 FProductId = p.FProductId,
			//                 FProductName = p.FProductName,
			//                 FConditionsName = p.FConditions.FConditionsName,
			//                 FPartName = p.FPart.FPartName,
			//                 FTagName = p.FTag.FTagName,
			//                 FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			//// 查詢tRTWProductImage的資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			//         // 根據某些條件 (如 FProductId) 合併 C 表的資料
			//         // 因為一個商品為多張圖片,所以必須把查出來的多個商品用foreach將多張圖片給塞進去單個商品
			//         foreach (var product in Products)
			//         {
			//             product.FProductImages = productImages
			//                 .Where(img => img.FProductId == product.FProductId) // 假設有 FProductId 來關聯
			//                 .Select(img => new ProductImageDTO
			//                 {
			//                     FProductImageId = img.FProductImageId,
			//                     FProductId = img.FProductId,
			//                     FProductImage = img.FProductImage
			//                 }).ToList();
			//         }

			return Ok(getAllProducts);
		}
		// GET: api/Products/5
		//商品頁面
		[HttpGet("{productId}")]
		public async Task<ActionResult<TRtwproduct>> GetTRtwproduct(int productId)
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			productsManagementFactory PMF = new productsManagementFactory(_context, _jwtService);
			var getProducts = await PMF.getProduct(fUserId, productId);

			//var Products = await _context.TRtwproducts
			//             .Include(t => t.FConditions)
			//             .Include(t => t.FPart)
			//             .Include(t => t.FTag)
			//             .Include(t => t.FUser)
			//	.Include(t => t.TRtwproductDetails)
			//	.ThenInclude(d=>d.FProductImage)
			//	.Where(p => p.FUserId == 1)
			//             .Select(p => new ProductsDTO
			//	{
			//                 FUserId = p.FUserId,
			//                 FProductId = p.FProductId,
			//                 FProductName = p.FProductName,
			//                 FConditionsName = p.FConditions.FConditionsName,
			//                 FPartName = p.FPart.FPartName,
			//                 FTagName = p.FTag.FTagName,
			//                 FOriginPrice = p.FOriginPrice
			//	}).ToListAsync();
			//// 查詢tRTWProductImage的資料
			//var productImages = await _context.TRtwproductImages.ToListAsync();

			//         // 根據某些條件 (如 FProductId) 合併 C 表的資料
			//         // 因為一個商品為多張圖片,所以必須把查出來的多個商品用foreach將多張圖片給塞進去單個商品
			//         foreach (var product in Products)
			//         {
			//             product.FProductImages = productImages
			//                 .Where(img => img.FProductId == product.FProductId) // 假設有 FProductId 來關聯
			//                 .Select(img => new ProductImageDTO
			//                 {
			//                     FProductImageId = img.FProductImageId,
			//                     FProductId = img.FProductId,
			//                     FProductImage = img.FProductImage
			//                 }).ToList();
			//         }

			return Ok(getProducts);
		}

		//個人賣場搜尋資料用
		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<ProductsDTO>>> SearchProducts(string? name, int? id)
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			productsManagementFactory PMF = new productsManagementFactory(_context, _jwtService);
			var searchProducts = await PMF.searchProduct(fUserId, name, id);
			return Ok(searchProducts);
		}
		// POST: api/Products
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//新增商品資料用
		[HttpPost("upProduct")]
		public async Task<ActionResult<TRtwproduct>> PostTRtwproduct(/*TRtwproduct tRtwproduct*/List<IFormFile> files, [FromForm] string products)
		{
			//_context.TRtwproducts.Add(tRtwproduct);
			//await _context.SaveChangesAsync();
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}


			// 反序列化 JSON 字串為 ProductsDTO 物件
			var productData = JsonConvert.DeserializeObject<ProductsDTO>(products);


			// 根據名稱查詢對應的 ID
			var condition = await _context.TConditions.FirstOrDefaultAsync(c => c.FConditionsName == productData.FConditionsName);
			var tag = await _context.TTags.FirstOrDefaultAsync(t => t.FTagName == productData.FTagName);
			var part = await _context.TParts.FirstOrDefaultAsync(p => p.FPartName == productData.FPartName);



			// 使用查詢到的 ID 建立新的商品物件
			var newProduct = new TRtwproduct
			{
				FUserId = fUserId /*productData.FUserId*/,
				FProductName = productData.FProductName,
				FTagId = tag.FTagId,                    // 保存查詢到的 ID
				FConditionsId = condition.FConditionsId,
				FPartId = part.FPartId,                 // 保存查詢到的 ID
				FOriginPrice = productData.FOriginPrice,
				FProductIllustrate = productData.FProductIllustrate,
				Fstate = productData.Fstate
			};



			// 保存商品資料至資料庫
			_context.TRtwproducts.Add(newProduct);
			await _context.SaveChangesAsync();
			// 獲取存完資料庫且自動生成的 FProductId
			var productId = newProduct.FProductId;


			//先建立一個TRtwproductDetail的集合容器來裝傳過來的productDetail集合
			//因為傳過來的productData已經經過反序列化並轉為productDTO的資料型態,才可以直接在C#使用
			//這邊使用foreach原因有二
			//一、FProductDetails裡面的顏色欄位等不是ID無法存進TRtwproductDetail
			//二、productData資料型態為ProductDetailsDTO無法存進TRtwproductDetails需先將欄位一一映射
			List<TRtwproductDetail> newProductDetail = new List<TRtwproductDetail>();
			foreach (var FProductDetail in productData.FProductDetails)
			{
				var FColorName = await _context.TColors.FirstOrDefaultAsync(c => c.FColorName == FProductDetail.FColorName);
				var FSizeName = await _context.TSizes.FirstOrDefaultAsync(c => c.FSizeName == FProductDetail.FSizeName);
				// 每次循環創建一個新的 TRtwproductImage 物件，並添加到 List 中
				newProductDetail.Add(new TRtwproductDetail
				{
					FProductId = productId,
					FColorId = FColorName.FColorId,
					FSizeId = FSizeName.FSizeId,
					FQty = FProductDetail.FQTY,
				});
			}
			_context.TRtwproductDetails.AddRange(newProductDetail);
			await _context.SaveChangesAsync();



			List<TRtwproductImage> newProductPics = new List<TRtwproductImage>();
			foreach (var file in files)
			{
				// 使用 FUserId + FProductId + 文件名稱 來命名文件
				var fileName = $"{newProduct.FUserId}-{productId}-{file.FileName}";

				// 文件保存的路徑,且名稱為自定義
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/productImages", fileName);

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				// 每次循環創建一個新的 TRtwproductImage 物件，並添加到 List 中
				newProductPics.Add(new TRtwproductImage
				{
					FProductId = productId,
					FProductImage = $"https://localhost:7187/uploads/productImages/{fileName}"
				});
			}

			//保存商品圖片至資料庫
			//使用AddRange意思是將newProductPics的集合一次保存進去資料庫
			//使用Add的話是一次只保存一筆
			_context.TRtwproductImages.AddRange(newProductPics);
			await _context.SaveChangesAsync();

			return Ok(new { message = "文件上傳成功" });
			//return CreatedAtAction("GetTRtwproduct", new { id = tRtwproduct.FProductId }, tRtwproduct);
		}

		//編輯單筆商品
		[HttpPost("{id}")]
		public async Task<IActionResult> PostTRtwproduct(/*TRtwproduct tRtwproduct*/List<IFormFile> files, [FromForm] string products, int id)
		{
			//_context.TRtwproducts.Add(tRtwproduct);
			//await _context.SaveChangesAsync();
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}


			// 反序列化 JSON 字串為 ProductsDTO 物件
			var productData = JsonConvert.DeserializeObject<ProductsDTO>(products);


			// 根據名稱查詢對應的 ID
			var condition = await _context.TConditions.FirstOrDefaultAsync(c => c.FConditionsName == productData.FConditionsName);
			var tag = await _context.TTags.FirstOrDefaultAsync(t => t.FTagName == productData.FTagName);
			var part = await _context.TParts.FirstOrDefaultAsync(p => p.FPartName == productData.FPartName);



			// 使用查詢到的 ID 建立新的商品物件
			var newProduct = new TRtwproduct
			{
				FProductId = id,
				FUserId = fUserId /*productData.FUserId*/,
				FProductName = productData.FProductName,
				FTagId = tag.FTagId,                    // 保存查詢到的 ID
				FConditionsId = condition.FConditionsId,
				FPartId = part.FPartId,                 // 保存查詢到的 ID
				FOriginPrice = productData.FOriginPrice,
				FProductIllustrate = productData.FProductIllustrate,
				Fstate = productData.Fstate
			};



			// 標記資料庫的商品資料還沒儲存
			_context.Entry(newProduct).State = EntityState.Modified;


			//先建立一個TRtwproductDetail的集合容器來裝傳過來的productDetail集合
			//因為傳過來的productData已經經過反序列化並轉為productDTO的資料型態,才可以直接在C#使用
			//這邊使用foreach原因有二
			//一、FProductDetails裡面的顏色欄位等不是ID無法存進TRtwproductDetail
			//二、productData資料型態為ProductDetailsDTO無法存進TRtwproductDetails需先將欄位一一映射
			List<TRtwproductDetail> newProductDetails = new List<TRtwproductDetail>();
			//先將資料庫顏色以及尺寸放入變數
			var allColors = await _context.TColors.ToListAsync();
			var allSizes = await _context.TSizes.ToListAsync();
			foreach (var FProductDetail in productData.FProductDetails)
			{
				//利用剛剛那的變數來查找顏色與尺寸的ID
				var FColor = allColors.FirstOrDefault(c => c.FColorName == FProductDetail.FColorName);
				var FSize = allSizes.FirstOrDefault(c => c.FSizeName == FProductDetail.FSizeName);
				newProductDetails.Add(new TRtwproductDetail
				{
					FProductDetailId = FProductDetail.FProductDetailId,
					FProductId = id,
					FColorId = FColor.FColorId,
					FSizeId = FSize.FSizeId,
					FQty = FProductDetail.FQTY,

				});
			}
			//因為丟進來資料有編輯現有productDetail與新增productDetail所以必須一筆一筆判斷並分開存
			//且編輯時也必須一筆一筆分開存,因為EF Core只接受一筆資料做編輯
			foreach (var newProductDetail in newProductDetails)
			{
				if (newProductDetail.FProductDetailId != 0)
				{
					_context.Entry(newProductDetail).State = EntityState.Modified;
				}
				else
				{
					_context.TRtwproductDetails.Add(newProductDetail);
				}

				await _context.SaveChangesAsync();
			}

            // 取得資料庫中目前商品的圖片列表
            var existingPics = await _context.TRtwproductImages
                .Where(img => img.FProductId == id)
                .ToListAsync();

            List<TRtwproductImage> newProductPics = new List<TRtwproductImage>();
			foreach (var file in files)
			{
				// 使用 FUserId + FProductId + 文件名稱 來命名文件
				var fileName = $"{newProduct.FUserId}-{id}-{file.FileName}";

				// 文件保存的路徑,且名稱為自定義
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/productImages", fileName);

				if (!System.IO.File.Exists(path))
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					// 每次循環創建一個新的 TRtwproductImage 物件，並添加到 List 中
					newProductPics.Add(new TRtwproductImage
					{
						FProductId = id,
						FProductImage = $"https://localhost:7187/uploads/productImages/{fileName}"
					});
				}

			}

            // 檢查資料庫現有的圖片是否需要刪除
            foreach (var existingPic in existingPics)
            {
                // 如果現有圖片不在新的圖片列表中，則刪除
                if (!productData.FProductImages.Any(np => np.FProductImage == existingPic.FProductImage))
                {
                    // 刪除圖片檔案
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/productImages", Path.GetFileName(existingPic.FProductImage));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // 從資料庫中刪除圖片
                    _context.TRtwproductImages.Remove(existingPic);
                }
				continue;
            }



            //_context.Entry(newProductPics).State = EntityState.Modified;
            // 確保 newProductPics 有資料再進行資料庫操作
            if (newProductPics.Count > 0)
			{
				_context.TRtwproductImages.AddRange(newProductPics); // 批量添加到資料庫
				await _context.SaveChangesAsync(); // 保存變更
			}
			await _context.SaveChangesAsync();

			return Ok(new { message = "文件上傳成功" });
			//return CreatedAtAction("GetTRtwproduct", new { id = tRtwproduct.FProductId }, tRtwproduct);
		}

		//下架
		// PUT: api/TRtwproducts/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchTRtwproduct(int id, [FromBody] ProductsDTO updateData)
		{
			// 使用 JwtService 來獲取 fUserId
			var result = _jwtService.GetfUserIDfromJWT(out int fUserId);
			if (result != null)
			{
				// 如果有錯誤，根據具體情況返回適當的 ActionResult
				if (result is UnauthorizedResult)
				{
					return Unauthorized(); // 返回 401 未授權
				}
				else if (result is BadRequestObjectResult)
				{
					return BadRequest(); // 返回 400 錯誤
				}
			}
			var product = await _context.TRtwproducts.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			// 更新 fstate
			if (updateData.Fstate.HasValue)
			{
				product.Fstate = updateData.Fstate;
			}

			// 保存更改
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: api/Products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTRtwproduct(int id)
		{
			var tRtwproduct = await _context.TRtwproducts.FindAsync(id);
			if (tRtwproduct == null)
			{
				return NotFound();
			}

			_context.TRtwproducts.Remove(tRtwproduct);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool TRtwproductExists(int id)
		{
			return _context.TRtwproducts.Any(e => e.FProductId == id);
		}
	}
}
