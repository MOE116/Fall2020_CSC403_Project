using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


namespace Fall2020_CSC403_Project {
  public partial class FrmLevel : Form {
    private Player player;
        public static FrmLevel frmlevel = null;                                         // to refer in FrmBattle.cs  
        private Enemy enemyPoisonPacket;
    private Enemy bossKoolaid;
        private FrmBattle frmbattle;
    private Enemy enemyCheeto;
    private Character[] walls;
        private bool hasKnife = false;                                                  //Knife Value
        private DateTime timeBegin;
    private FrmBattle frmBattle;
    private bool isPaused = false;                                                      // Pause button
    public StopwatchHelper stopwatchHelper;                                             // for pausing timer
    private FrmTutorial frmTutorial;
    private static bool isBackgroundMusicPlaying = false;
    private FrmMainMenu mainMenuForm; // Add a reference to the FrmMainMenu form
        
        public FrmLevel(FrmMainMenu mainMenuForm)
        {
            InitializeComponent();
            this.mainMenuForm = mainMenuForm; // Initialize the reference to FrmMainMenu
            frmlevel = this;
            stopwatchHelper = new StopwatchHelper();
            stopwatchHelper.Start();
            

        }
        private void FrmLevel_FormClosing(object sender, FormClosingEventArgs e)
    {
        
        MusicSettings.StopBackgroundMusic(); // Stops background music and close the FrmMainMenu form when FrmLevel is closing
        mainMenuForm.Close();
    }
      private void FrmLevel_Load(object sender, EventArgs e) {
      const int PADDING = 7;
      const int NUM_WALLS = 19;
        MusicSettings.StopBackgroundMusic();
        MusicSettings.PlayBackgroundMusicLevel();
            // Wire up the FormClosing event
            this.FormClosing += FrmLevel_FormClosing;        


            player = new Player(CreatePosition(picPlayer), CreateCollider(picPlayer, PADDING));
            bossKoolaid = new Enemy(CreatePosition(picBossKoolAid), CreateCollider(picBossKoolAid, PADDING));
            enemyPoisonPacket = new Enemy(CreatePosition(picEnemyPoisonPacket), CreateCollider(picEnemyPoisonPacket, PADDING));
            enemyCheeto = new Enemy(CreatePosition(picEnemyCheeto), CreateCollider(picEnemyCheeto, PADDING));


            bossKoolaid.Img = picBossKoolAid.BackgroundImage;
            enemyPoisonPacket.Img = picEnemyPoisonPacket.BackgroundImage;
            enemyCheeto.Img = picEnemyCheeto.BackgroundImage;

            bossKoolaid.Color = Color.Red;
            enemyPoisonPacket.Color = Color.Green;
            enemyCheeto.Color = Color.MediumPurple;

            //enemyCheeto.Color = Color.FromArgb(255, 245, 161);

            walls = new Character[NUM_WALLS];

            // Loop over control names that could represent walls
            string[] wallNames = new string[] {
    "picWall0", "picWall1", "picWall2", "picWall3", "picWall4", "picWall5",
    "picWall6", "picWall7", "picWall8", "picWall9", "picWall10", "picWall11",
    "picWall14", "pictureBox1", "pictureBox2", "pictureBox4", "picWall12", "pictureBox3"
};

            for (int w = 0, index = 0; w < wallNames.Length; w++)
            {
                Control[] foundControls = Controls.Find(wallNames[w], true);
                if (foundControls.Length > 0)
                {
                    PictureBox pic = foundControls[0] as PictureBox;
                    walls[index++] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
                }
                else
                {
                    // Log or handle the case where a PictureBox is not found
                    Debug.WriteLine("PictureBox not found: " + wallNames[w]);
                }
            }


            Game.player = player;
            timeBegin = DateTime.Now;


        }

        private Vector2 CreatePosition(PictureBox pic)
        {
            return new Vector2(pic.Location.X, pic.Location.Y);
        }

        private Collider CreateCollider(PictureBox pic, int padding)
        {
            Rectangle rect = new Rectangle(pic.Location, new Size(pic.Size.Width - padding, pic.Size.Height - padding));
            return new Collider(rect);
        }

        private void FrmLevel_KeyUp(object sender, KeyEventArgs e)
        {
            player.ResetMoveSpeed();
        }


        private void tmrUpdateInGameTime_Tick(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - timeBegin;
            string elapsedTimeString = stopwatchHelper.GetElapsedTimeString(); //stopwatchHelper.Reset();
            lblInGameTime.Text = "Time: " + elapsedTimeString;
        }



