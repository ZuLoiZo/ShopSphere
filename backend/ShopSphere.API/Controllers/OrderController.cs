using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.API.DTOs;
using ShopSphere.Core.Entities;
using ShopSphere.Core.Enums;
using ShopSphere.Infrastructure.Data;
using System.Security.Claims;

namespace ShopSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    // GET: api/orders - Mes commandes
    [HttpGet]
    public async Task<ActionResult<OrderListResponse>> GetMyOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();

        var query = _context.Orders
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderDto(
                o.Id,
                o.UserId,
                o.Status.ToString(),
                o.TotalAmount,
                o.ShippingAddress,
                o.Items.Select(oi => new OrderItemDto(
                    oi.Id,
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Product.ImageUrl,
                    oi.Quantity,
                    oi.PriceAtOrder,
                    oi.PriceAtOrder * oi.Quantity
                )).ToList(),
                o.CreatedAt
            ))
            .ToListAsync();

        return Ok(new OrderListResponse(
            orders,
            totalCount,
            page,
            pageSize,
            totalPages
        ));
    }

    // GET: api/orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var userId = GetUserId();

        var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound(new { message = "Commande non trouvée" });
        }

        var orderDto = new OrderDto(
            order.Id,
            order.UserId,
            order.Status.ToString(),
            order.TotalAmount,
            order.ShippingAddress,
            order.Items.Select(oi => new OrderItemDto(
                oi.Id,
                oi.ProductId,
                oi.Product.Name,
                oi.Product.ImageUrl,
                oi.Quantity,
                oi.PriceAtOrder,
                oi.PriceAtOrder * oi.Quantity
            )).ToList(),
            order.CreatedAt
        );

        return Ok(orderDto);
    }

    // POST: api/orders - Créer une commande depuis le panier
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto dto)
    {
        var userId = GetUserId();

        // Récupérer le panier avec les produits
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            return BadRequest(new { message = "Votre panier est vide" });
        }

        // Vérifier les stocks
        foreach (var item in cart.Items)
        {
            if (item.Product.Stock < item.Quantity)
            {
                return BadRequest(new
                {
                    message = $"Stock insuffisant pour {item.Product.Name}. Disponible: {item.Product.Stock}"
                });
            }
        }

        // Calculer le total
        var totalAmount = cart.Items.Sum(ci => ci.Product.Price * ci.Quantity);

        // Créer la commande
        var order = new Order
        {
            UserId = userId,
            TotalAmount = totalAmount,
            Status = OrderStatus.Pending,
            ShippingAddress = dto.ShippingAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Créer les order items et déduire les stocks
        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                PriceAtOrder = cartItem.Product.Price,
                CreatedAt = DateTime.UtcNow
            };

            _context.OrderItems.Add(orderItem);

            // Déduire le stock
            cartItem.Product.Stock -= cartItem.Quantity;
        }

        // Vider le panier
        _context.CartItems.RemoveRange(cart.Items);

        await _context.SaveChangesAsync();

        // Recharger la commande avec les items
        order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .FirstAsync(o => o.Id == order.Id);

        var orderDto = new OrderDto(
            order.Id,
            order.UserId,
            order.Status.ToString(),
            order.TotalAmount,
            order.ShippingAddress,
            order.Items.Select(oi => new OrderItemDto(
                oi.Id,
                oi.ProductId,
                oi.Product.Name,
                oi.Product.ImageUrl,
                oi.Quantity,
                oi.PriceAtOrder,
                oi.PriceAtOrder * oi.Quantity
            )).ToList(),
            order.CreatedAt
        );

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
    }

    // PUT: api/orders/5/status - Mettre à jour le statut (Admin seulement)
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound(new { message = "Commande non trouvée" });
        }

        // Valider le statut
        if (!Enum.TryParse<OrderStatus>(dto.Status, out var status))
        {
            return BadRequest(new { message = "Statut invalide. Valeurs possibles: Pending, Processing, Shipped, Delivered, Cancelled" });
        }

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/orders/all - Toutes les commandes (Admin seulement)
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderListResponse>> GetAllOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null)
    {
        var query = _context.Orders
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .AsQueryable();

        // Filtrer par statut si spécifié
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            query = query.Where(o => o.Status == orderStatus);
        }

        query = query.OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderDto(
                o.Id,
                o.UserId,
                o.Status.ToString(),
                o.TotalAmount,
                o.ShippingAddress,
                o.Items.Select(oi => new OrderItemDto(
                    oi.Id,
                    oi.ProductId,
                    oi.Product.Name,
                    oi.Product.ImageUrl,
                    oi.Quantity,
                    oi.PriceAtOrder,
                    oi.PriceAtOrder * oi.Quantity
                )).ToList(),
                o.CreatedAt
            ))
            .ToListAsync();

        return Ok(new OrderListResponse(
            orders,
            totalCount,
            page,
            pageSize,
            totalPages
        ));
    }
}
