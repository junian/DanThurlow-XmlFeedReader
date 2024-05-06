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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public float Price { get; set; }
        public IList<string> Categories { get; set; }
        public string Availability { get; set; }

        public IList<string> AdditionalImageLinks { get; set; }

        public string AddToCartLink { get; set; }
        public int StockLevel { get; set; }
        public string Hidden { get; set; }
        public IList<string> CategoryLinks { get; set; }

        public string Visibility { get; set; }
        public string Virtual { get; set; }

    }
}
