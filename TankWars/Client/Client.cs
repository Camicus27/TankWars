using System;
using System.Drawing;
using AxWMPLib;
using System.Windows.Forms;
using Models;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Media;

namespace TankWars
{
    public partial class TankGameWindow : Form
    {
        // The controller object that handles communication and logic
        private GameController theController;
        // The model of the game world
        private World theWorld;
        // The panel to draw objects on
        private DrawingPanel drawingPanel;
        // The color selection popup
        private ColorPopup colorPanel;
        // Boolean flags for disabling the connect button
        private bool nameTextIsEmpty = false;
        private bool serverTextIsEmpty = false;
        // Boolean for color selection
        private bool valid = true;
        // Color of the tank
        private string tankColor;
        // Audio assets
        private SoundPlayer timerSound;
        private AxWindowsMediaPlayer song;
        //private AxWindowsMediaPlayer shotSound;
        //private AxWindowsMediaPlayer beamSound;
        //private AxWindowsMediaPlayer powerupCollectedSound;
        //private AxWindowsMediaPlayer tankDeathSound;
        //private AxWindowsMediaPlayer respawnSound;

        public TankGameWindow(GameController ctrl)
        {
            InitializeComponent();
            theController = ctrl;
            theWorld = theController.GetWorld();
            theWorld.FrameSize = 900;

            // Place and add the drawing panel
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 60);
            drawingPanel.Size = new Size(theWorld.FrameSize, theWorld.FrameSize);
            this.Controls.Add(drawingPanel);
            // The multipliers are some stupid scaling error
            this.Size = new Size((int)(theWorld.FrameSize * 1.018), (int)(theWorld.FrameSize * 1.035) + 60);

            // Register handlers for the controller's events
            theController.Connected += HandleConnected;
            theController.UpdateArrived += OnFrame;
            theController.Error += ShowError;

            // Set up key and mouse handlers
            this.KeyDown += HandleKeyDown;
            this.KeyUp += HandleKeyUp;
            drawingPanel.MouseDown += HandleMouseDown;
            drawingPanel.MouseUp += HandleMouseUp;
            drawingPanel.MouseMove += HandleMouseMove;
            drawingPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            // Load audio
            timerSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Timer.wav");
            timerSound.Load();

            string currDirectory = Directory.GetCurrentDirectory();
            song.URL = currDirectory + "\\Song.wav";
            song.Ctlcontrols.stop();
            //tankDeathSound.URL = currDirectory + "\\Explosion.wav";
            //shotSound.URL = currDirectory + "\\Shot.wav";
            //beamSound.URL = currDirectory + "\\Beam.wav";
            //powerupCollectedSound.URL = currDirectory + "\\Collected.wav";
            //respawnSound.URL = currDirectory + "\\Respawn.wav";

            // --- For when using DLLs ---
            //string currDirectory = Directory.GetCurrentDirectory();
            //song.URL = currDirectory + "\\Resources\\Audio\\Song.wav";
            //tankDeathSound.URL = currDirectory + "\\Resources\\Audio\\Explosion.wav";
            //shotSound.URL = currDirectory + "\\Resources\\Audio\\Shot.wav";
            //beamSound.URL = currDirectory + "\\Resources\\Audio\\Beam.wav";
            //powerupCollectedSound.URL = currDirectory + "\\Resources\\Audio\\Collected.wav";
            //respawnSound.URL = currDirectory + "\\Resources\\Audio\\Respawn.wav";

            // Give needed audio to the drawing panel
            //drawingPanel.AddSound("tankDeathSound", tankDeathSound);
            //drawingPanel.AddSound("shotSound", shotSound);
            //drawingPanel.AddSound("beamSound", beamSound);
            //drawingPanel.AddSound("powerupCollectedSound", powerupCollectedSound);
            //drawingPanel.AddSound("respawnSound", respawnSound);
        }

        /// <summary>
        /// Helper for showing an error message
        /// </summary>
        /// <param name="error"></param>
        private void ShowError(string error)
        {
            // Disconnect? Display error message
            DialogResult message = MessageBox.Show(error, "Error");
            theController.Close();

            // Reenable the form controls
            this.Invoke(new MethodInvoker(delegate
            {
                Connect.Enabled = true;
                NameText.Enabled = true;
                ServerText.Enabled = true;
                KeyPreview = false;
            }));
        }

