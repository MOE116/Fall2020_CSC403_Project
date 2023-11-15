using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Media;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project
{
    public partial class FrmBattle : Form
    {
        public static FrmBattle instance = null;
        private Enemy enemy;
        private Player player;
        private int healCount = 0;   //Heal count for heal button added
                                  
        private static int characterbattle;
        private List<string> texts = new List<string> {"3", "2", "X" }; //numbers for the AdClose textbox to loop through
        private int currentIndex = 0;
        private bool ad3start = false;
        string urlToOpen = "";
     
        private FrmBattle()
        {
            InitializeComponent();
            player = Game.player;
        }

        public void Setup()
        {
            // update for this enemy
            picEnemy.BackgroundImage = enemy.Img;
            picEnemy.Refresh();
            BackColor = enemy.Color;
            picBossBattle.Visible = false;

            string val = BackColor.Name.ToString(); 
            if (val == "Green")
            {
                AdVisible(val);
            }
            else if (val == "MediumPurple")
            {
                AdVisible(val);
            }
			

            // Observer pattern
            enemy.AttackEvent += PlayerDamage;
            player.AttackEvent += EnemyDamage;

            // show health
            UpdateHealthBars();
        }

        public void SetupForBossBattle()
        {

            picBossBattle.Location = Point.Empty;
            picBossBattle.Size = ClientSize;
            picBossBattle.Visible = true;

            MusicSettings.StopBackgroundMusic();
            SoundPlayer simpleSound = new SoundPlayer(Resources.final_battle);
            simpleSound.Play();
            //MusicSettings.PlayBackgroundMusic();
            tmrFinalBattle.Enabled = true;
        }

        public static FrmBattle GetInstance(Enemy enemy)
        {
            if (instance == null)
            {
                instance = new FrmBattle();
                instance.enemy = enemy;
                instance.Setup();
            }
            return instance;
        }

        private void UpdateHealthBars()
        {
            float playerHealthPer = player.Health / (float)player.MaxHealth;
            float enemyHealthPer = enemy.Health / (float)enemy.MaxHealth;

            const int MAX_HEALTHBAR_WIDTH = 226;
            lblPlayerHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * playerHealthPer);
            lblEnemyHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * enemyHealthPer);

            lblPlayerHealthFull.Text = player.Health.ToString();
            lblEnemyHealthFull.Text = enemy.Health.ToString();

            if (playerHealthPer < 0.3)                                         // Health Warning
            {
                MessageBox.Show("Warning: Low health!");

            }

        }

        private void btnAttack_Click(object sender, EventArgs e)
        {
            // base damage value for the player
            int basePlayerDamage = -4;
            int playerDamageWithKnife = (int)(basePlayerDamage * player.AttackPower);

            player.OnAttack(playerDamageWithKnife);
            if (enemy.Health > 0)
            {
                enemy.OnAttack(-2); // Enemy's attack
            }

            UpdateHealthBars();
            if (player.Health <= 0 || enemy.Health <= 0)
            {
                instance = null;
                Close();
            }
        }


        private void EnemyDamage(int amount)
        {
            enemy.AlterHealth(amount);
        }

        private void PlayerDamage(int amount)
        {
            player.AlterHealth(amount);
        }

        private void tmrFinalBattle_Tick(object sender, EventArgs e)
        {
            picBossBattle.Visible = false;
            tmrFinalBattle.Enabled = false;
            MusicSettings.PlayBackgroundMusic();

            string val = BackColor.Name.ToString();
            if (val == "Red")
            {
                AdVisible(val);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentIndex < texts.Count)
            {
                AdClose.Text = texts[currentIndex];
                currentIndex++;
            }
            else
            {
                // When you reach the end of the list, reset the index to start over.
                currentIndex = 0;
            }
            if (AdClose.Text == "X")
            {
                timer1.Stop();
            }
        }

        private void AdVisible(string color) //Condition to select adpanel for each battle instance based on background color of battle form
        {
            timer1.Start();
            if (color == "Green")
            {
                urlToOpen = "https://xtremehomeimp.com/2020/11/12/winter-is-coming-are-you-ready/"; // Url to ad website is inserted here. Can be changed subsequently
                AdPanel.BackgroundImage = Resources.Adwinter2;
            }
            else if (color == "MediumPurple")
            {
                urlToOpen = "https://www.amazon.com/JBL-Tune-760NC-Lightweight-Cancellation/dp/B094YTN8JV?ref_=ast_sto_dp&th=1"; // Url to ad website is inserted here. Can be changed subsequently
                AdPanel.BackgroundImage = Resources.AdJBL;
            }
            else if (color == "Red")
            {
                urlToOpen = "https://www.amazon.com/Secretlab-Titan-Knight-Gaming-Chair/dp/B0B3RL4BP7?ref_=ast_sto_dp"; // Url to ad website is inserted here. Can be changed subsequently
                AdPanel.BackgroundImage = Resources.AdBatman;
            }

            AdPanel.Visible = true;
            AdPanel.Enabled = true;
        }

        private void AdClose_Click(object sender, EventArgs e)
        {
            if (AdClose.Text == "X")
            {
                AdPanel.Visible = false;
                AdPanel.Enabled = false;
                ad3start = false;
                timer1.Stop();
            }
        }

        private void advertisingPanel_Click(object sender, EventArgs e)
        {
            // Specify the URL you want to open in the default web browser


            try
            {
                // Open the default web browser with the specified URL
                System.Diagnostics.Process.Start(urlToOpen);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur when opening the web browser
                MessageBox.Show("An error occurred while opening the web browser: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        ///////////////////////////////////////////////stop			

    

         private void FleeButton_Click(object sender, EventArgs e)
         {
            instance = null;
            Close();
         }

        private void Healbtn_Click(object sender, EventArgs e)  //Player heal
        {
        if (healCount < 5)
        {
            if (player.Health < player.MaxHealth)
            {
                player.AlterHealth(4);
                UpdateHealthBars();
                healCount++;

            }
        }
        }

        private void FrmBattle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)    //Exit application
        {
        Application.Exit();
        }
    }
}