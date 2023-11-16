using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project
{
    public partial class FrmMainMenu : Form
    {
        private int currentLine = 0; // To keep track of the current line being displayed
        private string[] lines;      // Array of lines to display in the TextBox
        public FrmMainMenu()
        {
            InitializeComponent();
            lines = new string[]
            {
                "Mr peanut has come back from a journey to see his other Fabaceae clan members in the village of Legume.",
                "Expecting to bring report to his wife and daughter, he was in for a shock.",
                "His wife and kid had fallen victim to the menace of bean village.",
                "Mr Peanut went over the edge and descended into his pyschopatic alter ego.",
                "With a smile on his face he is fully geared to embark on a mission to annahilate Poison packet his enablers.",
                "He promises to deliver pain!!!",
                "See to it that Mr peanut makes good on his promise and restores sanity to bean Village",
                "Somewhere in Bean village...",
                // Add more lines here
            };
        }

        private void FrmMainMenu_Load(object sender, EventArgs e)
        {
            // Start the timer when the form loads
            scrollTimer.Start();
            // Play background music for the main menu
            MusicSettings.StopBackgroundMusic();
            MusicSettings.PlayBackgroundMusicMainMenu();
        }
        private void scrollTimer_Tick(object sender, EventArgs e)
        {
            // Display the current line in the TextBox
            textBox1.Text = lines[currentLine];

            // Increment the current line index and loop back to the first line if at the end
            currentLine = (currentLine + 1) % lines.Length;
        }
        private void StartBtn_Click(object sender, EventArgs e)
        {
            // Create and show the FrmLevel form when the "Start Game" button is clicked.
            FrmLevel levelForm = new FrmLevel(this);
            levelForm.Show();
            this.Hide(); // Hide the main menu form
        }
    }


}
