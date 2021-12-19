using Newtonsoft.Json;
using TankWars;

namespace Models
{
    // {"power":1,"loc":{"x":486.0684871673584,"y":54.912471771240234},"died":false}

    [JsonObject(MemberSerialization.OptIn)]
    public class Powerup
    {
        // "1"
        [JsonProperty(PropertyName = "power")]
        private int power;
        // "x":486.0684871673584,"y":54.912471771240234
        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;
        // false
        [JsonProperty(PropertyName = "died")]
        private bool died;

        public Powerup(int powerID, Vector2D location, bool hasDied)
        {
            // ID of the powerup
            power = powerID;
            // Location of the powerup spawn
            loc = location;
            // Died is true if a player has picked up this powerup during this frame
            died = hasDied;
        }

        /// <summary>
        /// The unique identifier of this particular powerup.
        /// </summary>
        public int ID { get { return power; } set { power = value; } }

        /// <summary>
        /// The location of this particular powerup
        /// </summary>
        /// <returns>A Vector2D object representing the powerup's current location.</returns>
        public Vector2D Location { get { return loc; } set { loc = value; } }

        /// <summary>
        /// Tracks the current state of the object, living or dead.
        /// </summary>
        public bool Died { get { return died; } set { died = value; } }
    }
}