        /// <summary>
        /// Handler for when the connect button has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, EventArgs e)
        {
            // Don't allow players to connect with default name or no name
            if (NameText.Text == "Insert Name Here" || NameText.Text == "")
            {
                DialogResult message = MessageBox.Show("Please Enter a Player Name", "Error");
                return;
            }

            SelectColor();

            // Disable the form controls
            Connect.Enabled = false;
            NameText.Enabled = false;
            ServerText.Enabled = false;

            // Attempt a connection
            theController.Connect(ServerText.Text);
        }

        /// <summary>
        /// Handler for when the controller has established a network connection
        /// </summary>
        private void HandleConnected()
        {
            theController.SendToServer(NameText.Text + "," + tankColor);

            // Enable the global form to capture key presses
            KeyPreview = true;
            this.Invoke(new MethodInvoker(delegate { GamemodeButton.Enabled = true; }));
        }

        /// <summary>
        /// Handler for the controller's UpdateArrived event
        /// </summary>
        private void OnFrame()
        {
            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can
            try { this.Invoke(new MethodInvoker(delegate { this.Invalidate(true); })); }
            catch { Application.Exit(); }
        }

        private void ControlsButton_Click(object sender, EventArgs e)
        {
            // Display the list of controls
            string controls = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\controls.txt");
            DialogResult message = MessageBox.Show(controls, "Game Controls");
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            // Display the 'about' info
            string about = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\about.txt");
            DialogResult message = MessageBox.Show(about, "About Tank Wars");
        }

        private void GamemodeButton_Click(object sender, EventArgs e)
        {
            if (GamemodeButton.Text.ToLower() == "basic")
            {
                GamemodeButton.Text = "Extra";
                theController.SendToServer("extra");
                GamemodeButton.Enabled = false;
                Thread t = new Thread(GamemodeChangeDelay);
                t.Start();
            }
            else
            {
                GamemodeButton.Text = "Basic";
                theController.SendToServer("basic");
                GamemodeButton.Enabled = false;
                Thread t = new Thread(GamemodeChangeDelay);
                t.Start();
            }
        }

