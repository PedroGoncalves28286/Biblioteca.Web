﻿using System.ComponentModel;

namespace Biblioteca.Web.Data.Entities
{
    public class Reservation : IEntity
    {
        
        public int Id { get; set; } 

        public string Name { get; set; }

        public string UserId { get; set; }  

        public int ReservationNumber { get; set; }  
    }
}
