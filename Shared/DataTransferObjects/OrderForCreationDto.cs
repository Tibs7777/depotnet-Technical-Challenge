using Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

public class OrderForCreationDto
{
    [MaxLength(5, ErrorMessage = "CustomerId must not exceed 5 characters.")]
    [MinLength(5, ErrorMessage = "CustomerId must be 5 characters.")]
    public string CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? RequiredDate { get; set; }

    public int ShipVia { get; set; }

    public decimal? Freight { get; set; }

    [MaxLength(40, ErrorMessage = "ShipName must not exceed 40 characters.")]
    public string ShipName { get; set; }

    [MaxLength(60, ErrorMessage = "ShipAddress must not exceed 60 characters.")]
    public string ShipAddress { get; set; }

    [MaxLength(15, ErrorMessage = "ShipCity must not exceed 15 characters.")]
    public string ShipCity { get; set; }

    [MaxLength(15, ErrorMessage = "ShipRegion must not exceed 15 characters.")]
    public string ShipRegion { get; set; }

    [MaxLength(10, ErrorMessage = "ShipPostalCode must not exceed 10 characters.")]
    public string ShipPostalCode { get; set; }

    [MaxLength(15, ErrorMessage = "ShipCountry must not exceed 15 characters.")]
    public string ShipCountry { get; set; }

    public IEnumerable<OrderDetailForCreationDto> OrderDetails { get; set; }
}
