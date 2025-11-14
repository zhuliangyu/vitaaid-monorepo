using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vitaaid.com.Models.ShoppingCart
{
    public class ShoppingCartItem
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual int Qty { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal DropShipPrice { get; set; }
    }
}
