using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using AxWMPLib;
using Models;

namespace TankWars
{
    public class DrawingPanel : Panel
    {
        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        private const int multiplier = 4000;
        private bool isRespawning;

        // Various image assets for drawing
        private World theWorld;
        private Image proj;
        private Image powerup;
        private Image background;
        private Image farBackground;
        private Image wall;
        private Image explo;
        private Image GreenTank;
        private Image GreenTurret;
        private Image BlueTank;
        private Image BlueTurret;
        private Image MagentaTank;
        private Image MagentaTurret;
        private Image OrangeTank;
        private Image OrangeTurret;
        private Image PurpleTank;
        private Image PurpleTurret;
        private Image RedTank;
        private Image RedTurret;
        private Image YellowTank;
        private Image YellowTurret;
        private Image CyanTank;
        private Image CyanTurret;

        // Sound dictionary
        private Dictionary<string, AxWindowsMediaPlayer> sounds;

        private SoundPlayer tankDeathSound;
        private SoundPlayer shotSound;
        private SoundPlayer beamSound;
        private SoundPlayer powerupCollectedSound;
        private SoundPlayer respawnSound;


        public DrawingPanel(World w)
        {
            // Settings
            DoubleBuffered = true;
            theWorld = w;
            // Load images
            proj = Image.FromFile("..\\..\\..\\Resources\\Images\\Bullet.png");
            powerup = Image.FromFile("..\\..\\..\\Resources\\Images\\Powerup.png");
            background = Image.FromFile("..\\..\\..\\Resources\\Images\\Background.png");
            farBackground = Image.FromFile("..\\..\\..\\Resources\\Images\\FarBackground.png");
            wall = Image.FromFile("..\\..\\..\\Resources\\Images\\WallSprite.png");
            explo = Image.FromFile("..\\..\\..\\Resources\\Images\\Explosion.png");
            GreenTank = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTank.png");
            GreenTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTurret.png");
            BlueTank = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTank.png");
            BlueTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTurret.png");
            MagentaTank = Image.FromFile("..\\..\\..\\Resources\\Images\\MagentaTank.png");
            MagentaTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\MagentaTurret.png");
            OrangeTank = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTank.png");
            OrangeTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTurret.png");
            PurpleTank = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTank.png");
            PurpleTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTurret.png");
            RedTank = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTank.png");
            RedTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTurret.png");
            YellowTank = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTank.png");
            YellowTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTurret.png");
            CyanTank = Image.FromFile("..\\..\\..\\Resources\\Images\\CyanTank.png");
            CyanTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\CyanTurret.png");

            // Images for .dll
            //string currDirectory = Directory.GetCurrentDirectory();
            //proj = Image.FromFile(currDirectory + "\\Resources\\Bullet.png");
            //powerup = Image.FromFile(currDirectory + "\\Resources\\Powerup.png");
            //background = Image.FromFile(currDirectory + "\\Resources\\Background.png");
            //farBackground = Image.FromFile(currDirectory + "\\Resources\\FarBackground.png");
            //wall = Image.FromFile(currDirectory + "\\Resources\\WallSprite.png");
            //explo = Image.FromFile(currDirectory + "\\Resources\\Explosion.png");

            //GreenTank = Image.FromFile(currDirectory + "\\Resources\\GreenTank.png");
            //GreenTurret = Image.FromFile(currDirectory + "\\Resources\\GreenTurret.png");
            //BlueTank = Image.FromFile(currDirectory + "\\Resources\\BlueTank.png");
            //BlueTurret = Image.FromFile(currDirectory + "\\Resources\\BlueTurret.png");
            //MagentaTank = Image.FromFile(currDirectory + "\\Resources\\MagentaTank.png");
            //MagentaTurret = Image.FromFile(currDirectory + "\\Resources\\MagentaTurret.png");
            //OrangeTank = Image.FromFile(currDirectory + "\\Resources\\OrangeTank.png");
            //OrangeTurret = Image.FromFile(currDirectory + "\\Resources\\OrangeTurret.png");
            //PurpleTank = Image.FromFile(currDirectory + "\\Resources\\PurpleTank.png");
            //PurpleTurret = Image.FromFile(currDirectory + "\\Resources\\PurpleTurret.png");
            //RedTank = Image.FromFile(currDirectory + "\\Resources\\RedTank.png");
            //RedTurret = Image.FromFile(currDirectory + "\\Resources\\RedTurret.png");
            //YellowTank = Image.FromFile(currDirectory + "\\Resources\\YellowTank.png");
            //YellowTurret = Image.FromFile(currDirectory + "\\Resources\\YellowTurret.png");
            //CyanTank = Image.FromFile(currDirectory + "\\Resources\\CyanTank.png");
            //CyanTurret = Image.FromFile(currDirectory + "\\Resources\\CyanTurret.png");


            // Create sounds list
            sounds = new Dictionary<string, AxWindowsMediaPlayer>();

            tankDeathSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Explosion.wav");
            shotSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Shot.wav");
            beamSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Beam.wav");
            powerupCollectedSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Collected.wav");
            respawnSound = new SoundPlayer("..\\..\\..\\Resources\\Audio\\Respawn.wav");

            // For DLLs
            //string currDirectory = Directory.GetCurrentDirectory();
            //tankDeathSound = new SoundPlayer(currDirectory + "\\Resources\\Audio\\Explosion.wav");
            //shotSound = new SoundPlayer(currDirectory + "\\Resources\\Audio\\Shot.wav");
            //beamSound = new SoundPlayer(currDirectory + "\\Resources\\Audio\\Beam.wav");
            //powerupCollectedSound = new SoundPlayer(currDirectory + "\\Resources\\Audio\\Collected.wav");
            //respawnSound = new SoundPlayer(currDirectory + "\\Resources\\Audio\\Respawn.wav");
        }

