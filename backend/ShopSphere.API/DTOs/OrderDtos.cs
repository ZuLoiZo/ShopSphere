namespace ShopSphere.API.DTOs;

// Item dans une commande
public record OrderItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string ProductImageUrl,
    int Quantity,
    decimal PriceAtOrder,
    decimal Subtotal
);

// Commande complète
public record OrderDto(
    int Id,
    int UserId,
    string Status,
    decimal TotalAmount,
    string ShippingAddress,
    List<OrderItemDto> Items,
    DateTime CreatedAt
);

// Pour créer une commande depuis le panier
public record CreateOrderDto(
    string ShippingAddress
);

// Pour mettre à jour le statut (Admin seulement)
public record UpdateOrderStatusDto(
    string Status  // Pending, Processing, Shipped, Delivered, Cancelled
);

// Liste des commandes avec pagination
public record OrderListResponse(
    List<OrderDto> Orders,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
