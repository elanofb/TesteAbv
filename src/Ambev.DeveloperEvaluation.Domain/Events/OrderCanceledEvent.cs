namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class OrderCanceledEvent
    {
        public string OrderId { get; set; }

        public OrderCanceledEvent(string orderId)
        {
            OrderId = orderId;
        }
    }
}
