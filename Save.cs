using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Blueprints
{
    class Save : MonoBehaviour
    {
        [ModAPI.Attributes.ExecuteOnGameStart]
        private static void Init()
        {
            GameObject go = new GameObject("__Save__");
            go.AddComponent<Save>();
        }

        //private void OnGUI()
        //{
        //
        //}

        private void Update()
        {
            if (ModAPI.Input.GetButtonDown("Save_State", "Blueprints"))
            {
                ModAPI.Log.Write("Saved current state");

                TheForest.Utils.LocalPlayer.Stats.JustSave();
            }          
        }
    }
}
