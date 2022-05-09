using League.BL.DTO;
using League.BL.Managers;
using League.DL;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace League.UI
{
    /// <summary>
    /// Interaction logic for UpdateSpelerWindow.xaml
    /// </summary>
    public partial class UpdateSpelerWindow : Window
    {
        private SpelerManager spelerManager;
        public UpdateSpelerWindow()
        {
            InitializeComponent();
            spelerManager = new SpelerManager(
                new SpelerRepoADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString()));
        }

        private void ZoekButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? spelerId = null;
                string naam = null;
                if(!string.IsNullOrWhiteSpace(ZoekNaamTextBox.Text)) naam=ZoekNaamTextBox.Text;
                if (!string.IsNullOrWhiteSpace(ZoekSpelerIdTextBox.Text))
                {
                    spelerId=int.Parse(ZoekSpelerIdTextBox.Text);
                }
                IReadOnlyList<SpelerInfo> spelers=spelerManager.SelecteerSpelers(spelerId, naam);
                if (spelers.Count == 0)
                {
                    NaamTextBox.Text = "";
                    SpelerIdTextBox.Text = "";
                    GewichtTextBox.Text = "";
                    LengteTextBox.Text = "";
                    RugnummerTextBox.Text = "";
                    TeamTextBox.Text = "";
                }
                if (spelers.Count == 1)
                {
                    NaamTextBox.Text =spelers[0].naam;
                    SpelerIdTextBox.Text = spelers[0].id.ToString();
                    GewichtTextBox.Text = spelers[0].gewicht.ToString();
                    LengteTextBox.Text = spelers[0].lengte.ToString();
                    RugnummerTextBox.Text = spelers[0].rugnummer.ToString();
                    TeamTextBox.Text = spelers[0].teamNaam;
                }
                if (spelers.Count > 1)
                {
                    SelecteerSpelerWindow selecteerSpelerWindow = new SelecteerSpelerWindow(spelers);
                    if (selecteerSpelerWindow.ShowDialog() == true) {
                        NaamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.naam;
                        SpelerIdTextBox.Text = selecteerSpelerWindow.SelectedSpeler.id.ToString();
                        GewichtTextBox.Text = selecteerSpelerWindow.SelectedSpeler.gewicht.ToString();
                        LengteTextBox.Text = selecteerSpelerWindow.SelectedSpeler.lengte.ToString();
                        RugnummerTextBox.Text = selecteerSpelerWindow.SelectedSpeler.rugnummer.ToString();
                        TeamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.teamNaam;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int spelerId = int.Parse(SpelerIdTextBox.Text);
                int? gewicht = null;
                if (!string.IsNullOrWhiteSpace(GewichtTextBox.Text))
                    gewicht = int.Parse(GewichtTextBox.Text);
                int? lengte = null;
                if (!string.IsNullOrWhiteSpace(LengteTextBox.Text))
                    lengte = int.Parse(LengteTextBox.Text);
                int? rugnummer = null;
                if (!string.IsNullOrWhiteSpace(RugnummerTextBox.Text))
                    rugnummer = int.Parse(RugnummerTextBox.Text);
                SpelerInfo spelerInfo = new SpelerInfo(spelerId, NaamTextBox.Text, lengte, gewicht, rugnummer, TeamTextBox.Text);
                spelerManager.UpdateSpeler(spelerInfo);
                MessageBox.Show($"speler : {spelerInfo}", "Speler is up to date");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
