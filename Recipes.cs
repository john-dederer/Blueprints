using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Blueprints
{
    class Recipes : MonoBehaviour
    {
        public static bool bOpened = false;
        protected Dictionary<int, TheForest.Items.Craft.Receipe> dicItemIdToReceipe;
        protected string sInput = "Search for recipe...";
        public static float fStamina = 0f;
        public static float fEnergy = 0f;

        [ModAPI.Attributes.ExecuteOnGameStart]
        private static void Init()
        {
            GameObject GO = new GameObject("__RECIPES__");
            GO.AddComponent<Recipes>();

            ModAPI.Log.Write("Recipes added to Scene.");
        }

        private void Start()
        {
            try
            {
                dicItemIdToReceipe = new Dictionary<int, TheForest.Items.Craft.Receipe>();

                foreach (var recipe in TheForest.Items.Craft.ReceipeDatabase.Receipes)
                {
                    if (dicItemIdToReceipe.ContainsKey(recipe._productItemID))
                    {
                        continue;
                    }

                    dicItemIdToReceipe.Add(recipe._productItemID, recipe);
                }

                ModAPI.Log.Write("Dictionary initialized.");
            }
            catch(Exception e)
            {
                ModAPI.Log.Write(e.Message);
            }
        }

        private void OnGUI()
        {
            if (bOpened)
            {
                //TheForest.Utils.LocalPlayer.Inventory.Block();
                //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
                //TheForest.Utils.LocalPlayer.Inventory.StashEquipedWeapon(false);

                float cY = 30f;

                GUI.skin = ModAPI.Gui.Skin;

                Matrix4x4 bkpMatrix = GUI.matrix;

                sInput = GUI.TextField(new Rect(110, 110, 900, 30), sInput);

                foreach (var item in TheForest.Items.ItemDatabase.Items)
                {
                    if (item._name.ToUpper().Contains(sInput.ToUpper()))
                    {
                        if (sInput.Equals(string.Empty)) { continue;  }

                        bool bfound = false;

                        string sRecipe = GetRecipeString(item._id, ref bfound);
                        //ModAPI.Log.Write(string.Format("Recipe found for Item: {0} => {1}", item._name, bfound));

                        if (!bfound) { continue;  }

                        GUI.TextField(new Rect(110f, 120f + cY, 800, 30f), sRecipe);

                        cY += 30f;
                    }
                }

                GUI.matrix = bkpMatrix;
            }
        }

        private string GetRecipeString(int id, ref bool bNoEntry)
        {
            try
            {
                string sRecipe = string.Empty; 

                if (!dicItemIdToReceipe.ContainsKey(id))
                {
                    bNoEntry = true;
                    return string.Empty;
                }

                sRecipe = dicItemIdToReceipe[id]._name;
                bNoEntry = true;
                //foreach (var ingredient in dicItemIdToReceipe[id]._ingredients)
                //{
                //    sRecipe += string.Format(", [Item: {0} - Amount: {1}]", TheForest.Items.ItemDatabase.ItemById(ingredient._itemID)._name, ingredient._amount);
                //}

                return sRecipe;
            }
            catch(Exception e)
            {
                ModAPI.Log.Write(e.Message);
                return string.Empty;
            }
        }

        private void Update()
        {
            if (ModAPI.Input.GetButtonDown("Recipes", "Blueprints"))
            {
                //TheForest.Utils.LocalPlayer.Inventory.Block();
                //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
                

                if (bOpened)
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                    //TheForest.Utils.LocalPlayer.Inventory.UnBlock();
                    TheForest.Utils.LocalPlayer.Inventory.EquipPreviousWeaponDelayed();
                }
                else
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.LockView();
                    fStamina = TheForest.Utils.LocalPlayer.Stats.Stamina;
                    fEnergy = TheForest.Utils.LocalPlayer.Stats.Energy;
                    TheForest.Utils.LocalPlayer.Inventory.StashEquipedWeapon(false);
                }
                bOpened = !bOpened;
            }
        }
    }
}
