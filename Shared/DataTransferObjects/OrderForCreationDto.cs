using Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

public class OrderForCreationDto
{
    [Required(ErrorMessage = "CustomerId is required.")]
    [MaxLength(5, ErrorMessage = "CustomerId must not exceed 5 characters.")]
    public string CustomerId { get; set; }

    [Required(ErrorMessage = "EmployeeId is required.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "RequiredDate is required.")]
    public DateTime? RequiredDate { get; set; }

    [Required(ErrorMessage = "ShipVia is required.")]
    public int ShipVia { get; set; }

    public decimal? Freight { get; set; }

    [Required(ErrorMessage = "ShipName is required.")]
    [StringLength(40, ErrorMessage = "ShipName must not exceed 40 characters.")]
    public string ShipName { get; set; }

    [StringLength(60, ErrorMessage = "ShipAddress must not exceed 60 characters.")]
    public string ShipAddress { get; set; }

    [StringLength(15, ErrorMessage = "ShipCity must not exceed 15 characters.")]
    public string ShipCity { get; set; }

    [StringLength(15, ErrorMessage = "ShipRegion must not exceed 15 characters.")]
    public string ShipRegion { get; set; }

    [StringLength(10, ErrorMessage = "ShipPostalCode must not exceed 10 characters.")]
    public string ShipPostalCode { get; set; }

    [StringLength(15, ErrorMessage = "ShipCountry must not exceed 15 characters.")]
    public string ShipCountry { get; set; }

    public IEnumerable<OrderDetailForCreationDto> OrderDetails { get; set; }
}
