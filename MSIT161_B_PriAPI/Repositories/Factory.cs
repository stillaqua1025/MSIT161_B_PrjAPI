using Microsoft.EntityFrameworkCore;
using MSIT161_B_PriAPI.DTOs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;

namespace MSIT161_B_PriAPI.Repositories
{
    public class Factory
    {
        private readonly dbMSTI161_B_ProjectContext _context;
		private readonly JwtService _jwtService;
		public Factory(dbMSTI161_B_ProjectContext context, JwtService jwtService) 
        {
            _context = context;
			_jwtService = jwtService;
		}
        public IQueryable<OrdersDTO> getUserOders(int userId) 
        {

            var orders = _context.TRtworders.Where(o => o.FUserId == userId)
                .Select(o => new OrdersDTO
                {
                    number = o.FOrderNumber,
                    time = o.FTime,
                    price = Convert.ToInt32(o.FPrice),
                    state = o.FFinished? "已完成" : "已付款"
                });
            return orders;
        }
        public IQueryable<OrderInfoDTO> getOrderInfo(int orderId)
        {
            var info = _context.TRtworders
                .Include(o => o.FShipState)
                .Where(o => o.FOrderId == orderId)
                .Select(o => new OrderInfoDTO
                {
                    time = o.FTime,
                    finished = o.FFinished? "已完成" : "已付款",
                    finishedTime = o.FFinishedTime,
                    shipState = o.FShipState.FShipState,
                    price = Convert.ToInt32(o.FPrice)
                });
            return info;
        }

        public IQueryable<OrderDetailDTO> getOrderDetail(int orderId)
        {
            var detail = _context.TRtworderDetails
                .Include(s => s.FProductDetail)
                .Include(s => s.FProductDetail.FProduct)
                .Include(s => s.FProductDetail.FProductImage)
                .Include(s => s.FProductDetail.FColor)
                .Include(s => s.FProductDetail.FSize)
                .Where(o => o.FOrderId == orderId)
                .Select(s => new OrderDetailDTO
                {
                    name = $"{s.FProductDetail.FProduct.FProductName}/{s.FProductDetail.FColor.FColorName}/{s.FProductDetail.FSize.FSizeName}", //商品名稱+顏色+尺寸
                    price = Convert.ToInt32(s.FProductDetail.FProduct.FOriginPrice),
                    quantity = s.FQty,
                    imageUrl = s.FProductDetail.FProductImage.FProductImage
                });
            return detail;
        }
        public async Task<List<TRtworderDetail>> InsertOrderDetail(int userId,int orderId)
        {
            var detail = await _context.TRtwshoppingCarts
                .Include(s => s.FProductDetail)
                .Include(s => s.FProductDetail.FProduct)
                .Select(s => new TRtworderDetail
                {
                    FOrderId = orderId,
                    FProductDetailId = s.FProductDetailId,
                    FQty = s.FQty,
                    FPrice = Convert.ToInt32(s.FProductDetail.FProduct.FOriginPrice),
                }).ToListAsync();

            return detail;
        }

        public TRtworder InsertOders(CheckOutDTO dto,int userId)
        {
            string currentTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            TRtworder order = new TRtworder();
            order.FOrderNumber = userId.ToString() + currentTimeString;
            order.FUserId = userId;
            order.FCouponCodeId = dto.couponId;
            order.FPrice = dto.price;
            order.FTime = DateTime.Now;
            order.FShipMethodId = 2;
            order.FPayMethodId = 4;
            order.FAddress = dto.address;
            order.FShipStateId = 1;
            order.FFinished = false;

            return order;
        }

        public TRtwshoppingCart InsertShoppingCart(AddCartDTO dto, int userId)
        {
            TRtwshoppingCart cart = new TRtwshoppingCart();
            cart.FUserId = userId;
            cart.FProductDetailId = dto.productid;
            cart.FQty = dto.qty;
            return cart;
        }

        public IQueryable<ShoppingCartDTO> getUserShoppingCart(int userId)
        {
            var cart = _context.TRtwshoppingCarts
                .Include(s => s.FProductDetail)
                .Include(s => s.FProductDetail.FProduct)
                .Include(s => s.FProductDetail.FProductImage)
                .Include(s => s.FProductDetail.FColor)
                .Include(s => s.FProductDetail.FSize)
                .Where(s => s.FUserId == userId)
                .Select(s => new ShoppingCartDTO
                {
                    scid = s.FShoppingCartId,
                    pdid = s.FProductDetailId,
                    name = $"{s.FProductDetail.FProduct.FProductName}/{s.FProductDetail.FColor.FColorName}/{s.FProductDetail.FSize.FSizeName}", //商品名稱+顏色+尺寸
                    price = Convert.ToInt32(s.FProductDetail.FProduct.FOriginPrice),
                    quantity = s.FQty,
                    imageUrl = s.FProductDetail.FProductImage.FProductImage
                });
            return cart;
        }

