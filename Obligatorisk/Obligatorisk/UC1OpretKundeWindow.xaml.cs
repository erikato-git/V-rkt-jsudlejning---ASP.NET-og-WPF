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


namespace Obligatorisk
{
    /// <summary>
    /// Interaction logic for UC1OpretKundeWindow.xaml
    /// </summary>
    public partial class UC1OpretKundeWindow : Window
    {
        public UC1OpretKundeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Lav regex-validation
            string name = NameTextBox.Text.ToLower();
            string adress = AdressTextBox.Text.ToLower();
            string email = EmailTextBox.Text.ToLower();

            if(name == "" || adress == "" || email == "")
            {
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }

            KundeSet kunde = new KundeSet(){ Navn = name, Adresse = adress, Email = email};

            MainWindow._db.KundeSet.Add(kunde);
            MainWindow._db.SaveChanges();

            Console.WriteLine("Ny kunde gemt.............");


            this.Close();
        }


    }
}
