using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BetServerManager
{
   
    public partial class mainWindow : Form
    {
        ServerConnection server = new ServerConnection();
        

        public mainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void connectToSeverButton_Click(object sender, EventArgs e)
        {
            if(serverConnectionPicker.Text =="LocalHost")
            {
                //We are going to initalize connection to the localhost.
                server.svLogin("localhost", "recipe_app", "root", "", "3306", "none");
                try
                {
                    server.connection.Open();
                    MessageBox.Show("Connection to " + server.serverName + " succesful");
                    connectionStateLabel.Text = "Connected : "+ server.serverName;
                    server.connection.Close();
                }
                catch (MySqlException err)
                {
                    MessageBox.Show(err.Message + server.serverName);
                    connectionStateLabel.Text = "Failed to connect to " + server.serverName +" Consider checking XAMPP.";
                }
            }
            if (serverConnectionPicker.Text == "OnlineWebHost")
            {
                MessageBox.Show("No Webhost has been set in config file, defaulting to localhost connection.");
                //We are going to initalize connection to the localhost.
                server.svLogin("localhost", "recipe_app", "root", "", "3306", "none");
                try
                {
                    server.connection.Open();
                    MessageBox.Show("Connection to " + server.serverName + " succesful");
                    server.connection.Close();
                }
                catch (MySqlException err)
                {
                    MessageBox.Show(err.Message + server.serverName);
                    connectionStateLabel.Text = "Failed to connect to " + server.serverName + " Consider checking XAMPP.";
                }
            }
        }

    }
}
