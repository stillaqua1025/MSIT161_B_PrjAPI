using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;

namespace MSIT161_B_PriAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderInfoController : ControllerBase
    {
        private readonly dbMSTI161_B_ProjectContext _context;
        private readonly JwtService _jwtService;
        public OrderInfoController(dbMSTI161_B_ProjectContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("{id}")]
        public IActionResult GetTRtworderDetail(string id)
        {
            int orderid = _context.TRtworders
                .Where(o => o.FOrderNumber == id).Select(o => o.FOrderId).FirstOrDefault();
            Factory f = new Factory(_context, _jwtService);
            var info = f.getOrderInfo(orderid);
            return Ok(info);
        }
    }
}
