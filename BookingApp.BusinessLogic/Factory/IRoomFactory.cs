using BookingApp.Domain.Entities;
using BookingApp.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Factory
{
    public interface IRoomFactory
    {
        /// Создает номер указанного типа
        Room CreateRoom(RoomType type);
    }
}