using System.Runtime.Serialization;

namespace Store.Core.Entities.Order_Aggregate;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "Payment Received")]
    PaymentSucceded,

    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
}