        ///// <summary>
        ///// Adds a specified Media Player sound object to the list of sounds.
        ///// </summary>
        //public void AddSound(string nameOfSound, AxWindowsMediaPlayer theSound)
        //{
        //    sounds.Add(nameOfSound, theSound);
        //}

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX * (float)theWorld.ZoomLevel, (int)worldY * (float)theWorld.ZoomLevel);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            // Draw the tank
            if (t.Health != 0)
            {
                e.Graphics.DrawImage(SelectColor(t, true), -30F * (float)theWorld.ZoomLevel, -30F * (float)theWorld.ZoomLevel, (float)(theWorld.TankSize * theWorld.ZoomLevel), (float)(theWorld.TankSize * theWorld.ZoomLevel));
            }
        }

        private void TurretDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            // Draw the turret
            if (t.Health != 0)
            {
                e.Graphics.DrawImage(SelectColor(t, false), -25f * (float)theWorld.ZoomLevel, -25f * (float)theWorld.ZoomLevel, 50f * (float)theWorld.ZoomLevel, 50f * (float)theWorld.ZoomLevel);
            }
        }

        private void PlayerInfoDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            if (t.Health != 0 && theWorld.ZoomLevel > 0.5)
            {
                // Draw name + score
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(t.Name + ": " + t.Score, this.Font, new SolidBrush(Color.White), 0, 35, format);

                // Draw health
                Pen p = null;
                switch (t.Health)
                {
                    case 3:
                        p = new Pen(Color.Green, 6);
                        e.Graphics.DrawLine(p, -20, -40, 20, -40);
                        break;
                    case 2:
                        p = new Pen(Color.Yellow, 6);
                        e.Graphics.DrawLine(p, -20, -40, 5, -40);
                        break;
                    case 1:
                        p = new Pen(Color.Red, 6);
                        e.Graphics.DrawLine(p, -20, -40, -10, -40);
                        break;
                    case 0:
                        // dead
                        break;
                }
            }
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void BeamDrawer(object o, PaintEventArgs e)
        {
            BeamAnimation b = o as BeamAnimation;

            if (b.FrameCount > 0)
            {
                // Draw the image
                Pen p = new Pen(Color.White, b.FrameCount / 6);
                e.Graphics.DrawLine(p, 0, 0, 0, -1 * multiplier);

                // Decrement the counter
                b.FrameCount--;
            }
            else
            {
                b.IsDead = true;
            }
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void DeathDrawer(object o, PaintEventArgs e)
        {
            DeathAnimation d = o as DeathAnimation;

            if (d.FrameCount > 0)
            {
                //Draw the next frame in the animation.
                e.Graphics.DrawImage(explo, -60f * (float)theWorld.ZoomLevel, -60f * (float)theWorld.ZoomLevel, 120f * (float)theWorld.ZoomLevel, 120f * (float)theWorld.ZoomLevel);
                
                // Decrement the counter
                d.FrameCount--;
            }
            else
                d.IsDead = true;
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            // Maybe the color of the proj is required? so have some get color code like the tank

            // Draw the image
            e.Graphics.DrawImage(proj, -15f * (float)theWorld.ZoomLevel, -15f * (float)theWorld.ZoomLevel, 30f * (float)theWorld.ZoomLevel, 30f * (float)theWorld.ZoomLevel);
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void PowerupDrawer(object o, PaintEventArgs e)
        {
            Powerup pwr = o as Powerup;

            // Get the location of the tank object
            double powerupLocX = pwr.Location.GetX();
            double powerupLocY = pwr.Location.GetY();

            // Draw the image
            e.Graphics.DrawImage(powerup, -10f * (float)theWorld.ZoomLevel, -10f * (float)theWorld.ZoomLevel, 30f * (float)theWorld.ZoomLevel, 30f * (float)theWorld.ZoomLevel);
        }

        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall wall = o as Wall;
            e.Graphics.DrawImage(this.wall, -25f * (float)theWorld.ZoomLevel, -25f * (float)theWorld.ZoomLevel, (float)(theWorld.WallSize * theWorld.ZoomLevel), (float)(theWorld.WallSize * theWorld.ZoomLevel));
        }

        private void DrawWall(Wall wall, PaintEventArgs e)
        {
            //{ "wall":1,"p1":{ "x":-575.0,"y":-575.0},"p2":{ "x":-575.0,"y":575.0} }
            double P1_Y = wall.Point1.GetY(), P1_X = wall.Point1.GetX();
            double P2_Y = wall.Point2.GetY(), P2_X = wall.Point2.GetX();
            double StartPoint = 0;
            double EndPoint = 0;

            // wall is verticle
            if (P1_X == P2_X)
            {
                // Which Y coord is the lowest
                if (P1_Y > P2_Y)
                {
                    StartPoint = P1_Y;
                    EndPoint = P2_Y;
                }
                else
                {
                    StartPoint = P2_Y;
                    EndPoint = P1_Y;
                }

                // Loop drawing a 50px sprite from start to end point
                while (StartPoint >= EndPoint)
                {
                    DrawObjectWithTransform(e, wall, P1_X, StartPoint,
                        0, WallDrawer);
                    StartPoint -= 50;
                }
            }
            // wall is horizontal
            else
            {
                // Which X coord is the rightmost
                if (P1_X > P2_X)
                {
                    StartPoint = P1_X;
                    EndPoint = P2_X;
                }
                else
                {
                    StartPoint = P2_X;
                    EndPoint = P1_X;
                }

                // Loop drawing a 50px sprite from start to end point
                while (StartPoint >= EndPoint)
                {
                    DrawObjectWithTransform(e, wall, StartPoint, P1_Y,
                        0, WallDrawer);
                    StartPoint -= 50;
                }
            }
        }

        private Image SelectColor(Tank t, bool isBody)
        {
            Image body = null;
            Image turret = null;

            switch (t.Color)
            {
                case "red":
                    body = RedTank; turret = RedTurret; break;
                case "blue":
                    body = BlueTank; turret = BlueTurret; break;
                case "green":
                    body = GreenTank; turret = GreenTurret; break;
                case "purple":
                    body = PurpleTank; turret = PurpleTurret; break;
                case "orange":
                    body = OrangeTank; turret = OrangeTurret; break;
                case "cyan":
                    body = CyanTank; turret = CyanTurret; break;
                case "magenta":
                    body = MagentaTank; turret = MagentaTurret; break;
                case "yellow":
                    body = YellowTank; turret = YellowTurret; break;
                case "default":
                    if (t.ID % 8 == 0)
                    { body = RedTank; turret = RedTurret; }
                    else if (t.ID % 8 == 1)
                    { body = BlueTank; turret = BlueTurret; }
                    else if (t.ID % 8 == 2)
                    { body = GreenTank; turret = GreenTurret; }
                    else if (t.ID % 8 == 3)
                    { body = PurpleTank; turret = PurpleTurret; }
                    else if (t.ID % 8 == 4)
                    { body = OrangeTank; turret = OrangeTurret; }
                    else if (t.ID % 8 == 5)
                    { body = CyanTank; turret = CyanTurret; }
                    else if (t.ID % 8 == 6)
                    { body = MagentaTank; turret = MagentaTurret; }
                    else if (t.ID % 8 == 7)
                    { body = YellowTank; turret = YellowTurret; }
                    break;
            }

            if (isBody)
                return body;
            else
                return turret;
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            // Do not draw if we have not received our tank
            if (theWorld.ClientTank.ID == -1)
                return;

            // Update the object's coordinate system to center the view on the player
            double playerX = theWorld.ClientTank.Location.GetX();
            double playerY = theWorld.ClientTank.Location.GetY();
            e.Graphics.TranslateTransform((float)(-(playerX * theWorld.ZoomLevel) + (theWorld.FrameSize / 2)),
                (float)(-(playerY * theWorld.ZoomLevel) + (theWorld.FrameSize / 2)));

            // Draw the backgrounds
            lock (theWorld)
            {
                // The farther background is drawn set in place but scrolls slightly to add depth and perspective
                e.Graphics.DrawImage(farBackground, (float)((-theWorld.Size) - (-playerX) * .3),
                                                    (float)((-theWorld.Size) - (-playerY) * .3),
                                                    (float)(theWorld.Size * (theWorld.ZoomLevel + 2)), (float)(theWorld.Size * (theWorld.ZoomLevel + 2)));
                // The playing field is drawn to simulate moving in the world with a camera focused on the player
                e.Graphics.DrawImage(background, (float)(-(theWorld.Size) / 2) * (float)theWorld.ZoomLevel, (float)(-(theWorld.Size) / 2) * (float)theWorld.ZoomLevel,
                (float)(theWorld.Size) * (float)theWorld.ZoomLevel, (float)(theWorld.Size) * (float)theWorld.ZoomLevel);
            }

            // Draw the walls
            lock (theWorld)
            {
                foreach (Wall wall in theWorld.GetWalls())
                {
                    DrawWall(wall, e);
                }
            }

            // Draw the players
            lock (theWorld)
            {
                foreach (Tank tank in theWorld.GetTanks())
                {
                    // Is this the client tank
                    if (tank == theWorld.ClientTank)
                    {
                        // If dead, set flag true
                        if (tank.Health == 0)
                        {
                            isRespawning = true;
                            continue;
                        }
                        // Else alive
                        // If flag is true, this is respawn frame
                        if (isRespawning)
                        {
                            //sounds.TryGetValue("respawnSound", out AxWindowsMediaPlayer respawnSound);
                            //respawnSound.Ctlcontrols.play();

                            respawnSound.Play();

                            isRespawning = false; 
                        }
                    }

                    if (tank.Health > 0)
                    {
                        // Draw the tank body
                        tank.BodyDirection.Normalize();
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), tank.BodyDirection.ToAngle(), TankDrawer);
                        // Draw the tank turret
                        tank.TurretDirection.Normalize();
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), tank.TurretDirection.ToAngle(), TurretDrawer);
                        // Draw the tank information
                        DrawObjectWithTransform(e, tank, tank.Location.GetX(), tank.Location.GetY(), 0, PlayerInfoDrawer);
                    }
                }
            }

            // Draw the powerups
            lock (theWorld)
            {
                // Play audio if needed
                if (theWorld.PowerupCollected)
                {
                    //sounds.TryGetValue("powerupCollectedSound", out AxWindowsMediaPlayer powerupCollectedSound);
                    //powerupCollectedSound.Ctlcontrols.play();

                    powerupCollectedSound.Play();

                    theWorld.PowerupCollected = false;
                }

                foreach (Powerup pow in theWorld.GetPowerups())
                {
                    DrawObjectWithTransform(e, pow, pow.Location.GetX(), pow.Location.GetY(), 0, PowerupDrawer);
                }
            }

            // Draw the projectiles
            lock (theWorld)
            {
                foreach (Projectile proj in theWorld.GetProjectiles())
                {
                    DrawObjectWithTransform(e, proj, proj.Location.GetX(), proj.Location.GetY(), proj.Direction.ToAngle(), ProjectileDrawer);
                    if (!proj.HasPlayedAudio)
                    {
                        //sounds.TryGetValue("shotSound", out AxWindowsMediaPlayer shotSound);
                        //shotSound.Ctlcontrols.play();

                        shotSound.Play();

                        proj.HasPlayedAudio = true;
                    }
                }
            }

            // Animate the tank deaths
            lock (theWorld)
            {
                foreach (DeathAnimation death in theWorld.GetTankDeaths())
                {
                    DrawObjectWithTransform(e, death, death.Tank.Location.GetX(), death.Tank.Location.GetY(), 0, DeathDrawer);
                    if (!death.HasPlayedAudio)
                    {
                        //sounds.TryGetValue("tankDeathSound", out AxWindowsMediaPlayer tankDeathSound);
                        //tankDeathSound.Ctlcontrols.play();

                        tankDeathSound.Play();

                        death.HasPlayedAudio = true;
                    }
                }

                List<DeathAnimation> deaths = (List<DeathAnimation>)theWorld.GetTankDeaths();

                // Check if any tank deaths have finished their animation, remove if yes
                for (int i = 0; i < deaths.Count(); i++)
                {
                    if (deaths[i].IsDead)
                        theWorld.RemoveTankDeath(deaths[i]);
                }
            }

            // Animate the beams
            lock (theWorld)
            {
                // Draw each beam in it's current state
                foreach (BeamAnimation animation in theWorld.GetBeams())
                {
                    DrawObjectWithTransform(e, animation, animation.Beam.Origin.GetX(), animation.Beam.Origin.GetY(), animation.Beam.Direction.ToAngle(), BeamDrawer);
                    if (!animation.HasPlayedAudio)
                    {
                        //sounds.TryGetValue("beamSound", out AxWindowsMediaPlayer beamSound);
                        //beamSound.Ctlcontrols.play();

                        beamSound.Play();

                        animation.HasPlayedAudio = true;
                    }
                }

                List<BeamAnimation> beams = (List<BeamAnimation>)theWorld.GetBeams();

                // Check if any beams have finished their animation, remove if yes
                for (int i = 0; i < beams.Count(); i++)
                {
                    if (beams[i].IsDead)
                    {
                        theWorld.RemoveBeam(beams[i]);
                    }
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}