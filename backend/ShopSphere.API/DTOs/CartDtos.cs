namespace ShopSphere.API.DTOs;

// Item dans le panier
public record CartItemDto(
    int Id,
    int ProductId,
    string ProductName,
    string ProductImageUrl,
    decimal ProductPrice,
    int Quantity,
    decimal Subtotal
);

// Panier complet
public record CartDto(
    int Id,
    int UserId,
    List<CartItemDto> Items,
    decimal TotalAmount,
    int TotalItems
);

// Pour ajouter un produit au panier
public record AddToCartDto(
    int ProductId,
    int Quantity
);

// Pour modifier la quantité
public record UpdateCartItemDto(
    int Quantity
);
