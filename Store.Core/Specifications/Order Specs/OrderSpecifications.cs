using StackExchange.Redis;
using Order = Store.Core.Entities.Order_Aggregate.Order;

namespace Store.Core.Specifications.Order_Specs;

public class OrderSpecifications:BaseSpecifications<Order>
{
    public OrderSpecifications()
    {
        Includes.Add(O => O.Items);
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.ShippingAddress);
        AddOrderByDesc(O => O.OrderDate);
    }

    public OrderSpecifications( string buyerEmail)
        :base(O=>O.BuyerEmail== buyerEmail)
    {
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.Items);
		Includes.Add(O => O.ShippingAddress);
		AddOrderByDesc(O => O.OrderDate);
    }

    public OrderSpecifications(int orderId,string buyerEmail)
       : base(O=>O.Id==orderId && O.BuyerEmail == buyerEmail)
    {
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.Items);
		Includes.Add(O => O.ShippingAddress);
		AddOrderByDesc(O => O.OrderDate);
    }

}
