using System;
using System.Text.RegularExpressions;
using System.Collections;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NetworkUtil;

namespace TankWars
{
    public class GameController
    {
        // The server update delegate and handler
        public delegate void ServerUpdateHandler();
        public event ServerUpdateHandler UpdateArrived;

        // Controller Handlers
        public delegate void ConnectedHandler();
        public event ConnectedHandler Connected;

        public delegate void ErrorHandler(string err);
        public event ErrorHandler Error;

        // The world container. Containing powerups, projectiles, and tanks.
        private World theWorld;

        // 900x900 pixels
        public int worldSize;
        public int playerID;

        // Record whether the moving keys are pressed or the mouse is clicked
        private bool movingPressed = false;
        private bool mousePressed = false;
        // Keep track of some internal key/mouse button states
        private string mostRecentlyPressed;
        private string firingState;
        // Buffer to handle multiple movement inputs at once
        private ArrayList movements;

        // State representing the connection with the server
        SocketState theServer = null;

        public GameController()
        {
            theWorld = new World();
            movements = new ArrayList(4);
            mostRecentlyPressed = "none";
            firingState = "none";
        }

        /// <summary>
        /// Return the world
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {
            return theWorld;
        }

        /// <summary>
        /// Begins the process of connecting to the server
        /// </summary>
        /// <param name="addr"></param>
        public void Connect(string addr)
        {
            Networking.ConnectToServer(OnConnect, addr, 11000);
        }


        /// <summary>
        /// Method to be invoked by the networking library when a connection is made
        /// </summary>
        /// <param name="state"></param>
        private void OnConnect(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Inform the view
                Error("Error connecting to server");
                return;
            }
            theServer = state;

            // Start an event loop to receive messages from the server
            state.OnNetworkAction = ReceiveMessage;
            Networking.GetData(state);

            // Inform the view
            Connected();
        }

        /// <summary>
        /// Method to be invoked by the networking library when 
        /// data is available
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveMessage(SocketState state)
        {
            if (state.ErrorOccurred)
            {
                // Inform the view
                Error("Lost connection to server");
                return;
            }

            // Inform the view
            UpdateArrived();
            // Proccess those server messages
            ProcessMessages(state);

            // Continue the event loop
            // state.OnNetworkAction has not been changed, so this same method (ReceiveMessage) 
            // will be invoked when more data arrives
            Networking.GetData(state);
        }

        private void ProcessMessages(SocketState state)
        {
            // Get the data from the socket and parse it
            string totalData = state.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            lock (theWorld)
            {
                // Looping over the data for JSON information
                foreach (string p in parts)
                {
                    // Ignore empty strings added by the regex splitter
                    if (p.Length == 0)
                        continue;
                    // Ensuring our regex splitter
                    if (p[p.Length - 1] != '\n')
                        break;

                    // Remove the newline character and find the JSON's type
                    string pp = p.Substring(0, p.Length - 1);
                    string field = JsonField(pp);

                    // Based on the JSON's type, create an object of that type and update the world
                    if (field != "not_json")
                    {
                        switch (field)
                        {
                            case "tank":
                                Tank rebuilt_tank = JsonConvert.DeserializeObject<Tank>(pp);
                                theWorld.UpdateTank(rebuilt_tank.ID, rebuilt_tank);
                                break;
                            case "power":
                                Powerup rebuilt_pwr = JsonConvert.DeserializeObject<Powerup>(pp);
                                theWorld.UpdatePowerup(rebuilt_pwr.ID, rebuilt_pwr);
                                break;
                            case "proj":
                                Projectile rebuilt_proj = JsonConvert.DeserializeObject<Projectile>(pp);
                                theWorld.UpdateProjectile(rebuilt_proj.ID, rebuilt_proj);
                                break;
                            case "wall":
                                Wall rebuilt_wall = JsonConvert.DeserializeObject<Wall>(pp);
                                theWorld.AddWall(rebuilt_wall.ID, rebuilt_wall);
                                break;
                            case "beam":
                                Beam rebuilt_beam = JsonConvert.DeserializeObject<Beam>(pp);
                                theWorld.AddBeam(rebuilt_beam.ID, rebuilt_beam, theWorld.MousePosition);
                                break;
                            case "startup data":
                                break;
                        }

                    }// end if

                    state.RemoveData(0, p.Length);
                }
            }

            // For whatever user inputs happened during the last frame,
            // process them.
            ProcessInputs();
        }

