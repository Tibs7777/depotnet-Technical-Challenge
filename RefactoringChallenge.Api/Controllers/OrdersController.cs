using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects;
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


        [HttpGet("{orderId}", Name = "OrderById")]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            return Ok(await _orderService.GetOrderByIdAsync(orderId));
        }

        [HttpPost()]
        public async Task<IActionResult> Create(OrderForCreationDto orderForCreation)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderForCreation);

            return CreatedAtAction(nameof(GetById), new { orderId = createdOrder.OrderId }, createdOrder);
        }

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
