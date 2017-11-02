using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Blueprints
{
    class Blueprints : MonoBehaviour
    {

        [ModAPI.Attributes.ExecuteOnGameStart]
        private static void Init()
        {
            GameObject go = new GameObject("__BLUEPRINTS__");
            go.AddComponent<Blueprints>();
        }

        private void OnGUI()
        {

        }

        private void Update()
        { 
            if (ModAPI.Input.GetButtonDown("Save_State", "Blueprints"))
            {
                ModAPI.Log.Write("Saved current state");

                TheForest.Utils.LocalPlayer.Stats.JustSave();
            }

            if (ModAPI.Input.GetButtonDown("Blueprint", "Blueprints"))
            {
//                TheForest.Utils.LocalPlayer.Inventory.AddItem(12)

                
                //TheForest.Utils.LocalPlayer.Inventory.CurrentStorage.Add();
                //TheForest.Utils.LocalPlayer.Inventory.CurrentStorage.Remove();

            }
        }
    }
}
