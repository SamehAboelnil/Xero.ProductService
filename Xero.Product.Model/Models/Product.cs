using System;
using System.Collections.Generic;

namespace ProductService.Domain
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public List<Option> Options { get; set; }

        public Product()
        {

        }

        public Product(Guid id)
        {
            this.Id = id;
        }


    }
}
