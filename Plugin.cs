using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;

namespace SpeechAction
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const String modGUID = "Navinate.SpeechAction";
        private const String modName = "SpeechAction";
        private const String modVersion = "0.0.2";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static Plugin Instance;
        internal static ManualLogSource Log;

        private KeywordRecognizer keywordRecognizer;
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();

        internal static PlayerControllerB player;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Plugin.Log = base.Logger;

            harmony.PatchAll();

            Log.LogInfo($"Plugin {modName} is loaded!");
        }

        void Start()
        {
            actions.Add("dance", StartDanceEmote);
            actions.Add("party", StartDanceEmote);
            actions.Add("point", StartPointEmote);
            actions.Add("there", StartPointEmote);

            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += HandleRecognizedSpeech;
            keywordRecognizer.Start();
        }

        private void HandleRecognizedSpeech(PhraseRecognizedEventArgs speech)
        {
            Log.LogInfo(speech.text);
            actions[speech.text].Invoke();
        }

        void StartDanceEmote()
        {
            if (((player.IsOwner && player.isPlayerControlled && (!player.IsServer || player.isHostPlayerObject)) || player.isTestingPlayer) && !(player.timeSinceStartingEmote < 0.5f))
            {
                player.timeSinceStartingEmote = 0f;
                player.performingEmote = true;
                player.playerBodyAnimator.SetInteger("emoteNumber", 1);
                player.StartPerformingEmoteServerRpc();
            }
        }

        void StartPointEmote()
        {
            if (((player.IsOwner && player.isPlayerControlled && (!player.IsServer || player.isHostPlayerObject)) || player.isTestingPlayer) && !(player.timeSinceStartingEmote < 0.5f))
            {
                player.timeSinceStartingEmote = 0f;
                player.performingEmote = true;
                player.playerBodyAnimator.SetInteger("emoteNumber", 2);
                player.StartPerformingEmoteServerRpc();
            }
        }
    }
}