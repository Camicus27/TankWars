using Newtonsoft.Json;
using TankWars;

namespace Models
{
    // {"moving":"up","fire":"main","tdir":{"x":1,"y":0}}

    [JsonObject(MemberSerialization.OptIn)]
    public class Controls
    {
        // "left"
        [JsonProperty(PropertyName = "moving")]
        private string moving;
        // "main"
        [JsonProperty(PropertyName = "fire")]
        private string fire;
        // {"x":1,"y":0}
        [JsonProperty(PropertyName = "tdir")]
        private Vector2D tdir;

        /// <summary></summary>
        /// <param name="movingDirection">must be: "none", "up", "left", "down", "right"</param>
        /// <param name="fireState">must be: "none", "main", (for a normal projectile) and "alt" (for a beam attack)</param>
        /// <param name="aimDirection"></param>
        public Controls(string movingDirection, string fireState, Vector2D aimDirection)
        {
            // Whether we are moving or not
            moving = movingDirection;
            // Whether we are firing or not
            fire = fireState;
            // Direction of turret aim
            tdir = aimDirection;
        }

        // Controls Properties

        /// <summary>
        /// The direction the tank is moving in.
        /// </summary>
        public string MovingDirection { get { return moving; } set { moving = value; } }

        /// <summary>
        /// Whether the tank is firing the main cannon, using
        /// alternate fire, or not firing at all.
        /// </summary>
        public string FireState { get { return fire; } set { fire = value; } }

        /// <summary>
        /// Returns the direction in which the turret is aiming.
        /// </summary>
        /// <returns>A Vector2D object containing aim coordinates</returns>
        public Vector2D AimDirection { get { return tdir; } set { tdir = value; } }
    }
}
