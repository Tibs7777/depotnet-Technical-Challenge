using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects;
using Shared.Paging;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrdersController : ControllerBase
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
        public async Task<IActionResult> Create([FromBody] OrderForCreationDto orderForCreation)
        {
            OrderDto createdOrder = await _orderService.CreateOrderAsync(orderForCreation);

            return CreatedAtAction(nameof(GetById), new { orderId = createdOrder.OrderId }, createdOrder);
        }

        //Typically, we would do a created here, but I don't have a uri to point the user to for order details, so I have sent the new order details back with a 200
        [HttpPost("{orderId}/products")]
        public async Task<IActionResult> AddProductsToOrder([FromRoute] int orderId, [FromBody] IEnumerable<OrderDetailForCreationDto> orderDetails)
        {
            return Ok(await _orderService.CreateOrderDetailsByOrderIdAsync(orderId,orderDetails));
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteById([FromRoute] int orderId)
        {
            await _orderService.DeleteOrderByIdAsync(orderId);

            return NoContent();
        }
    }
}
