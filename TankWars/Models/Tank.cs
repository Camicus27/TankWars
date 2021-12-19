using Newtonsoft.Json;
using TankWars;

namespace Models
{
    //{"tank":0,"loc":{"x":220.995264,"y":-235.63331367},"bdir":{"x":1.0,"y":0.0},
    //"tdir":{"x":-0.795849908004867,"y":0.60549395036502607},
    //"name":"Danny","hp":3,"score":0,"died":false,"dc":false,"join":false}

    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        // "0"
        [JsonProperty(PropertyName = "tank")]
        private int tank;
        // "player"
        [JsonProperty(PropertyName = "name")]
        private string name;
        // "x":486.0684871673584,"y":54.912471771240234
        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;
        // {"x":1.0,"y":0.0}
        [JsonProperty(PropertyName = "bdir")]
        private Vector2D bdir;
        // {"x":-0.795849908004867,"y":0.60549395036502607}
        [JsonProperty(PropertyName = "tdir")]
        private Vector2D tdir;
        // "200"
        [JsonProperty(PropertyName = "score")]
        private int score;
        // 2 : (0-3)
        [JsonProperty(PropertyName = "hp")]
        private int hp;
        // false
        [JsonProperty(PropertyName = "died")]
        private bool died;
        // false
        [JsonProperty(PropertyName = "dc")]
        private bool dc;
        // false
        [JsonProperty(PropertyName = "join")]
        private bool join;
        // Red
        [JsonProperty(PropertyName = "color")]
        private string color;

        private bool hasMoved;
        private bool canFire;
        private int powerupCount;
        private int powerupDuration;

        /// <summary></summary>
        /// <param name="scoreID">Integer by 100s</param>
        /// <param name="healthPoints">Integers: 0, 1, 2, 3</param>
        public Tank(int tankID, string nameID, Vector2D location, Vector2D bodyDirection, Vector2D turretDirection,
            int scoreID, int healthPoints, bool hasDied, bool hasDisconnected, bool hasJoined, string tankColor)
        {
            // Unique tank ID
            tank = tankID;
            // Name of the player
            name = nameID;
            // Vector representing location
            loc = location;
            // Vector representing orientation of body
            bdir = bodyDirection;
            // Vector representing orientation of turret
            tdir = turretDirection;
            // Score of the player
            score = scoreID;
            // Health of the player
            hp = healthPoints;
            // If tank has died
            died = hasDied;
            // If tank has disconnected
            dc = hasDisconnected;
            // If tank has joined
            join = hasJoined;
            // Color of the tank
            color = tankColor;

            hasMoved = false;
            canFire = true;
            powerupCount = 0;
            powerupDuration = 0;
        }


        /// <summary>
        /// The unique identifier of this particular tank.
        /// </summary>
        public int ID { get { return tank; } set { tank = value; } }

        /// <summary>
        /// The name attributed to this tank by the client.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// The color attributed to this tank by the client.
        /// </summary>
        public string Color { get { return color; } set { color = value; } }

        /// <summary>
        /// The location of this particular Tank.
        /// </summary>
        /// <returns>A Vector2D object representing the tank's current location.</returns>
        public Vector2D Location { get { return loc; } set { loc = value; } }

        /// <summary>
        /// The orientation of this Tank.
        /// </summary>
        /// <returns>A Vector2D object representing the powerup's current location.
        /// left, right, up, and down, being represented as a Vector2D.
        /// </returns>
        public Vector2D BodyDirection { get { return bdir; } set { bdir = value; } }

        /// <summary>
        /// The orientation of this Tank's turret.
        /// </summary>
        /// <returns>A Vector2D object representing the turret's orientation.</returns>
        public Vector2D TurretDirection { get { return tdir; } set { tdir = value; } }

        /// <summary>
        /// The score assigned to this tank.
        /// </summary>
        public int Score { get { return score; } set { score = value; } }

        /// <summary>
        /// The health of this tank, ranging from 0-3.
        /// </summary>
        public int Health { get { return hp; } set { hp = value; } }

        /// <summary>
        /// Tracks the state of the object, living or dead.
        /// </summary>
        public bool Died { get { return died; } set { died = value; } }

        /// <summary>
        /// A bool that returns if the tank has disconnected from
        /// the game.
        /// </summary>
        public bool Disconnected { get { return dc; } set { dc = value; } }

        /// <summary>
        /// A bool that returns if the tank has connected to
        /// the game.
        /// </summary>
        public bool Joined { get { return join; } set { join = value; } }

        /// <summary>
        /// Whether or not the tank has moved.
        /// </summary>
        public bool HasMoved { get { return hasMoved; } set { hasMoved = value; } }

        /// <summary>
        /// Whether the tank has the ability to fire. 
        /// Meaning the cooldown period has elapsed.
        /// </summary>
        public bool CanFire { get { return canFire; } set { canFire = value; } }

        /// <summary>
        /// The amount of powerups currently held by the tank.
        /// </summary>
        public int Powerups { get { return powerupCount; } set { powerupCount = value; } }

        /// <summary>
        /// The time duration of the "extra" gamemode powerup
        /// </summary>
        public int PowerupDuration { get { return powerupDuration; } set { powerupDuration = value; } }
    }
}

