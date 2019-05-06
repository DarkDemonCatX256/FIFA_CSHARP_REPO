using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace GokkersApp
{
    
    public partial class mainForm : Form
    {
       
        RootObject rootObject;
        public List<Player> Players = new List<Player>();

        public mainForm()
        {
            InitializeComponent();
            LoadDatabase("players.json");
          
        }
        void RefreshTitleBar(string newText)
        {
            this.Text = newText;
        }
        void LoadDatabase(string dbName)
        {
            rootObject = new RootObject();
            StreamReader sr = File.OpenText(dbName);
            
            JsonSerializer serializer = new JsonSerializer();

            RootObject[] temp = JsonConvert.DeserializeObject<RootObject[]>(File.ReadAllText(dbName));
           
            List<Datum> players = temp[2].data;

            for (int i = 0; i < players.Count; i++)
            {

                int x = Int32.Parse(players[i].id);
                Players.Add(new Player(players[i].player_name, x, players[i].team_name));
                playerListView.Items.Add("Speler : " + Players[i].playerName + " | Team : " + Players[i].assignedTeamName);

            }
           

         
          
            playerListView.Refresh();
        }
 
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       


        
     
    }
}
