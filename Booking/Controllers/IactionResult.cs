using Microsoft.AspNetCore.Mvc;
using Solution.Models; 
using System.Collections.Generic;

namespace Solution.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var cards = new List<CardViewModel>
            {
                new CardViewModel
                {
                    Title = "Maison moderne",
                    Description = "Maison spacieuse avec jardin",
                    Image = "maison.jpg", // se trouve dans wwwroot/images/house1.jpg
                    location = "Paris",
                    numberOfBeds = 3,
                    numberOfBaths = 2,
                    numberOfRooms = 5,
                    price = 350000,
                    note = 4
                },
                new CardViewModel
                {
                    Title = "Appartement cosy",
                    Description = "Appartement au centre-ville",
                    Image = "maison.jpg",
                    location = "Lyon",
                    numberOfBeds = 2,
                    numberOfBaths = 1,
                    numberOfRooms = 3,
                    price = 220000,
                    note = 4
                },
                new CardViewModel
                {
                    Title = "Villa de luxe",
                    Description = "Villa avec piscine et vue mer",
                    Image = "maison.jpg",
                    location = "Nice",
                    numberOfBeds = 5,
                    numberOfBaths = 3,
                    numberOfRooms = 8,
                    price = 980000,
                    note = 4
                }
            };

            return View(cards);
        }
    }
}