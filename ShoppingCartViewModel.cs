using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SanaCommerce.Models;

namespace SanaCommerce.Models
{
    public class ShoppingCartViewModel
    {
        public List<ItemModel> itemModelList;
        public TotalModel totalModel;

        public ShoppingCartViewModel(List<ItemModel> _itemModelList, TotalModel _totalModel)
        {
            itemModelList = _itemModelList;
            totalModel = _totalModel;
        }
    }

    public class ItemModel
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public Decimal Qty { get; set; }
        public Decimal Rate { get; set; }
        public Decimal ItemTotal { get; set; }
    }

    public class TotalModel
    {
        public Decimal Total { get; set; }
    }
}