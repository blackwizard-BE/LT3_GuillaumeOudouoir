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

namespace LT3_OEF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //maakt alle glob variablen aan
        int maxPerDag = 7;
        Bankrekening[] bankrekening = new Bankrekening[7];
        int rekeningnummercount = 0;
        bool zoeken = true;
        public MainWindow()
        {
            InitializeComponent();


        }
        public string GenereerRekeningnummer()//genereert een rekeningsnummer door alles random te doen en dan een security id uit te rekenen met restwaarden 
        {
            string rekeningnummerbegin;
            int rekeningnummer;
            Random rnd = new Random();
            if (rnd.Next(0, 2) == 0)
            {
                rekeningnummerbegin = "447";
            }
            else
            {
                rekeningnummerbegin = "091";
            }
            rekeningnummer = Convert.ToInt32($"{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}{rnd.Next(0, 10)}");
            double controle = Convert.ToDouble($"{rekeningnummerbegin}{rekeningnummer}");
            controle = controle % 97;
            if (controle == 0)
            {
                controle = 97;
            }
            return $"{rekeningnummerbegin}-{rekeningnummer}-{controle}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)//voegt persoonlijke info toe aan de class en cleart dan alle textboxen om zo klaar te staan voor de volgende klant
        {
            try
            {

                bankrekening[rekeningnummercount] = new Bankrekening();
                bankrekening[rekeningnummercount].Name = txbNaam.Text;
                bankrekening[rekeningnummercount].Adres = txbAdres.Text;
                bankrekening[rekeningnummercount].Postcode = Convert.ToInt32(txbPostcode.Text);
                bankrekening[rekeningnummercount].Woonplaats = txbWoonplaats.Text;
                bankrekening[rekeningnummercount].Saldo = 0;
                bankrekening[rekeningnummercount].RekeningNummer = GenereerRekeningnummer();
                lsbBankrekeningen.Items.Add($"{rekeningnummercount + 1}:        {bankrekening[rekeningnummercount].RekeningNummer}");
                cmbVan.Items.Add(bankrekening[rekeningnummercount].RekeningNummer);
                cmbNaar.Items.Add(bankrekening[rekeningnummercount].RekeningNummer);
                rekeningnummercount++;
                txbNaam.Clear();
                txbAdres.Clear();
                txbWoonplaats.Clear();
                txbPostcode.Clear();
                if (rekeningnummercount + 1 == (maxPerDag))
                {
                    MessageBox.Show("je hebt je maximum bereikt.");
                    btnAdd.IsEnabled = false;
                }
                labelUpdate();
            }
            catch (Exception error)
            {
                MessageBox.Show($"geef juiste waarden in aub");
            }
        }



        private void lsbBankrekeningen_SelectionChanged(object sender, SelectionChangedEventArgs e)//kijkt of er iets geselecteerd is en als er een item geselecteerd is plaatst de info van de klant terug in de textboxen
        {
            if (lsbBankrekeningen.SelectedIndex != -1)
            {
                int index = lsbBankrekeningen.SelectedIndex;
                txbNaam.Text = bankrekening[index].Name;
                txbAdres.Text = bankrekening[index].Adres;
                txbPostcode.Text = bankrekening[index].Postcode.ToString();
                txbWoonplaats.Text = bankrekening[index].Woonplaats;
                btnStorting.IsEnabled = true;
                btnOphaling.IsEnabled = true;
                btnOverschrijven.IsEnabled = true;
            }
        }

        private void btnStorting_Click(object sender, RoutedEventArgs e)//stort het gegeven aantal geld in de rekening met behulp van class
        {
            try
            {
                if (lsbBankrekeningen.SelectedIndex != -1)
                {
                    int index = lsbBankrekeningen.SelectedIndex;
                    MessageBox.Show($"Storting successvol nieuwe bedrag ${bankrekening[index].Storting(Convert.ToDouble(txbBedrag.Text))}");
                    txbBedrag.Clear();
                }
            }
            catch
            {
                if (txbBedrag.Text != null)
                {
                    MessageBox.Show("Je hebt een verkeerde waarde ingegeven");
                }
                else
                {
                    MessageBox.Show("Je hebt geen waarde ingegeven");
                }
            }
            labelUpdate();
        }

        private void btnOphaling_Click(object sender, RoutedEventArgs e)//haalt het gegeven aantal geld uit de rekening met behulp van class
        {
            try
            {
                if (lsbBankrekeningen.SelectedIndex != -1)
                {
                    int index = lsbBankrekeningen.SelectedIndex;
                    MessageBox.Show($"Storting successvol nieuwe bedrag ${bankrekening[index].Ophaling(Convert.ToDouble(txbBedrag.Text))}");
                    txbBedrag.Clear();
                }
            }
            catch
            {
                if (txbBedrag.Text != null)
                {
                    MessageBox.Show("Je hebt een verkeerde waarde ingegeven");
                }
                else
                {
                    MessageBox.Show("Je hebt geen waarde ingegeven");
                }
            }
            labelUpdate();
        }

        private void btnOverschrijven_Click(object sender, RoutedEventArgs e)//gebruikt de ophalen en overschrijven class methodes om een overschrijving uit te voeren
        {
            bankrekening[rekeningSearch(cmbVan.Text)].Ophaling(Convert.ToDouble(txbBedrag.Text));
            bankrekening[rekeningSearch(cmbNaar.Text)].Storting(Convert.ToDouble(txbBedrag.Text));
            labelUpdate();
            txbBedrag.Clear();


        }

        public void labelUpdate()//updatet het saldolabel om visualisatie te voorzien
        {
            int imax = lsbBankrekeningen.Items.Count;
            lblSaldo.Content = "";
            for (int i = 0; i < imax; i++)
            {
                lblSaldo.Content = $"{lblSaldo.Content}\nSaldo rekening {i + 1}= $ {Math.Round(bankrekening[i].Saldo, 2)}";
            }
        }

        private void txbZoeken_MouseEnter(object sender, MouseEventArgs e)
        {
            if (zoeken)
            {
                zoeken = false;
                txbZoeken.Clear();
            }
        }

        private void txbZoeken_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txbZoeken.Text == "")
            {
                txbZoeken.Text = "Naam/Adres/Postcode/Woonplaats";
                zoeken = true;
            }
        }

        private void btnZoeken_Click(object sender, RoutedEventArgs e)//zoekt iedere waarde uit iedere class en kijkt of deze gelijk is aan de gegeven waarde indien niet geeft hij een messagebox met die error indien wel toont hij de gegevens van deze persoon
        {
            int index = 69;

            if (index == 69)
            {
                index = naamSearch(txbZoeken.Text);
            }
            if (index == 69)
            {
                index = AdresSearch(txbZoeken.Text);
            }
            if (index == 69)
            {
                index = PostcodeSearch(txbZoeken.Text);
            }
            if (index == 69)
            {
                index = WoonplaatsSearch(txbZoeken.Text);
            }
            if (index == 69)
            {
                MessageBox.Show("gebruiker niet gevonden. Check gegevens.");
            }
            else
            {
                MessageBox.Show($"Naam: {bankrekening[index].Name}\nAdres: {bankrekening[index].Adres}\nPostcode: {bankrekening[index].Postcode.ToString()}\nWoonplaats: {bankrekening[index].Woonplaats}\nRekeningsnummer: {bankrekening[index].RekeningNummer}\nSaldo: {bankrekening[index].Saldo}");

            }






        }
        public int rekeningSearch(string Text)//zoekt naar het juiste rekeningsnummer vanuit de gegeven waarde om zo dan een overschrijving te kunnen doen
        {
            bool search = true;
            int i = 0;
            do
            {
                if (Text == bankrekening[i].RekeningNummer)
                {
                    search = false;
                }
                else
                {
                    i++;
                }



            }
            while (search);
            return i;
        }
        public int naamSearch(string Text)//zoekt naar de juiste naam vanuit de gegeven waarde om zo dan een overschrijving te kunnen doen
        {
            bool search = true;
            int i = 0;
            int imax = lsbBankrekeningen.Items.Count;


            do
            {
                if (i == imax)
                {
                    return 69;
                }
                else if (Text == bankrekening[i].Name)
                {
                    search = false;
                }
                else
                {
                    i++;
                }



            }
            while (search);
            return i;
        }
        public int PostcodeSearch(string Text)//zoekt naar de juiste postcode vanuit de gegeven waarde om zo dan een overschrijving te kunnen doen
        {
            bool search = true;
            int i = 0;
            int imax = lsbBankrekeningen.Items.Count;


            do
            {
                if (i == imax)
                {
                    return 69;
                }
                else if (Text == bankrekening[i].Postcode.ToString())
                {
                    search = false;
                }
                else
                {
                    i++;
                }



            }
            while (search);
            return i;
        }

        public int WoonplaatsSearch(string Text)//zoekt naar de juiste woonplaats vanuit de gegeven waarde om zo dan een overschrijving te kunnen doen
        {
            bool search = true;
            int i = 0;
            int imax = lsbBankrekeningen.Items.Count;


            do
            {
                if (i == imax)
                {
                    return 69;
                }
                else if (Text == bankrekening[i].Woonplaats)
                {
                    search = false;
                }
                else
                {
                    i++;
                }



            }
            while (search);
            return i;
        }

        public int AdresSearch(string Text)//zoekt naar het juiste adress vanuit de gegeven combobox om zo dan een overschrijving te kunnen doen
        {
            bool search = true;
            int i = 0;
            int imax = lsbBankrekeningen.Items.Count;


            do
            {
                if (i == imax)
                {
                    return 69;
                }
                else if (Text == bankrekening[i].Adres)
                {
                    search = false;
                }
                else
                {
                    i++;
                }



            }
            while (search);
            return i;
        }




    }
}




