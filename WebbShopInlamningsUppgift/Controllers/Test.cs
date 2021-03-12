﻿using System;
using System.Collections.Generic;
using System.Text;
using WebbShopInlamningsUppgift.API;

namespace WebbShopInlamningsUppgift.Controllers
{
    class Test
    {
        public static void Run()
        {
            var api = new WebbShopAPI();

            //Login() funkar, kontrollerat ssms
            Console.Write("Logged in as: ");
            int userId = api.Login("TestCustomer", "Codic2021");
            Console.WriteLine(userId);
            Console.WriteLine();

            //---------------------------------------------------

            //GetCategories() funkar
            //Console.WriteLine("Searching all categories: ");
            //var listOfCategories = api.GetCategories();
            //if (listOfCategories.Count > 0)
            //{
            //    foreach (var category in listOfCategories)
            //    {
            //        Console.WriteLine(category.Genere);
            //    }
            //}
            //Console.ReadLine();

            //---------------------------------------------------

            //GetCategories(keyword) funkar
            //Console.WriteLine("Searching all categories with \"or\"-keyword: ");
            //var listOfCategories = api.GetCategories("or");
            //if(listOfCategories.Count > 0)
            //{
            //    foreach (var category in listOfCategories)
            //    {
            //        Console.WriteLine(category.Genere);
            //    }
            //}
            //Console.ReadLine();

            //---------------------------------------------------

            //GetCategories(int) funkar, "Returnera en lista på böcker med samma category"
            //Console.WriteLine("Searching for all books with category \"Horror\": ");
            //var listOfBooks = api.GetCategories(2);
            //if (listOfBooks.Count > 0)
            //{
            //    foreach (var book in listOfBooks)
            //    {
            //        Console.WriteLine(book.Title);
            //    }
            //}
            //Console.ReadLine();

            //---------------------------------------------------

            //GetAvailableBooks(int) funkar, "Returnera en lista på böcker med samma category som har amount > 0"
            //Console.WriteLine("Searching for all available books with category \"Horror\": ");
            //var listOfAvailableBooks = api.GetAvailableBooks(2);
            //foreach (var book in listOfAvailableBooks)
            //{
            //    Console.WriteLine($"{book.Title}, Amount: {book.Amount}");
            //}
            //Console.ReadLine();

            //---------------------------------------------------

            //GetBook(int) funkar
            //Console.WriteLine("Information around all books with genere \"Horror\"");
            //var description = api.GetBook(4);
            //Console.WriteLine(description);

            //---------------------------------------------------

            //GetBooks(keyword) funkar
            //Console.WriteLine("Searching for books matching search word \"shi\"");
            //var listOfBooks = api.GetBooks("shi");
            //foreach(var book in listOfBooks)
            //{
            //    Console.WriteLine(book.Title);
            //}

            //---------------------------------------------------

            //GetAuthor(Keyword), funkar
            Console.WriteLine("Searching for books matching Author \"Stephen King\"");
            var listOfBooks = api.GetAuthor("Stephen King");
            foreach (var book in listOfBooks)
            {
                Console.WriteLine(book.Title);
            }
            Console.WriteLine();

            //---------------------------------------------------
            Console.WriteLine("Selected book to purchase: \"Doctor Sleep\"");
            var succeed = api.BuyBook(userId, 2);
            if (succeed)
            {
                Console.WriteLine("Purchase made");
            }

            //---------------------------------------------------

            //Logout() funkar, kontrollerat ssms
            api.Logout(userId);
        }
    }
}
