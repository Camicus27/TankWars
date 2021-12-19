
namespace TankWars
{
    partial class TankGameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TankGameWindow));
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ServerText = new System.Windows.Forms.TextBox();
            this.NameText = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.HelpButtonPanel = new System.Windows.Forms.Panel();
            this.HelpButtonStrip = new System.Windows.Forms.MenuStrip();
            this.HelpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ControlsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Connect = new System.Windows.Forms.Button();
            this.PlayerNameTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ServerTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.ConnectTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.GamemodeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.HelpButtonPanel.SuspendLayout();
            this.HelpButtonStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(0, 17);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(91, 26);
            this.ServerLabel.TabIndex = 1;
            this.ServerLabel.Text = "Server:";
            // 
            // ServerText
            // 
            this.ServerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerText.Location = new System.Drawing.Point(104, 17);
            this.ServerText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerText.Name = "ServerText";
            this.ServerText.Size = new System.Drawing.Size(119, 27);
            this.ServerText.TabIndex = 2;
            this.ServerText.Text = "localhost";
            this.ServerTooltip.SetToolTip(this.ServerText, "Type your desired server to connect to.");
            this.ServerText.TextChanged += new System.EventHandler(this.ServerText_TextChanged);
            // 
            // NameText
            // 
            this.NameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameText.Location = new System.Drawing.Point(404, 17);
            this.NameText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NameText.MaxLength = 16;
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(179, 27);
            this.NameText.TabIndex = 3;
            this.NameText.Text = "Insert Name Here";
            this.PlayerNameTooltip.SetToolTip(this.NameText, "Type your desired player name here. Max 16 characters.");
            this.NameText.TextChanged += new System.EventHandler(this.NameText_TextChanged);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(230, 18);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(161, 26);
            this.NameLabel.TabIndex = 4;
            this.NameLabel.Text = "Player Name:";
            // 
            // HelpButtonPanel
            // 
            this.HelpButtonPanel.Controls.Add(this.HelpButtonStrip);
            this.HelpButtonPanel.Location = new System.Drawing.Point(712, 16);
            this.HelpButtonPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HelpButtonPanel.Name = "HelpButtonPanel";
            this.HelpButtonPanel.Size = new System.Drawing.Size(79, 44);
            this.HelpButtonPanel.TabIndex = 0;
            // 
            // HelpButtonStrip
            // 
            this.HelpButtonStrip.BackColor = System.Drawing.Color.Transparent;
            this.HelpButtonStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.HelpButtonStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpButton});
            this.HelpButtonStrip.Location = new System.Drawing.Point(0, 0);
            this.HelpButtonStrip.Name = "HelpButtonStrip";
            this.HelpButtonStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.HelpButtonStrip.Size = new System.Drawing.Size(79, 31);
            this.HelpButtonStrip.TabIndex = 0;
            this.HelpButtonStrip.Text = "menuStrip1";
            // 
            // HelpButton
            // 
            this.HelpButton.AutoToolTip = true;
            this.HelpButton.BackColor = System.Drawing.Color.Salmon;
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ControlsButton,
            this.AboutButton});
            this.HelpButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(62, 27);
            this.HelpButton.Text = "Help";
            this.HelpButton.ToolTipText = "The help menu.";
            // 
            // ControlsButton
            // 
            this.ControlsButton.AutoToolTip = true;
            this.ControlsButton.BackColor = System.Drawing.Color.MistyRose;
            this.ControlsButton.Name = "ControlsButton";
            this.ControlsButton.Size = new System.Drawing.Size(161, 28);
            this.ControlsButton.Text = "Controls";
            this.ControlsButton.ToolTipText = "Learn the controls.";
            this.ControlsButton.Click += new System.EventHandler(this.ControlsButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.AutoToolTip = true;
            this.AboutButton.BackColor = System.Drawing.Color.MistyRose;
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(161, 28);
            this.AboutButton.Text = "About";
            this.AboutButton.ToolTipText = "Information about the game.";
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // Connect
            // 
            this.Connect.BackColor = System.Drawing.Color.Salmon;
            this.Connect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Connect.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Connect.FlatAppearance.BorderSize = 0;
            this.Connect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.IndianRed;
            this.Connect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            this.Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connect.Location = new System.Drawing.Point(599, 17);
            this.Connect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(100, 31);
            this.Connect.TabIndex = 5;
            this.Connect.Text = "Connect";
            this.ConnectTooltip.SetToolTip(this.Connect, "Click to connect!");
            this.Connect.UseVisualStyleBackColor = false;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // PlayerNameTooltip
            // 
            this.PlayerNameTooltip.ToolTipTitle = "Player Name";
            // 
            // ServerTooltip
            // 
            this.ServerTooltip.ToolTipTitle = "Server";
            // 
            // GamemodeButton
            // 
            this.GamemodeButton.BackColor = System.Drawing.Color.DarkSalmon;
            this.GamemodeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GamemodeButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.GamemodeButton.Enabled = false;
            this.GamemodeButton.FlatAppearance.BorderSize = 0;
            this.GamemodeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.IndianRed;
            this.GamemodeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            this.GamemodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.GamemodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GamemodeButton.ForeColor = System.Drawing.Color.Black;
            this.GamemodeButton.Location = new System.Drawing.Point(796, 29);
            this.GamemodeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GamemodeButton.Name = "GamemodeButton";
            this.GamemodeButton.Size = new System.Drawing.Size(150, 29);
            this.GamemodeButton.TabIndex = 6;
            this.GamemodeButton.Text = "Basic";
            this.GamemodeButton.UseVisualStyleBackColor = false;
            this.GamemodeButton.Click += new System.EventHandler(this.GamemodeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(802, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Gamemode:";
            // 
            // TankGameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 1153);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GamemodeButton);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.HelpButtonPanel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.ServerText);
            this.Controls.Add(this.ServerLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.HelpButtonStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TankGameWindow";
            this.Text = "Tank Wars";
            this.HelpButtonPanel.ResumeLayout(false);
            this.HelpButtonPanel.PerformLayout();
            this.HelpButtonStrip.ResumeLayout(false);
            this.HelpButtonStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox ServerText;
        private System.Windows.Forms.TextBox NameText;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Panel HelpButtonPanel;
        private System.Windows.Forms.MenuStrip HelpButtonStrip;
        private System.Windows.Forms.ToolStripMenuItem HelpButton;
        private System.Windows.Forms.ToolStripMenuItem ControlsButton;
        private System.Windows.Forms.ToolStripMenuItem AboutButton;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.ToolTip ServerTooltip;
        private System.Windows.Forms.ToolTip PlayerNameTooltip;
        private System.Windows.Forms.ToolTip ConnectTooltip;
        private System.Windows.Forms.Button GamemodeButton;
        private System.Windows.Forms.Label label1;
    }
}

