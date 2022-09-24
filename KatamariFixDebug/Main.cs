using BepInEx;
using HarmonyLib;
using System;

namespace KatamariFixDebug
{
    [BepInPlugin("net.hytus.katamarifixdebug", "KatamariFixDebug", "1.0.0.0")]
    public class Main: BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("net.hytus.katamarifixdebug");
            harmony.PatchAll(typeof(DisableClick));
        }
    }
}
