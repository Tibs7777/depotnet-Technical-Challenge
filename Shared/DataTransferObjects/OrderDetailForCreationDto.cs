using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public class OrderDetailForCreationDto
    {
        [Required(ErrorMessage = "ProductId is a required field.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "UnitPrice is a required field.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity is a required field.")]
        public short Quantity { get; set; }

        [Required(ErrorMessage = "Discount is a required field.")]
        public float Discount { get; set; }
    }
}