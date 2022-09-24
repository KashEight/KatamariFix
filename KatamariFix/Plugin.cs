using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;

namespace KatamariFix
{
    [BepInPlugin("net.hytus.kamatarifix", "KatamariFix", "1.0.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("net.hytus.harmony.kamatarifix");
            harmony.PatchAll(typeof(AudioPatch));
            
        }
    }
}
