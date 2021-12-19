using System;
using System.Linq;
using System.Collections.Generic;
using TankWars;
using Models;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace AI
{
    /// <summary>
    /// Creates a prompted number of AI Clients to join a Tank Wars Server
    /// </summary>
    public class AIClient
    {
        // Some constant variables for setting up the server connections
        private static int AICount;
        private static string address;
        // Keep track of how many AIs have connected
        private static int numberOfConnectedAIs = 0;
        // Random generator for random movements and tank targeting
        private static Random rand;
        // The world for finding tanks
        private static World theWorld;
        // Dictionaries to keep track of each AI client's controller and movements
        private static Dictionary<string, GameController> controllers;
        private static Dictionary<GameController, string> ctrlMovements;

        static void Main(string[] args)
        {
            // Initialize some variables
            bool valid = true;
            rand = new Random();
            AICount = 0;
            Console.Title = "AI CLIENT";

            // Prompt for the address
            Console.WriteLine("Please enter the server address:");
            // Hang waiting for address
            address = Console.ReadLine();

            while (valid)
            {
                // Prompt for the number of AIs
                Console.WriteLine("How many AIs would you like to connect?");
                // Hang waiting for number
                if (Int32.TryParse(Console.ReadLine(), out AICount))
                    valid = false;
                else
                    // Did not enter a number, ask again
                    Console.WriteLine("Please enter a number.");
            }

            // Create a dictionary of all AIs and their current movements
            controllers = new Dictionary<string, GameController>(AICount);
            ctrlMovements = new Dictionary<GameController, string>(AICount);
            for (int i = 0; i < AICount; i++)
            {
                controllers.Add("AI_" + i, new GameController());
                GameController clientController;
                controllers.TryGetValue("AI_" + i, out clientController);
                ctrlMovements.Add(clientController, "up");
            }

            // Start the connections
            AllConnections();

            // Keep AI client console open
            Console.Read();
        }

        /// <summary>
        /// Callback for when an error occurs
        /// </summary>
        /// <param name="error"></param>
        private static void ShowError(string error)
        {
            Console.WriteLine(error);
            Console.WriteLine("AI Disconnected");
            return;
        }

        /// <summary>
        /// Begins the process of connecting to the server for all AIs
        /// </summary>
        /// <param name="addr"></param>
        private static void AllConnections()
        {
            foreach (GameController controller in controllers.Values)
            {
                // Establish the controller to our callbacks
                controller.Error += ShowError;
                controller.Connected += HandleConnected;
                controller.UpdateArrived += UpdateHasArrived;

                // Get the world instance
                theWorld = controller.GetWorld();

                // Connect to the address
                controller.Connect(address);
            }
        }

        /// <summary>
        /// Handles when clients have connected successfully
        /// </summary>
        private static void HandleConnected()
        {
            // Increment the number of AI connected
            numberOfConnectedAIs++;
            // If all clients are connected
            if (numberOfConnectedAIs == AICount)
            {
                int AI_ID = -1;
                foreach (GameController controller in controllers.Values)
                {
                    AI_ID++;
                    Console.WriteLine("Connection with ID :" + AI_ID + " complete");
                    // Send the name and color to the server
                    controller.SendToServer("AI_" + AI_ID);
                }

                // Start the update staller to begin infinitely sending client info to the server
                Thread t = new Thread(UpdateStaller);
                t.Start();
            }
        }

        /// <summary>
        /// Staller to pace out sending client info to the server
        /// </summary>
        private static void UpdateStaller()
        {
            Stopwatch updateWatch = new Stopwatch();
            updateWatch.Start();

            while (updateWatch.ElapsedMilliseconds < 1000)
            {
                // Wait for 1 second to allow the sending of ID, world size, and walls
            }

            // Repeat until clients are disconnected
            while (true)
            {
                // Wait 15 milliseconds
                while (updateWatch.ElapsedMilliseconds < 15) { }
                updateWatch.Restart();
                // Update all movements
                ConstantMovements();
            }
        }

        /// <summary>
        /// Update method to keep all AI moving and shooting at the closest alive tank
        /// </summary>
        private static void ConstantMovements()
        {
            // Loop over each AI
            foreach (GameController controller in controllers.Values)
            {
                // Check if current tank is dead
                if (theWorld.GetTank(controller.playerID) == null)
                    continue;

                // Use RNG to determine new direction or keep moving same direction
                string direction;
                if (rand.Next(0, 1000) > 950)
                {
                    direction = GetRandomMove();
                    ctrlMovements.Remove(controller);
                    ctrlMovements.Add(controller, direction);
                }
                else
                {
                    ctrlMovements.TryGetValue(controller, out direction);
                }

                // Aim at the closest tank
                Vector2D aimDir = GetClosestTank(theWorld.GetTank(controller.playerID));

                // Update the server side client information
                Controls move = new Controls(direction, "main", aimDir);
                controller.SendToServer(JsonConvert.SerializeObject(move));
            }
        }

        /// <summary>
        /// Helper to determine a random direction using RNG
        /// </summary>
        private static string GetRandomMove()
        {
            string direction = null;

            // Randomly pick a direction
            switch (rand.Next(0, 4) + "")
            {
                case "0":
                    direction = "down";
                    break;
                case "1":
                    direction = "left";
                    break;
                case "2":
                    direction = "right";
                    break;
                case "3":
                    direction = "up";
                    break;
            }

            return direction;
        }

        /// <summary>
        /// Helper to determine what tank to shoot based on player location
        /// </summary>
        private static Vector2D GetClosestTank(Tank playerTank)
        {
            // Get tank count and create the tank to shoot
            int tankCount = theWorld.GetTanks().ToList().Count;
            Tank tankToShoot = null;

            // Create a list to search for the max distance
            List<double> list = new List<double>();
            // Create a dictionary to attach distances to Tanks
            Dictionary<double, Tank> dict = new Dictionary<double, Tank>();

            for (int i = 0; i < tankCount; i++)
            {
                // Verify tank is not the player, and tank is alive
                if (i == playerTank.ID || theWorld.GetTank(i) == null) continue;

                // Calculate distance from player to target
                double distance = Math.Sqrt(Math.Pow(playerTank.Location.GetX() - theWorld.GetTank(i).Location.GetX(), 2) +
                    Math.Pow(playerTank.Location.GetY() - theWorld.GetTank(i).Location.GetY(), 2));

                // Add to the data structures
                dict.Add(distance, theWorld.GetTank(i));
                list.Add(distance);
            }

            // If all other tanks are dead, return default aim direction
            if (list.Count < 1) return new Vector2D(0, -1);

            // Get the smallest and set that tank to the target
            double smallestDistance = list.Min();
            dict.TryGetValue(smallestDistance, out tankToShoot);

            // Get the direction from player to target and normalize
            Vector2D aimDirection = tankToShoot.Location - playerTank.Location;
            aimDirection.Normalize();
            // return the normalized direction
            return aimDirection;
        }


        private static void UpdateHasArrived()
        {
            // Placeholder to do nothing, since we aren't updating anything visually
        }
    }
}