        public async Task<List<TRtwshoppingCart>> deleteUserShoppingCarts(int userId)
        {
            var shoppingCarts = await _context.TRtwshoppingCarts
                .Where(c => c.FUserId == userId).ToListAsync();
            return shoppingCarts;
        }

        public IQueryable<CouponDTO> getCoupons(int userid, int? fCoupon_from = null)
        {
            var coupons = _context.TCouponRedemptions.Where(c => c.FUserId == userid)
                .Select(c => new CouponDTO
                {
                    FCouponCodeId = c.FCouponCodeId,
                    FCouponCodeName = c.FCouponCode.FCouponCodeName,
                    CouponDescription = c.FCouponCode.CouponDescription,
                    FCouponCode = c.FCouponCode.FCouponCode,
                    FCouponCreatday = c.FCouponCode.FCouponCreatday,
                    FCouponDiscount = c.FCouponCode.FCouponDiscount,
                    FCouponEndday = c.FCouponCode.FCouponEndday,
                    FCouponFrom = c.FCouponCode.FCouponFrom,
                    MinSellMoney = c.FCouponCode.MinSellMoney
                });

            // 如果傳遞了 fCoupon_from 參數，則進一步篩選創建者為特定用戶的優惠券
            if (fCoupon_from.HasValue) //HasValue 用來判斷 nullable 類型（如 int?）是否有值。
                                       //如果有值為true就執行程式碼,所以當如果是要搜尋全部的優惠券時就不會給參數也不匯進到這裡來避免跟賣場優惠券的條件搞混
            {
                if (fCoupon_from.Value == 11)
                {
                    //如果有傳遞2個參數就用此參數去尋找FCouponFrom
                    coupons = coupons.Where(c => c.FCouponFrom == fCoupon_from.Value);
                }
                else if (fCoupon_from.Value == -1)
                {
                    coupons = coupons.Where(c => c.FCouponFrom != 11);
                }

            }
            return coupons;
        }
        public IQueryable<CouponDTO> getSellerCoupons(int couponfromID, SellerCouponSearchDTO searchDTO)
        {

            var coupons = _context.TCoupons.Where(c => c.FCouponFrom == couponfromID).
           Select(c => new CouponDTO
           {
               FCouponCodeId = c.FCouponCodeId,
               FCouponCodeName = c.FCouponCodeName,
               CouponDescription = c.CouponDescription,
               FCouponCode = c.FCouponCode,
               FCouponCreatday = c.FCouponCreatday,
               FCouponDiscount = c.FCouponDiscount,
               FCouponEndday = c.FCouponEndday,
               FCouponFrom = c.FCouponFrom,
               MinSellMoney = c.MinSellMoney,
           });

            if (!string.IsNullOrEmpty(searchDTO.keyword))
            {
                if (!string.IsNullOrEmpty(searchDTO.CouponType))
                {
                    if (searchDTO.CouponType == "name")
                    {
                        coupons = coupons.Where(c => c.FCouponCodeName.Contains(searchDTO.keyword));
                    }
                    else if (searchDTO.CouponType == "code")
                    {
                        coupons = coupons.Where(c => c.FCouponCode.Contains(searchDTO.keyword));
                    }
                }

            }
            return coupons;
        }
        public IQueryable<NotifiesDTO> getUserNotify(int Id)
        {
            var ntf = _context.TRtwnotifies.Include(x => x.FNotifyType).Where(x => x.FUserId == Id)
                .Select(x => new NotifiesDTO
                {
                    FNotifyId = x.FNotifyId,
                    FUserId = x.FUserId,
                    notify = x.FNotify,
                    FNotifyTypeId = x.FNotifyTypeId,
                    FIsNotRead = x.FIsNotRead,
                    notifytype = x.FNotifyType.FNotifyType,
                    FNotifyState = x.FNotifyState
                    
                });
            return ntf;
        }
        public async Task<List<ChatDTO>> getchatmessageAsync(int Id, int Id2)
        {
            var message = await _context.TMemberChats
                .Include(x => x.FSender)
                .Include(x => x.FReceiver)
                .Where(x => (x.FSenderId == Id && x.FReceiverId == Id2) || (x.FReceiverId == Id && x.FSenderId == Id2))
                .OrderBy(x => x.FSendTime)
                .Select(x => new ChatDTO
                {
                    senderId = x.FSender.FUserId,
                    receiverId = x.FReceiver.FUserId,
                    senderName = x.FSender.FName,
                    receiverName = x.FReceiver.FName,
                    chatimage = x.FChatImage != null ? x.FChatImage.FImageData : null,
                    sendTime = x.FSendTime,
                    message = x.FMessage,
                    FReport = x.FReport
                }).ToListAsync(); // 使用 ToListAsync 來等待查詢結果

            return message;
        }


        public async Task<List<ChatUsersDTO>> getchatusersAsync()
        {
            var user = await _context.TUsers
                .Select(x => new ChatUsersDTO
                {
                    FUserId = x.FUserId,
                    FName = x.FName,
                    FProfileImage = x.FProfileImage
                }).ToListAsync();
            return user;
        }

    }
}
