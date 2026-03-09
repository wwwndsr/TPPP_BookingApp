using BookingApp.Domain.Entities;
using BookingApp.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Factory
{
    // Factory Method паттерн
    public class RoomFactory : IRoomFactory  
    {
        public Room CreateRoom(RoomType type)  
        {
            return type switch
            {
                RoomType.Single => new SingleRoom(),
                RoomType.Lux => new LuxRoom(),
                RoomType.Apartment => new Apartment(),
                _ => new SingleRoom()
            };
        }

        // Дополнительный метод для создания по строке
        public Room CreateRoom(string typeName)
        {
            return typeName.ToLower() switch
            {
                "single" => new SingleRoom(),
                "lux" => new LuxRoom(),
                "apartment" => new Apartment(),
                _ => new SingleRoom()
            };
        }
    }
}