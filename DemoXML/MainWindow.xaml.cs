using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;

namespace DemoXML
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Txt_Estrai.IsReadOnly = true;
            Txt_DatiSerie.IsReadOnly = true;
            Rtg_Mod.Visibility = Visibility.Hidden;
            Btn_Salva.Visibility = Visibility.Hidden;
            Lbl_TitoloMod.Visibility = Visibility.Hidden;
            Lbl_StagioniMod.Visibility = Visibility.Hidden;
            Lbl_EpisodiMod.Visibility = Visibility.Hidden;
            Txt_NomeMod.Visibility = Visibility.Hidden;
            Txt_StagioniMod.Visibility = Visibility.Hidden;
            Txt_EpisodiMod.Visibility = Visibility.Hidden;

        }

        CancellationTokenSource ct;

        private void Btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            Lst_Lista.Items.Clear();
            Task.Factory.StartNew(() => CaricaDati());
        }

        private void CaricaDati()
        {
            string path = @"presenze.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlNetflix = xmlDoc.Element("netflix");
            var xmlSerie = xmlNetflix.Elements("serie");
            foreach (var item in xmlSerie)
            {
                XElement xmlNome = item.Element("nome");
                XElement xmlStagioni = item.Element("stagioni");
                XElement xmlEpisodi = item.Element("episodi");
                Serie s = new Serie();
                s.Nome = xmlNome.Value;
                s.Stagioni = Convert.ToInt32(xmlStagioni.Value);
                s.Episodi = Convert.ToInt32(xmlEpisodi.Value);
                Dispatcher.Invoke(() => Lst_Lista.Items.Add(s));
                Thread.Sleep(50);
            }
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
            foreach (Serie item in Lst_Lista.Items)
            {
                if (item.Nome == Txt_Estrai.Text)
                    Dispatcher.Invoke(() => Txt_DatiSerie.Text = item.Stagioni.ToString() + " stagioni, " + item.Episodi.ToString() + " episodi totali");
            }
        }

        private void Btn_Estrai_Click(object sender, RoutedEventArgs e)
        {
            ct = new CancellationTokenSource();
            Task.Factory.StartNew(()=> Estrai());
        }

        private void Estrai()
        {
            string path = @"presenze.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlNetflix = xmlDoc.Element("netflix");
            var xmlSerie = xmlNetflix.Elements("serie");
            Serie s = new Serie();

            while (!ct.IsCancellationRequested)
            {
                foreach (var item in xmlSerie)
                {
                    XElement xmlNome = item.Element("nome");
                    s.Nome = xmlNome.Value;
                    Dispatcher.Invoke(() => Txt_Estrai.Text = s.Nome);
                    Thread.Sleep(50);
                    if (ct.IsCancellationRequested)
                        break;
                } 
            }
        }

        private void Btn_Modifica_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => ContollaSelezione());
        }

        private void ContollaSelezione()
        {
            if (Convert.ToString(Lst_Lista.SelectedItem) == "")
            {
                Rtg_Mod.Visibility = Visibility.Hidden;
                Btn_Salva.Visibility = Visibility.Hidden;
                Lbl_TitoloMod.Visibility = Visibility.Hidden;
                Lbl_StagioniMod.Visibility = Visibility.Hidden;
                Lbl_EpisodiMod.Visibility = Visibility.Hidden;
                Txt_NomeMod.Visibility = Visibility.Hidden;
                Txt_StagioniMod.Visibility = Visibility.Hidden;
                Txt_EpisodiMod.Visibility = Visibility.Hidden;
                MessageBox.Show("Seleziona un elemento");
            }
            else
            {
                Rtg_Mod.Visibility = Visibility.Visible;
                Btn_Salva.Visibility = Visibility.Visible;
                Lbl_TitoloMod.Visibility = Visibility.Visible;
                Lbl_StagioniMod.Visibility = Visibility.Visible;
                Lbl_EpisodiMod.Visibility = Visibility.Visible;
                Txt_NomeMod.Visibility = Visibility.Visible;
                Txt_StagioniMod.Visibility = Visibility.Visible;
                Txt_EpisodiMod.Visibility = Visibility.Visible;

                count = 0;

                string path = @"presenze.xml";
                XDocument xmlDoc = XDocument.Load(path);
                XElement xmlNetflix = xmlDoc.Element("netflix");
                var xmlSerie = xmlNetflix.Elements("serie");
                foreach (var item in xmlSerie)
                {
                    XElement xmlNome = item.Element("nome");
                    XElement xmlStagioni = item.Element("stagioni");
                    XElement xmlEpisodi = item.Element("episodi");
                    Serie s = new Serie();
                    s.Nome = xmlNome.Value;
                    s.Stagioni = Convert.ToInt32(xmlStagioni.Value);
                    s.Episodi = Convert.ToInt32(xmlEpisodi.Value);

                    if (Convert.ToString(Lst_Lista.SelectedItem) == s.Nome)
                    {
                        Txt_NomeMod.Text = s.Nome;
                        Txt_StagioniMod.Text = s.Stagioni.ToString();
                        Txt_EpisodiMod.Text = s.Episodi.ToString();
                        break;
                    }
                    count++;
                }
            }
        }

        int count = 0;

        private void Btn_Salva_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => SalvaModifiche());
        }


        private void SalvaModifiche()
        {
            int count2 = 0;
            string path = @"presenze.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlNetflix = xmlDoc.Element("netflix");
            var xmlSerie = xmlNetflix.Elements("serie");
            foreach (var item in xmlSerie)
            {
                if (count2 == count)
                {
                    item.SetElementValue("nome", Txt_NomeMod.Text);
                    item.SetElementValue("stagioni", Txt_StagioniMod.Text);
                    item.SetElementValue("episodi", Txt_EpisodiMod.Text);
                    break;
                }
                count2++;
            }
            xmlDoc.Save("presenze.xml");
        }
    }
}
