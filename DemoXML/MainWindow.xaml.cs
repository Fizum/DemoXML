﻿using System;
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
        }

        CancellationTokenSource ct;

        private void Btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            Lst_Lista.Items.Clear();
            ct = new CancellationTokenSource();
            Task.Factory.StartNew(() => CaricaDati());
        }

        private void CaricaDati()
        {
            Serie serie = new Serie();
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
                serie = s;
                Dispatcher.Invoke(() => Lst_Lista.Items.Add(serie));
                Thread.Sleep(500);
                if (ct.IsCancellationRequested)
                    break;
            }
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
        }
    }
}
