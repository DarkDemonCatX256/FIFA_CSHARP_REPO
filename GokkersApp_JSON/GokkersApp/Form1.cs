﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace GokkersApp
{

    public partial class mainForm : Form
    {
       
        RootObject rootObject;
        public List<Player> Players = new List<Player>();
        public List<string> Teams = new List<string>();
        public int wins, losses, points;
        public bool connected = false;
        SaveData saveData = new SaveData();
        ServerConnection serverConnection;
        public mainForm()
        {
            InitializeComponent();
            establishConnection();
            if (connected)
            {
                downloadFromDatabase();
            }
            LoadDatabase("gamedatax.gok");
            loadGame();
            updateWinLabels();
          
        }
        void RefreshTitleBar(string newText)
        {
            this.Text = newText;
        }
        void establishConnection ()
        {
            serverConnection = new ServerConnection();
            try
            {
                serverConnection.svLoginEncrypted();
                serverConnection.connection.Open();
              
                connected = true;
               
            }
            catch (MySqlException err)
            {
                MessageBox.Show("Connection to" + " database" + " failed...");
                connected = false;
                
            }
        }
        void downloadFromDatabase()
        {
            string commandString = "SELECT * FROM players";
            MySqlDataAdapter sda = new MySqlDataAdapter(commandString, serverConnection.connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            StreamWriter save = File.CreateText("gamedatax.gok");
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(save, dt);
            save.Close();
            MessageBox.Show("Database is succesvol vernieuwed");

        }
        void saveGame()
        {
            saveData.wins = wins;
            saveData.losses = losses;
            saveData.points = points;
           
            StreamWriter save = File.CreateText("gamedata.gok");
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(save, saveData);
            save.Close();

        }
        void loadGame()
        {
            bool fileFound = false;
            try
            {
                using (File.Open("gamedata.gok", FileMode.Open))
                {
                    fileFound = true;
                    
                  
                }
            }
            catch (FileNotFoundException ex)
            {
                fileFound = false;
                points = 5;
            }
            if(fileFound)
            {
                StreamReader sr = File.OpenText("gamedata.gok");


                JsonSerializer serializer = new JsonSerializer();
                saveData = (SaveData)serializer.Deserialize(sr, typeof(SaveData));
                wins = saveData.wins;
                losses = saveData.losses;
                points = saveData.points;
                
                sr.Close();
            }
            else
            {
                MessageBox.Show("Geen SaveGame getecteerd.");
            }
            updateWinLabels();
        }
        void LoadDatabase(string dbName)
        {
            
            rootObject = new RootObject();
            StreamReader sr = File.OpenText(dbName);
            
            JsonSerializer serializer = new JsonSerializer();

            Datum[] temp = JsonConvert.DeserializeObject<Datum[]>(File.ReadAllText(dbName));
            //MessageBox.Show(temp[0].player_name.ToString());
            List<Datum> players = temp.ToList<Datum>();
            playerListView.Clear();
            teamPickerComboBox.Items.Clear();
            Players.Clear();
            for (int i = 0; i < players.Count; i++)
            {

                int x = Int32.Parse(players[i].id);
                
                Players.Add(new Player(players[i].player_name, x, players[i].team_name));
                
                
                Teams.Add(Players[i].assignedTeamName);
               
                if (!teamPickerComboBox.Items.Contains(Players[i].assignedTeamName))
                {
                    teamPickerComboBox.Items.Add(Teams[i].ToString());
                    playerListView.Items.Add(Players[i].assignedTeamName);
                }

            }
            sr.Close();
            
            
         
          
            playerListView.Refresh();
        }
        void PerformRNG()
        {
            int x = 0;
            Random rng = new Random();
            x = (int)pointUpDown.Value;
            if (teamPickerComboBox.SelectedItem == null)
            {
                MessageBox.Show("Alstublieft selecteer een Team");
            }
            else
            {
                if (pointUpDown.Value < 2 || pointUpDown.Value > 15)
                {
                    MessageBox.Show("Je moet niet minder dan 2 en niet hoger dan 15 punten gokken");
                }
                else
                {
                    if (points >= pointUpDown.Value)
                    {
                        points = points - (int)pointUpDown.Value;
                        if (rng.Next(2) == 0)
                        {
                            wins++;
                            MessageBox.Show("Uw Team : " + teamPickerComboBox.SelectedItem.ToString() + " heeft gewonnen!");
                            RefreshTitleBar(teamPickerComboBox.SelectedItem.ToString() + " heeft gewonnen!!! HORAAH!");
                            points = points + (x * 2);
                        }
                        else
                        {
                            losses++;
                            MessageBox.Show("Uw Team : " + teamPickerComboBox.SelectedItem.ToString() + " verloor de wedstrijd!");
                            RefreshTitleBar(teamPickerComboBox.SelectedItem.ToString() + " verloor de wedstrijd!!! OH NEE!");
                            
                        }
                        saveGame();
                    }
                    else
                    {
                        MessageBox.Show("Je heeft te weing punten te gokken.");
                    }
                }
            }
        }
        void updateWinLabels()
        {
            winsLabel.Text = "Wins : " + wins.ToString();
            lossLabel.Text = "Losses : " + losses.ToString();
            pointLabel.Text ="Punten : " + points.ToString();
        }

        private void refreshConnectionButton_Click(object sender, EventArgs e)
        {
            establishConnection();
            if (connected)
            {
                downloadFromDatabase();
            }
            LoadDatabase("gamedatax.gok");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void betButton_Click(object sender, EventArgs e)
        {
            PerformRNG();
            updateWinLabels();
        }
    }
}
