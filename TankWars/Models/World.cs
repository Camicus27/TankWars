using System.Collections.Generic;
using TankWars;
using NetworkUtil;

namespace Models
{
    public class World
    {
        // The assests availiable to both the 
        // client and server. 
        private Dictionary<int, Tank> tanks;
        private Dictionary<int, Powerup> powerups;
        private Dictionary<int, Projectile> proj;
        private Dictionary<int, Wall> walls;
        private List<DeathAnimation> tankDeaths;
        private List<BeamAnimation> beams;
        private Tank clientTank;
        private Vector2D mousePosition;
        private int clientPlayerID;

        // Assets availiable only to the server.
        private Dictionary<long, SocketState> clients;
        private Dictionary<int, Tank> deadTanks;
        private List<Beam> shotBeams;
        private int universeSize;
        private int msPerFrame;
        private int framesPerShot;
        private int respawnRate;
        private int startingHP;
        private int projectileSpeed;
        private double engineStrength;
        private int tankSize;
        private int wallSize;
        private int maxPowerups;
        private int maxPowerupDelay;
        public string gamemode;

        public World()
        {
            tanks = new Dictionary<int, Tank>();
            tankDeaths = new List<DeathAnimation>();
            powerups = new Dictionary<int, Powerup>();
            proj = new Dictionary<int, Projectile>();
            walls = new Dictionary<int, Wall>();
            beams = new List<BeamAnimation>();
            clientPlayerID = 0;
            clientTank = new Tank(-1, "default", new Vector2D(0.0, 0.0), null, null, 0, 3, false, false, true, "default");

            // Server only
            clients = new Dictionary<long, SocketState>();
            deadTanks = new Dictionary<int, Tank>();
            shotBeams = new List<Beam>();
            universeSize = 0;
            msPerFrame = 0;
            framesPerShot = 0;
            respawnRate = 0;
            startingHP = 0;
            projectileSpeed = 0;
            engineStrength = 0.0;
            tankSize = 0;
            wallSize = 0;
            maxPowerups = 0;
            maxPowerupDelay = 0;
            gamemode = "";
        }

        /// <summary>
        /// The size of the World.
        /// </summary>
        public int Size { get { return universeSize; } set { universeSize = value; } }

        /// <summary>
        /// The unique identifier belonging to the client/player
        /// </summary>
        public int ID { get { return clientPlayerID; } set { clientPlayerID = value; } }

        /// <summary>
        /// The tank belonging to the client.
        /// </summary>
        public Tank ClientTank { get { return clientTank; } set { clientTank = value; } }

        /// <summary>
        /// The location of the client's mouse.
        /// </summary>
        /// <returns>A Vector2D object representing the client's mouse position
        /// within the world.
        /// </returns>
        public Vector2D MousePosition { get { return mousePosition; } set { mousePosition = value; } }


        // Server settings properties

        /// <summary>
        /// The amount of time each frame should take.
        /// </summary>
        public int MSPerFrame { get { return msPerFrame; } set { msPerFrame = value; } }

        /// <summary>
        /// The amount of frames that should elapse between shots.
        /// </summary>
        public int FramesPerShot { get { return framesPerShot; } set { framesPerShot = value; } }

        /// <summary>
        /// The amount of time elapsed after a tank's death before 
        /// it may be respawned.
        /// </summary>
        public int RespawnRate { get { return respawnRate; } set { respawnRate = value; } }

        /// <summary>
        /// The amount of health each tank starts with. 
        /// (starting with this health on respawn as well).
        /// </summary>
        public int StartingHP { get { return startingHP; } set { startingHP = value; } }

        /// <summary>
        /// The speed at which projectiles will travel.
        /// </summary>
        public int ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

        /// <summary>
        /// How fast the tanks move.
        /// </summary>
        public double EngineStrength { get { return engineStrength; } set { engineStrength = value; } }

        /// <summary>
        /// The size of the Tanks.
        /// </summary>
        public int TankSize { get { return tankSize; } set { tankSize = value; } }

        /// <summary>
        /// The size of the walls.
        /// </summary>
        public int WallSize { get { return wallSize; } set { wallSize = value; } }

        /// <summary>
        /// The maximum amount of powerups allowed to 
        /// spawn in the world at any given time.
        /// </summary>
        public int MaxPowerups { get { return maxPowerups; } set { maxPowerups = value; } }

        /// <summary>
        /// The maximum delay time bewtween powerup spawns.
        /// </summary>
        public int MaxPowerupDelay { get { return maxPowerupDelay; } set { maxPowerupDelay = value; } }

        /// <summary>
        /// Verifies if the gamemode is extra (true) or basic (false).
        /// </summary>
        public bool GamemodeIsExtra { get { if (gamemode == "extra") return true; else return false; } }

        // Getters for dictionary values

        /// <summary>
        /// The tanks within the world.
        /// </summary>
        /// <returns>A dictionary containing the world's tanks and
        /// their IDs.
        /// </returns>
        public IEnumerable<Tank> GetTanks()
        {
            return tanks.Values;
        }

        /// <summary>
        /// The projectiles within the world.
        /// </summary>
        /// <returns>A dictionary containing the world's projectiles and
        /// their IDs.
        /// </returns>
        public IEnumerable<Projectile> GetProjectiles()
        {
            return proj.Values;
        }

