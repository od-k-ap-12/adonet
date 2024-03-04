using adonet.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.Models
{
    public class ProductModel
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string? Price { get; set; }
        public static ProductModel FromEntity(Product entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price.ToString(),
        };
    }
}
