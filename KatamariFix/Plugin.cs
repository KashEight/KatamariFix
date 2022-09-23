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

    class AudioPatch
    {
        [HarmonyPatch(typeof(SoundController), "Setup")]
        [HarmonyPostfix]
        static void SetGameVolumeSilent(SoundController __instance)
        {
            Plugin.Log.LogMessage("Patching SoundController::Setup(); Set volume to 1/8");
            foreach (SoundController.eAudioMixerGroup eAudioMixer in Enum.GetValues(typeof(SoundController.eAudioMixerGroup))) {
                if (!eAudioMixer.Equals(SoundController.eAudioMixerGroup.MAX))
                {
                    Plugin.Log.LogMessage("eAudioMixer: " + eAudioMixer.ToString());
                    __instance.SetVolume(eAudioMixer, 0.125f);
                }
            }
        }

        [HarmonyPatch(typeof(UIMoviePlayer), "PlayMove")]
        [HarmonyPrefix]
        static void SetMovieVolumeSilent(ref float volume)
        {
            Plugin.Log.LogMessage("Patching UIMoviePlayer::PlayMove(); Set volume to 1/8");
            volume = 0.125f;
        }
    }
}
