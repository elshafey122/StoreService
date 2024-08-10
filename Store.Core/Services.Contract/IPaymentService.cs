using Store.Core.Entities;
using Store.Core.Entities.Order_Aggregate;

namespace Store.Core.Services.Contract;

public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

    Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId, bool isSucceeded);
}
