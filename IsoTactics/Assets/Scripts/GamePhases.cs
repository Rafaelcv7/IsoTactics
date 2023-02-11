using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace IsoTactics
{
    public static class GamePhases
    {
        private static readonly Dictionary<string, bool> Phases;

        static GamePhases()
        {
            Phases = new Dictionary<string, bool>()
            {
                { "Positioning", true },
                { "Turn", false }
            };
        }

        public static void ChangeCurrentPhase(string toPhase)
        {
            Phases[CurrentPhase] = false;
            Phases[toPhase] = true;
            Debug.Log($"Current Phase: {toPhase}");
        }

        public static string CurrentPhase => Phases.FirstOrDefault(x => x.Value == true).Key;
    }
}