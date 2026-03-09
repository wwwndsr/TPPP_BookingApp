using BookingApp.BusinessLogic.Builder;
using BookingApp.BusinessLogic.AbstractFactory;
using BookingApp.BusinessLogic.Factory;
using BookingApp.BusinessLogic.Singleton;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Entities.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.Views
{
    public partial class MainWindow : Window
    {
        private IHotelFactory? _currentHotel;
        private Room? _currentRoom;
        private readonly IRoomFactory _roomFactory = new RoomFactory();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            rdCityHotel.IsChecked = true;
            rdSingleRoom.IsChecked = true;

            UpdateHotelInfo();
            UpdateRoomInfo();
            UpdateServicesTotal();
            UpdateSummary();
            UpdateTotalPrice();

            UpdatePlaceholderVisibility(txtGuestName, txtGuestNamePlaceholder);
            UpdatePlaceholderVisibility(txtGuestEmail, txtGuestEmailPlaceholder);
        }

        // ========== РАДИОКНОПКИ ==========

        private void rdCityHotel_Checked(object sender, RoutedEventArgs e)
        {
            _currentHotel = new CityHotelFactory();
            UpdateHotelInfo();
            UpdateSummary();
            UpdateTotalPrice();
        }

        private void rdResortHotel_Checked(object sender, RoutedEventArgs e)
        {
            _currentHotel = new ResortHotelFactory();
            UpdateHotelInfo();
            UpdateSummary();
            UpdateTotalPrice();
        }

        private void rdSingleRoom_Checked(object sender, RoutedEventArgs e)
        {
            // Исправляем: используем правильный enum
            _currentRoom = _roomFactory.CreateRoom(RoomType.Single);
            UpdateRoomInfo();
            UpdateSummary();
            UpdateTotalPrice();
        }

        private void rdLuxRoom_Checked(object sender, RoutedEventArgs e)
        {
            _currentRoom = _roomFactory.CreateRoom(RoomType.Lux);  
            UpdateRoomInfo();
            UpdateSummary();
            UpdateTotalPrice();
        }

        private void rdApartment_Checked(object sender, RoutedEventArgs e)
        {
            _currentRoom = _roomFactory.CreateRoom(RoomType.Apartment);  
            UpdateRoomInfo();
            UpdateSummary();
            UpdateTotalPrice();
        }

        // ========== ЧЕКБОКСЫ ==========

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateServicesTotal();
            UpdateSummary();
            UpdateTotalPrice();
        }

        // ========== ТЕКСТОВЫЕ ПОЛЯ ==========

        private void txtGuestName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGuestNamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtGuestName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGuestName.Text))
            {
                txtGuestNamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void txtGuestName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility(txtGuestName, txtGuestNamePlaceholder);
        }

        private void txtGuestEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGuestEmailPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtGuestEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGuestEmail.Text))
            {
                txtGuestEmailPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void txtGuestEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility(txtGuestEmail, txtGuestEmailPlaceholder);
        }

        private static void UpdatePlaceholderVisibility(TextBox textBox, TextBlock placeholder)
        {
            if (textBox == null || placeholder == null) return;
            placeholder.Visibility = string.IsNullOrWhiteSpace(textBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // ========== МЕТОДЫ ОБНОВЛЕНИЯ ==========

        private void UpdateHotelInfo()
        {
            if (txtHotelServices == null) return;
            txtHotelServices.Text = _currentHotel != null
                ? $"Доступные услуги: {_currentHotel.GetServices()}"
                : "Доступные услуги: не выбрано";
        }

        private void UpdateRoomInfo()
        {
            if (txtRoomDescription == null) return;
            txtRoomDescription.Text = _currentRoom != null
                ? _currentRoom.GetDescription()
                : "Номер не выбран";
        }

        private void UpdateServicesTotal()
        {
            if (txtServicesTotal == null) return;

            decimal total = 0;
            if (chkBreakfast.IsChecked == true) total += 50;
            if (chkTransfer.IsChecked == true) total += 150;
            if (chkMinibar.IsChecked == true) total += 100;
            if (chkLateCheckout.IsChecked == true) total += 100;
            txtServicesTotal.Text = $"Дополнительные услуги: €{total}";
        }

        private void UpdateSummary()
        {
            if (txtBookingSummary == null) return;

            string hotelName = _currentHotel?.HotelName ?? "не выбран";
            string roomName = _currentRoom?.Name ?? "не выбран";

            string services = "";
            if (chkBreakfast.IsChecked == true) services += "Завтрак ";
            if (chkTransfer.IsChecked == true) services += "Трансфер ";
            if (chkMinibar.IsChecked == true) services += "Мини-бар ";
            if (chkLateCheckout.IsChecked == true) services += "Поздний выезд ";

            if (string.IsNullOrWhiteSpace(services)) services = "нет";

            txtBookingSummary.Text = $"Отель: {hotelName} | Номер: {roomName} | Услуги: {services}";
        }

        private void UpdateTotalPrice()
        {
            if (txtTotalPrice == null) return;

            if (_currentRoom == null)
            {
                txtTotalPrice.Text = "€0";
                return;
            }

            decimal total = _currentRoom.BasePrice;
            if (chkBreakfast.IsChecked == true) total += 50;
            if (chkTransfer.IsChecked == true) total += 150;
            if (chkMinibar.IsChecked == true) total += 100;
            if (chkLateCheckout.IsChecked == true) total += 100;
            txtTotalPrice.Text = $"€{total}";
        }

        private decimal CalculateTotalPrice()
        {
            if (_currentRoom == null) return 0;
            decimal total = _currentRoom.BasePrice;
            if (chkBreakfast.IsChecked == true) total += 50;
            if (chkTransfer.IsChecked == true) total += 150;
            if (chkMinibar.IsChecked == true) total += 100;
            if (chkLateCheckout.IsChecked == true) total += 100;
            return total;
        }

        // ========== КНОПКИ ==========

        private void btnBookNow_Click(object sender, RoutedEventArgs e)  
        {
            if (_currentHotel == null || _currentRoom == null)
            {
                MessageBox.Show("Выберите отель и тип номера!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGuestName.Text))
            {
                MessageBox.Show("Введите имя гостя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGuestEmail.Text))
            {
                MessageBox.Show("Введите email!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var builder = new BookingBuilder();
            var booking = builder
                .SetGuest(txtGuestName.Text, txtGuestEmail.Text)
                .SetHotel(_currentHotel)
                .SetRoom(_currentRoom)
                .Build();

            if (chkBreakfast.IsChecked == true) booking.Services.Add("Завтрак");
            if (chkTransfer.IsChecked == true) booking.Services.Add("Трансфер");
            if (chkMinibar.IsChecked == true) booking.Services.Add("Мини-бар");
            if (chkLateCheckout.IsChecked == true) booking.Services.Add("Поздний выезд");

            booking.TotalPrice = CalculateTotalPrice();

            // Сохраняем бронь
            BookingManager.Instance.AddBooking(booking);

            string servicesText = booking.Services.Count > 0
                ? string.Join(", ", booking.Services)
                : "нет";

            MessageBox.Show(
                $"✅ БРОНИРОВАНИЕ УСПЕШНО!\n\n" +
                $"🏨 Отель: {booking.HotelName}\n" +
                $"🛏️ Номер: {booking.RoomName}\n" +
                $"👤 Гость: {booking.GuestName}\n" +
                $"📧 Email: {booking.Email}\n" +
                $"🍽️ Услуги: {servicesText}\n" +
                $"💰 Итого: €{booking.TotalPrice}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            txtStatus.Text = $"Бронирование для {txtGuestName.Text} создано";
        }

        private void BtnCloneBooking_Click(object sender, RoutedEventArgs e)  // С большой буквы как в XAML
        {
            if (BookingManager.Instance.GetAllBookings().Count == 0)
            {
                MessageBox.Show("Сначала создайте хотя бы одну бронь!", "Нет данных",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var cloneWindow = new CloneWindow();
            cloneWindow.Owner = this;
            cloneWindow.ShowDialog();
        }
    }
}