        /// <summary>
        /// Parses a given JSON to determine the type
        /// of the JSON object.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string JsonField(string p)
        {
            // Check if input is not a JSON
            if (!p.Contains("{") && !p.Contains("}"))
            {
                // Checking for Player ID and world size at the
                // beginning of the server-reported JSON
                if (p.Length < 3)
                {
                    Int32.TryParse(p, out int ID);
                    playerID = ID;
                    theWorld.ID = ID;
                }
                else
                {
                    Int32.TryParse(p, out int size);
                    worldSize = size;
                    theWorld.Size = size;
                }
                return "startup data";
            }

            JObject obj = JObject.Parse(p);

            JToken token = obj["tank"];
            if (token != null)
                return "tank";

            JToken token_1 = obj["power"];
            if (token_1 != null)
                return "power";

            JToken token_2 = obj["proj"];
            if (token_2 != null)
                return "proj";

            JToken token_3 = obj["wall"];
            if (token_3 != null)
                return "wall";

            JToken token_4 = obj["beam"];
            if (token_4 != null)
                return "beam";

            return "not_json";
        }

        /// <summary>
        /// Checks which inputs are currently held down, updates the
        /// client view, and sends the request to the server
        /// </summary>
        private void ProcessInputs()
        {
            // If an action is currently in motion
            if (movingPressed || mousePressed)
            {
                // Send a request to the server and update the client view
                Controls move = new Controls(mostRecentlyPressed, firingState, theWorld.ClientTank.TurretDirection);
                SendToServer(JsonConvert.SerializeObject(move));
                if (firingState == "alt")
                    firingState = "none";
            }
        }

        /// <summary>
        /// Handling a movement request
        /// </summary>
        public void HandleMoveRequest(string direction)
        {
            // Set moving to true and update most recently pressed key
            movingPressed = true;
            mostRecentlyPressed = direction;
            // Add given direction to list and increment pointer
            if (!movements.Contains(direction))
                movements.Add(direction);
        }

        /// <summary>
        /// Canceling a movement request
        /// </summary>
        public void CancelMoveRequest(string direction)
        {
            // Remove from list and decrement pointer
            int index = movements.IndexOf(direction);
            movements.Remove(direction);

            // Check if list is empty and update
            if (movements.Count == 0)
            {
                movements.Clear();
                movingPressed = false;
                mostRecentlyPressed = "none";
                return;
            }

            // Update the most recently pressed key
            mostRecentlyPressed = (string)movements[movements.Count - 1];
        }

        /// <summary>
        /// Handling a mouse movement
        /// </summary>
        /// <param name="direction"></param>
        public void HandleMouseMovement(Vector2D direction)
        {
            // Update the client side direction
            theWorld.ClientTank.TurretDirection = direction;

            // Update the server side direction
            Controls move = new Controls(mostRecentlyPressed, firingState, direction);
            SendToServer(JsonConvert.SerializeObject(move));
        }

        /// <summary>
        /// Handling a mouse request
        /// </summary>
        public void HandleMouseRequest(string fireMode)
        {
            mousePressed = true;
            switch (fireMode)
            {
                case "main":
                    firingState = "main";
                    break;
                case "alt":
                    firingState = "alt";
                    break;
            }
        }

        /// <summary>
        /// Canceling mouse request
        /// </summary>
        public void CancelMouseRequest()
        {
            mousePressed = false;
            firingState = "none";
        }

        /// <summary>
        /// Closes the connection with the server
        /// </summary>
        public void Close()
        {
            theServer?.TheSocket.Close();
        }

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        public void SendToServer(string message)
        {
            if (theServer != null)
                Networking.Send(theServer.TheSocket, message + "\n");
        }
    }
}