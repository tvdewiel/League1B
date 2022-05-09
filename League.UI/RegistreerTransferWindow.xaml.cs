using League.BL.DTO;
using League.BL.Interfaces;
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
    /// Interaction logic for RegistreerTransferWindow.xaml
    /// </summary>
    public partial class RegistreerTransferWindow : Window
    {
        private SpelerManager spelerManager;
        private TeamManager teamManager;
        private TransferManager transferManager;
        private SpelerInfo spelerInfo;
        public RegistreerTransferWindow()
        {
            InitializeComponent();
            ISpelerRepository spelerRepo = new SpelerRepoADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString());
            ITeamRepository teamRepo = new TeamRepoADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString());
            ITransferRepository transferRepo = new TransferRepoADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString());
            spelerManager = new SpelerManager(spelerRepo);         
            teamManager = new TeamManager(teamRepo);
            transferManager = new TransferManager(transferRepo,spelerRepo,teamRepo);
            NieuwTeamComboBox.ItemsSource = teamManager.SelecteerTeams();
            NieuwTeamComboBox.SelectedIndex = 0;
            spelerInfo = null;
        }

        private void ZoekSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? spelerId = null;
                string naam = null;
                if (!string.IsNullOrWhiteSpace(ZoekNaamTextBox.Text)) naam = ZoekNaamTextBox.Text;
                if (!string.IsNullOrWhiteSpace(ZoekSpelerIDTextBox.Text))
                {
                    spelerId = int.Parse(ZoekSpelerIDTextBox.Text);
                }
                IReadOnlyList<SpelerInfo> spelers = spelerManager.SelecteerSpelers(spelerId, naam);
                if (spelers.Count == 0)
                {
                    NaamTextBox.Text = "";
                    SpelerIDTextBox.Text = "";                    
                    HuidigTeamTextBox.Text = "";
                    spelerInfo = null;
                }
                if (spelers.Count == 1)
                {
                    NaamTextBox.Text = spelers[0].naam;
                    SpelerIDTextBox.Text = spelers[0].id.ToString();                   
                    HuidigTeamTextBox.Text = spelers[0].teamNaam;
                    spelerInfo=spelers[0];
                }
                if (spelers.Count > 1)
                {
                    SelecteerSpelerWindow selecteerSpelerWindow = new SelecteerSpelerWindow(spelers);
                    if (selecteerSpelerWindow.ShowDialog() == true)
                    {
                        NaamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.naam;
                        SpelerIDTextBox.Text = selecteerSpelerWindow.SelectedSpeler.id.ToString();
                        HuidigTeamTextBox.Text = selecteerSpelerWindow.SelectedSpeler.teamNaam;
                        spelerInfo=selecteerSpelerWindow.SelectedSpeler;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PrijsTextBox.Text)) MessageBox.Show("Prijs invullen");
                else 
                {
                    int prijs=int.Parse(PrijsTextBox.Text);
                    TeamInfo teamInfo=(TeamInfo)NieuwTeamComboBox.SelectedItem;
                    transferManager.RegistreerTransfer(spelerInfo,teamInfo, prijs);
                    MessageBox.Show("Transfer uitgevoerd");
                    Close();
                }
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.ToString()); 
            }
        }
    }
}
