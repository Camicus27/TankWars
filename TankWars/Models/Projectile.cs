using Newtonsoft.Json;
using TankWars;

namespace Models
{
    // {"proj":5,"loc":{"x":-279.01316584757348,"y":-226.5850218601696},"dir":{"x":-0.99983624342305211,"y":0.018096583591367461},
    // "died":false,"owner":0}

    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        // "0"
        [JsonProperty(PropertyName = "proj")]
        private int proj;
        // {"x":-144.24487352371216,"y":264.43469524383545}
        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;
        // {"x":0.45973361932084461,"y":-0.88805686713529608}
        [JsonProperty(PropertyName = "dir")]
        private Vector2D dir;
        // false
        [JsonProperty(PropertyName = "died")]
        private bool died;
        // "3"
        [JsonProperty(PropertyName = "owner")]
        private int owner;

        private Vector2D velocity;

        public Projectile(int projectileID, Vector2D location, Vector2D direction, bool hasDied, int ownerID)
        {
            // Unique projectile ID
            proj = projectileID;
            // Vector representing location
            loc = location;
            // Vector representing orientation
            dir = direction;
            // If projectile has hit something or left world bounds
            died = hasDied;
            // Tank ID of player who shot the projectile
            owner = ownerID;

            velocity = direction;
        }

        /// <summary>
        /// The unique identifier of this particular projectile.
        /// </summary>
        public int ID { get { return proj; } set { proj = value; } }

        /// <summary>
        /// The location of this particular projectile.
        /// </summary>
        /// <returns>A Vector2D object representing the projectile's current location.</returns>
        public Vector2D Location { get { return loc; } set { loc = value; } }

        /// <summary>
        /// The direction the projectile is facing/headed.
        /// </summary>
        /// <returns>A Vector2D object representing the projectile's current orientation.</returns>
        public Vector2D Direction { get { return dir; } set { dir = value; } }

        /// <summary>
        /// Tracks the state of the object, living or dead.
        /// </summary>
        public bool Died { get { return died; } set { died = value; } }

        /// <summary>
        /// The unique ID of the projectile's owner,
        /// the owner being another tank.
        /// </summary>
        public int Owner { get { return owner; } set { owner = value; } }

        /// <summary>
        /// The velocity of the projectile, being the 
        /// direction and speed.
        /// </summary>
        /// <returns>A Vector2D object representing the projectile's current velocity.</returns>
        public Vector2D Velocity { get { return velocity; } set { velocity = value; } }
    }
}
