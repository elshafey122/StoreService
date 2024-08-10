﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities.Order_Aggregate;

public class Order:BaseEntity
{
    public Order()
    {
        
    }
    public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        Items = items;
        SubTotal = subTotal;
        PaymentIntentId = paymentIntentId;  
    }

    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public Address ShippingAddress { get; set; } 

    public DeliveryMethod? DeliveryMethod { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    public decimal SubTotal { get; set; }

    public decimal GetTotal()
        => SubTotal + DeliveryMethod.Cost;

    public string PaymentIntentId { get; set; } = string.Empty;
}
