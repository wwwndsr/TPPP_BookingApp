using BookingApp.BusinessLogic.Proxy;
using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.Views
{
    public partial class AdminWindow : Window
    {
        private readonly AdminProxy _adminProxy;

        public AdminWindow()
        {
            InitializeComponent();
            _adminProxy = new AdminProxy();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password;

            if (_adminProxy.Authenticate(password))
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                AdminPanel.Visibility = Visibility.Visible;

                txtStatus.Text = "Авторизация успешна!";
                txtLoginError.Text = "";
                LoadBookings();
            }
            else
            {
                txtLoginError.Text = "Неверный пароль! Попробуйте еще раз.";
                txtPassword.Password = "";
            }
        }

        private void LoadBookings()
        {
            try
            {
                var bookings = _adminProxy.GetAllBookings();

                lstBookings.Items.Clear();

                if (bookings.Count == 0)
                {
                    lstBookings.Items.Add("❌ Нет броней в системе");
                }
                else
                {
                    foreach (var b in bookings)
                    {
                        string item = $"ID: {b.Id} | {b.GuestName} | {b.HotelName} | {b.RoomName} | €{b.TotalPrice} | {b.Status}";
                        lstBookings.Items.Add(item);
                    }
                }

                txtStatus.Text = $"✅ Загружено {bookings.Count} броней";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                txtStatus.Text = "Ошибка загрузки";
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadBookings();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _adminProxy.Logout();

            LoginPanel.Visibility = Visibility.Visible;
            AdminPanel.Visibility = Visibility.Collapsed;

            txtPassword.Password = "";
            txtStatus.Text = "🔒 Требуется авторизация";
            txtLoginError.Text = "";
            lstBookings.Items.Clear();
        }
    }
}