        private void GamemodeChangeDelay()
        {
            // Setup a timer delay
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < 10000) { }
            try { this.Invoke(new MethodInvoker(delegate { GamemodeButton.Enabled = true; })); }
            catch { }
        }

        private void NameText_TextChanged(object sender, EventArgs e)
        {
            if (NameText.Text == "")
                nameTextIsEmpty = true;
            else
                nameTextIsEmpty = false;

            if (nameTextIsEmpty && serverTextIsEmpty)
                Connect.Enabled = false;
            else
                Connect.Enabled = true;
        }

        private void ServerText_TextChanged(object sender, EventArgs e)
        {
            if (ServerText.Text == "")
                serverTextIsEmpty = true;
            else
                serverTextIsEmpty = false;

            if (nameTextIsEmpty && serverTextIsEmpty)
                Connect.Enabled = false;
            else
                Connect.Enabled = true;
        }

        /// <summary>
        /// Key down handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                theController.HandleMoveRequest("up");
            if (e.KeyCode == Keys.A)
                theController.HandleMoveRequest("left");
            if (e.KeyCode == Keys.S)
                theController.HandleMoveRequest("down");
            if (e.KeyCode == Keys.D)
                theController.HandleMoveRequest("right");
            if (e.KeyCode == Keys.Oemplus && theWorld.ZoomLevel < 2)
                theWorld.ZoomLevel += 0.25;
            if (e.KeyCode == Keys.OemMinus && theWorld.ZoomLevel > 0.25)
                theWorld.ZoomLevel -= 0.25;
            if (e.KeyCode == Keys.Escape)
                Application.Exit();

            // Prevent other key handlers from running
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        /// <summary>
        /// Key up handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                theController.CancelMoveRequest("up");
            if (e.KeyCode == Keys.A)
                theController.CancelMoveRequest("left");
            if (e.KeyCode == Keys.S)
                theController.CancelMoveRequest("down");
            if (e.KeyCode == Keys.D)
                theController.CancelMoveRequest("right");
        }

        /// <summary>
        /// Handle mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                theController.HandleMouseRequest("main");
            if (e.Button == MouseButtons.Right)
                theController.HandleMouseRequest("alt");
        }

        /// <summary>
        /// Handle mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                theController.CancelMouseRequest();
        }

        /// <summary>
        /// Handle a mouse movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (KeyPreview)
            {
                // Get mouse position and angle from center
                Vector2D aimDir = new Vector2D(e.X - (theWorld.FrameSize / 2), e.Y - (theWorld.FrameSize / 2));
                theWorld.MousePosition = aimDir;
                aimDir.Normalize();

                theController.HandleMouseMovement(aimDir);
            }
        }

        private void SelectColor()
        {
            tankColor = "default";
            // Setup the color select popup
            colorPanel = new ColorPopup();
            // Setup button click handlers
            colorPanel.GreenButton.Click += new System.EventHandler(this.GreenButton_Click);
            colorPanel.BlueButton.Click += new System.EventHandler(this.BlueButton_Click);
            colorPanel.RedButton.Click += new System.EventHandler(this.RedButton_Click);
            colorPanel.OrangeButton.Click += new System.EventHandler(this.OrangeButton_Click);
            colorPanel.MagentaButton.Click += new System.EventHandler(this.MagentaButton_Click);
            colorPanel.YellowButton.Click += new System.EventHandler(this.YellowButton_Click);
            colorPanel.CyanButton.Click += new System.EventHandler(this.CyanButton_Click);
            colorPanel.PurpleButton.Click += new System.EventHandler(this.PurpleButton_Click);
            colorPanel.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorPanel_Closing);
            colorPanel.CountdownTime.Text = "10";

            // Start a 10 second timer to auto close the window
            Thread t = new Thread(ColorSelectDelay);
            t.Start();
            timerSound.Play();

            // Open the window
            colorPanel.ShowDialog();
            valid = false;
            song.settings.setMode("Loop", true);
            song.Ctlcontrols.play();
        }

        private void ColorSelectDelay()
        {
            // Setup a timer till the window closes
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (valid && watch.ElapsedMilliseconds < 1000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "9"; }));
            while (valid && watch.ElapsedMilliseconds < 2000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "8"; }));
            while (valid && watch.ElapsedMilliseconds < 3000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "7"; }));
            while (valid && watch.ElapsedMilliseconds < 4000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "6"; }));
            while (valid && watch.ElapsedMilliseconds < 5000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "5"; }));
            while (valid && watch.ElapsedMilliseconds < 6000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "4"; }));
            while (valid && watch.ElapsedMilliseconds < 7000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "3"; }));
            while (valid && watch.ElapsedMilliseconds < 8000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "2"; }));
            while (valid && watch.ElapsedMilliseconds < 9000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "1"; }));
            while (valid && watch.ElapsedMilliseconds < 10000) { }
            if (valid) colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.CountdownTime.Text = "0"; }));
            while (valid && watch.ElapsedMilliseconds < 11000) { }
            if (valid)
                colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void GreenButton_Click(object sender, EventArgs e)
        {
            tankColor = "green";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            tankColor = "blue";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void RedButton_Click(object sender, EventArgs e)
        {
            tankColor = "red";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void OrangeButton_Click(object sender, EventArgs e)
        {
            tankColor = "orange";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void MagentaButton_Click(object sender, EventArgs e)
        {
            tankColor = "magenta";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void YellowButton_Click(object sender, EventArgs e)
        {
            tankColor = "yellow";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void CyanButton_Click(object sender, EventArgs e)
        {
            tankColor = "cyan";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void PurpleButton_Click(object sender, EventArgs e)
        {
            tankColor = "purple";
            valid = false;
            colorPanel.Invoke(new MethodInvoker(delegate { colorPanel.Close(); }));
        }

        private void ColorPanel_Closing(object sender, FormClosingEventArgs e)
        {
            valid = false;
            timerSound.Stop();
        }
    }
}