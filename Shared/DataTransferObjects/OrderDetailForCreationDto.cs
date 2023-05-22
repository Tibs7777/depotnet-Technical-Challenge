using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record OrderDetailForCreationDto
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "UnitPrice is required.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public short Quantity { get; set; }

        [Required(ErrorMessage = "Discount is required.")]
        public float Discount { get; set; }
    }
}