using System.Diagnostics.CodeAnalysis;
using ModAPI;
using ModAPI.Attributes;
using TheForest.Utils;
using UnityEngine;
using Input = ModAPI.Input;

namespace Blueprints
{
    internal class Save : MonoBehaviour
    {
        [ExecuteOnGameStart]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void Init()
        {
            var go = new GameObject("__Save__");
            go.AddComponent<Save>();
            Log.Write("Save added to scene");
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnGUI()
        {
            //base.ToString();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Update()
        {
            if (!Input.GetButtonDown("Save_State", "Blueprints")) return;
            Log.Write("Saved current state");

            LocalPlayer.Stats.JustSave();
        }
    }
}