using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlFeedReader.Models
{
    public class Product
    {
        public Product() 
        {
            Categories = new List<string>();
            AdditionalImageLinks = new List<string>();
            CategoryLinks = new List<string>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string Price { get; set; }
        public IList<string> Categories { get; set; }
        public string Availability { get; set; }

        public IList<string> AdditionalImageLinks { get; set; }

        public string AddToCartLink { get; set; }
        public string StockLevel { get; set; }
        public string Hidden { get; set; }
        public IList<string> CategoryLinks { get; set; }

        public string Visibility { get; set; }
        public string Virtual { get; set; }

        public string LastModified { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                ["<product_id>"] = Id,
                ["<product_title>"] = Title,
                ["<product_description>"] = Description,
                ["<product_link>"] = Link,
                ["<product_image_link>"] = ImageLink,
                ["<product_price>"] = Price,
                ["<product_availability>"] = Availability,
                ["<product_stock_level>"] = StockLevel,
                ["<product_last_modified>"] = LastModified,
            };
            return dict;
        }

    }
}
