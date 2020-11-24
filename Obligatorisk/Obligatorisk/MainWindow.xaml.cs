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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Obligatorisk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static VærktøjsudlejningEntities2 _db;


        public MainWindow()
        {
            _db = new VærktøjsudlejningEntities2();

            InitializeComponent();
        }

        // TODO: Gør rede for, hvad denne metode gør godt for
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _db.Dispose();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            UC1OpretKundeWindow createWindow = new UC1OpretKundeWindow();
            createWindow.Show();

            return;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            string kundeinfo = KundeinfoTextBox.Text.ToLower();
            KundeSet kundeFound = null;
            UC2RedigerKundeWindow editWindow = new UC2RedigerKundeWindow();
            bool found = false;

            Console.WriteLine("Kundeinfo: "+kundeinfo);

            foreach (var kunde in _db.KundeSet)
            {
                if(kunde.Navn.Equals(kundeinfo))
                {
                    Console.WriteLine("name found");
                    kundeFound = _db.KundeSet.First(x => x.Navn.Equals(kundeinfo));
                    editWindow.Show(kundeFound);
                    found = true;
                    break;
                }
                else if (kunde.Email.Equals(kundeinfo))
                {
                    kundeFound = _db.KundeSet.First(x => x.Email.Equals(kundeinfo));
                    editWindow.Show(kundeFound);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Kunden blev ikke fundet");
            }

            return;

        }

        private void ShowBookings_Button_Click(object sender, RoutedEventArgs e)
        {
            string kundeinfo = KundeinfoTextBox.Text.ToLower();
            KundeSet kundeFound = null;
            UC3VisBookingerWindow bookingerWindow = new UC3VisBookingerWindow();

            foreach (var kunde in _db.KundeSet)
            {
                if (kunde.Navn.Equals(kundeinfo))
                {
                    kundeFound = _db.KundeSet.First(x => x.Navn.Equals(kundeinfo));
                    bookingerWindow.Show(kundeFound);
                    break;
                }
                else if (kunde.Email.Equals(kundeinfo))
                {
                    kundeFound = _db.KundeSet.First(x => x.Email.Equals(kundeinfo));
                    bookingerWindow.Show(kundeFound);
                    break;
                }
            }

            if (kundeFound == null)
            {
                MessageBox.Show("Kunden blev ikke fundet");
            }

            return;

        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            string kundeinfo = KundeinfoTextBox.Text.ToLower();
            KundeSet kundeFound = null;
            bool found = false;

            MessageBoxResult messageBoxResult = MessageBox.Show("Er du sikker på at du vil slette kunde: " + kundeinfo, "Slet kunde", MessageBoxButton.YesNo);

            if(messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var kunde in _db.KundeSet)
                {
                    if (kunde.Navn.Equals(kundeinfo))
                    {
                        Console.WriteLine("name found");
                        kundeFound = _db.KundeSet.First(x => x.Navn.Equals(kundeinfo));
                        _db.KundeSet.Remove(kundeFound);
                        found = true;
                        break;
                    }
                    else if (kunde.Email.Equals(kundeinfo))
                    {
                        kundeFound = _db.KundeSet.First(x => x.Email.Equals(kundeinfo));
                        found = true;
                        _db.KundeSet.Remove(kundeFound);
                        break;
                    }
                }

                _db.SaveChanges();

                if (!found)
                {
                    MessageBox.Show("Kunden blev ikke fundet");
                }
            }
            return;
        }
    }

}
