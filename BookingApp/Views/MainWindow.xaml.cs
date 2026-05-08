using BookingApp.BusinessLogic.Builder;
using BookingApp.BusinessLogic.AbstractFactory;
using BookingApp.BusinessLogic.Command;
using BookingApp.BusinessLogic.Factory;
using BookingApp.BusinessLogic.Singleton;
using BookingApp.BusinessLogic.Adapter;
using BookingApp.BusinessLogic.Decorator;
using BookingApp.BusinessLogic.Facade;
using BookingApp.BusinessLogic.Composite;
using BookingApp.BusinessLogic.Observer;
using BookingApp.BusinessLogic.Strategy;
using BookingApp.BusinessLogic.State;
using BookingApp.BusinessLogic.Mediator;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.Views
{
    public partial class MainWindow : Window
    {
        private IHotelFactory? _currentHotel;
        private Room? _currentRoom;
        private readonly IRoomFactory _roomFactory = new RoomFactory();
        private ICurrencyAdapter _currentCurrency;
        private readonly BookingFacade _bookingFacade;

        private List<GroupBooking> _groups = new();
        private GroupBooking? _selectedGroup;

        private EmailObserver? _emailObserver;
        private SmsObserver? _smsObserver;
        private PushObserver? _pushObserver;

        private PaymentContext? _paymentContext;
        private IPaymentStrategy? _currentPaymentStrategy;

        private CommandInvoker _commandInvoker;
        private Booking? _selectedBookingForCommand;

        private BookingContext? _selectedBookingContext;
        private List<BookingContext> _bookingContexts = new();

        private IChatMediator _chatMediator;
        private List<ChatUser> _chatUsers = new();
        public MainWindow()
        {
            InitializeComponent();
            _currentCurrency = new EuroAdapter();
            _bookingFacade = new BookingFacade();
            _chatMediator = new ChatMediator();
            Loaded += MainWindow_Loaded;

            InitializeObservers();
            InitializePaymentStrategy();
            InitializeChat();

            _commandInvoker = new CommandInvoker();
        }

        private void InitializeObservers()
        {
            _emailObserver = new EmailObserver(txtGuestEmail.Text);
            _smsObserver = new SmsObserver("+37312345678");
            _pushObserver = new PushObserver("Device-001");

            UpdateObserversSubscription();
        }

        private void InitializePaymentStrategy()
        {
            _paymentContext = new PaymentContext();
            SetDefaultPaymentStrategy();
        }

        private void SetDefaultPaymentStrategy()
        {
            var cardStrategy = new CardPaymentStrategy("****1234", "Иван Иванов", "12/26");
                if (_paymentContext != null)
                {
                    _paymentContext.SetStrategy(cardStrategy);
                }
            _currentPaymentStrategy = cardStrategy;
            //UpdatePaymentInfo();
        }

        private void UpdateObserversSubscription()
        {
            var notifier = BookingNotifier.Instance;

            if (_emailObserver != null) notifier.Detach(_emailObserver);
            if (_smsObserver != null) notifier.Detach(_smsObserver);
            if (_pushObserver != null) notifier.Detach(_pushObserver);

            if (chkEmailNotify.IsChecked == true && _emailObserver != null)
                notifier.Attach(_emailObserver);
            if (chkSmsNotify.IsChecked == true && _smsObserver != null)
                notifier.Attach(_smsObserver);
            if (chkPushNotify.IsChecked == true && _pushObserver != null)
                notifier.Attach(_pushObserver);
        }

        private void UpdateNotificationLog()
        {
            var log = "";
            if (_emailObserver?.LastNotification != null)
                log += _emailObserver.LastNotification + "\n\n";
            if (_smsObserver?.LastNotification != null)
                log += _smsObserver.LastNotification + "\n\n";
            if (_pushObserver?.LastNotification != null)
                log += _pushObserver.LastNotification;

            txtNotificationLog.Text = log;
        }

        private void UpdatePaymentInfo()
        {
            if (_paymentContext != null)
            {
                txtPaymentInfo.Text = _paymentContext.GetPaymentDescription();
            }
            else
            {
                txtPaymentInfo.Text = "Способ оплаты не выбран";
            }
        }

        private void RefreshBookingListForCommand()
        {
            var bookings = BookingManager.Instance.GetAllBookings();
            var displayItems = bookings.Select(b => new
            {
                Booking = b,
                DisplayText = $"ID: {b.Id} | {b.GuestName} | {b.HotelName} | {b.RoomName} | {b.Status}"
            }).ToList();

            cmbBookingForCommand.ItemsSource = displayItems;
            if (displayItems.Any())
                cmbBookingForCommand.SelectedIndex = 0;
        }

        private void CmbBookingForCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBookingForCommand.SelectedItem != null)
            {
                var selected = (dynamic)cmbBookingForCommand.SelectedItem;
                _selectedBookingForCommand = selected.Booking;
                UpdateCommandStatus();
            }
        }

        private void UpdateCommandStatus()
        {
            if (_selectedBookingForCommand != null)
            {
                txtCommandStatus.Text = $"📋 Бронь #{_selectedBookingForCommand.Id}\n" +
                                        $"👤 Гость: {_selectedBookingForCommand.GuestName}\n" +
                                        $"🏨 Отель: {_selectedBookingForCommand.HotelName}\n" +
                                        $"🛏️ Номер: {_selectedBookingForCommand.RoomName}\n" +
                                        $"💰 Цена: €{_selectedBookingForCommand.TotalPrice}\n" +
                                        $"📌 Статус: {_selectedBookingForCommand.Status}";
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            rdCityHotel.IsChecked = true;
            rdSingleRoom.IsChecked = true;
            rdEuro.IsChecked = true;
            rdCardPayment.IsChecked = true;  

            UpdateHotelInfo();
            UpdateRoomInfo();
            UpdateServicesTotal();
            UpdateSummary();
            UpdateTotalPrice();

            UpdatePlaceholderVisibility(txtGuestName, txtGuestNamePlaceholder);
            UpdatePlaceholderVisibility(txtGuestEmail, txtGuestEmailPlaceholder);

            RefreshGroupList();
            RefreshBookingListForCommand();
            RefreshBookingListForState();

            if (_emailObserver != null)
            {
                _emailObserver = new EmailObserver(txtGuestEmail.Text);
                UpdateObserversSubscription();
            }

            //UpdatePaymentInfo();
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

        // ========== АДАПТЕР (ВАЛЮТА) ==========

        private void Currency_Changed(object sender, RoutedEventArgs e)
        {
            if (rdEuro.IsChecked == true)
                _currentCurrency = new EuroAdapter();
            else if (rdDollar.IsChecked == true)
                _currentCurrency = new DollarAdapter();
            else if (rdLei.IsChecked == true)
                _currentCurrency = new LeiAdapter();

            UpdateTotalPrice();
            UpdateServicesTotal();
        }

        // ========== STRATEGY (СПОСОБЫ ОПЛАТЫ) ==========

        private void PaymentStrategy_Changed(object sender, RoutedEventArgs e)
        {
            if (rdCardPayment.IsChecked == true)
            {
                _currentPaymentStrategy = new CardPaymentStrategy("****1234", "Иван Иванов", "12/26");
            }
            else if (rdCashPayment.IsChecked == true)
            {
                _currentPaymentStrategy = new CashPaymentStrategy();
            }
            else if (rdPayPalPayment.IsChecked == true)
            {
                _currentPaymentStrategy = new PayPalPaymentStrategy("user@example.com");
            }

            if (_paymentContext != null && _currentPaymentStrategy != null)
            {
                _paymentContext.SetStrategy(_currentPaymentStrategy);
            }

           // UpdatePaymentInfo();
        }

        // ========== FACADE (БЫСТРОЕ БРОНИРОВАНИЕ) ==========

        private void BtnRomantic_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGuestName.Text) ||
                string.IsNullOrWhiteSpace(txtGuestEmail.Text))
            {
                MessageBox.Show("Сначала введите имя и email!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("🌹 Романтический пакет\n\n" +
                "Включено:\n" +
                "• Курортный отель (СПА + бассейн)\n" +
                "• Номер Люкс\n" +
                "• Завтрак\n" +
                "• Мини-бар\n" +
                "• Ужин при свечах\n" +
                "• Шампанское\n\n" +
                $"💰 Цена: €750\n\nЗабронировать?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var booking = _bookingFacade.QuickBookRomantic(txtGuestName.Text, txtGuestEmail.Text);

                string servicesText = string.Join(", ", booking.Services);
                string totalPriceFormatted = _currentCurrency.GetFormattedPrice(booking.TotalPrice);

                MessageBox.Show(
                    $"✅ РОМАНТИЧЕСКИЙ ПАКЕТ ЗАБРОНИРОВАН!\n\n" +
                    $"🏨 Отель: {booking.HotelName}\n" +
                    $"🛏️ Номер: {booking.RoomName}\n" +
                    $"👤 Гость: {booking.GuestName}\n" +
                    $"📧 Email: {booking.Email}\n" +
                    $"🍽️ Услуги: {servicesText}\n" +
                    $"💰 Итого: {totalPriceFormatted}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateTotalPrice();
                RefreshGroupList();
            }
        }

        private void BtnBusiness_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGuestName.Text) ||
                string.IsNullOrWhiteSpace(txtGuestEmail.Text))
            {
                MessageBox.Show("Сначала введите имя и email!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("💼 Деловой пакет\n\n" +
                "Включено:\n" +
                "• Городской отель\n" +
                "• Номер Одноместный\n" +
                "• Завтрак\n" +
                "• Трансфер\n" +
                "• Бизнес-ланч\n" +
                "• Премиум Wi-Fi\n\n" +
                $"💰 Цена: €450\n\nЗабронировать?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var booking = _bookingFacade.QuickBookBusiness(txtGuestName.Text, txtGuestEmail.Text);

                string servicesText = string.Join(", ", booking.Services);
                string totalPriceFormatted = _currentCurrency.GetFormattedPrice(booking.TotalPrice);

                MessageBox.Show(
                    $"✅ ДЕЛОВОЙ ПАКЕТ ЗАБРОНИРОВАН!\n\n" +
                    $"🏨 Отель: {booking.HotelName}\n" +
                    $"🛏️ Номер: {booking.RoomName}\n" +
                    $"👤 Гость: {booking.GuestName}\n" +
                    $"📧 Email: {booking.Email}\n" +
                    $"🍽️ Услуги: {servicesText}\n" +
                    $"💰 Итого: {totalPriceFormatted}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateTotalPrice();
                RefreshGroupList();
            }
        }

        private void BtnFamily_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGuestName.Text) ||
                string.IsNullOrWhiteSpace(txtGuestEmail.Text))
            {
                MessageBox.Show("Сначала введите имя и email!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("👨‍👩‍👧 Семейный пакет\n\n" +
                "Включено:\n" +
                "• Курортный отель (СПА + бассейн)\n" +
                "• Апартаменты\n" +
                "• Завтрак\n" +
                "• Трансфер\n" +
                "• Мини-бар\n" +
                "• Детская кроватка\n" +
                "• Игровая комната\n\n" +
                $"💰 Цена: €950\n\nЗабронировать?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var booking = _bookingFacade.QuickBookFamily(txtGuestName.Text, txtGuestEmail.Text);

                string servicesText = string.Join(", ", booking.Services);
                string totalPriceFormatted = _currentCurrency.GetFormattedPrice(booking.TotalPrice);

                MessageBox.Show(
                    $"✅ СЕМЕЙНЫЙ ПАКЕТ ЗАБРОНИРОВАН!\n\n" +
                    $"🏨 Отель: {booking.HotelName}\n" +
                    $"🛏️ Номер: {booking.RoomName}\n" +
                    $"👤 Гость: {booking.GuestName}\n" +
                    $"📧 Email: {booking.Email}\n" +
                    $"🍽️ Услуги: {servicesText}\n" +
                    $"💰 Итого: {totalPriceFormatted}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateTotalPrice();
                RefreshGroupList();
            }
        }

        // ========== COMPOSITE (ГРУППОВЫЕ БРОНИРОВАНИЯ) ==========

        private void BtnCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            string groupName = txtGroupName.Text;
            if (string.IsNullOrWhiteSpace(groupName))
            {
                MessageBox.Show("Введите название группы!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var group = new GroupBooking(groupName);
            _groups.Add(group);
            RefreshGroupList();

            txtGroupName.Text = "";
            MessageBox.Show($"Группа '{groupName}' создана!", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshGroupList()
        {
            var displayItems = _groups.Select(g => new { DisplayText = $"{g.GetName()} | €{g.GetTotalPrice()}" }).ToList();
            lstGroups.ItemsSource = displayItems;
        }

        private void LstGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGroups.SelectedIndex >= 0 && lstGroups.SelectedIndex < _groups.Count)
            {
                _selectedGroup = _groups[lstGroups.SelectedIndex];
                txtGroupInfo.Text = _selectedGroup.GetDetails();
            }
        }

        private void BtnAddCurrentBooking_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGroup == null)
            {
                MessageBox.Show("Сначала выберите группу!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentHotel == null || _currentRoom == null)
            {
                MessageBox.Show("Сначала выберите отель и номер!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGuestName.Text))
            {
                MessageBox.Show("Введите имя гостя!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var builder = new BookingBuilder();
            var booking = builder
                .SetGuest(txtGuestName.Text, txtGuestEmail.Text)
                .SetHotel(_currentHotel)
                .SetRoom(_currentRoom)
                .Build();

            booking.TotalPrice = _currentRoom.BasePrice;

            IBookingComponent decoratedBooking = new BaseBooking(booking);
            decoratedBooking = ApplyDecorators(decoratedBooking);
            booking.TotalPrice = decoratedBooking.GetPrice();

            booking.Services.Clear();
            if (chkBreakfast.IsChecked == true) booking.Services.Add("Завтрак");
            if (chkTransfer.IsChecked == true) booking.Services.Add("Трансфер");
            if (chkMinibar.IsChecked == true) booking.Services.Add("Мини-бар");
            if (chkLateCheckout.IsChecked == true) booking.Services.Add("Поздний выезд");

            var individualBooking = new IndividualBooking(booking);
            _selectedGroup.Add(individualBooking);

            RefreshGroupList();
            txtGroupInfo.Text = _selectedGroup.GetDetails();

            MessageBox.Show($"Бронь добавлена в группу '{_selectedGroup.GetName()}'!\n" +
                $"Цена: €{booking.TotalPrice}", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnShowGroupTotal_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGroup == null)
            {
                MessageBox.Show("Сначала выберите группу!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string totalFormatted = _currentCurrency.GetFormattedPrice(_selectedGroup.GetTotalPrice());

            MessageBox.Show(
                $"📊 ИНФОРМАЦИЯ О ГРУППЕ\n\n" +
                $"📋 Название: {_selectedGroup.GetName()}\n" +
                $"👥 Количество гостей: {_selectedGroup.GetPeopleCount()}\n" +
                $"💰 Общая стоимость: {totalFormatted}",
                "Групповая бронь", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ========== ЧЕКБОКСЫ ==========

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateServicesTotal();
            UpdateSummary();
            UpdateTotalPrice();
            UpdateObserversSubscription();
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

            if (_emailObserver != null)
            {
                _emailObserver = new EmailObserver(txtGuestEmail.Text);
                UpdateObserversSubscription();
            }
        }

        private static void UpdatePlaceholderVisibility(TextBox textBox, TextBlock placeholder)
        {
            if (textBox == null || placeholder == null) return;
            placeholder.Visibility = string.IsNullOrWhiteSpace(textBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // ========== МЕТОДЫ ОБНОВЛЕНИЯ UI ==========

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

            decimal totalInEuro = 0;
            if (chkBreakfast.IsChecked == true) totalInEuro += 50;
            if (chkTransfer.IsChecked == true) totalInEuro += 150;
            if (chkMinibar.IsChecked == true) totalInEuro += 100;
            if (chkLateCheckout.IsChecked == true) totalInEuro += 100;

            string convertedPrice = _currentCurrency.GetFormattedPrice(totalInEuro);
            txtServicesTotal.Text = $"Дополнительные услуги: {convertedPrice}";
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
                txtTotalPrice.Text = _currentCurrency.GetFormattedPrice(0);
                return;
            }

            decimal totalInEuro = _currentRoom.BasePrice;
            if (chkBreakfast.IsChecked == true) totalInEuro += 50;
            if (chkTransfer.IsChecked == true) totalInEuro += 150;
            if (chkMinibar.IsChecked == true) totalInEuro += 100;
            if (chkLateCheckout.IsChecked == true) totalInEuro += 100;

            txtTotalPrice.Text = _currentCurrency.GetFormattedPrice(totalInEuro);
        }

        private decimal CalculateTotalPrice()
        {
            if (_currentRoom == null) return 0;
            return _currentRoom.BasePrice;
        }

        // ========== ДЕКОРАТОР ==========

        private IBookingComponent ApplyDecorators(IBookingComponent booking)
        {
            if (chkBreakfast.IsChecked == true)
                booking = new BreakfastDecorator(booking);
            if (chkTransfer.IsChecked == true)
                booking = new TransferDecorator(booking);
            if (chkMinibar.IsChecked == true)
                booking = new MinibarDecorator(booking);
            if (chkLateCheckout.IsChecked == true)
                booking = new LateCheckoutDecorator(booking);
            return booking;
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

            booking.TotalPrice = _currentRoom.BasePrice;

            IBookingComponent decoratedBooking = new BaseBooking(booking);
            decoratedBooking = ApplyDecorators(decoratedBooking);

            string decoratorDescription = decoratedBooking.GetDescription();
            decimal decoratorPrice = decoratedBooking.GetPrice();

            bool paymentSuccess = false;

            if (_paymentContext != null)
            {
                paymentSuccess = _paymentContext.ProcessPayment(decoratorPrice);
            }
            else
            {
                MessageBox.Show("Система оплаты не инициализирована!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!paymentSuccess)
            {
                MessageBox.Show("Ошибка при оплате! Бронь не создана.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BookingManager.Instance.AddBooking(booking);

            var notifier = BookingNotifier.Instance;
            notifier.NotifyBookingCreated(booking.GuestName, booking.HotelName, decoratorPrice);
            UpdateNotificationLog();

            string servicesText = "";
            if (chkBreakfast.IsChecked == true) servicesText += "Завтрак ";
            if (chkTransfer.IsChecked == true) servicesText += "Трансфер ";
            if (chkMinibar.IsChecked == true) servicesText += "Мини-бар ";
            if (chkLateCheckout.IsChecked == true) servicesText += "Поздний выезд ";
            if (string.IsNullOrWhiteSpace(servicesText)) servicesText = "нет";

            string totalPriceFormatted = _currentCurrency.GetFormattedPrice(decoratorPrice);
            string paymentInfo = _paymentContext != null ? _paymentContext.GetPaymentDescription() : "Способ оплаты не выбран";

            MessageBox.Show(
                $"✅ БРОНИРОВАНИЕ УСПЕШНО!\n\n" +
                $"🏨 Отель: {booking.HotelName}\n" +
                $"🛏️ Номер: {booking.RoomName}\n" +
                $"👤 Гость: {booking.GuestName}\n" +
                $"📧 Email: {booking.Email}\n" +
                $"🍽️ Услуги: {servicesText}\n" +
                $"💰 Итого: {totalPriceFormatted}\n\n" +
                $"💳 Оплата: {paymentInfo}\n\n" +
                $"📝 Детали: {decoratorDescription}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            txtStatus.Text = $"Бронирование для {txtGuestName.Text} создано";
            RefreshGroupList();
            RefreshBookingListForCommand();
            RefreshBookingListForState();
        }

        private void BtnCloneBooking_Click(object sender, RoutedEventArgs e)
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

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            var adminWindow = new AdminWindow();
            adminWindow.Owner = this;
            adminWindow.ShowDialog();
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            var testWindow = new TestWindow();
            testWindow.ShowDialog();
        }

        private void BtnConfirmBooking_Click(object sender, RoutedEventArgs e)
{
    if (_selectedBookingForCommand == null)
    {
        MessageBox.Show("Выберите бронь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    var command = new ConfirmBookingCommand(_selectedBookingForCommand);
    _commandInvoker.ExecuteCommand(command);
    
    UpdateCommandStatus();
    RefreshBookingListForCommand();
    RefreshGroupList();
    RefreshBookingListForState();

            MessageBox.Show($"Бронь #{_selectedBookingForCommand.Id} подтверждена!", "Успех", 
        MessageBoxButton.OK, MessageBoxImage.Information);
}

private void BtnCancelBooking_Click(object sender, RoutedEventArgs e)
{
    if (_selectedBookingForCommand == null)
    {
        MessageBox.Show("Выберите бронь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    var command = new CancelBookingCommand(_selectedBookingForCommand);
    _commandInvoker.ExecuteCommand(command);
    
    UpdateCommandStatus();
    RefreshBookingListForCommand();
    RefreshGroupList();
    
    MessageBox.Show($"Бронь #{_selectedBookingForCommand.Id} отменена!", "Успех", 
        MessageBoxButton.OK, MessageBoxImage.Information);
}

private void BtnUndo_Click(object sender, RoutedEventArgs e)
{
    if (!_commandInvoker.CanUndo)
    {
        MessageBox.Show("Нет действий для отмены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
    }
    
    _commandInvoker.Undo();
    UpdateCommandStatus();
    RefreshBookingListForCommand();
    RefreshGroupList();
    
    MessageBox.Show("Последнее действие отменено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
}

private void BtnRedo_Click(object sender, RoutedEventArgs e)
{
    if (!_commandInvoker.CanRedo)
    {
        MessageBox.Show("Нет действий для возврата!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
    }
    
    _commandInvoker.Redo();
    UpdateCommandStatus();
    RefreshBookingListForCommand();
    RefreshGroupList();
    
    MessageBox.Show("Действие возвращено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
}
        private void RefreshBookingListForState()
{
    // Обновляем контексты для всех броней
    var bookings = BookingManager.Instance.GetAllBookings();
    _bookingContexts.Clear();
    
    var displayItems = new List<object>();
    foreach (var b in bookings)
    {
        var context = new BookingContext(b);
        _bookingContexts.Add(context);
        displayItems.Add(new 
        { 
            Context = context,
            DisplayText = $"ID: {b.Id} | {b.GuestName} | {b.HotelName} | {b.RoomName} | {context.GetStatus()}"
        });
    }
    
    cmbBookingForState.ItemsSource = displayItems;
    if (displayItems.Any())
        cmbBookingForState.SelectedIndex = 0;
}

private void CmbBookingForState_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (cmbBookingForState.SelectedItem != null)
    {
        var selected = (dynamic)cmbBookingForState.SelectedItem;
        _selectedBookingContext = selected.Context;
        UpdateStateInfo();
    }
}

private void UpdateStateInfo()
{
    if (_selectedBookingContext != null)
    {
        var booking = _selectedBookingContext.GetBooking();
        txtStateInfo.Text = $"📋 Бронь #{booking.Id}\n" +
                           $"👤 Гость: {booking.GuestName}\n" +
                           $"🏨 Отель: {booking.HotelName}\n" +
                           $"🛏️ Номер: {booking.RoomName}\n" +
                           $"💰 Цена: €{booking.TotalPrice}\n" +
                           $"📌 Статус: {_selectedBookingContext.GetStatus()}";
        
        var actions = new List<string>();
        if (_selectedBookingContext.CanConfirm()) actions.Add("✅ Подтвердить");
        if (_selectedBookingContext.CanCancel()) actions.Add("❌ Отменить");
        if (_selectedBookingContext.CanComplete()) actions.Add("🏁 Завершить");
        
        txtAvailableActions.Text = actions.Count > 0 
            ? $"Доступные действия: {string.Join(" | ", actions)}" 
            : "Нет доступных действий";
    }
}

private void BtnStateConfirm_Click(object sender, RoutedEventArgs e)
{
    if (_selectedBookingContext == null)
    {
        MessageBox.Show("Выберите бронь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }
    
    try
    {
        _selectedBookingContext.Confirm();
        UpdateStateInfo();
        RefreshBookingListForCommand();
        RefreshBookingListForState();
        RefreshGroupList();
        
        MessageBox.Show($"Бронь #{_selectedBookingContext.GetBooking().Id} подтверждена!", "Успех", 
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

private void BtnStateCancel_Click(object sender, RoutedEventArgs e)
{
    if (_selectedBookingContext == null)
    {
        MessageBox.Show("Выберите бронь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }
    
    try
    {
        _selectedBookingContext.Cancel();
        UpdateStateInfo();
        RefreshBookingListForCommand();
        RefreshBookingListForState();
        RefreshGroupList();
        
        MessageBox.Show($"Бронь #{_selectedBookingContext.GetBooking().Id} отменена!", "Успех", 
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

private void BtnStateComplete_Click(object sender, RoutedEventArgs e)
{
    if (_selectedBookingContext == null)
    {
        MessageBox.Show("Выберите бронь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }
    
    try
    {
        _selectedBookingContext.Complete();
        UpdateStateInfo();
        RefreshBookingListForCommand();
        RefreshBookingListForState();
        RefreshGroupList();
        
        MessageBox.Show($"Бронь #{_selectedBookingContext.GetBooking().Id} завершена!", "Успех", 
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
        private void InitializeChat()
        {
            _chatMediator = new ChatMediator();

            // Создаем участников
            var client = new ClientUser(_chatMediator, "Гость");
            var admin = new AdminUser(_chatMediator, "Анна");
            var bot = new BotUser(_chatMediator, "Чат-бот");

            _chatUsers.Add(client);
            _chatUsers.Add(admin);
            _chatUsers.Add(bot);

            UpdateChatInfo();

            // Приветственное сообщение
            AddChatMessage("Чат-бот: Добро пожаловать в чат поддержки! Задайте свой вопрос.");
        }

        private void UpdateChatInfo()
        {
            var userList = string.Join(", ", _chatUsers.Select(u => $"{u.GetRole()} {u.GetName()}"));
            txtChatInfo.Text = $"Участники чата: {userList}";
        }

        private void AddChatMessage(string message)
        {
            var currentMessages = lstChatLog.ItemsSource as List<string> ?? new List<string>();
            var newMessages = currentMessages.ToList();

            // Добавляем иконку в зависимости от типа сообщения
            string displayMessage = message;
            if (message.Contains("Чат-бот") && !message.Contains("Вы:"))
                displayMessage = "🤖 " + message;
            else if (message.Contains("Оператор"))
                displayMessage = "👩‍💼 " + message;
            else if (message.Contains("Вы:"))
                displayMessage = "👤 " + message;

            newMessages.Add($"[{DateTime.Now:HH:mm:ss}] {displayMessage}");
            lstChatLog.ItemsSource = newMessages;
            lstChatLog.ScrollIntoView(lstChatLog.Items[^1]);
        }

        private void TxtChatMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtChatMessage.Text == "Введите сообщение...")
            {
                txtChatMessage.Text = "";
                txtChatMessage.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtChatMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChatMessage.Text))
            {
                txtChatMessage.Text = "Введите сообщение...";
                txtChatMessage.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChatMessage.Text) ||
                txtChatMessage.Text == "Введите сообщение...")
            {
                MessageBox.Show("Введите сообщение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var message = txtChatMessage.Text;
            AddChatMessage($"Вы: {message}");

            // Отправляем сообщение от пользователя
            var client = _chatUsers.OfType<ClientUser>().FirstOrDefault();
            if (client != null)
            {
                client.Send(message);

                // Добавляем ответы от других участников
                foreach (var user in _chatUsers.Where(u => u != client))
                {
                    if (user.LastMessage != null)
                    {
                        AddChatMessage($"{user.GetRole()} {user.GetName()}: {user.LastMessage}");
                    }
                }
            }

            txtChatMessage.Text = "";
            txtChatMessage.Focus();
        }

        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new ClientUser(_chatMediator, $"Гость_{_chatUsers.Count + 1}");
            _chatUsers.Add(newUser);
            UpdateChatInfo();
            AddChatMessage($"Новый пользователь {newUser.GetName()} присоединился к чату");
        }

        private void BtnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            var newAdmin = new AdminUser(_chatMediator, $"Оператор_{_chatUsers.Count + 1}");
            _chatUsers.Add(newAdmin);
            UpdateChatInfo();
            AddChatMessage($"Новый оператор {newAdmin.GetName()} присоединился к чату");
        }

        private void BtnClearChat_Click(object sender, RoutedEventArgs e)
        {
            lstChatLog.ItemsSource = new List<string>();
            AddChatMessage("Чат очищен");
            AddChatMessage("Чат-бот: Чат очищен. Задайте новый вопрос.");
        }

    }
}

