using HarmonyLib;
using System;

namespace KatamariFix
{
    class AudioPatch
    {
        [HarmonyPatch(typeof(SoundController), "Setup")]
        [HarmonyPostfix]
        static void SetGameVolumeSilent(SoundController __instance)
        {
            Plugin.Log.LogMessage("Patching SoundController::Setup(); Set volume to 1/8");
            foreach (SoundController.eAudioMixerGroup eAudioMixer in Enum.GetValues(typeof(SoundController.eAudioMixerGroup)))
            {
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
