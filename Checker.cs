using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Items;
using UnityEngine;

namespace Blueprints
{
    class Checker : UnityEngine.MonoBehaviour
    {
        protected Dictionary<string, bool> dic = null;
        protected Dictionary<string, List<TheForest.Items.Item>> dicItems = null;
        protected bool bVisible = false;

        [ModAPI.Attributes.ExecuteOnGameStart]
        static void AddMeToScene()
        {
            ModAPI.Log.Write("Checker added to scene.");
            UnityEngine.GameObject GO = new UnityEngine.GameObject("__Checker__");
            GO.AddComponent<Checker>();
        }

        private void LogCurrentInventoryView()
        {
            if (TheForest.Utils.LocalPlayer.Inventory.IsOpenningInventory)
            {
                ModAPI.Log.Write(TheForest.Utils.LocalPlayer.Inventory.CurrentView.SerializeToString());
            }
        }

        private void Start()
        {
            foreach (var category in Enum.GetNames(typeof(TheForest.Items.Item.Types)))
            {
                dic.Add(category, false);
            }

            for (int index = 0; index < TheForest.Items.ItemDatabase.Items.Length; ++index)
            {
                foreach (var category in Enum.GetNames(typeof(TheForest.Items.Item.Types)))
                {
                    if (ItemDatabase.Items[index]._type.ToString().Equals(category))
                    {
                        if (dicItems[category] == null) { dicItems[category] = new List<Item>(); }
                        dicItems[category].Add(ItemDatabase.Items[index]);
                    }
                }

            }
        }

        private void OnGUI()
        {
            if (bVisible)
            {
                float height = 0f;

                GUI.skin = ModAPI.Gui.Skin;

                Matrix4x4 bkpMatrix = GUI.matrix;

                GUI.Box(new Rect(left: 10, top: 30f - 10f, width: 200f, height: 700f), "Filter", GUI.skin.window);
                int _y = 0;
                int _x = 0;


                foreach (var category in Enum.GetNames(typeof(TheForest.Items.Item.Types)))
                {
                    _x = 0;
                    bool isToggled = false;
                    dic.TryGetValue(category, out isToggled);

                    dic[category] = GUI.Toggle(new Rect(10, 30f + _y, 200f, 20f), isToggled, category, GUI.skin.button);
                    _y += 20;

                    if (isToggled)
                    {
                        foreach (var lists in dicItems.Values)
                        {
                            foreach (var item in lists)
                            {
                                Rect nr = new Rect(10 + _x, 40f + _y, 100f, 20f);
                                GUI.color = Color.white;
                                nr = new Rect(35 + _x, Screen.height - (height) - 10f + _y, 65f, 20f);

                            }
                        }
                    }
                }
                GUI.matrix = bkpMatrix;

                //TheForest.Utils.Scene.HudGui.Icon
            }
        }

        private void Update()
        {
            if (ModAPI.Input.GetButtonDown("Inventory_Open", "Blueprints"))
            {
                ModAPI.Log.Write("Inventor yoepned");
                TheForest.Utils.LocalPlayer.Inventory.CurrentStorage.Open();
                //TheForest.Utils.LocalPlayer.Inventory.

                
            }
        }
    }
}
