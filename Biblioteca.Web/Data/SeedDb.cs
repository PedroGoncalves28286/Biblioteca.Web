using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace Biblioteca.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public SeedDb(
            DataContext context,
            IUserHelper userHelper,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userHelper = userHelper;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Staff");
            await _userHelper.CheckRoleAsync("Reader");

            if (!_context.Users.Any())
            {
                var libraries = new List<Library>();
                libraries.Add(new Library { Name = "Biblioteca Municipal de Lisboa" });
                libraries.Add(new Library { Name = "Biblioteca Municipal de Oeiras" });
                libraries.Add(new Library { Name = "Biblioteca Municipal de Cascais" });

                _context.Cities.Add(new City
                {
                    Libraries = libraries,
                    Name = "Lisboa"
                });

                await _context.SaveChangesAsync();
            }


            var user = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Pedro",
                    LastName = "Goncalves",
                    Email = "pedro@gmail.com",
                    UserName = "pedro@gmail.com",
                    PhoneNumber = "919945526",
                    Address = "Rua da Liberdade",
                    LibraryId = _context.Cities.FirstOrDefault().Libraries.FirstOrDefault().Id,
                    Library = _context.Cities.FirstOrDefault().Libraries.FirstOrDefault(),
                    RegistrationDate = DateTime.Now
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Authors.Any())
            {
                AddAuthor("Marcele", "Proust", user);
                AddAuthor("J.R.R", "Tolkien",user);
                AddAuthor("Dante", "Alighieri",user);
                AddAuthor("Nathaniel", "Hawthorne",user);
                AddAuthor("Franz", "Kafka",user);
                AddAuthor("Thomas", "Mann",user);
                AddAuthor("George", "Orwell",user);
                AddAuthor("Mark", "Twain",user);
                AddAuthor("Gabriel", "Marquez",user);
                AddAuthor("Miguel", "Cervantes",user);

                await _context.SaveChangesAsync();
            }
            if (!_context.Genres.Any())
            {
                AddGenre("Fiction", user);
                AddGenre("Adventure", user);
                AddGenre("Romance",user);
                AddGenre("History",user);
                AddGenre("Fantasy",user);
                AddGenre("Mistery",user);
                AddGenre("NonFiction", user);
                AddGenre("Novel", user);

                await _context.SaveChangesAsync();
            }
            
            if (!_context.Newsletters.Any())
            {
                AddNewsletter(1, "Divine Comedy", "The Divine Comedy is an epic poem by Dante Alighieri in the early 14th century. It consists of three parts: Inferno, Purgatorio, and Paradiso. The poem follows Dante's journey through the afterlife, guided first by the poet Virgil and later by his beloved Beatrice.", DateTime.Now,user);
                AddNewsletter(1, "The Metamorphosis ", "The Metamorphosis is a novella by Franz Kafka about a man who inexplicably transforms into a giant insect. Traveling salesman Gregor Samsa wakes up one morning to discover that he has transformed into a giant insect. His metamorphosis makes it impossible for him to work. When Gregor finally opens the door, his hideous figure frightens his family.", DateTime.Now,user);
                AddNewsletter(1, "One Hundred Years of Solitude", "One Hundred Years of Solitude is a magical realist novel from the author Gabriel García Márquez that tells the multi-generational story of the Buendía family in the fictional town of Macondo. It explores themes of love, power, and the cyclical nature of history. The novel is known for its blending of reality and fantasy, with characters facing surreal events and a recurring curse of solitude and repetition. It's a rich tapestry of characters and events spanning a century, creating a unique and mesmerizing narrative.", DateTime.Now,user);
                AddNewsletter(1, "War and Peace", " War and Peace is a novel by Leo Tolstoy that explores the lives of several Russian aristocratic families during the Napoleonic Wars. It delves into themes of love, war, and human nature while providing a detailed account of the historical events. The novel is known for its rich characters and philosophical reflections on history and free will. It remains a classic that offers a comprehensive view of society and history while exploring the complexities of human existence.", DateTime.Now,user);
                AddNewsletter(1, "Romeu ad Juliet", " Romeo and Juliet is a classic Shakespearean tragedy about two young lovers, Romeo and Juliet, from feuding families in Verona. They fall in love but are separated by a series of unfortunate events and misunderstandings. Believing each other to be dead, they both tragically take their own lives. Their deaths lead to the reconciliation of their families. The play explores themes of love, fate, and the destructive impact of family conflicts.", DateTime.Now,user);

                await _context.SaveChangesAsync();
            }

            if (!_context.Books.Any())
            {
                // Add a default rental when the "Rentals" collection is empty.
                AddBook("1000", "Kafka", "Metamorfose", "Fiction", true, "9798719003521", "bertrand", user);
                AddBook("1001", "Proust", "Em Busca do Tempo Perdido", "Adventure", false, "9798724003522", "bertrand", user);
                AddBook("1002", "VHugo", "Os Miseráveis ", "Novel", true, "9798719033523", "bertrand", user);
                AddBook("1003", "VNabokov", "Lolita", "Romance", true, "9798259003524", "bertrand", user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Members.Any())
            {
                AddMember("Maria", "Albertina", "919942532", DateTime.Now, user);

                await _context.SaveChangesAsync();
            }

            if (!_context.Reservations.Any())
            {
                AddReservation("1984", "1001", 3, user);
                AddReservation("Dom Quixote", "1002", 4, user);

                await _context.SaveChangesAsync();
            }
            
        }

        private void AddMember(string firstName, string lastName, string phone, DateTime birthdate, User user)
        {
            _context.Members.Add(new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                BirthDate = birthdate,
                User = user


                
            });
        }

        private void AddReservation(string name,string borrower,int reservationNumber,User user)
        {
            _context.Reservations.Add(new Reservation
            {
                Name = name,
                Borrower = borrower,
                ReservationNumber = reservationNumber,
                User = user
            });
        }

        private void AddBook(string borrower, string author, string title, string genreName, bool isAvailable,
            string isbn, string publisher, User user)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Name == genreName);

            _context.Books.Add(new Book
            {
                Borrower = borrower,
                Author = author,
                Title = title,
                GenreName = genre.Name,
                //IsAvailable = isAvailable,
                ISBN = isbn,
                Publisher = publisher,
                User = user,
                
            });
        }

        private void AddNewsletter(int newsId,string title,string content,DateTime date, User user)
        {
            _context.Newsletters.Add(new Newsletter
            {
                NewsID = newsId,
                Title = title,
                Content = content,
                AddDate = date,
                User = user
             
            });
        }

        private void AddGenre(string name, User user)
        {
            _context.Genres.Add(new Genre
            {
                Name = name,
                User = user
            });

        }

        private void AddAuthor(string firstName, string lastName, User user )
        {
            _context.Authors.Add(new Author
            {
                FirstName = firstName,
                LastName = lastName
            
            });
        }
        

    }
}
