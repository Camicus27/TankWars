using System;
using NetworkUtil;
using System.Linq;
using Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml;
using TankWars;
using System.Threading;
using System.Diagnostics;

namespace Server
{
    class ServerController
    {
        // Model of the world
        private static World theWorld;

        // Variables to manage server assets
        private static int IDCounter;
        private static int projectileCount;
        private static int powerupCount;

        static void Main(string[] args)
        {
            // set everything up and start an event loop for accepting
            // connections, and start the frame loop (this one is not
            // an event loop, and is on its own thread).
            Console.Title = "TANK WARS SERVER";
            theWorld = new World();
            IDCounter = 0;
            projectileCount = 0;
            powerupCount = 0;
            // Read the XML and set appropriate model data
            Setup();

            Networking.StartServer(HandleNewClient, 11000);

            // Call update on a new thread
            Thread t1 = new Thread(FrameStaller);
            t1.Start();

            // Start the infinite delay loop on a new thread
            Thread t2 = new Thread(PowerupStaller);
            t2.Start();

            // Continue the main thread infinitely
            Console.WriteLine("Server is running. Now accepting clients.");
            Console.Read();
        }

        /// <summary>
        /// Delegate to handle when a new client is connected to the server
        /// </summary>
        /// <param name="s"></param>
        private static void HandleNewClient(SocketState state)
        {
            Console.WriteLine("Accepted new connection.");
            // Sets socketState's stored callback to be ReceiveName
            state.OnNetworkAction = ReceiveName;
            // Ask the client for data
            Networking.GetData(state);
        }

        /// <summary>
        /// Parsing the data of a given SocketState
        /// and sending the appropriate information
        /// to the client.
        /// </summary>
        /// <param name="state"></param>
        private static void ReceiveName(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                return;
            }

            lock (theWorld)
            {
                // Parse socket.GetData to find sent name
                string data = state.GetData();
                // Remove the newline character
                string[] nameAndColor = Regex.Split(data, "[,]");
                string playerName = nameAndColor[0];

                string playerColor = null;
                if (nameAndColor.Length < 2)
                    playerColor = "default";
                else
                    playerColor = nameAndColor[1].Substring(0, nameAndColor[1].Length - 1);

                state.RemoveData(0, data.Length);

                // Create tank with that name and assign unique ID from socket.ID
                Tank newPlayer = new Tank(Convert.ToInt32(state.ID), playerName, RandomLocation(), new Vector2D(0, -1),
                    new Vector2D(0, -1), 0, theWorld.StartingHP, false, false, true, playerColor);

                // Set the socketState's callback to handle client command requests
                state.OnNetworkAction = HandleClientRequests;

                // Send the startup info to the client
                // Player ID
                Networking.Send(state.TheSocket, newPlayer.ID + "\n");
                // World size
                Networking.Send(state.TheSocket, theWorld.Size + "\n");
                // Walls
                foreach (Wall w in theWorld.GetWalls())
                {
                    Networking.Send(state.TheSocket, JsonConvert.SerializeObject(w) + "\n");
                }

                // Add client's socket to the list of all clients / tanks
                theWorld.AddClient(state);
                theWorld.AddTank(newPlayer.ID, newPlayer);

                Console.WriteLine("Player(" + newPlayer.ID + ") \"" + newPlayer.Name + "\" joined.");
            }

            // Then ask the client for more data (waiting for command requests)
            Networking.GetData(state);
        }

        /// <summary>
        /// A callback method.
        /// 
        /// Checks for errors in the given SocketState
        /// then continues to process the data and retrieve
        /// more.
        /// </summary>
        /// <param name="state"></param>
        private static void HandleClientRequests(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                return;
            }

            // Proccess those server messages
            ProcessMessages(state);

