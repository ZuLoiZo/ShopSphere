using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.API.DTOs;
using ShopSphere.Core.Entities;
using ShopSphere.Infrastructure.Data;
using System.Security.Claims;

namespace ShopSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    // GET: api/cart
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        var userId = GetUserId();

        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            // Créer un panier vide si l'utilisateur n'en a pas
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var cartDto = new CartDto(
            cart.Id,
            cart.UserId,
            cart.Items.Select(ci => new CartItemDto(
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Product.ImageUrl,
                ci.Product.Price,
                ci.Quantity,
                ci.Product.Price * ci.Quantity
            )).ToList(),
            cart.Items.Sum(ci => ci.Product.Price * ci.Quantity),
            cart.Items.Sum(ci => ci.Quantity)
        );

        return Ok(cartDto);
    }

    // POST: api/cart/items
    [HttpPost("items")]
    public async Task<ActionResult<CartDto>> AddToCart(AddToCartDto dto)
    {
        var userId = GetUserId();

        // Vérifier que le produit existe et est disponible
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
        {
            return NotFound(new { message = "Produit non trouvé" });
        }

        if (!product.IsActive)
        {
            return BadRequest(new { message = "Ce produit n'est plus disponible" });
        }

        if (product.Stock < dto.Quantity)
        {
            return BadRequest(new { message = $"Stock insuffisant. Disponible: {product.Stock}" });
        }

        // Récupérer ou créer le panier
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        // Vérifier si le produit est déjà dans le panier
        var existingItem = cart.Items.FirstOrDefault(ci => ci.ProductId == dto.ProductId);

        if (existingItem != null)
        {
            // Mettre à jour la quantité
            existingItem.Quantity += dto.Quantity;

            if (product.Stock < existingItem.Quantity)
            {
                return BadRequest(new { message = $"Stock insuffisant. Disponible: {product.Stock}" });
            }
        }
        else
        {
            // Ajouter un nouvel item
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow
            };
            _context.CartItems.Add(cartItem);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Recharger le panier avec les produits
        cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstAsync(c => c.Id == cart.Id);

        var cartDto = new CartDto(
            cart.Id,
            cart.UserId,
            cart.Items.Select(ci => new CartItemDto(
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Product.ImageUrl,
                ci.Product.Price,
                ci.Quantity,
                ci.Product.Price * ci.Quantity
            )).ToList(),
            cart.Items.Sum(ci => ci.Product.Price * ci.Quantity),
            cart.Items.Sum(ci => ci.Quantity)
        );

        return Ok(cartDto);
    }

    // PUT: api/cart/items/5
    [HttpPut("items/{itemId}")]
    public async Task<ActionResult<CartDto>> UpdateCartItem(int itemId, UpdateCartItemDto dto)
    {
        var userId = GetUserId();

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.Cart.UserId == userId);

        if (cartItem == null)
        {
            return NotFound(new { message = "Item non trouvé dans le panier" });
        }

        if (dto.Quantity <= 0)
        {
            return BadRequest(new { message = "La quantité doit être supérieure à 0" });
        }

        if (cartItem.Product.Stock < dto.Quantity)
        {
            return BadRequest(new { message = $"Stock insuffisant. Disponible: {cartItem.Product.Stock}" });
        }

        cartItem.Quantity = dto.Quantity;
        cartItem.Cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Recharger le panier complet
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstAsync(c => c.Id == cartItem.CartId);

        var cartDto = new CartDto(
            cart.Id,
            cart.UserId,
            cart.Items.Select(ci => new CartItemDto(
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Product.ImageUrl,
                ci.Product.Price,
                ci.Quantity,
                ci.Product.Price * ci.Quantity
            )).ToList(),
            cart.Items.Sum(ci => ci.Product.Price * ci.Quantity),
            cart.Items.Sum(ci => ci.Quantity)
        );

        return Ok(cartDto);
    }

    // DELETE: api/cart/items/5
    [HttpDelete("items/{itemId}")]
    public async Task<ActionResult<CartDto>> RemoveFromCart(int itemId)
    {
        var userId = GetUserId();

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.Cart.UserId == userId);

        if (cartItem == null)
        {
            return NotFound(new { message = "Item non trouvé dans le panier" });
        }

        var cartId = cartItem.CartId;
        _context.CartItems.Remove(cartItem);
        
        var cart = await _context.Carts.FindAsync(cartId);
        if (cart != null)
        {
            cart.UpdatedAt = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();

        // Recharger le panier complet
        cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstAsync(c => c.Id == cartId);

        var cartDto = new CartDto(
            cart.Id,
            cart.UserId,
            cart.Items.Select(ci => new CartItemDto(
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Product.ImageUrl,
                ci.Product.Price,
                ci.Quantity,
                ci.Product.Price * ci.Quantity
            )).ToList(),
            cart.Items.Sum(ci => ci.Product.Price * ci.Quantity),
            cart.Items.Sum(ci => ci.Quantity)
        );

        return Ok(cartDto);
    }

    // DELETE: api/cart
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return NotFound(new { message = "Panier non trouvé" });
        }

        _context.CartItems.RemoveRange(cart.Items);
        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
