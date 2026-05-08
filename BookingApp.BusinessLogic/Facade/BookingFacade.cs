using BookingApp.BusinessLogic.AbstractFactory;
using BookingApp.BusinessLogic.Builder;
using BookingApp.BusinessLogic.Factory;
using BookingApp.BusinessLogic.Singleton;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Entities.Enums;


namespace BookingApp.BusinessLogic.Facade
{
    /// <summary>
    /// FACADE - упрощает процесс создания сложных броней
    /// </summary>
    public class BookingFacade
    {
        private readonly IRoomFactory _roomFactory;
        private readonly IBookingBuilder _builder;
        private readonly BookingManager _bookingManager;

        public BookingFacade()
        {
            _roomFactory = new RoomFactory();
            _builder = new BookingBuilder();
            _bookingManager = BookingManager.Instance;
        }

        /// <summary>
        /// 🌹 Романтический пакет: Люкс + Завтрак + Мини-бар + Ужин
        /// </summary>
        public Booking QuickBookRomantic(string guestName, string email)
        {
            // 1. Выбираем курортный отель (СПА, бассейн)
            IHotelFactory hotel = new ResortHotelFactory();

            // 2. Создаем номер люкс
            Room room = _roomFactory.CreateRoom(RoomType.Lux);

            // 3. Строим базовую бронь
            Booking booking = _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .Build();

            // 4. Добавляем романтические услуги
            booking.Services.Add("Завтрак");
            booking.Services.Add("Мини-бар");
            booking.Services.Add("Ужин при свечах");
            booking.Services.Add("Шампанское");

            // 5. Устанавливаем цену
            booking.TotalPrice = 750;
            booking.Status = "Романтический пакет";

            // 6. Сохраняем
            _bookingManager.AddBooking(booking);

            return booking;
        }

        /// <summary>
        /// 💼 Деловой пакет: Одноместный + Завтрак + Трансфер + Wi-Fi
        /// </summary>
        public Booking QuickBookBusiness(string guestName, string email)
        {
            IHotelFactory hotel = new CityHotelFactory();
            Room room = _roomFactory.CreateRoom(RoomType.Single);

            Booking booking = _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .Build();

            booking.Services.Add("Завтрак");
            booking.Services.Add("Трансфер");
            booking.Services.Add("Бизнес-ланч");
            booking.Services.Add("Премиум Wi-Fi");

            booking.TotalPrice = 450;
            booking.Status = "Деловой пакет";

            _bookingManager.AddBooking(booking);

            return booking;
        }

        /// <summary>
        /// 👨‍👩‍👧 Семейный пакет: Апартаменты + Завтрак + Трансфер + Мини-бар
        /// </summary>
        public Booking QuickBookFamily(string guestName, string email)
        {
            IHotelFactory hotel = new ResortHotelFactory();
            Room room = _roomFactory.CreateRoom(RoomType.Apartment);

            Booking booking = _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .Build();

            booking.Services.Add("Завтрак");
            booking.Services.Add("Трансфер");
            booking.Services.Add("Мини-бар");
            booking.Services.Add("Детская кроватка");
            booking.Services.Add("Игровая комната");

            booking.TotalPrice = 950;
            booking.Status = "Семейный пакет";

            _bookingManager.AddBooking(booking);

            return booking;
        }

        /// <summary>
        /// 🎯 Кастомный пакет по параметрам
        /// </summary>
        public Booking QuickBookCustom(
            string guestName,
            string email,
            string hotelType,  // "city" или "resort"
            string roomType,    // "single", "lux", "apartment"
            List<string> services)
        {
            IHotelFactory hotel = hotelType == "city"
                ? new CityHotelFactory()
                : new ResortHotelFactory();

            RoomType roomEnum = roomType switch
            {
                "single" => RoomType.Single,
                "lux" => RoomType.Lux,
                "apartment" => RoomType.Apartment,
                _ => RoomType.Single
            };

            Room room = _roomFactory.CreateRoom(roomEnum);

            Booking booking = _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .Build();

            foreach (var service in services)
            {
                booking.Services.Add(service);
            }

            booking.TotalPrice = CalculateCustomPrice(room, services);
            booking.Status = "Кастомный пакет";

            _bookingManager.AddBooking(booking);

            return booking;
        }

        private decimal CalculateCustomPrice(Room room, List<string> services)
        {
            decimal price = room.BasePrice;

            foreach (var service in services)
            {
                price += service switch
                {
                    "Завтрак" => 50,
                    "Трансфер" => 150,
                    "Мини-бар" => 100,
                    "Поздний выезд" => 100,
                    "Ужин" => 200,
                    "СПА" => 300,
                    _ => 0
                };
            }

            return price;
        }
    }
}