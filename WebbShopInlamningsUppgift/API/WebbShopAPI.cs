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
                                if(book.Amount == 0)
                                {
                                    Console.WriteLine("Book not available in stock.");
                                    return false;
                                }
                                if(book.Amount > 0)
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
                                    book.Amount -= 1;
                                    db.SaveChanges();
                                    return true;
                                }
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

        public string Ping(int userId)
        {
            using (var db = new WebbshopContext())
            {
                var user = db.Users.FirstOrDefault(u => u.ID == userId);
                if (user.IsActive)
                {
                    var timer = DateTime.Now - user.SessionTimer;
                    var minutes = timer.TotalMinutes;
                    if(minutes < 15)
                    {
                        return "Pong";
                    }
                }
                return string.Empty;
            }
        }

        public bool Register(string name, string password, string passwordVerify)
        {
            using (var db = new WebbshopContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Name == name);
                if(user == null && password == passwordVerify)
                {
                    var newlyCreatedUser = new Users
                    {
                        Name = name,
                        Password = password
                    };
                    db.Users.Add(newlyCreatedUser);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }

        }

        //--------------------------------------------------------------------------------------
        //ADMIN-functionality

        public bool AddBook(int adminId, string title, string author, int price, int amount, int bookID = default)
        {
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var book = db.Books.FirstOrDefault(b => b.Title == title);
                        if(book == null)
                        {
                            var newBook = new Books
                            {
                                Title = title,
                                Author = author,
                                Price = price,
                                Amount = amount
                            };
                            db.Books.Add(newBook);
                            db.SaveChanges();
                            return true;
                        }
                        if(book != null)
                        {
                            book.Amount += 1;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to add book");
                }
                return false;
            }
        }

        //Assumption that this method should return true or false
        public bool SetAmount(int adminId, int bookId, int newAmount)
        {
            
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var book = db.Books.FirstOrDefault(b => b.ID == bookId);
                        if (book != null)
                        {
                            book.Amount = newAmount;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to add book");
                }
                return false;
            }
        }

        public List<Users> ListUsers(int adminId)
        {
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var listOfUsers = db.Users.OrderBy(c => c.Name).ToList();
                        if (listOfUsers.Count > 0)
                        {
                            return listOfUsers;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find users");
                }
                return new List<Users>();
            }
        }

        public List<Users> FindUser(int adminId, string keyword)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var listOfUsers = db.Users.Where(u => u.Name.Contains(keyword)).ToList();
                        if (listOfUsers.Count > 0)
                        {
                            return listOfUsers;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find book");
                }
                return new List<Users>();
            }
        }

        public bool UpdateBook(int adminId, int bookId, string title, string author, int price)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var book = db.Books.FirstOrDefault(b => b.ID == bookId);
                        if (book != null)
                        {
                            book.Title = title;
                            book.Author = author;
                            book.Price = price;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to update book");
                }
                return false;
            }
        }

        public bool DeleteBook(int adminId, int bookId)
        {
            // would like to add ON DELETE SET NULL (Unsure of correct set up for this one, possibly modelbuilder in context(?))
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var book = db.Books.FirstOrDefault(b => b.ID == bookId);
                        if(book != null)
                        {
                            if(book.Amount > 0)
                            {
                                book.Amount -= 1;
                                db.SaveChanges();
                                return true;
                            }
                            if(book.Amount == 0)
                            {
                                db.Books.Remove(book);
                                db.SaveChanges();
                                return true;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return false;
            }
        }

        public bool AddCategory(int adminId, string name)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var category = db.BookCategories.FirstOrDefault(b => b.Genere == name);
                        if (category == null)
                        {
                            var newBookCategory = new BookCategory
                            {
                                Genere = name
                            };
                            db.BookCategories.Add(newBookCategory);
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to add category");
                }
                return false;
            }
        }

        public bool AddBookToCategory(int adminId, int bookId, int categoryId)
        {
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var book = db.Books.FirstOrDefault(b => b.ID == bookId);
                        var bookCategory = db.BookCategories.FirstOrDefault(b => b.ID == categoryId);
                        
                        book.BookCategoryId = bookCategory.ID;
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to set book category");
                }
                return false;
            }
        }

        public bool UpdateCategory(int adminId, int categoryId, string name)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var bookCategory = db.BookCategories.FirstOrDefault(b => b.ID == categoryId);
                        if (bookCategory != null)
                        {
                            bookCategory.Genere = name;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to update category");
                }
                return false;
            }
        }

        public bool DeleteCategory(int adminId, int categoryId)
        {
            //would like to add ON DELETE SET NULL (Unsure of correct set up for this one, possibly modelbuilder in context(?))
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var category = db.BookCategories.FirstOrDefault(b => b.ID == categoryId);
                        if(category != null)
                        {
                            var categoryRelation = db.Books
                                .Include(b => b.BookCategory)
                                .Where(b => b.BookCategoryId == categoryId).ToList();

                            if(categoryRelation.Count == 0)
                            {
                                db.BookCategories.Remove(category);
                                db.SaveChanges();
                                return true;
                            }
                            if(categoryRelation.Count > 0)
                            {
                                Console.WriteLine("Failed to delete category, there are books related to this category.");
                                return false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return false;
            }
        }

        public bool AddUser(int adminId, string userName, string userPassword = "Codic2021")
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminId).IsAdmin;
                    if (adminUser)
                    {
                        var user = db.Users.FirstOrDefault(u => u.Name == userName);
                        if (user == null)
                        {
                            var newlyCreatedUser = new Users
                            {
                                Name = userName,
                                Password = userPassword
                            };
                            db.Users.Add(newlyCreatedUser);
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to create user");
                }
                return false;
            }
        }

        //--------------------------------------------------------------------------------------

        public List<SoldBooks> SoldItems(int adminID)
        {
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        var soldBooks = db.SoldBooks.OrderBy(b => b.Title).ToList();
                        if (soldBooks != null)
                        {
                            return soldBooks;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to find sold items");
                }
                return new List<SoldBooks>();
            }

        }

        public int MoneyEarned(int adminID)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        var totalAmount = db.SoldBooks.Sum(b => b.Price);
                        if (totalAmount > 0)
                        {
                            return totalAmount;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to get total price");
                }
                return 0;
            }

        }

        //Retrieving data with group by searched in ISBN 978-1-80056-810-5, page 425 and 431
        //https://stackoverflow.com/questions/16522645/linq-groupby-sum-and-count
        public int BestCustomer(int adminID)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        // First create groups of userId for every userId in SoldBooks Table, count number of datarows(sold books) order by highest up top(DESC), select top 1 row.
                        //return "key" which is the userId of that customer(due to grouped by)
                        var customers = db.SoldBooks.AsEnumerable()
                            .GroupBy(u => u.UsersId)
                            .OrderByDescending(u => u.Count())
                            .FirstOrDefault();

                        return customers.Key;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return 0;
            }

        }

        public bool Promote(int adminID, int userID)
        {
            using(var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        var user = db.Users.FirstOrDefault(u => u.ID == userID);
                        if (user != null)
                        {
                            user.IsAdmin = true;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to promote user");
                }
                return false;
            }
        }

        public bool Demote(int adminID, int userID)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        var user = db.Users.FirstOrDefault(u => u.ID == userID);
                        if (user != null)
                        {
                            user.IsAdmin = false;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to demote user");
                }
                return false;
            }
        }

        public bool InactivateUser(int adminID, int userID)
        {
            using (var db = new WebbshopContext())
            {
                try
                {
                    var adminUser = db.Users.FirstOrDefault(u => u.ID == adminID).IsAdmin;
                    if (adminUser)
                    {
                        var user = db.Users.FirstOrDefault(u => u.ID == userID);
                        if (user != null)
                        {
                            user.IsActive = false;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to inactivate user");
                }
                return false;
            }
        }




    }
}
