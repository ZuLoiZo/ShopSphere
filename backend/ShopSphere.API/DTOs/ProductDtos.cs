namespace ShopSphere.API.DTOs;

// Pour la liste des produits
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string ImageUrl,
    int CategoryId,
    string CategoryName,
    bool IsActive
);

// Pour créer un produit
public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string ImageUrl,
    int CategoryId
);

// Pour mettre à jour un produit
public record UpdateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string ImageUrl,
    int CategoryId,
    bool IsActive
);

// Pour la pagination
public record ProductListResponse(
    List<ProductDto> Products,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
