using WEB_153501_Kosach.Domain.Models;
using System.Text.Json.Serialization;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Extensions;

namespace WEB_153501_Kosach.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services
                                    .GetRequiredService<IHttpContextAccessor>()
                                    .HttpContext?.Session;

            SessionCart cart = session?.Get<SessionCart>("Cart")
                                                    ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(Furniture product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
