using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Models;
using System.Collections.Generic;


namespace MyShop.ViewModels
{
    public class ModelViewModel
    {
        public Model Model { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<SelectListItem> CSelectListItem(IEnumerable<Make> Items)
        {
            List<SelectListItem> MakeList = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "----Select----",
                Value = "0"
            };
            MakeList.Add(sli);
            foreach(Make make in Items)
            {
                sli = new SelectListItem
                {
                    Text = make.Name,
                    Value = make.Id.ToString()
                };
                MakeList.Add(sli);
            }
            return MakeList;
        }
    }
}
