using Newtonsoft.Json;
using TankWars;

namespace Models
{
    //{"beam":0,"org":{"x":-144.24487352371216,"y":264.43469524383545},"dir":{"x":0.45973361932084461,"y":-0.88805686713529608},"owner":0}

    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        // "0"
        [JsonProperty(PropertyName = "beam")]
        private int beam;
        // {"x":-144.24487352371216,"y":264.43469524383545}
        [JsonProperty(PropertyName = "org")]
        private Vector2D org;
        // {"x":0.45973361932084461,"y":-0.88805686713529608}
        [JsonProperty(PropertyName = "dir")]
        private Vector2D dir;
        // "3"
        [JsonProperty(PropertyName = "owner")]
        private int owner;

        public Beam(int beamID, Vector2D origin, Vector2D direction, int tankID)
        {
            // Unique beam ID
            beam = beamID;
            // Vector representing the origin
            org = origin;
            // Vector representing orientation
            dir = direction;
            // Tank ID of the player who shot the beam
            owner = tankID;
        }

        /// <summary>
        /// Returns the origin of the beam being shot
        /// </summary>
        /// <returns>A Vector2D object containing the origin coordinates</returns>
        public Vector2D Origin { get { return org; } set { org = value; } }

        /// <summary>
        /// Direction property of the beam
        /// </summary>
        /// <returns>A Vector2D object containing the directional coordinates</returns>
        public Vector2D Direction { get { return dir; } set { dir = value; } }

        /// <summary>
        /// Returns the ID of the beam object
        /// </summary>
        public int ID { get { return beam; } set { beam = value; } }

        /// <summary>
        /// The owner property of the beam.
        /// Returns the owner (tankID) of the beam
        /// and sets the tankID as well.
        /// </summary>
        public int Owner { get { return owner; } set { owner = value; } }
    }
}
