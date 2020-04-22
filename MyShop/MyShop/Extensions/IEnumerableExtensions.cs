using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace MyShop.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "----Select----",
                Value = "0"
            };
            List.Add(sli);
            foreach (var Item in Items)
            {
                sli = new SelectListItem
                {
                    Text = Item.GetType().GetProperty("Name").GetValue(Item, null).ToString(),
                    Value = Item.GetType().GetProperty("Id").GetValue(Item, null).ToString()
                };
            List.Add(sli);
            }
            return List;

        }
    }
}

           
       

