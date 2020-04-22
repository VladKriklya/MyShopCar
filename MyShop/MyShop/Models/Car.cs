using System;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Car
    {
        public int Id { get; set; }
        public Make Make { get; set; }
        [RegularExpression("^[1-9]*$", ErrorMessage = "Sellect Make")]
        public int MakeID { get; set; }
        public Model Model { get; set; }
        [RegularExpression("^[1-9]*$", ErrorMessage = "Sellect Make")]
        public int ModelID { get; set; }
        [Required(ErrorMessage ="Provide Year")]
        [Range(1990,2020,ErrorMessage ="Invalid Year")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Provide Milleage")]
        [Range(1, int.MaxValue,ErrorMessage ="Provide Milleage")]
        public int Milleage { get; set; }
        public string Features { get; set; }
        [Required(ErrorMessage = "Provide Seller Name")]
        public string SellectName { get; set; }
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string SellectEmail { get; set; }
        [Required(ErrorMessage = "Invalid Number Phone")]
        public string SelectPhone { get; set; }
        [Required(ErrorMessage = "Provide Selling Price")]
        public int Price { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Sellect Currency")]
        public string Currency { get; set; }
        public string ImagePath { get; set; }

    }
}
