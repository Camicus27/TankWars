using System;
using System.Collections.Generic;
using System.Text;
using TankWars;

namespace Models
{
    public class BeamAnimation
    {
        private Beam beam;
        private int frameCounter;
        private Vector2D Endpoint;
        private bool hasDied;
        private bool hasPlayedAudio;

        public BeamAnimation(Beam beam, Vector2D Endpoint)
        {
            this.beam = beam;
            frameCounter = 50;
            this.Endpoint = Endpoint;
            hasPlayedAudio = false;
            hasDied = false;
        }

        /// <summary>
        /// Returns the origin of the beam being shot
        /// </summary>
        /// <returns>A Vector2D object containing the origin coordinates</returns>
        public Beam Beam { get { return beam; } set { beam = value; } }

        /// <summary>
        /// The amount of frames the beam is animated for.
        /// </summary>
        public int FrameCount { get { return frameCounter; } set { frameCounter = value; } }

        /// <summary>
        /// Tracks the current state of the object, living or dead.
        /// </summary>
        public bool IsDead { get { return hasDied; } set { hasDied = value; } }

        /// <summary>
        /// Returns the origin of the beam being shot
        /// </summary>
        /// <returns>A Vector2D object containing thh endpoint of the animation object</returns>
        public Vector2D EndPoint { get { return Endpoint; } set { Endpoint = value; } }

        /// <summary>
        /// Tracks the current state of the audio.
        /// </summary>
        public bool HasPlayedAudio { get { return hasPlayedAudio; } set { hasPlayedAudio = value; } }
    }
}
