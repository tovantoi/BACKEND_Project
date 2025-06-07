namespace chuyennganh.Domain.DTOs
{
    public class PayOSRequestDto
    {
            public int OrderId { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public string ReturnUrl { get; set; }
            public string CancelUrl { get; set; }
            public string WebhookUrl { get; set; }
            public string BuyerName { get; set; }
            public string BuyerEmail { get; set; }
            public string BuyerPhone { get; set; }
            public List<OrderItemDTOPayos> Items { get; set; }
        }

    }

