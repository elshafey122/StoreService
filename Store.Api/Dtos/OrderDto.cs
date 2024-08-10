using System.ComponentModel.DataAnnotations;

namespace Store.API.Dtos;

public class OrderDto
{
    [Required]
    public string BasketId { get; set; }
    [Required]
    public int DeliveryMethodId { get; set; }
    [Required]
    public AddressDto ShipToAddress { get; set; }
}
