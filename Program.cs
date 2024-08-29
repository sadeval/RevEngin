using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DishMenu
{
    public class Dish
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");

            var config = builder.Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    class Program
    {
        static void Main()
        {
            // Проверка доступности базы данных
            using (var context = new AppDbContext())
            {
                if (context.Database.CanConnect())
                {
                    Console.WriteLine("База данных доступна.");
                }
                else
                {
                    Console.WriteLine("Не удалось подключиться к базе данных.");
                    return;
                }
            }

            // Получение всех данных из таблицы DishMenu
            using (var context = new AppDbContext())
            {
                var dishes = context.Dishes.ToList();

                Console.WriteLine("Все блюда в таблице:");
                foreach (var dish in dishes)
                {
                    Console.WriteLine($"{dish.DishId}: {dish.Name} - {dish.Price} грн.");
                }
            }

            // Добавление новых объектов в таблицу
            using (var context = new AppDbContext())
            {
                var newDishes = new List<Dish>
                {
                    new Dish { Name = "Пельмени", Description = "Домашние пельмени", Price = 157.59m },
                    new Dish { Name = "Котлета", Description = "Котлета по-киевски", Price = 98.35m }
                };

                context.Dishes.AddRange(newDishes);
                context.SaveChanges();
                Console.WriteLine("Новые блюда добавлены.");
            }

            // Получение и отображение обновленных данных из таблицы
            using (var context = new AppDbContext())
            {
                var dishes = context.Dishes.ToList();

                Console.WriteLine("Обновленный список блюд:");
                foreach (var dish in dishes)
                {
                    Console.WriteLine($"{dish.DishId}: {dish.Name} - {dish.Price} грн.");
                }
            }
        }
    }
}
