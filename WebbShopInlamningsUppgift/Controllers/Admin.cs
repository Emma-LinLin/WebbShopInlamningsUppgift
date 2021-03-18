using System;
using System.Collections.Generic;
using System.Text;
using WebbShopInlamningsUppgift.API;
using WebbShopInlamningsUppgift.Models;

namespace WebbShopInlamningsUppgift.Controllers
{
    class Admin
    {
        public static void Run()
        {
            var api = new WebbShopAPI();

            //Console.Write("Logged in as: ");
            //int userId = api.Login("Administrator", "CodicRulez");
            //Console.WriteLine(userId);

            ////-------------------------------------------------------------------------

            //bool success = api.AddBook(userId, "Sagan om ringen", "J.R.R Tolkien", 300, 5);
            //if (success)
            //{
            //    Console.WriteLine("Added book: Sagan om ringen by J.R.R Tolkien");
            //}

            ////-------------------------------------------------------------------------

            //success = api.SetAmount(userId, 2, 4);
            //if (success)
            //{
            //    Console.WriteLine("Added new amount to book.");
            //}

            ////-------------------------------------------------------------------------

            //var list = api.ListUsers(userId);
            //foreach(var user in list)
            //{
            //    Console.WriteLine(user.Name);
            //}

            ////-------------------------------------------------------------------------

            //list = api.FindUser(userId, "te");
            //foreach(var user in list)
            //{
            //    Console.WriteLine(user.Name);
            //}

            ////-------------------------------------------------------------------------

            //var book = Helper.GetBookObject("Sagan om ringen");
            //if(book != null)
            //{
            //    success = api.UpdateBook(userId, book.ID, book.Title, book.Author, 350);
            //    if (success)
            //    {
            //        Console.WriteLine("Sucessfully updated book");
            //    }
            //}

            ////-------------------------------------------------------------------------

            //success = api.AddCategory(userId, "Fantasy");
            //if (success)
            //{
            //    Console.WriteLine("Added category: Fantasy");
            //}

            //success = api.AddCategory(userId, "Action");
            //if (success)
            //{
            //    Console.WriteLine("Added category: Action");
            //}

            ////-------------------------------------------------------------------------

            //var bookId = Helper.GetBookID("Sagan om ringen");
            //var categoryId = Helper.GetCategoryId("Fantasy");

            //success = api.AddBookToCategory(userId, bookId, categoryId);
            //if (success)
            //{
            //    Console.WriteLine("Successfully added book to category");
            //}

            ////-------------------------------------------------------------------------

            //categoryId = Helper.GetCategoryId("Action");
            //success = api.UpdateCategory(userId, categoryId, "ActionPower");
            //if (success)
            //{
            //    Console.WriteLine("Successfully updated category.");
            //}

            ////-------------------------------------------------------------------------

            //success = api.AddUser(userId, "Legolas", "L3mb4sBr34d");
            //if (success)
            //{
            //    Console.WriteLine("Successfully added user: Legolas");
            //}

            ////-------------------------------------------------------------------------

            //var customerId = Helper.GetUserID("Legolas");
            //success = api.Promote(userId, customerId);
            //if (success)
            //{
            //    Console.WriteLine("Sucessfully promoted Legolas");
            //}

            //api.Logout(userId);

            ////-------------------------------------------------------------------------

            //Console.Write("Logged in as: ");
            //userId = api.Login("Legolas", "L3mb4sBr34d");
            //Console.WriteLine(userId);

            //bookId = Helper.GetBookID("Sagan om ringen");
            //success = api.BuyBook(userId, bookId);
            //if (success)
            //{
            //    Console.WriteLine("Purchase successful.");
            //}
            //success = api.BuyBook(userId, bookId);
            //if (success)
            //{
            //    Console.WriteLine("Purchase successful.");
            //}
            //success = api.BuyBook(userId, bookId);
            //if (success)
            //{
            //    Console.WriteLine("Purchase successful.");
            //}

            //api.Logout(userId);

            //-------------------------------------------------------------------------

            Console.Write("Logged in as: ");
            var userId = api.Login("Administrator", "CodicRulez");
            Console.WriteLine(userId);

            var soldBooks = api.SoldItems(userId);
            if(soldBooks.Count > 0)
            {
                foreach(var item in soldBooks)
                {
                    Console.WriteLine($"{item.Title} - {item.Author}");
                }
            }

            int moneyEarned = api.MoneyEarned(userId);
            if(moneyEarned > 0)
            {
                Console.WriteLine($"Money earned: {moneyEarned}");
            }

            int bestCustomer = api.BestCustomer(userId);
            if(bestCustomer > 0)
            {
                Console.WriteLine($"Best customer's ID: {bestCustomer}");
            }

            //var bookId = Helper.GetBookID("I Robot");
            //var success = api.DeleteBook(userId, bookId);
            //if (success)
            //{
            //    Console.WriteLine("Successfully deleted book");
            //}

            //First try, deleted category "Romance"
            //var categoryId = Helper.GetCategoryId("Romance");
            //var success = api.DeleteCategory(userId, categoryId);
            //if (success)
            //{
            //    Console.WriteLine("Successfully deleted category");
            //}

            var categoryId = Helper.GetCategoryId("Fantasy");
            var success = api.DeleteCategory(userId, categoryId);
            if (success)
            {
                Console.WriteLine("Successfully deleted category");
            }

            api.Logout(userId);
        }
    }
}
