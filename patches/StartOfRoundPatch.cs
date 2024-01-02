using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechAction.patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void storePlayerScript(ref PlayerControllerB ___localPlayerController)
        {
            Plugin.player = ___localPlayerController;
        }
    }
}