            // Continue the event loop
            Networking.GetData(state);
        }

        /// <summary>
        /// Receives a given SocketState and deserializes
        /// the appropriate data in order to keep the 
        /// server up-to-date.
        /// </summary>
        /// <param name="state"></param>
        private static void ProcessMessages(SocketState state)
        {
            lock (theWorld)
            {
                // Get the data from the socket and parse it
                string totalData = state.GetData();
                string[] parts = Regex.Split(totalData, @"(?<=[\n])");

                // Looping over the data for JSON information
                foreach (string p in parts)
                {
                    // Ignore empty strings added by the regex splitter
                    if (p.Length == 0)
                        continue;
                    // Ensuring our regex splitter
                    if (p[p.Length - 1] != '\n')
                        continue;

                    // Remove the newline character and deserialize into a control object
                    string pp = p.Substring(0, p.Length - 1);
                    try
                    {
                        if (pp == "basic" || pp == "extra")
                            theWorld.gamemode = pp;
                        else
                        {
                            Controls clientRequest = JsonConvert.DeserializeObject<Controls>(pp);
                            // Update the tank state for move requests and turret updates
                            UpdateTankState(clientRequest, Convert.ToInt32(state.ID));
                            // Spawn a projectile if client requests main fire
                            if (clientRequest.FireState == "main")
                                SpawnProjectile(Convert.ToInt32(state.ID));
                            // Spawn a beam if client requests alternate fire
                            if (!theWorld.GamemodeIsExtra && clientRequest.FireState == "alt")
                                SpawnBeam(Convert.ToInt32(state.ID));
                        }
                    }
                    catch
                    {
                        // If a client sends a malformed request, disconnect that client.
                        Console.WriteLine("Client " + state.ID + " has been kicked.");
                        theWorld.RemoveClient(state);
                        theWorld.RemoveTank(Convert.ToInt32(state.ID));
                        state.TheSocket.Disconnect(false);
                    }

                    state.RemoveData(0, p.Length);
                }
            }
        }

        /// <summary>
        /// Translates the Tank controls of a client into
        /// movement within the server.
        /// </summary>
        /// <param name="playerInput"></param>
        /// <param name="ID"></param>
        private static void UpdateTankState(Controls playerInput, int ID)
        {
            string direction = playerInput.MovingDirection;
            Tank playerTank = theWorld.GetTank(ID);

            // Check to be sure tank is not dead
            if (playerTank is null || playerTank.Health == 0)
            {
                return;
            }

            // Update the player's turret direction
            playerTank.TurretDirection = playerInput.AimDirection;

            // Check if move request puts tank into an illegal area and if they have not moved this frame
            if (!playerTank.HasMoved && SafeMovement(playerTank, ref direction))
            {
                switch (direction)
                {
                    case "up":
                        playerTank.BodyDirection = new Vector2D(0, -1);
                        break;
                    case "down":
                        playerTank.BodyDirection = new Vector2D(0, 1);
                        break;
                    case "left":
                        playerTank.BodyDirection = new Vector2D(-1, 0);
                        break;
                    case "right":
                        playerTank.BodyDirection = new Vector2D(1, 0);
                        break;
                }
                playerTank.HasMoved = true;
            }
        }


        /// <summary>
        /// Private helper to determine if a tank can safely move
        /// in the requested direction without collision
        /// </summary>
        /// <param name="playerTank"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private static bool SafeMovement(Tank playerTank, ref string direction)
        {
            // If not moving, just return false
            if (direction == "none")
                return false;

            // Save current position then update tank's position with desired move request
            Vector2D currentLocation = playerTank.Location;
            switch (direction)
            {
                case "up":
                    playerTank.Location = new Vector2D(playerTank.Location.GetX(), playerTank.Location.GetY() - theWorld.EngineStrength);
                    break;
                case "down":
                    playerTank.Location = new Vector2D(playerTank.Location.GetX(), playerTank.Location.GetY() + theWorld.EngineStrength);
                    break;
                case "left":
                    playerTank.Location = new Vector2D(playerTank.Location.GetX() - theWorld.EngineStrength, playerTank.Location.GetY());
                    break;
                case "right":
                    playerTank.Location = new Vector2D(playerTank.Location.GetX() + theWorld.EngineStrength, playerTank.Location.GetY());
                    break;
            }

            // Check each wall section for potential movement collision
            foreach (Wall w in theWorld.GetWalls())
            {
                // If move request puts tank into an illegal area, deny move request
                if (IsIntersectingWithOther(playerTank, w))
                {
                    // Set moving to none
                    direction = "none";
                    // Revert move
                    playerTank.Location = currentLocation;
                    // Safe Movement is false
                    return false;
                }
            }

            // Check to verify the player is within the world bounds, wrap if not
            // If off right border, wrap to the left
            if (playerTank.Location.GetX() > theWorld.Size / 2)
                playerTank.Location = new Vector2D(-(theWorld.Size / 2) + 1, playerTank.Location.GetY());
            // If off left border, wrap to the right
            else if (playerTank.Location.GetX() < -(theWorld.Size / 2))
                playerTank.Location = new Vector2D(theWorld.Size / 2 - 1, playerTank.Location.GetY());
            // If off bottom border, wrap to the top
            else if (playerTank.Location.GetY() > theWorld.Size / 2)
                playerTank.Location = new Vector2D(playerTank.Location.GetX(), -(theWorld.Size / 2) + 1);
            // If off top border, wrap to the bottom
            else if (playerTank.Location.GetY() < -(theWorld.Size / 2))
                playerTank.Location = new Vector2D(playerTank.Location.GetX(), theWorld.Size / 2 - 1);

            return true;
        }

        /// <summary>
        /// Spawns a projectile
        /// </summary>
        /// <param name="ID"></param>
        private static void SpawnProjectile(int ID)
        {
            // Get the owner of the projectile
            Tank playerTank = theWorld.GetTank(ID);

            // Check to be sure tank is not dead
            if (playerTank is null || playerTank.Health == 0)
            {
                return;
            }

            lock (theWorld)
            {
                if (playerTank.CanFire)
                {
                    playerTank.CanFire = false;
                    // Create new projectile object
                    Projectile proj = new Projectile(projectileCount,
                        playerTank.Location, playerTank.TurretDirection, false, ID);
                    // Initialize velocity
                    proj.Velocity *= theWorld.ProjectileSpeed;
                    theWorld.AddProjectile(projectileCount, proj);
                    projectileCount++;
                    Thread t = new Thread(new Timer(playerTank, theWorld, false).Cooldown);
                    t.Start();
                }
            }
        }

        /// <summary>
        /// Spawns a powerup.
        /// </summary>
        private static void SpawnPowerup()
        {
            List<Powerup> pwrList = theWorld.GetPowerups().ToList();
            if (pwrList.Count == theWorld.MaxPowerups)
                return;

            lock (theWorld)
            {
                // Create new powerup in random location
                Powerup pwr = new Powerup(powerupCount, RandomLocation(), false);
                // Add the powerup to the world and increment number of powerups
                theWorld.AddPowerup(powerupCount, pwr);
                powerupCount++;
            }
        }

        /// <summary>
        /// Spawns a beam.
        /// </summary>
        /// <param name="ID"></param>
        private static void SpawnBeam(int ID)
        {
            // Get the owner of the beam
            Tank playerTank = theWorld.GetTank(ID);
            // Check to be sure tank is not dead
            if (playerTank is null || playerTank.Health == 0 || playerTank.Powerups == 0)
                return;

            lock (theWorld)
            {
                // Create new beam at the tank owner's position
                Beam beam = new Beam(ID, theWorld.GetTank(ID).Location, theWorld.GetTank(ID).TurretDirection, ID);

                // Add the beam to the world
                theWorld.AddShotBeam(beam);
                // Remove it from the player's inventory
                playerTank.Powerups--;
            }
        }


        /// <summary>
        /// Private driver method to determine if one object intersects with another
        /// </summary>
        /// <param name="obj1"> | Tank | Proj | Proj | Beam | Pwer | </param>
        /// <param name="obj2"> | Wall | Wall | Tank | Tank | Tank | </param>
        /// <returns></returns>
        private static bool IsIntersectingWithOther(Object obj1, Object obj2)
        {
            if (obj1 is Tank && obj2 is Wall)
            {
                Tank tank = (Tank)obj1;
                Wall wall = (Wall)obj2;

                Vector2D tankUpperLeft = new Vector2D(tank.Location.GetX() - (theWorld.TankSize / 2), tank.Location.GetY() - (theWorld.TankSize / 2));
                Vector2D tankBottomRight = new Vector2D(tank.Location.GetX() + 30, tank.Location.GetY() + 30);
                Vector2D wallUpperLeft = new Vector2D(wall.GetWallHorizontalBounds(theWorld.WallSize / 2).GetY(), wall.GetWallVerticalBounds(theWorld.WallSize / 2).GetX());
                Vector2D wallBottomRight = new Vector2D(wall.GetWallHorizontalBounds(theWorld.WallSize / 2).GetX(), wall.GetWallVerticalBounds(theWorld.WallSize / 2).GetY());

                return RectangleIntersects(tankUpperLeft, tankBottomRight, wallUpperLeft, wallBottomRight);
            }
            else if (obj1 is Projectile && obj2 is Wall)
            {
                Projectile proj = (Projectile)obj1;
                Wall wall = (Wall)obj2;

                Vector2D projUpperLeft = new Vector2D(proj.Location.GetX() - 15, proj.Location.GetY() - 15);
                Vector2D projBottomRight = new Vector2D(proj.Location.GetX() + 15, proj.Location.GetY() + 15);
                Vector2D wallUpperLeft = new Vector2D(wall.GetWallHorizontalBounds(theWorld.WallSize / 2).GetY(), wall.GetWallVerticalBounds(theWorld.WallSize / 2).GetX());
                Vector2D wallBottomRight = new Vector2D(wall.GetWallHorizontalBounds(theWorld.WallSize / 2).GetX(), wall.GetWallVerticalBounds(theWorld.WallSize / 2).GetY());

                return RectangleIntersects(projUpperLeft, projBottomRight, wallUpperLeft, wallBottomRight);
            }
            else if (obj1 is Projectile && obj2 is Tank)
            {
                Projectile proj = (Projectile)obj1;
                Tank tank = (Tank)obj2;

                Vector2D projUpperLeft = new Vector2D(proj.Location.GetX() - 15, proj.Location.GetY() - 15);
                Vector2D projBottomRight = new Vector2D(proj.Location.GetX() + 15, proj.Location.GetY() + 15);
                Vector2D tankUpperLeft = new Vector2D(tank.Location.GetX() - (theWorld.TankSize / 2), tank.Location.GetY() - (theWorld.TankSize / 2));
                Vector2D tankBottomRight = new Vector2D(tank.Location.GetX() + (theWorld.TankSize / 2), tank.Location.GetY() + (theWorld.TankSize / 2));

                return RectangleIntersects(projUpperLeft, projBottomRight, tankUpperLeft, tankBottomRight);
            }
            else if (obj1 is Beam && obj2 is Tank)
            {
                Beam beam = (Beam)obj1;
                Tank tank = (Tank)obj2;

                return Intersects(beam.Origin, beam.Direction, tank.Location, theWorld.TankSize / 2);
            }
            else if (obj1 is Powerup && obj2 is Tank)
            {
                Powerup pwr = (Powerup)obj1;
                Tank tank = (Tank)obj2;

                Vector2D pwrUpperLeft = new Vector2D(pwr.Location.GetX() - 15, pwr.Location.GetY() - 15);
                Vector2D pwrBottomRight = new Vector2D(pwr.Location.GetX() + 15, pwr.Location.GetY() + 15);
                Vector2D tankUpperLeft = new Vector2D(tank.Location.GetX() - (theWorld.TankSize / 2), tank.Location.GetY() - (theWorld.TankSize / 2));
                Vector2D tankBottomRight = new Vector2D(tank.Location.GetX() + (theWorld.TankSize / 2), tank.Location.GetY() + (theWorld.TankSize / 2));

                return RectangleIntersects(pwrUpperLeft, pwrBottomRight, tankUpperLeft, tankBottomRight);
            }

            return false;
        }


        /// <summary>
        /// Check if two rectangles intersect
        /// </summary>
        /// <param name="upperLeft_1"></param>
        /// <param name="bottomRight_1"></param>
        /// <param name="upperLeft_2"></param>
        /// <param name="bottomRight_2"></param>
        /// <returns></returns>
        private static bool RectangleIntersects(Vector2D upperLeft_1, Vector2D bottomRight_1,
                          Vector2D upperLeft_2, Vector2D bottomRight_2)
        {
            // If one rectangle is on left side of other
            if (upperLeft_1.GetX() >= bottomRight_2.GetX() || upperLeft_2.GetX() >= bottomRight_1.GetX())
            {
                return false;
            }

            // If one rectangle is above other
            if (bottomRight_1.GetY() < upperLeft_2.GetY() || bottomRight_2.GetY() < upperLeft_1.GetY())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if a ray interescts a circle
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        public static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substituting to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }


        /// <summary>
        /// Reads the settings file and creates the world
        /// </summary>
        private static void Setup()
        {
            // Read settings.xml, update all appropriate information in world model
            // "..\\..\\..\\..\\Resources\\settings.xml"
            // ".\\settings.xml"

            try
            {
                using (XmlReader reader = XmlReader.Create("..\\..\\..\\..\\Resources\\settings.xml"))
                {
                    // While there is another XML tag
                    while (reader.Read())
                    {
                        // If the next item is an XML tag
                        if (reader.IsStartElement())
                        {
                            // Read the name of the tag
                            switch (reader.Name.ToString())
                            {
                                case "UniverseSize":
                                    theWorld.Size = reader.ReadElementContentAsInt();
                                    break;
                                case "MSPerFrame":
                                    theWorld.MSPerFrame = reader.ReadElementContentAsInt();
                                    break;
                                case "FramesPerShot":
                                    theWorld.FramesPerShot = reader.ReadElementContentAsInt();
                                    break;
                                case "RespawnRate":
                                    theWorld.RespawnRate = reader.ReadElementContentAsInt();
                                    break;
                                case "StartingHP":
                                    theWorld.StartingHP = reader.ReadElementContentAsInt();
                                    break;
                                case "ProjectileSpeed":
                                    theWorld.ProjectileSpeed = reader.ReadElementContentAsInt();
                                    break;
                                case "EngineStrength":
                                    theWorld.EngineStrength = reader.ReadElementContentAsInt();
                                    break;
                                case "TankSize":
                                    theWorld.TankSize = reader.ReadElementContentAsInt();
                                    break;
                                case "WallSize":
                                    theWorld.WallSize = reader.ReadElementContentAsInt();
                                    break;
                                case "MaxPowerups":
                                    theWorld.MaxPowerups = reader.ReadElementContentAsInt();
                                    break;
                                case "MaxPowerupDelay":
                                    theWorld.MaxPowerupDelay = reader.ReadElementContentAsInt();
                                    break;
                                case "Gamemode":
                                    theWorld.gamemode = reader.ReadElementContentAsString();
                                    break;
                                case "Wall":
                                    // Whitespace
                                    reader.Read();
                                    // P1 header
                                    reader.Read();
                                    // X header
                                    reader.Read();
                                    // Assign P1 x-coord
                                    double p1_x = reader.ReadElementContentAsDouble();
                                    // Assign P1 y-coord
                                    double p1_y = reader.ReadElementContentAsDouble();
                                    // Whitespace
                                    reader.Read();
                                    // P2 header
                                    reader.Read();
                                    // X header
                                    reader.Read();
                                    // Assign P2 x-coord
                                    double p2_x = reader.ReadElementContentAsDouble();
                                    // Assign P2 y-coord
                                    double p2_y = reader.ReadElementContentAsDouble();
                                    // Create a new wall
                                    Vector2D p1 = new Vector2D(p1_x, p1_y);
                                    Vector2D p2 = new Vector2D(p2_x, p2_y);
                                    theWorld.AddWall(IDCounter, new Wall(IDCounter, p1, p2));
                                    IDCounter++;
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Something wrong with reading the settings file
                Console.WriteLine("There was an error reading the settings.xml");
            }
        }


        /// <summary>
        /// Helper method to determine a valid, random spawn location
        /// </summary>
        /// <returns>Valid spawnpoint</returns>
        private static Vector2D RandomLocation()
        {
            // Find where the walls are, to avoid them
            // Keep player within world size boundaries
            bool isValid = false;
            Double loc_x = 0;
            Double loc_y = 0;

            // Loop until a valid spot is found
            while (!isValid)
            {
                // Create a random spot inside world border
                Random r = new Random();
                loc_x = r.NextDouble() * ((theWorld.Size / 2) - (-(theWorld.Size / 2))) + (-(theWorld.Size / 2));
                loc_y = r.NextDouble() * ((theWorld.Size / 2) - (-(theWorld.Size / 2))) + (-(theWorld.Size / 2));

                // Create a "ghost" tank to represent the spot so we can test for collisions
                Tank t = new Tank(-1, "temp", new Vector2D(loc_x, loc_y), new Vector2D(0, 0), new Vector2D(0, 0), 0, 0, false, false, false, "");

                isValid = true;

                // Check each wall section for spawn point collision
                foreach (Wall w in theWorld.GetWalls())
                {
                    // If spawn point collides with wall
                    if (IsIntersectingWithOther(t, w))
                        // Invalidate the spawn
                        isValid = false;
                    // If the world has projectiles
                    if (theWorld.GetProjectiles().Any())
                    {
                        // Check if spawn point collides with any of them
                        foreach (Projectile p in theWorld.GetProjectiles())
                        {
                            if (IsIntersectingWithOther(p, t))
                                // Invalidate the spawn
                                isValid = false;
                        }
                    }
                }
            }

            // Spawnpoint is valid, return
            return new Vector2D(loc_x, loc_y);
        }


        /// <summary>
        /// Updates the world state on every frame
        /// To be invoked every frame loop iteration
        /// </summary>
        private static void Update()
        {
            lock (theWorld)
            {
                // Check if any clients have disconnected and remove if so
                List<SocketState> clientList = theWorld.GetClients().ToList();
                for (int i = 0; i < clientList.Count; i++)
                {
                    if (!(clientList[i].TheSocket.Connected))
                    {
                        theWorld.RemoveClient(clientList[i]);
                        theWorld.RemoveTank(Convert.ToInt32(clientList[i].ID));
                        Console.WriteLine("Client " + clientList[i].ID + " has disconnected.");
                    }
                }

                // Update all projectiles
                foreach (Projectile proj in theWorld.GetProjectiles())
                {
                    // Add 1 velocity to the current location to move it
                    proj.Location += proj.Velocity;

                    // check for collisions with tanks and walls
                    //  if colliding with a tank, be sure to update that tank's health
                    foreach (Tank tank in theWorld.GetTanks())
                    {
                        if (proj.Owner != tank.ID && IsIntersectingWithOther(proj, tank) && !tank.Died)
                        {
                            proj.Died = true;
                            tank.Health--;
                            if (tank.Health == 0)
                            {
                                // Get projectile owner and increase score
                                // If kill from the grave, get dead projectile owner
                                try { theWorld.GetTank(proj.Owner).Score++; }
                                catch { theWorld.GetDeadTank(proj.Owner).Score++; }

                                tank.Died = true;
                            }
                        }
                    }

                    foreach (Wall wall in theWorld.GetWalls())
                    {
                        if (IsIntersectingWithOther(proj, wall))
                        {
                            proj.Died = true;
                        }
                    }
                }

                // Update all powerups
                foreach (Powerup pwr in theWorld.GetPowerups())
                {
                    // Check for collisions with tanks
                    //  if colliding with tank, that tank 'collects' the powerup, kill the powerup
                    foreach (Tank tank in theWorld.GetTanks())
                    {
                        if (IsIntersectingWithOther(pwr, tank) && !tank.Died)
                        {
                            pwr.Died = true;
                            // Increment tank's powerups count
                            tank.Powerups++;
                            tank.PowerupDuration += 240;
                        }
                    }
                }

                if (theWorld.GamemodeIsExtra)
                {
                    foreach (Tank t in theWorld.GetTanks())
                    {
                        if (t.Powerups > 0)
                        {
                            if (t.PowerupDuration > 0)
                                t.PowerupDuration--;
                            if (t.PowerupDuration == 0)
                                t.Powerups--;
                        }
                    }
                }

                // Check for beam-tank collisions
                //  if colliding with tank, that tank is destroyed
                if (!theWorld.GamemodeIsExtra)
                {
                    foreach (Beam beam in theWorld.GetShotBeams())
                    {
                        foreach (Tank tank in theWorld.GetTanks())
                        {
                            if (IsIntersectingWithOther(beam, tank) && !tank.Died && beam.Owner != tank.ID)
                            {
                                tank.Health = 0;
                                tank.Died = true;
                            }
                        }
                    }
                }

                // Sends new world data to each client
                foreach (SocketState client in theWorld.GetClients())
                {
                    // Send the current state of all tanks
                    foreach (Tank tank in theWorld.GetTanks())
                    {
                        Networking.Send(client.TheSocket, JsonConvert.SerializeObject(tank) + "\n");
                        tank.HasMoved = false;
                    }

                    // Send the current state of all projectiles
                    foreach (Projectile proj in theWorld.GetProjectiles())
                    {
                        Networking.Send(client.TheSocket, JsonConvert.SerializeObject(proj) + "\n");
                    }

                    // Send the current state of all powerups
                    foreach (Powerup pwr in theWorld.GetPowerups())
                    {
                        Networking.Send(client.TheSocket, JsonConvert.SerializeObject(pwr) + "\n");
                    }

                    foreach (Beam beam in theWorld.GetShotBeams())
                    {
                        Networking.Send(client.TheSocket, JsonConvert.SerializeObject(beam) + "\n");
                    }
                }

                // Check if any projectiles have died and remove if so
                List<Projectile> projectileList = theWorld.GetProjectiles().ToList();
                for (int i = 0; i < projectileList.Count; i++)
                {
                    if (projectileList[i].Died)
                    {
                        theWorld.UpdateProjectile(projectileList[i].ID, projectileList[i]);
                    }
                }

                // Check if any tanks have died and remove if so
                List<Tank> tankList = theWorld.GetTanks().ToList();
                for (int i = 0; i < tankList.Count; i++)
                {
                    if (tankList[i].Died)
                    {
                        theWorld.RemoveTank(tankList[i].ID);
                        tankList[i].Died = false;
                        tankList[i].Location = RandomLocation();
                        theWorld.AddDeadTank(tankList[i].ID, tankList[i]);
                        Thread t1 = new Thread(new Timer(tankList[i], theWorld, true).Cooldown);
                        t1.Start();
                    }
                }

                // Check if any powerups have died and remove if so
                List<Powerup> powerupList = theWorld.GetPowerups().ToList();
                for (int i = 0; i < powerupList.Count; i++)
                {
                    if (powerupList[i].Died)
                    {
                        theWorld.RemovePowerup(powerupList[i].ID);
                    }
                }

                // Remove all beams
                theWorld.RemoveAllShotBeams();
            }
        }

        /// <summary>
        /// Worker method for new thread
        /// Stalls the server to maintain a frame count
        /// </summary>
        private static void PowerupStaller()
        {
            Stopwatch powerupWatch = new Stopwatch();
            Random r = new Random();
            powerupWatch.Start();

            while (true)
            {
                double delay = r.NextDouble() * ((theWorld.MaxPowerupDelay) - 1) + 1;

                while (powerupWatch.ElapsedMilliseconds < delay * theWorld.MSPerFrame)
                { }
                powerupWatch.Restart();
                SpawnPowerup();
            }
        }


        /// <summary>
        /// Worker method for new thread
        /// Stalls the server to maintain a frame count
        /// </summary>
        private static void FrameStaller()
        {
            Stopwatch frameWatch = new Stopwatch();
            frameWatch.Start();

            while (true)
            {
                while (frameWatch.ElapsedMilliseconds < theWorld.MSPerFrame)
                { }
                frameWatch.Restart();
                Update();
            }
        }
    }

    /// <summary>
    /// A Timer class to aid with respawns and shot cooldowns.
    /// </summary>
    class Timer
    {
        private Tank tank;
        private World theWorld;
        private bool isRespawn;

        public Timer(Tank t, World w, bool isRespawn)
        {
            tank = t;
            theWorld = w;
            this.isRespawn = isRespawn;
        }

        /// <summary>
        /// Begins either a respown cooldown
        /// or projectile cooldown.
        /// </summary>
        public void Cooldown()
        {
            if (isRespawn)
                RespawnCooldown();
            else
                ProjectileCooldown();
        }

        /// <summary>
        /// Creates a delay between a tank's death
        /// and respawn.
        /// </summary>
        private void RespawnCooldown()
        {
            Stopwatch respawnWatch = new Stopwatch();
            respawnWatch.Start();

            // Stall for cooldown time
            while (respawnWatch.ElapsedMilliseconds < theWorld.RespawnRate * theWorld.MSPerFrame) { }

            // Remove from dead tanks add back to alive tanks with restored HP
            lock (theWorld)
            {
                theWorld.RemoveDeadTank(tank.ID);
                tank.Health = theWorld.StartingHP;
                theWorld.AddTank(tank.ID, tank);
            }
        }

        /// <summary>
        /// Creates a delay for tank's firing
        /// state after the tank has fired.
        /// </summary>
        private void ProjectileCooldown()
        {
            Stopwatch projWatch = new Stopwatch();
            projWatch.Start();

            // Stall for cooldown time
            if (theWorld.GamemodeIsExtra && tank.Powerups > 0)
            {
                // Fast fire mode
                while (projWatch.ElapsedMilliseconds < theWorld.FramesPerShot * theWorld.MSPerFrame / 10) { }
            }
            else
            {
                while (projWatch.ElapsedMilliseconds < theWorld.FramesPerShot * theWorld.MSPerFrame)
                {
                    // run an if check to see if a powerup was picked up, cut off the cooldown and start fast fire mode
                    if (theWorld.GamemodeIsExtra && tank.Powerups > 0) break;
                }
            }
            // Allow fire again
            lock (theWorld)
            {
                tank.CanFire = true;
            }
        }
    }
}