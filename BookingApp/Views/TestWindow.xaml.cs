using System.Windows;
using System.Windows.Controls;

namespace BookingApp.Views
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            lstTest.Items.Clear();
            lstTest.Items.Add("Тестовая строка 1");
            lstTest.Items.Add("Тестовая строка 2");
            lstTest.Items.Add("Тестовая строка 3");
        }
    }
}