using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DeathAnimation
    {
        private Tank dyingTank;
        private int frameCounter;
        private bool hasPlayedAudio;
        private bool hasDied;

        public DeathAnimation(Tank tankToDie)
        {
            dyingTank = tankToDie;
            hasPlayedAudio = false;
            frameCounter = 15;
        }

        /// <summary>
        /// The tank on which the animation is to be performed.
        /// </summary>
        public Tank Tank { get { return dyingTank; } set { dyingTank = value; } }

        /// <summary>
        /// The amount of frames the tank's death is animated for.
        /// </summary>
        public int FrameCount { get { return frameCounter; } set { frameCounter = value; } }

        /// <summary>
        /// Tracks the current state of the object, living or dead.
        /// </summary>
        public bool IsDead { get { return hasDied; } set { hasDied = value; } }

        /// <summary>
        /// Tracks the current state of the audio.
        /// </summary>
        public bool HasPlayedAudio { get { return hasPlayedAudio; } set { hasPlayedAudio = value; } }
    }
}
