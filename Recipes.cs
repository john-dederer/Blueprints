using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ModAPI;
using ModAPI.Attributes;
using TheForest.Items;
using TheForest.Items.Craft;
using TheForest.Utils;
using UnityEngine;
using Input = ModAPI.Input;

namespace Blueprints
{
    internal class Recipes : MonoBehaviour
    {
        public static bool BOpened;
        public static float FStamina;
        public static float FEnergy;
        protected Dictionary<int, Receipe> DicItemIdToReceipe;
        protected string SInput = "Search for recipe...";

        [ExecuteOnGameStart]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void Init()
        {
            var go = new GameObject("__RECIPES__");
            go.AddComponent<Recipes>();

            Log.Write("Recipes added to Scene.");
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Start()
        {
            try
            {
                DicItemIdToReceipe = new Dictionary<int, Receipe>();

                foreach (var recipe in ReceipeDatabase.Receipes)
                {
                    if (DicItemIdToReceipe.ContainsKey(recipe._productItemID))
                        continue;

                    DicItemIdToReceipe.Add(recipe._productItemID, recipe);
                }

                Log.Write("Dictionary initialized.");
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnGUI()
        {
            if (!BOpened) return;
            //TheForest.Utils.LocalPlayer.Inventory.Block();
            //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
            //TheForest.Utils.LocalPlayer.Inventory.StashEquipedWeapon(false);

            var cY = 30f;

            GUI.skin = Gui.Skin;

            var bkpMatrix = GUI.matrix;

            SInput = GUI.TextField(new Rect(110, 110, 900, 30), SInput);

            foreach (var item in ItemDatabase.Items)
                if (item._name.ToUpper().Contains(SInput.ToUpper()))
                {
                    if (SInput.Equals(string.Empty)) continue;

                    var bfound = false;

                    var sRecipe = GetRecipeString(item._id, ref bfound);
                    //ModAPI.Log.Write(string.Format("Recipe found for Item: {0} => {1}", item._name, bfound));

                    if (!bfound) continue;

                    GUI.TextField(new Rect(110f, 120f + cY, 800, 30f), sRecipe);

                    cY += 30f;
                }

            GUI.matrix = bkpMatrix;
        }

        private string GetRecipeString(int id, ref bool bNoEntry)
        {
            try
            {
                if (!DicItemIdToReceipe.ContainsKey(id))
                {
                    bNoEntry = true;
                    return string.Empty;
                }

                var sRecipe = DicItemIdToReceipe[id]._name;
                bNoEntry = true;
                //foreach (var ingredient in dicItemIdToReceipe[id]._ingredients)
                //{
                //    sRecipe += string.Format(", [Item: {0} - Amount: {1}]", TheForest.Items.ItemDatabase.ItemById(ingredient._itemID)._name, ingredient._amount);
                //}

                return sRecipe;
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
                return string.Empty;
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Update()
        {
            if (!Input.GetButtonDown("Recipes", "Blueprints")) return;
            //TheForest.Utils.LocalPlayer.Inventory.Block();
            //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;


            if (BOpened)
            {
                LocalPlayer.FpCharacter.UnLockView();
                //TheForest.Utils.LocalPlayer.Inventory.UnBlock();
                LocalPlayer.Inventory.EquipPreviousWeaponDelayed();
            }
            else
            {
                LocalPlayer.FpCharacter.LockView();
                FStamina = LocalPlayer.Stats.Stamina;
                FEnergy = LocalPlayer.Stats.Energy;
                LocalPlayer.Inventory.StashEquipedWeapon(false);
            }
            BOpened = !BOpened;
        }
    }
}