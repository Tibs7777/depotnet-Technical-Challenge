using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.Paging;

namespace RefactoringChallenge.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService ordersService)
        {
            _orderService = ordersService;
        }

        //Given more time I would add a filter for validating pageParameters
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParameters pageParameters)
        {
            return Ok(await _orderService.GetOrdersAsync(pageParameters.PageSize, pageParameters.PageNumber));
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            return Ok(await _orderService.GetOrderByIdAsync(orderId));
        }

        //[HttpPost("[action]")]
        //public IActionResult Create(
        //    string customerId,
        //    int? employeeId,
        //    DateTime? requiredDate,
        //    int? shipVia,
        //    decimal? freight,
        //    string shipName,
        //    string shipAddress,
        //    string shipCity,
        //    string shipRegion,
        //    string shipPostalCode,
        //    string shipCountry,
        //    IEnumerable<OrderDetailRequest> orderDetails
        //    )
        //{
        //    var newOrderDetails = new List<OrderDetail>();
        //    foreach (var orderDetail in orderDetails)
        //    {
        //        newOrderDetails.Add(new OrderDetail
        //        {
        //            ProductId = orderDetail.ProductId,
        //            Discount = orderDetail.Discount,
        //            Quantity = orderDetail.Quantity,
        //            UnitPrice = orderDetail.UnitPrice,
        //        });
        //    }

        //    var newOrder = new Order
        //    {
        //        CustomerId = customerId,
        //        EmployeeId = employeeId,
        //        OrderDate = DateTime.UtcNow,
        //        RequiredDate = requiredDate,
        //        ShipVia = shipVia,
        //        Freight = freight,
        //        ShipName = shipName,
        //        ShipAddress = shipAddress,
        //        ShipCity = shipCity,
        //        ShipRegion = shipRegion,
        //        ShipPostalCode = shipPostalCode,
        //        ShipCountry = shipCountry,
        //        OrderDetails = newOrderDetails,
        //    };
        //    _northwindDbContext.Orders.Add(newOrder);
        //    _northwindDbContext.SaveChanges();

        //    return Json(newOrder.Adapt<OrderResponse>());
        //}

        //[HttpPost("{orderId}/[action]")]
        //public IActionResult AddProductsToOrder([FromRoute] int orderId, IEnumerable<OrderDetailRequest> orderDetails)
        //{
        //    var order = _northwindDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
        //    if (order == null)
        //        return NotFound();

        //    var newOrderDetails = new List<OrderDetail>();
        //    foreach (var orderDetail in orderDetails)
        //    {
        //        newOrderDetails.Add(new OrderDetail
        //        {
        //            OrderId = orderId,
        //            ProductId = orderDetail.ProductId,
        //            Discount = orderDetail.Discount,
        //            Quantity = orderDetail.Quantity,
        //            UnitPrice = orderDetail.UnitPrice,
        //        });
        //    }

        //    _northwindDbContext.OrderDetails.AddRange(newOrderDetails);
        //    _northwindDbContext.SaveChanges();

        //    return Json(newOrderDetails.Select(od => od.Adapt<OrderDetailResponse>()));
        //}

        //[HttpPost("{orderId}/[action]")]
        //public IActionResult Delete([FromRoute] int orderId)
        //{
        //    var order = _northwindDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
        //    if (order == null)
        //        return NotFound();

        //    var orderDetails = _northwindDbContext.OrderDetails.Where(od => od.OrderId == orderId);

        //    _northwindDbContext.OrderDetails.RemoveRange(orderDetails);
        //    _northwindDbContext.Orders.Remove(order);
        //    _northwindDbContext.SaveChanges();

        //    return Ok();
        //}
    }
}
