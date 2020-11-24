using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;

namespace Obligatorisk
{
    /// <summary>
    /// Interaction logic for UC3VisBookingerWindow.xaml
    /// </summary>
    public partial class UC3VisBookingerWindow : Window
    {
        private string status;
        private BookingSet selectedBooking;
        private KundeSet _kunde;

        public UC3VisBookingerWindow()
        {
            InitializeComponent();
        }

        public void Show(KundeSet kunde)
        {
            _kunde = kunde;
            KundenummerTextBox.DataContext = kunde;

            Bookinger.ItemsSource = kunde.BookingSet.ToList().OrderByDescending(x => x.Afhentningstidspunkt);

            Show();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            selectedBooking = (BookingSet)Bookinger.SelectedItem;
            
            // Hvis ikke der er trykket på et element i listen inden returneres null
            if (selectedBooking != null)
            {
                var radioBtn = sender as RadioButton;

                Console.WriteLine(radioBtn);

                selectedBooking.Status = radioBtn.Content.ToString().ToLower();
            }
        }

        private void Godkend_Button_Click(object sender, RoutedEventArgs e)
        {
            if(selectedBooking != null)
            {
                var bookinger = MainWindow._db.BookingSet.Where(b => _kunde.KundenummerID == b.KundeKundenummerID);
                Bookinger.ItemsSource = bookinger.ToList();
                MainWindow._db.SaveChanges();
            }
            return;
        }

        private void Slet_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedBookingLocal = (BookingSet)Bookinger.SelectedItem;

            if (selectedBookingLocal != null)
            {
                // TODO: Virker ikke!
                _kunde.BookingSet.Remove(selectedBookingLocal);
                MainWindow._db.SaveChanges();

                var bookinger = MainWindow._db.BookingSet.Where(b => _kunde.KundenummerID == b.KundeKundenummerID);
                Bookinger.ItemsSource = bookinger.ToList();

                Console.WriteLine("Bookingen er blevet slettet");
            }

            return;
        }

    }
}
