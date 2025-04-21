using Microsoft.AspNetCore.Http;
using SC_701_PAW_Lunes.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace SC_701_PAW_Lunes.Services
{
    public class ShoppingCartService
    {
        private const string SessionKey = "CartSession";
        private readonly ISession _session;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public List<CartItem> GetCart()
        {
            var cartJson = _session.GetString(SessionKey);
            return cartJson != null
                ? JsonSerializer.Deserialize<List<CartItem>>(cartJson)
                : new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            _session.SetString(SessionKey, JsonSerializer.Serialize(cart));
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(p => p.ProductoId == item.ProductoId);
            if (existing != null)
                existing.Cantidad += item.Cantidad;
            else
                cart.Add(item);

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductoId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            _session.Remove(SessionKey);
        }
    }
}
