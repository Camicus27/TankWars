using Newtonsoft.Json;
using TankWars;

namespace Models
{
    // {"wall":1,"p1":{"x":-575.0,"y":-575.0},"p2":{"x":-575.0,"y":575.0}}

    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        // "0"
        [JsonProperty(PropertyName = "wall")]
        private int wall;
        // {"x":-575.0,"y":-575.0}
        [JsonProperty(PropertyName = "p1")]
        private Vector2D p1;
        // {"x":-575.0,"y":575.0}
        [JsonProperty(PropertyName = "p2")]
        private Vector2D p2;

        public Wall(int wallID, Vector2D point1, Vector2D point2)
        {
            // Unique wall ID
            wall = wallID;
            // Endpoint 1 location
            p1 = point1;
            // Endpoint 2 location
            p2 = point2;
        }

        /// <summary>
        /// The unique identifier of this particular wall.
        /// </summary>
        public int ID { get { return wall; } set { wall = value; } }

        /// <summary>
        /// The location of the wall's first point.
        /// </summary>
        /// <returns>A Vector2D object representing the wall's first point.</returns>
        public Vector2D Point1 { get { return p1; } set { p1 = value; } }

        /// <summary>
        /// The location of the wall's second point.
        /// </summary>
        /// <returns>A Vector2D object representing the wall's second point.</returns>
        public Vector2D Point2 { get { return p2; } set { p2 = value; } }

        // Return whether the wall is vertical or horizonal
        public bool isVertical()
        {
            return p1.GetX() == p2.GetX();
        }

        /// <summary>
        /// Calculates the vertical boundaries of this wall.
        /// </summary>
        /// <returns>A Vector2D - GetX = Upper, GetY = Lower</returns>
        public Vector2D GetWallVerticalBounds(int size)
        {
            double upperBound = 0;
            double lowerBound = 0;

            // Vertical Wall
            double y_coord1 = Point1.GetY(), y_coord2 = Point2.GetY();

            if (y_coord1 < y_coord2)
            {
                upperBound = y_coord1 - size;
                lowerBound = y_coord2 + size;
            }

            else
            {
                upperBound = y_coord2 - size;
                lowerBound = y_coord1 + size;
            }

            return new Vector2D(upperBound, lowerBound);
        }

        /// <summary>
        /// Calculates the horizontal boundaries of this wall.
        /// </summary>
        /// <returns>A Vector2D - GetX = Upper, GetY = Lower</returns>
        public Vector2D GetWallHorizontalBounds(int size)
        {
            double upperBound = 0;
            double lowerBound = 0;

            // Horizontal Wall
            double x_coord1 = Point1.GetX(), x_coord2 = Point2.GetX();

            if (x_coord1 > x_coord2)
            {
                upperBound = x_coord1 + size;
                lowerBound = x_coord2 - size;
            }

            else
            {
                upperBound = x_coord2 + size;
                lowerBound = x_coord1 - size;
            }

            return new Vector2D(upperBound, lowerBound);
        }
    }
}
