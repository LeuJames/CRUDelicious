using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
            return View(AllDishes);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Dish newDish)
        {
          if(ModelState.IsValid)
          {
            dbContext.Add(newDish);
            dbContext.SaveChanges();
            return Redirect ("/");
          }
          else{
            return View("New");
          }
        }

        [HttpGet("/dish/{id}")]
        public IActionResult Details(int id)
        {
          Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);
          return View(dish);
        }

        [HttpGet("/dish/{id}/edit")]
        public IActionResult Edit(int id)
        {
          Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);
          return View(dish);
        }

        [HttpPost("/dish/{id}/update")]
        public IActionResult Update(int id, Dish d)
        {
          if(ModelState.IsValid)
          {
            Dish dish = dbContext.Dishes.FirstOrDefault(di => di.DishId == id);
            dish.Name = d.Name;
            dish.Chef = d.Chef;
            dish.Calories = d.Calories;
            dish.Tastiness = d.Tastiness;
            dish.Description = d.Description;
            dish.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges();
            return Redirect("/");
          }
          else
          {
            d.DishId = id;
            return View("Edit", d);
          }
        }

        [HttpGet("/dish/{id}/delete")]
        public IActionResult Delete(int id)
        {
          Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);

          dbContext.Dishes.Remove(dish);
          dbContext.SaveChanges();
          return Redirect ("/");
        }

        [HttpPost("search")]
        public IActionResult Search(string q)
        {
            List<Dish> SearchResults = dbContext.Dishes
                                            .Where(
                                                d => d.Chef.Contains(q) || 
                                                d.Name.Contains(q) || 
                                                d.Description.Contains(q)
                                            )
                                            .ToList();
            return View("Index", SearchResults);
        }

    }
}