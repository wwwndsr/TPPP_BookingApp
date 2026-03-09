using BookingApp.BusinessLogic.Prototype;
using BookingApp.BusinessLogic.Singleton;
using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.Views
{
    public partial class CloneWindow : Window
    {
        private readonly BookingManager _bookingManager;
        private readonly BookingCloneService _cloneService;
        private Booking? _selectedBooking;

        // Исправлено: Добавлен public и переопределен ToString()
        public class BookingDisplay
        {
            public Booking Booking { get; set; } = new();

            // Вместо DisplayText используем ToString()
            public override string ToString()
            {
                return $"{Booking.GuestName} | {Booking.HotelName} | {Booking.RoomName} | €{Booking.TotalPrice}";
            }
        }

        public CloneWindow()
        {
            InitializeComponent();
            _bookingManager = BookingManager.Instance;
            _cloneService = new BookingCloneService();

            Loaded += CloneWindow_Loaded;
            sliderCopyCount.ValueChanged += SliderCopyCount_ValueChanged;
        }

        private void CloneWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookings();
            UpdatePreview();
        }

        private void LoadBookings()
        {
            var bookings = _bookingManager.GetAllBookings();

            if (bookings.Count == 0)
            {
                MessageBox.Show("Нет доступных броней для клонирования. Сначала создайте бронь.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            var displayItems = bookings.Select(b => new BookingDisplay { Booking = b }).ToList();
            cmbSourceBooking.ItemsSource = displayItems;
            cmbSourceBooking.SelectedIndex = 0;
        }

        private void CmbSourceBooking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSourceBooking.SelectedItem is BookingDisplay display)
            {
                _selectedBooking = display.Booking;
                UpdateSourceDisplay();
                UpdatePreview();
            }
        }

        private void UpdateSourceDisplay()
        {
            if (_selectedBooking == null)
            {
                txtSourceGuest.Text = "👤 Гость: не выбран";
                txtSourceHotel.Text = "🏨 Отель: -";
                txtSourceRoom.Text = "🛏️ Номер: -";
                txtSourceServices.Text = "🍽️ Услуги: -";
                txtSourcePrice.Text = "💰 Цена: -";
                return;
            }

            txtSourceGuest.Text = $"👤 Гость: {_selectedBooking.GuestName}";
            txtSourceHotel.Text = $"🏨 Отель: {_selectedBooking.HotelName}";
            txtSourceRoom.Text = $"🛏️ Номер: {_selectedBooking.RoomName}";

            string services = _selectedBooking.Services.Count > 0
                ? string.Join(", ", _selectedBooking.Services)
                : "нет";
            txtSourceServices.Text = $"🍽️ Услуги: {services}";

            txtSourcePrice.Text = $"💰 Цена: €{_selectedBooking.TotalPrice}";
        }

        private void SliderCopyCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int count = (int)sliderCopyCount.Value;
            txtCopyCount.Text = $"{count} копий";
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (_selectedBooking == null)
            {
                txtPreview.Text = "Выберите исходную бронь";
                txtPreviewNames.Text = "";
                return;
            }

            int count = (int)sliderCopyCount.Value;
            string prefix = txtNamePrefix.Text;
            bool increment = chkIncrementNumber.IsChecked == true;

            txtPreview.Text = $"Будет создано {count} копий брони для {_selectedBooking.GuestName}";

            if (increment && count > 0)
            {
                txtPreviewNames.Text = $"{prefix} 1, {prefix} 2, ... {prefix} {count}";
            }
            else
            {
                txtPreviewNames.Text = $"{prefix} (все {count} копий)";
            }
        }

        private void BtnClone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedBooking == null)
                {
                    MessageBox.Show("Выберите исходную бронь!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int count = (int)sliderCopyCount.Value;
                string prefix = txtNamePrefix.Text;
                bool saveToManager = chkSaveToManager.IsChecked == true;

                List<Booking> clones;

                if (chkIncrementNumber.IsChecked == true)
                {
                    clones = _cloneService.CloneBookingMultiple(_selectedBooking, count, prefix);
                }
                else
                {
                    clones = new List<Booking>();
                    for (int i = 0; i < count; i++)
                    {
                        var clone = _cloneService.CloneBooking(_selectedBooking, prefix);
                        clones.Add(clone);
                        if (saveToManager)
                            _bookingManager.AddBooking(clone);
                    }
                }

                string message = $"✅ Успешно создано {clones.Count} копий!\n\n" +
                                $"Исходная бронь: {_selectedBooking.GuestName}\n" +
                                $"Отель: {_selectedBooking.HotelName}\n" +
                                $"Номер: {_selectedBooking.RoomName}\n\n";

                if (clones.Count > 0)
                {
                    message += $"Первая копия: {clones[0].GuestName}\n";
                    if (clones.Count > 1)
                        message += $"Последняя копия: {clones[clones.Count - 1].GuestName}";
                }

                MessageBox.Show(message, "Клонирование завершено",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при клонировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}