        private void tmrPlayerMove_Tick(object sender, EventArgs e) {
      // move player
      player.Move();

      // check collision with walls
      if (HitAWall(player)) {
        player.MoveBack();
      }

      // check collision with enemies
      if (HitAChar(player, enemyPoisonPacket)) {
        Fight(enemyPoisonPacket);
      }
      else if (HitAChar(player, enemyCheeto)) {
        Fight(enemyCheeto);
      }
      if (HitAChar(player, bossKoolaid)) {
        Fight(bossKoolaid);
      }

      // update player's picture box
      picPlayer.Location = new Point((int)player.Position.x, (int)player.Position.y);
            if (!hasKnife && picPlayer.Bounds.IntersectsWith(picKnife.Bounds))
            {
                PickUpKnife();
            }
        }
        private void PickUpKnife()
        {
            hasKnife = true;
            picKnife.Visible = false; // Hides the knife from the game world
            player.AttackPower += 0.5f; // Increases attack power
            MessageBox.Show("You picked up the knife! Your attack power has increased.", "Knife Acquired", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private bool HitAWall(Character c)
        {
            if (c == null || c.Collider == null)
            {
                return false;
            }

            for (int w = 0; w < walls.Length; w++)
            {
                if (walls[w] != null && walls[w].Collider != null && c.Collider.Intersects(walls[w].Collider))
                {
                    return true;
                }
            }
            return false;
        }


        private bool HitAChar(Character you, Character other) {
            return other != null && you.Collider.Intersects(other.Collider);
        }

        private void Fight(Enemy enemy)
        {
            player.ResetMoveSpeed();
            player.MoveBack();
            frmBattle = FrmBattle.GetInstance(enemy);
            frmBattle.Show();

            if (enemy == bossKoolaid)
            {
                frmBattle.SetupForBossBattle();
            }

            frmBattle.FormClosed += (s, ev) =>  // Make Enemy disappear
            {
                if (player.Health > 0 && enemy.Health <= 0)
                {
                    if (enemy == bossKoolaid)
                    {
                        RemoveEnemy(picBossKoolAid);
                        bossKoolaid = null;
                    }
                    else if (enemy == enemyPoisonPacket)
                    {
                        RemoveEnemy(picEnemyPoisonPacket);
                        enemyPoisonPacket = null;
                    }
                    else if (enemy == enemyCheeto)
                    {
                        RemoveEnemy(picEnemyCheeto);
                        enemyCheeto = null;
                    }
                }
            };
            UpdatePlayerStatus(player.Health, player.MaxHealth);

        }

        private void RemoveEnemy(PictureBox enemyPic)
        {
            this.Controls.Remove(enemyPic);
            enemyPic.Dispose();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keydata)     //Movement change
        {
            player.ResetMoveSpeed();

            switch (keydata)
            {
                case Keys.Left: player.GoLeft(); break;
                case Keys.Right: player.GoRight(); break;
                case Keys.Up: player.GoUp(); break;
                case Keys.Down: player.GoDown(); break;
            }

            return true;
        }
        public void UpdatePlayerStatus(int Health, int MaxHealth)
        {
            float playerHealthPer = Health / (float)MaxHealth;
            const int MAX_HEALTHBAR_WIDTH = 151;
            lblPlayerHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * playerHealthPer);
            lblPlayerHealthFull.Text = "player health" + Health.ToString();
        }




        private void FrmLevel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.GoLeft();
                    break;

                case Keys.Right:
                    player.GoRight();
                    break;

                case Keys.Up:
                    player.GoUp();
                    break;

                case Keys.Down:
                    player.GoDown();
                    break;

                default:
                    player.ResetMoveSpeed();
                    break;
            }
        }

        private void lblInGameTime_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)              //Pause button
        {
            if (!isPaused)
            {
                stopwatchHelper.Stop();
                tmrPlayerMove.Stop();
                button1.Text = "PLAY";
            }
            else
            {
                stopwatchHelper.Start();
                tmrPlayerMove.Start();
                button1.Text = "PAUSE";
            }
            isPaused = !isPaused;
        }

        private void MainMenuLbl_Click(object sender, EventArgs e)
        {
            DialogResult MainMenuDialogue = MessageBox.Show("You will lose all progress, do you still wish to continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MainMenuDialogue == DialogResult.Yes)
            {
                this.Close();
                Application.Restart();
                
                
            }
        }
    }
}
