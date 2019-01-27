using System.Collections.Generic;

namespace BasketApi.Contracts
{
    public class BasketContract
    {
        public int Id { get; set; }

        public IList<BasketContractItem> Items { get; set; }

    }

    
    public class BasketContractItem
    {
        public int Id { get; set; }
        public ContractItem ContractItem { get; set; }
        public int Qty { get; set; }


    }

    public class ContractItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}