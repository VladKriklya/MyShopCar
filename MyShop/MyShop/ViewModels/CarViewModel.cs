using MyShop.Models;
using System;
using System.Collections.Generic;

namespace MyShop.ViewModels
{
    public class CarViewModel
    {
        public Car Car { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Curency> Curencies { get; set; }

        private List<Curency> CList = new List<Curency>();

        private List<Curency> CreateList()
        {
            CList.Add(new Curency("USD", "USD"));
            CList.Add(new Curency("EUR", "EUR"));
            CList.Add(new Curency("UAH", "UAH"));
            return CList;
        }

        public CarViewModel()
        {
            Curencies = CreateList();
        }
    }


    public class Curency
    {
        public String Id { get; set; }
        public String Name { get; set; }

        public Curency(String id, String name)
        {
            Id = id;
            Name = name;
        }

    }
}
