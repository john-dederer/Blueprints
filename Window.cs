using UnityEngine;
using TheForest.Items;
using TheForest.Utils;

namespace Blueprints
{
    class Window : MonoBehaviour
    {
        /*
        protected bool visible = false;
        protected GUIStyle labelStyle;
        public Vector2 scrollPosition = Vector2.zero;
        private float cY;

        public string stringToEdit = "Search for...";


        [ModAPI.Attributes.ExecuteOnGameStart]
        static void AddMeToScene()
        {
            ModAPI.Log.Write("AddToScene called.");
            GameObject GO = new GameObject("__Window__");
            GO.AddComponent<Window>();
        }

        private void OnGUI()
        {
            ModAPI.Log.Write("OnGUI called.");
            if (this.visible)
            {
                System.Collections.Generic.IEnumerable<TheForest.Items.Item> itemList = TheForest.Items.ItemDatabase.Items;
                System.Collections.ArrayList test = null;

                GUI.skin = ModAPI.Gui.Skin;

                Matrix4x4 bkpMatrix = GUI.matrix;

                if (labelStyle == null)
                {
                    labelStyle = new GUIStyle(GUI.skin.label);
                    labelStyle.fontSize = 12;
                }

                GUI.Box(new Rect(10, 10, 400, 450), "Blueprints", GUI.skin.window);
                scrollPosition = GUI.BeginScrollView(new Rect(10, 50, 390, 350), scrollPosition, new Rect(0, 0, 350, cY));
                this.cY = 25f;

                for (int index = 0; index < ItemDatabase.Items.Length; ++index)
                {
                    GUI.TextField(new Rect(20f, cY, 150f, 20f), ItemDatabase.Items[index]._name, labelStyle);
                    if (GUI.Button(new Rect(170f, cY, 150f, 20f), "Add"))
                    {
                        LocalPlayer.Inventory.AddItem(ItemDatabase.Items[index]._id);

                        LocalPlayer.Inventory.CurrentView.ToString();
                    }
                    this.cY += 30f;
                }
                GUI.EndScrollView();

                if (GUI.Button(new Rect(20f, cY, 100f, 20f), "Close"))
                {
                    this.visible = false;
                }

                GUI.matrix = bkpMatrix;
            }
        }

        private void GenerateList()
        {
            ModAPI.Log.Write("GenerateList called.");
            for (int index = 0; index < ItemDatabase.Items.Length; ++index)
                ModAPI.Console.Write("itemName" + ItemDatabase.Items[index]._name + " itemID: " + ItemDatabase.Items[index]._id);
        }

        private void Update()
        {
            ModAPI.Log.Write("Update called.");
            if (ModAPI.Input.GetButtonDown("Window", "Blueprints"))
            {
                if (this.visible)
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                }
                else
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.LockView();
                }
                this.visible = !this.visible;
            }

        }
     */   
    }
}
