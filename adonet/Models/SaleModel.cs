using adonet.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.Models
{
    public class SaleModel
    {
        public int Quantity { get; set; }
        public IdName Product { get; set; }
        public IdName Manager { get; set; }
        public List<IdName> Products { get; set; }
        public List<IdName> Managers { get; set; }
        public SaleModel(Sale entity)
        {
            Quantity = entity.Quantity;

            Product = entity.ProductId==default?null!: new IdName
            {
                Id = entity.Product.Id,
                Name = entity.Product.Name
            };
            Manager = entity.ManagerId == default ? null! : new IdName
            {
                Id = entity.Manager.Id,
                Name = entity.Manager.Name
            };
        }
    }
}
