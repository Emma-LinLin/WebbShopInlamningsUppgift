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
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfCategories = db.BookCategories.OrderBy(c => c.Genere).ToList();
                    if(listOfCategories.Count > 0)
                    {
                        return listOfCategories;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find book categories");
                }
                return new List<BookCategory>();
            }
            
        }

        public List<BookCategory> GetCategories(string keyword)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfCategories = db.BookCategories.Where(b => b.Genere.Contains(keyword)).ToList();
                    if (listOfCategories.Count > 0)
                    {
                        return listOfCategories;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find book categories based on input");
                }
                return new List<BookCategory>();
            }
        }

        public List<Books> GetCategories(int CategoryId)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfBooks = db.Books.Include(b => b.BookCategory).Where(b => b.BookCategoryId == CategoryId).ToList();
                    if (listOfBooks.Count > 0)
                    {
                        return listOfBooks;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find books based on input");
                }
                return new List<Books>();
            }
        }
        public List<Books> GetAvailableBooks(int CategoryId)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfAvailableBooks = db.Books.Include(b => b.BookCategory)
                        .Where(b => b.BookCategoryId == CategoryId)
                        .Where(b => b.Amount > 0).ToList();
                    
                    if (listOfAvailableBooks.Count > 0)
                    {
                        return listOfAvailableBooks;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find books based on input");
                }
                return new List<Books>();
            }
        }

        public string GetBook(int bookId)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var book = db.Books.Include(b => b.BookCategory)
                        .FirstOrDefault(b => b.ID == bookId);
                    if(book != null)
                    {
                        return $"Title: {book.Title} - Author: {book.Author}\nGenere: {book.BookCategory.Genere}, Price: {book.Price}";
                    }
                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find book");
                }
                return string.Empty;
            }
        }
        public List<Books> GetBooks(string keyword)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfBooks = db.Books.Where(b => b.Title.Contains(keyword)).ToList();
                    if (listOfBooks.Count > 0)
                    {
                        return listOfBooks;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find book");
                }
                return new List<Books>();
            }
        }
        public List<Books> GetAuthor(string keyword)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var listOfBooks = db.Books.Where(b => b.Author.Contains(keyword)).ToList();
                    if (listOfBooks.Count > 0)
                    {
                        return listOfBooks;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find books");
                }
                return new List<Books>();
            }
        }

        public bool BuyBook(int userID, int bookID)
        {
            //TODO: Lägg till möjligheten att köpa en bok om användare fortfarande är aktiv
            using (var db = new WebbshopContext())
            {
                try
                {
                    var user = db.Users.FirstOrDefault(u => u.ID == userID);
                    if(user == null)
                    {
                        Console.WriteLine("User does not exist");
                        return false;
                    }

                    var isUserActive = user.IsActive;
                    if (isUserActive)
                    {
                        var timer = DateTime.Now - user.SessionTimer;
                        var minutes = timer.TotalMinutes;
                        if(minutes < 5)
                        {
                            var book = db.Books.Include(b => b.BookCategory).FirstOrDefault(b => b.ID == bookID);
                            if(book != null)
                            {
                                var soldBook = new SoldBooks 
                                { 
                                    Title = book.Title, 
                                    Author = book.Author, 
                                    Price = book.Price, 
                                    Amount = book.Amount, 
                                    BookCategory = book.BookCategory, 
                                    UsersId = userID, 
                                    PurchaseDate = DateTime.Now
                                };
                                db.SoldBooks.Add(soldBook);
                                db.SaveChanges();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed purchase");
                }
                return false;
            }
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
