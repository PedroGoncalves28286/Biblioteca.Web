using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace Biblioteca.Web.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Authors.Any())
            {
                AddAuthor("Marcele", "Proust");
                AddAuthor("J.R.R", "Tolkien");
                AddAuthor("Dante", "Alighieri");
                AddAuthor("Nathaniel", "Hawthorne");
                AddAuthor("Franz", "Kafka");
                AddAuthor("Thomas", "Mann");
                AddAuthor("George", "Orwell");
                AddAuthor("Mark", "Twain");
                AddAuthor("Gabriel", "Marquez");
                AddAuthor("Miguel", "Cervantes");

                await _context.SaveChangesAsync();
            }
            if (!_context.Genres.Any())
            {
                AddGenre("Ficcion");
                AddGenre("Romance");
                AddGenre("History");
                AddGenre("Fantasy");
                AddGenre("Mistery");
                AddGenre("NonFicion");

                await _context.SaveChangesAsync();
            }
            if (!_context.Memberships.Any())
            {
                AddMembership("João Manuel", 5, 10, 12);
                AddMembership("Rafael Santos", 5, 10, 12);
                AddMembership("Jorge Tomé", 5, 10, 12);
                AddMembership("Maria Albertina", 5, 10, 12);
                AddMembership("Maria Joana", 5, 10, 12);
                AddMembership("Branca de Neve", 5, 10, 12);

                await _context.SaveChangesAsync();
            }
            if (!_context.Newsletters.Any())
            {
                AddNewsletter(1, "A Divina Comedia", "An Italian narrative poem by Dante Alighieri", DateTime.Now);
                AddNewsletter(1,"A metamorfose", "is a novella written by Franz Kafka which was first published in 1915", DateTime.Now);
                AddNewsletter(1, "Cem anos de solidão", "apresenta uma das mais fascinantes aventuras literárias do século XX", DateTime.Now);
                AddNewsletter(1, "Guerra e Paz", "is a literary work by Russian author Leo Tolstoy. Set during the Napoleonic Wars", DateTime.Now);
                AddNewsletter(1, "Romeu e Julieta", "is a tragedy written by William Shakespeare early in his career about the romance between two Italian youths from feuding families", DateTime.Now);

                await _context.SaveChangesAsync();
            }

            if (!_context.Rentals.Any())
            {
                // Add a default rental when the "Rentals" collection is empty.
                AddRental("1000", "Kafka", "Metamorfose", 1, "Available", "9798719003528", "bertrand", DateTime.Now, DateTime.Now, 4);
                AddRental("1021", "Proust", "Em Busca do Tempo Perdido", 1, "Available", "9798724003528", "bertrand", DateTime.Now, DateTime.Now, 4);
                AddRental("1000", "VHugo", "Os Miseráveis ", 1, "Available", "9798719033528", "bertrand", DateTime.Now, DateTime.Now, 4);
                AddRental("1000", "VNabokov", "Lolita", 1, "Available", "9798259003528", "bertrand", DateTime.Now, DateTime.Now, 4);
                await _context.SaveChangesAsync();
            }

            if (!_context.Members.Any())
            {
                AddMember("Maria","Albertina","919942532",DateTime.Now,true,123456);

                await _context.SaveChangesAsync();
            }
            
        }

        private void AddMember(string firstName,string lastName,string phone,DateTime birthdate,bool disable,int membershipId)
        {
            _context.Members.Add(new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                BirthDate = birthdate,
                Disable = disable,
                MembershipID = membershipId
                
            });
        }

        private void AddReservation(string name,string userId,int reservationNumber)
        {
            _context.Reservations.Add(new Reservation
            {
                Name = name,
                UserId = userId,
                ReservationNumber = reservationNumber
            });
        }

        private void AddRental(string userId, string author, string title, int bookId, string availability, string isbn, string publisher, DateTime scheduleReturnDate, DateTime actualReturnDate, int rentalDuration)
        {
            _context.Rentals.Add(new Rental
            {
                UserId = userId,
                Author = author,
                Title = title,
                BookId = bookId,
                Availability = availability,
                ISBN = isbn,
                Publisher = publisher,
                ScheduleReturnDate = scheduleReturnDate,
                ActualReturnDate = actualReturnDate,
                RentalDuration = rentalDuration
            });
        }


        private void AddNewsletter(int newsId,string title,string content,DateTime date)
        {
            _context.Newsletters.Add(new Newsletter
            {
                NewsID = newsId,
                Title = title,
                Content = content,
                AddDate = date
             
            });
        }

        private void AddMembership(string name, byte signUpFee, byte chargeRateSixMonth, byte chargeRateTwelveMonth)
        {
            _context.Memberships.Add(new Membership
            {
                Name = name,
                SignUpFee = signUpFee,
                ChargeRateSixMonth = chargeRateSixMonth,
                ChargeRateTwelveMonth = chargeRateTwelveMonth,

            });

        }

        private void AddGenre(string name)
        {
            _context.Genres.Add(new Genre
            {
                Name = name
            });

        }

        private void AddAuthor(string firstName, string lastName )
        {
            _context.Authors.Add(new Author
            {
                FirstName = firstName,
                LastName = lastName
            });
        }
        

        


    }
}
