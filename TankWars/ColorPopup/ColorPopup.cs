using System.Windows.Forms;

namespace TankWars
{
    public class ColorPopup : Form
    {
        private System.Windows.Forms.Label GreenLabel;
        private System.Windows.Forms.Label MagentaLabel;
        private System.Windows.Forms.Label BlueLabel;
        private System.Windows.Forms.Label YellowLabel;
        private System.Windows.Forms.Label PurpleLabel;
        private System.Windows.Forms.Label RedLabel;
        private System.Windows.Forms.Label CyanLabel;
        private System.Windows.Forms.Label OrangeLabel;
        public System.Windows.Forms.Button GreenButton;
        public System.Windows.Forms.Button MagentaButton;
        public System.Windows.Forms.Button BlueButton;
        public System.Windows.Forms.Button YellowButton;
        public System.Windows.Forms.Button OrangeButton;
        public System.Windows.Forms.Button CyanButton;
        public System.Windows.Forms.Button RedButton;
        public System.Windows.Forms.Button PurpleButton;
        private System.Windows.Forms.Label CountdownLabel;
        public System.Windows.Forms.Label CountdownTime;
        private System.ComponentModel.IContainer components = null;

        public ColorPopup()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPopup));
            this.GreenLabel = new System.Windows.Forms.Label();
            this.MagentaLabel = new System.Windows.Forms.Label();
            this.BlueLabel = new System.Windows.Forms.Label();
            this.YellowLabel = new System.Windows.Forms.Label();
            this.PurpleLabel = new System.Windows.Forms.Label();
            this.RedLabel = new System.Windows.Forms.Label();
            this.CyanLabel = new System.Windows.Forms.Label();
            this.OrangeLabel = new System.Windows.Forms.Label();
            this.GreenButton = new System.Windows.Forms.Button();
            this.MagentaButton = new System.Windows.Forms.Button();
            this.BlueButton = new System.Windows.Forms.Button();
            this.YellowButton = new System.Windows.Forms.Button();
            this.OrangeButton = new System.Windows.Forms.Button();
            this.CyanButton = new System.Windows.Forms.Button();
            this.RedButton = new System.Windows.Forms.Button();
            this.PurpleButton = new System.Windows.Forms.Button();
            this.CountdownLabel = new System.Windows.Forms.Label();
            this.CountdownTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GreenLabel
            // 
            this.GreenLabel.AutoSize = true;
            this.GreenLabel.BackColor = System.Drawing.Color.Transparent;
            this.GreenLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GreenLabel.Location = new System.Drawing.Point(18, 9);
            this.GreenLabel.Name = "GreenLabel";
            this.GreenLabel.Size = new System.Drawing.Size(129, 22);
            this.GreenLabel.TabIndex = 0;
            this.GreenLabel.Text = "Green Tank:";
            // 
            // MagentaLabel
            // 
            this.MagentaLabel.AutoSize = true;
            this.MagentaLabel.BackColor = System.Drawing.Color.Transparent;
            this.MagentaLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MagentaLabel.Location = new System.Drawing.Point(175, 9);
            this.MagentaLabel.Name = "MagentaLabel";
            this.MagentaLabel.Size = new System.Drawing.Size(152, 22);
            this.MagentaLabel.TabIndex = 2;
            this.MagentaLabel.Text = "Magenta Tank:";
            // 
            // BlueLabel
            // 
            this.BlueLabel.AutoSize = true;
            this.BlueLabel.BackColor = System.Drawing.Color.Transparent;
            this.BlueLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlueLabel.Location = new System.Drawing.Point(359, 9);
            this.BlueLabel.Name = "BlueLabel";
            this.BlueLabel.Size = new System.Drawing.Size(116, 22);
            this.BlueLabel.TabIndex = 6;
            this.BlueLabel.Text = "Blue Tank:";
            // 
            // YellowLabel
            // 
            this.YellowLabel.AutoSize = true;
            this.YellowLabel.BackColor = System.Drawing.Color.Transparent;
            this.YellowLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YellowLabel.Location = new System.Drawing.Point(515, 9);
            this.YellowLabel.Name = "YellowLabel";
            this.YellowLabel.Size = new System.Drawing.Size(135, 22);
            this.YellowLabel.TabIndex = 10;
            this.YellowLabel.Text = "Yellow Tank:";
            // 
            // PurpleLabel
            // 
            this.PurpleLabel.AutoSize = true;
            this.PurpleLabel.BackColor = System.Drawing.Color.Transparent;
            this.PurpleLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PurpleLabel.Location = new System.Drawing.Point(515, 185);
            this.PurpleLabel.Name = "PurpleLabel";
            this.PurpleLabel.Size = new System.Drawing.Size(137, 22);
            this.PurpleLabel.TabIndex = 18;
            this.PurpleLabel.Text = "Purple Tank:";
            // 
            // RedLabel
            // 
            this.RedLabel.AutoSize = true;
            this.RedLabel.BackColor = System.Drawing.Color.Transparent;
            this.RedLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RedLabel.Location = new System.Drawing.Point(363, 185);
            this.RedLabel.Name = "RedLabel";
            this.RedLabel.Size = new System.Drawing.Size(110, 22);
            this.RedLabel.TabIndex = 16;
            this.RedLabel.Text = "Red Tank:";
            // 
            // CyanLabel
            // 
            this.CyanLabel.AutoSize = true;
            this.CyanLabel.BackColor = System.Drawing.Color.Transparent;
            this.CyanLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CyanLabel.Location = new System.Drawing.Point(191, 185);
            this.CyanLabel.Name = "CyanLabel";
            this.CyanLabel.Size = new System.Drawing.Size(118, 22);
            this.CyanLabel.TabIndex = 14;
            this.CyanLabel.Text = "Cyan Tank:";
            // 
            // OrangeLabel
            // 
            this.OrangeLabel.AutoSize = true;
            this.OrangeLabel.BackColor = System.Drawing.Color.Transparent;
            this.OrangeLabel.Font = new System.Drawing.Font("Elephant", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrangeLabel.Location = new System.Drawing.Point(16, 185);
            this.OrangeLabel.Name = "OrangeLabel";
            this.OrangeLabel.Size = new System.Drawing.Size(140, 22);
            this.OrangeLabel.TabIndex = 12;
            this.OrangeLabel.Text = "Orange Tank:";
            // 
            // GreenButton
            // 
            this.GreenButton.BackColor = System.Drawing.Color.Transparent;
            this.GreenButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GreenButton.BackgroundImage")));
            this.GreenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GreenButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.GreenButton.ForeColor = System.Drawing.Color.Transparent;
            this.GreenButton.Location = new System.Drawing.Point(22, 34);
            this.GreenButton.Name = "GreenButton";
            this.GreenButton.Size = new System.Drawing.Size(125, 120);
            this.GreenButton.TabIndex = 20;
            this.GreenButton.UseVisualStyleBackColor = false;
            // 
            // MagentaButton
            // 
            this.MagentaButton.BackColor = System.Drawing.Color.Transparent;
            this.MagentaButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MagentaButton.BackgroundImage")));
            this.MagentaButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MagentaButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.MagentaButton.ForeColor = System.Drawing.Color.Transparent;
            this.MagentaButton.Location = new System.Drawing.Point(188, 34);
            this.MagentaButton.Name = "MagentaButton";
            this.MagentaButton.Size = new System.Drawing.Size(125, 120);
            this.MagentaButton.TabIndex = 21;
            this.MagentaButton.UseVisualStyleBackColor = false;
            // 
            // BlueButton
            // 
            this.BlueButton.BackColor = System.Drawing.Color.Transparent;
            this.BlueButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BlueButton.BackgroundImage")));
            this.BlueButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BlueButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.BlueButton.ForeColor = System.Drawing.Color.Transparent;
            this.BlueButton.Location = new System.Drawing.Point(354, 34);
            this.BlueButton.Name = "BlueButton";
            this.BlueButton.Size = new System.Drawing.Size(125, 120);
            this.BlueButton.TabIndex = 22;
            this.BlueButton.UseVisualStyleBackColor = false;
            // 
            // YellowButton
            // 
            this.YellowButton.BackColor = System.Drawing.Color.Transparent;
            this.YellowButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("YellowButton.BackgroundImage")));
            this.YellowButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.YellowButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.YellowButton.ForeColor = System.Drawing.Color.Transparent;
            this.YellowButton.Location = new System.Drawing.Point(520, 34);
            this.YellowButton.Name = "YellowButton";
            this.YellowButton.Size = new System.Drawing.Size(125, 120);
            this.YellowButton.TabIndex = 23;
            this.YellowButton.UseVisualStyleBackColor = false;
            // 
            // OrangeButton
            // 
            this.OrangeButton.BackColor = System.Drawing.Color.Transparent;
            this.OrangeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OrangeButton.BackgroundImage")));
            this.OrangeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OrangeButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.OrangeButton.ForeColor = System.Drawing.Color.Transparent;
            this.OrangeButton.Location = new System.Drawing.Point(22, 210);
            this.OrangeButton.Name = "OrangeButton";
            this.OrangeButton.Size = new System.Drawing.Size(125, 120);
            this.OrangeButton.TabIndex = 24;
            this.OrangeButton.UseVisualStyleBackColor = false;
            // 
            // CyanButton
            // 
            this.CyanButton.BackColor = System.Drawing.Color.Transparent;
            this.CyanButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CyanButton.BackgroundImage")));
            this.CyanButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CyanButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.CyanButton.ForeColor = System.Drawing.Color.Transparent;
            this.CyanButton.Location = new System.Drawing.Point(188, 210);
            this.CyanButton.Name = "CyanButton";
            this.CyanButton.Size = new System.Drawing.Size(125, 120);
            this.CyanButton.TabIndex = 25;
            this.CyanButton.UseVisualStyleBackColor = false;
            // 
            // RedButton
            // 
            this.RedButton.BackColor = System.Drawing.Color.Transparent;
            this.RedButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RedButton.BackgroundImage")));
            this.RedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RedButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.RedButton.ForeColor = System.Drawing.Color.Transparent;
            this.RedButton.Location = new System.Drawing.Point(354, 210);
            this.RedButton.Name = "RedButton";
            this.RedButton.Size = new System.Drawing.Size(125, 120);
            this.RedButton.TabIndex = 26;
            this.RedButton.UseVisualStyleBackColor = false;
            // 
            // PurpleButton
            // 
            this.PurpleButton.BackColor = System.Drawing.Color.Transparent;
            this.PurpleButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PurpleButton.BackgroundImage")));
            this.PurpleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PurpleButton.Cursor = System.Windows.Forms.Cursors.Cross;
            this.PurpleButton.ForeColor = System.Drawing.Color.Transparent;
            this.PurpleButton.Location = new System.Drawing.Point(520, 210);
            this.PurpleButton.Name = "PurpleButton";
            this.PurpleButton.Size = new System.Drawing.Size(125, 120);
            this.PurpleButton.TabIndex = 27;
            this.PurpleButton.UseVisualStyleBackColor = false;
            // 
            // CountdownLabel
            // 
            this.CountdownLabel.Font = new System.Drawing.Font("Elephant", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountdownLabel.Location = new System.Drawing.Point(16, 341);
            this.CountdownLabel.Name = "CountdownLabel";
            this.CountdownLabel.Size = new System.Drawing.Size(629, 32);
            this.CountdownLabel.TabIndex = 28;
            this.CountdownLabel.Text = "TIME LEFT:";
            this.CountdownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CountdownTime
            // 
            this.CountdownTime.Font = new System.Drawing.Font("Elephant", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountdownTime.Location = new System.Drawing.Point(22, 373);
            this.CountdownTime.Name = "CountdownTime";
            this.CountdownTime.Size = new System.Drawing.Size(623, 61);
            this.CountdownTime.TabIndex = 29;
            this.CountdownTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColorPopup
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.PropertyPage;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(669, 443);
            this.Controls.Add(this.CountdownTime);
            this.Controls.Add(this.CountdownLabel);
            this.Controls.Add(this.PurpleButton);
            this.Controls.Add(this.RedButton);
            this.Controls.Add(this.CyanButton);
            this.Controls.Add(this.OrangeButton);
            this.Controls.Add(this.YellowButton);
            this.Controls.Add(this.BlueButton);
            this.Controls.Add(this.MagentaButton);
            this.Controls.Add(this.GreenButton);
            this.Controls.Add(this.PurpleLabel);
            this.Controls.Add(this.RedLabel);
            this.Controls.Add(this.CyanLabel);
            this.Controls.Add(this.OrangeLabel);
            this.Controls.Add(this.YellowLabel);
            this.Controls.Add(this.BlueLabel);
            this.Controls.Add(this.MagentaLabel);
            this.Controls.Add(this.GreenLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPopup";
            this.ShowIcon = false;
            this.Text = "Select Your Color:";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

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
    }
}
