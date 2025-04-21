using Microsoft.AspNetCore.Mvc;
using SC_701_PAW_Lunes.Models;
using SC_701_PAW_Lunes.Services;
using SC_701_PAW_Lunes.Data;

namespace SC_701_PAW_Lunes.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingCartService _cartService;
        private readonly PAWDbContext _context;

        public CartController(ShoppingCartService cartService, PAWDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // GET: /Cart/
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.Total = cart.Sum(item => item.Subtotal);
            return View(cart);
        }

        // GET: /Cart/Add/5
        public IActionResult Add(int id)
        {
            var producto = _context.Inventory.FirstOrDefault(p => p.Id_Inv == id);

            if (producto == null || producto.Cantidad < 1)
                return NotFound();

            var item = new CartItem
            {
                ProductoId = producto.Id_Inv,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = 1
            };

            _cartService.AddToCart(item);
            return RedirectToAction("Index");
        }

        // GET: /Cart/Remove/5
        public IActionResult Remove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction("Index");
        }

        // GET: /Cart/Checkout
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();

            if (!cart.Any())
                return RedirectToAction("Index");

            ViewBag.Total = cart.Sum(item => item.Subtotal);
            return View(cart);
        }

        // POST: /Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckoutConfirm()
        {
            var cart = _cartService.GetCart();

            if (!cart.Any())
                return RedirectToAction("Index");

            foreach (var item in cart)
            {
                var producto = _context.Inventory.FirstOrDefault(p => p.Id_Inv == item.ProductoId);

                if (producto == null || producto.Cantidad < item.Cantidad)
                {
                    TempData["Error"] = $"No hay suficiente stock de {item.Nombre}.";
                    return RedirectToAction("Index");
                }

                producto.Cantidad -= item.Cantidad;
            }

            _context.SaveChanges();
            _cartService.ClearCart();

            return RedirectToAction("CheckoutSuccess");
        }

        // GET: /Cart/CheckoutSuccess
        public IActionResult CheckoutSuccess()
        {
            return View();
        }
    }
}
