using System;
using System.Collections.Generic;

namespace BookingApp.Domain.Entities
{
    public class Booking : ICloneable
    {
        public int Id { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public List<string> Services { get; set; } = new();
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Новая";  

        public object Clone()
        {
            return new Booking
            {
                Id = this.Id,
                GuestName = this.GuestName,
                Email = this.Email,
                HotelName = this.HotelName,
                RoomName = this.RoomName,
                TotalPrice = this.TotalPrice,
                Services = new List<string>(this.Services),
                BookingDate = this.BookingDate,
                Status = "Копия"  
            };
        }

        public override string ToString()
        {
            return $"{GuestName} - {HotelName} - {RoomName} (€{TotalPrice})";
        }
    }
}