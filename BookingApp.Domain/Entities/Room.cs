namespace BookingApp.Domain.Entities
{
    public abstract class Room
    {
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string Description { get; set; } = string.Empty;

        public virtual string GetDescription() => Description;
    }

    public class SingleRoom : Room
    {
        public SingleRoom()
        {
            Name = "Одноместный";
            BasePrice = 200;
            Description = "Одноместный номер: 1 кровать, Wi-Fi, ТВ";
        }
    }

    public class LuxRoom : Room
    {
        public LuxRoom()
        {
            Name = "Люкс";
            BasePrice = 500;
            Description = "Люкс: двуспальная кровать, гостиная, Wi-Fi, ТВ, мини-бар";
        }
    }

    public class Apartment : Room
    {
        public Apartment()
        {
            Name = "Апартаменты";
            BasePrice = 800;
            Description = "Апартаменты: 2 комнаты, кухня, Wi-Fi, ТВ, кондиционер";
        }
    }
}