using System.Linq;
using System.Windows;

namespace Obligatorisk
{
    /// <summary>
    /// Interaction logic for UC2RedigerKundeWindow.xaml
    /// </summary>
    public partial class UC2RedigerKundeWindow : Window
    {
        public UC2RedigerKundeWindow()
        {
            InitializeComponent();
        }

        public void Show(KundeSet kunde)
        {
            KundenummerTextBox.DataContext = kunde;
            NavnTextBox.DataContext = kunde;
            AdresseTextBox.DataContext = kunde;
            EmailTextBox.DataContext = kunde;
            Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int findID = int.Parse(KundenummerTextBox.Text);
            KundeSet kunde = MainWindow._db.KundeSet.First(x => x.KundenummerID == findID);
            kunde.Navn = NavnTextBox.Text;
            kunde.Adresse = AdresseTextBox.Text;
            kunde.Email = EmailTextBox.Text;

            MainWindow._db.SaveChanges();

            this.Close();
        }


    }
}
