using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebbShopInlamningsUppgift.Database;
using WebbShopInlamningsUppgift.Models;

namespace WebbShopInlamningsUppgift.API
{
    class WebbShopAPI
    {
        public int Login(string userName, string userPassword)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.Name == userName && u.Password == userPassword);
                    if(user != null)
                    {
                        user.IsActive = true;
                        user.SessionTimer = DateTime.Now;
                        db.SaveChanges();

                        return user.ID;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid name- or password.");  
                }
                return 0;
            }
        }
        public void Logout(int userId)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.ID == userId);
                    if (user != null)
                    {
                        user.IsActive = false;
                        user.SessionTimer = DateTime.MinValue;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        public List<BookCategory> GetCategories()
        {
            //TODO: Lägg till möjligheten att hämta alla kategorier
            List<BookCategory> listOfCategories = new List<BookCategory>();

            return listOfCategories;
        }

        public List<BookCategory> GetCategories(string keyword)
        {
            //TODO: Lägg till möjligheten att hämta alla kategorier baserat på sökresultat
            List<BookCategory> listOfCategories = new List<BookCategory>();

            return listOfCategories;
        }

        public List<BookCategory> GetCategories(int CategoryId)
        {
            //TODO: Lägg till möjligheten att hämta alla böcker med samma kategori för ID
            List<BookCategory> listOfCategories = new List<BookCategory>();

            return listOfCategories;
        }
        public List<BookCategory> GetAvailableBooks(int CategoryId)
        {
            //TODO: Lägg till möjligheten att hämta alla böcker som har Amount > 0, inom samma category
            List<BookCategory> listOfCategories = new List<BookCategory>();

            return listOfCategories;
        }

        public void GetBook(int bookId)
        {
            //TODO: Lägg till metod for Description, vad den har för titel, kategori, författare yada-yada
        }
        public void GetBooks(string keyword)
        {
            //TODO: Lägg till metod för matchande böcker (samma kategori?)
        }
        public void GetAuthor(string keyword)
        {
            //TODO: Lägg till möjligheten att lista alla böcker baserad på samma författare
        }

        //--------------------------------------------------------------------------------------
        //ADMIN-rättigheter

        public bool AddBook(int adminId, int bookID, string title, string author, int price, int amount)
        {
            return false; //TODO: if fails, returnera true om boken har adderats korrekt. 
        }

        public void SetAmount(int adminId, int bookId)
        {
            //TODO: Sätter mängden på antal böcker
        }

        public List<Users> ListUsers(int adminId)
        {
            //TODO: Lägg till möjligheten att hämta alla Användare. 
            List<Users> listOfUsers = new List<Users>();

            return listOfUsers;
        }

        public void FindUser(int adminId, string keyword)
        {
            //TODO: list matching users
        }

        public void UpdateBook(int adminId, int bookId, string title, string author, int price)
        {
            //true if success, false if fail.
        }

        public void DeleteBook(int adminId, int bookId)
        {
            //true if success, false if fail.
        }

        public void AddCategory(int adminId, string name)
        {
            //true if success, false if fail.
        }

        public void AddBookToCategory(int adminId, int bookId, int categoryId)
        {
            //true if success, false if fail.
        }

        public void UpdateCategory(int adminId, int categoryId, string name)
        {
            //true if success, false if fail.
        }

        public void DeleteCategory(int adminId, int categoryId)
        {
            //true if success, false if fail.
        }

        public void AddUser(int adminId, string userName, string userPassword)
        {
            //true if success, false if fail.
        }



    }
}
