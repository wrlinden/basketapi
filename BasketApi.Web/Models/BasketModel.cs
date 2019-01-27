using System.Collections.Generic;

namespace BasketApi.Models
{
    public class BasketModel
    {
        public int Id { get; set; }

        public IList<BasketModelItem> Items { get; set; }
    }

    public class BasketModelItem
    {
        public int Id { get; set; }
        public ModelItem ModelItem { get; set; }
        public int Qty { get; set; }


    }

    public class ModelItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}