        /// <summary>
        /// The powerups within the world.
        /// </summary>
        /// <returns>A dictionary containing the world's powerups and
        /// their IDs.
        /// </returns>
        public IEnumerable<Powerup> GetPowerups()
        {
            return powerups.Values;
        }

        /// <summary>
        /// The walls within the world.
        /// </summary>
        /// <returns>A dictionary containing the world's walls and
        /// their IDs.
        /// </returns>
        public IEnumerable<Wall> GetWalls()
        {
            return walls.Values;
        }

        /// <summary>
        /// The BeamAnimations within the world.
        /// </summary>
        /// <returns>A list containing the world's BeamAnimations.
        /// </returns>
        public IEnumerable<BeamAnimation> GetBeams()
        {
            return beams;
        }

        /// <summary>
        /// The DeathAnimations within the world.
        /// </summary>
        /// <returns>A list containing the world's DeathAnimations.
        /// </returns>
        public IEnumerable<DeathAnimation> GetTankDeaths()
        {
            return tankDeaths;
        }

        /// <summary>
        /// The clients currently connected.
        /// </summary>
        /// <returns>A dictionary containing the wclients 
        /// currently connected and their unique IDs.
        /// </returns>
        public IEnumerable<SocketState> GetClients()
        {
            return clients.Values;
        }

        /// <summary>
        /// The beam objects that have been fired and 
        /// thus retired.
        /// </summary>
        /// <returns>A list of retired Beams.</returns>
        public IEnumerable<Beam> GetShotBeams()
        {
            return shotBeams;
        }


        // Adders and removers for each object and thier dictionaries.
        public void AddTank(int tankID, Tank addedTank)
        {
            tanks.Add(tankID, addedTank);
        }
        public void RemoveTank(int tankID)
        {
            tanks.Remove(tankID);
        }
        public void AddPowerup(int pwrID, Powerup addedPwr)
        {
            powerups.Add(pwrID, addedPwr);
        }
        public void RemovePowerup(int pwrID)
        {
            powerups.Remove(pwrID);
        }
        public void AddProjectile(int projID, Projectile addedProj)
        {
            proj.Add(projID, addedProj);
        }
        public void AddWall(int wallID, Wall theWall)
        {
            walls.Add(wallID, theWall);
        }
        public void AddBeam(int beamID, Beam theBeam, Vector2D Endpoint)
        {
            beams.Add(new BeamAnimation(theBeam, 100, Endpoint));
        }
        public void RemoveBeam(BeamAnimation beam)
        {
            beams.Remove(beam);
        }
        public void RemoveTankDeath(DeathAnimation tankDeath)
        {
            tankDeaths.Remove(tankDeath);
        }
        public void AddClient(SocketState state)
        {
            clients.Add(state.ID, state);
        }
        public void RemoveClient(SocketState state)
        {
            clients.Remove(state.ID);
        }
        public void AddDeadTank(int tankID, Tank addedTank)
        {
            deadTanks.Add(tankID, addedTank);
        }
        public void RemoveDeadTank(int tankID)
        {
            deadTanks.Remove(tankID);
        }
        public void AddShotBeam(Beam beam)
        {
            shotBeams.Add(beam);
        }
        public void RemoveAllShotBeams()
        {
            shotBeams.Clear();
        }

        public Tank GetTank(int ID)
        {
            tanks.TryGetValue(ID, out Tank tank);
            return tank;
        }
        public Tank GetDeadTank(int ID)
        {
            deadTanks.TryGetValue(ID, out Tank tank);
            return tank;
        }


        // Updating the world

        /// <summary>
        /// Updates the world as to the condition of a 
        /// tank. If a tank has died, it must be removed
        /// and if it is alive, the world must be aware
        /// of that fact.
        /// </summary>
        /// <param name="tankID"></param>
        /// <param name="updateTank"></param>
        public void UpdateTank(int tankID, Tank updateTank)
        {
            // Update client tank if given tank is client tank
            if (tankID == ID)
                clientTank = updateTank;

            // Check if given tank exists and update it, add if not
            if (tanks.ContainsKey(tankID))
                tanks[tankID] = updateTank;
            else
                tanks.Add(tankID, updateTank);

            // Check if tank has died
            if (updateTank.Died || updateTank.Disconnected)
            {
                // Add to list of death animations to perform
                tankDeaths.Add(new DeathAnimation(updateTank));
                // And remove from list of tanks
                tanks.Remove(tankID);
            }
        }

        /// <summary>
        /// Updates the world on the status of a given 
        /// powerup. If the powerup has been collected,
        /// it's death state must be updated and the
        /// appropriate dictionary notified.
        /// </summary>
        /// <param name="powerupID"></param>
        /// <param name="updatePowerup"></param>
        public void UpdatePowerup(int powerupID, Powerup updatePowerup)
        {
            powerups[powerupID] = updatePowerup;

            // Check if powerup has died, remove if true
            if (updatePowerup.Died)
                powerups.Remove(powerupID);
        }

        /// <summary>
        /// Updates the world on the status of a given 
        /// projectile. If the projectile has collided with,
        /// a wall or tank, its death state must be updated.
        /// Along with the appropriate dictionary being
        /// notified.
        /// </summary>
        /// <param name="projID"></param>
        /// <param name="updateProj"></param>
        public void UpdateProjectile(int projID, Projectile updateProj)
        {
            proj[projID] = updateProj;

            // Check if projectile has died, remove if true
            if (updateProj.Died)
                proj.Remove(projID);
        }
    }
}