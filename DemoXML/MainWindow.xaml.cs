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
    }
}
