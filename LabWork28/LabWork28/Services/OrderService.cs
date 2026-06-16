using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Collections.Concurrent;

namespace LabWork28.Services
{
    public class OrderService : Orderer.OrdererBase
    {
        private static readonly ConcurrentDictionary<string, OrderResponse> _orders = new();

        public override Task<OrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var newOrder = new OrderResponse
            {
                Id = Guid.NewGuid().ToString(),
                OrderDate = Timestamp.FromDateTime(DateTime.UtcNow)
            };
            newOrder.Products.AddRange(request.Products);

            _orders[newOrder.Id] = newOrder;
            return Task.FromResult(newOrder);
        }

        public override Task<OrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            if (_orders.TryGetValue(request.Id, out var order))
            {
                return Task.FromResult(order);
            }
            throw new RpcException(new Status(StatusCode.NotFound, $"Заказ с id {request.Id} не найден"));
        }

        public override Task<OrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
            if (!_orders.ContainsKey(request.Id))
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Заказ с id {request.Id} не найден"));
            }

            var updatedOrder = new OrderResponse
            {
                Id = request.Id,
                OrderDate = Timestamp.FromDateTime(DateTime.UtcNow) 
            };
            updatedOrder.Products.AddRange(request.Products);

            _orders[request.Id] = updatedOrder;
            return Task.FromResult(updatedOrder);
        }

        public override Task<Empty> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
        {
            if (_orders.TryRemove(request.Id, out _))
            {
                return Task.FromResult(new Empty());
            }
            throw new RpcException(new Status(StatusCode.NotFound, $"Заказ с id {request.Id} не найден"));
        }

        public override Task<OrderListResponse> ListOrders(Empty request, ServerCallContext context)
        {
            var response = new OrderListResponse();
            response.Orders.AddRange(_orders.Values);
            return Task.FromResult(response);
        }

        public override Task<OrderListResponse> FilterOrders(FilterOrdersRequest request, ServerCallContext context)
        {
            var response = new OrderListResponse();

            var filtered = _orders.Values.Where(o =>
                o.OrderDate.ToDateTime() >= request.StartDate.ToDateTime() &&
                o.Products.Sum(p => p.Price) >= request.MinPrice
            );

            response.Orders.AddRange(filtered);
            return Task.FromResult(response);
        }
    }
}
