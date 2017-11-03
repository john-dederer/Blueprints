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
    internal class Interactive : MonoBehaviour
    {
        // Members
        //protected Texture2D myTexture = null;
        public static bool BOpened;

        public static float FStamina;
        public static float FEnergy;
        protected Dictionary<int, Receipe> DicItemIdToReceipe;
        protected string SCount = "1";
        protected string SInput = "Search for item...";

        [ExecuteOnGameStart]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private static void Init()
        {
            var go = new GameObject("__Interactive__");
            go.AddComponent<Interactive>();
            Log.Write("Interactive added to scene");
        }

        //private void Start()
        //{
        //    //myTexture = ModAPI.Resources.GetTexture("Mods/Blueprints/map.jpg", "Blueprints");
        //    try
        //    {
        //        ModAPI.Log.Write(myTexture);
        //        myTexture = new Texture2D(700, 700, TextureFormat.RGB24, false, true);
        //        //myTexture.LoadImage(System.IO.File.ReadAllBytes(@"C:\Program Files (x86)\Steam\steamapps\common\The Forest\Mods\Blueprints\map.jpg"));
        //
        //        ModAPI.Log.Write(myTexture);
        //    }
        //    catch (Exception e)
        //    {
        //        ModAPI.Log.Write(e.Message);
        //    }
        //}

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnGUI()
        {
            try
            {
                if (!BOpened) return;
                //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
                //LocalPlayer.Inventory.Block();

                //TheForest.Utils.LocalPlayer.Inventory.StashEquipedWeapon(false);

                var cY = 30f;

                GUI.skin = Gui.Skin;

                var bkpMatrix = GUI.matrix;

                //GUI.DrawTexture(new Rect(100, 100, 700, 700), myTexture);
                SInput = GUI.TextField(new Rect(110, 110, 1000, 30), SInput);

                foreach (var item in ItemDatabase.Items)
                {
                    if (!item._name.ToUpper().Contains(SInput.ToUpper())) continue;
                    if (SInput.Equals(string.Empty)) continue;
                    //ModAPI.Log.Write(string.Format("{0} found for item {1}", sInput, item._name));
                    //if (!item._type.Equals(TheForest.Items.Item.Types.)) { continue; }
                    var bfound = false;
                    var sRecipe = GetRecipeString(item._id, ref bfound);

                    GUI.TextField(new Rect(110f, 120f + cY, 620f, 30f), sRecipe);

                    SCount = GUI.TextField(new Rect(750f, 120f + cY, 120f, 30f), SCount);

                    if (GUI.Button(new Rect(880f, 120f + cY, 100f, 30f), "Add"))
                    {
                        Log.Write($"Craft {SCount} items");

                        var count = Convert.ToInt32(SCount);

                        var dictionary = new Dictionary<int, int>();

                        for (var i = 0; i < count; i++)
                            if (CheckInventory(item._id, 1, ref dictionary))
                            {
                                RemoveItemsFromInventory(ref dictionary);

                                // add items to inventory
                                LocalPlayer.Inventory.AddItem(item._id);

                                dictionary = new Dictionary<int, int>();
                            }
                            else
                            {
                                // maximum crafted
                                break;
                            }
                    }

                    //if (GUI.Button(new Rect(670f, 120f + cY, 100f, 30f), "Add 1"))
                    //{
                    //    Dictionary<int, int> dictionary = new Dictionary<int, int>();
                    //
                    //    if (CheckInventory(item._id, 1, ref dictionary))
                    //    {
                    //        RemoveItemsFromInventory(ref dictionary);
                    //
                    //        // add items to inventory
                    //        LocalPlayer.Inventory.AddItem(item._id);
                    //        LocalPlayer.Inventory.CurrentView.ToString();
                    //        
                    //    }
                    //}

                    if (GUI.Button(new Rect(1000f, 120f + cY, 100f, 30f), "Add Max"))
                    {
                        var dictionary = new Dictionary<int, int>();

                        var max = ItemDatabase.ItemById(item._id)._maxAmount;

                        for (var i = 0; i < max; i++)
                            if (CheckInventory(item._id, 1, ref dictionary))
                            {
                                RemoveItemsFromInventory(ref dictionary);

                                // add items to inventory
                                LocalPlayer.Inventory.AddItem(item._id);

                                dictionary = new Dictionary<int, int>();
                            }
                            else
                            {
                                // maximum crafted
                                break;
                            }
                    }

                    cY += 30f;
                }

                GUI.matrix = bkpMatrix;
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
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

                if (sRecipe == string.Empty)
                    sRecipe = ItemDatabase.ItemById(DicItemIdToReceipe[id]._productItemID)._name;

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
            if (!Input.GetButtonDown("Interactive", "Blueprints")) return;
            //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
            //LocalPlayer.Inventory.Block();


            if (BOpened)
            {
                LocalPlayer.FpCharacter.UnLockView();
                SInput = "Search for item...";
                //LocalPlayer.Inventory.UnBlock();
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

        private static bool CheckInventory(int itemId, int amount, ref Dictionary<int, int> dicIngredients, bool bMax = false)
        {
            try
            {
                var bCanCraft = false;

                if (ItemDatabase.ItemById(itemId)._maxAmount.Equals(LocalPlayer.Inventory.AmountOf(itemId)))
                {
                    Log.Write($"Maximum amount of item {ItemDatabase.ItemById(itemId)._name} in Inventory.");
                    return false;
                }

                foreach (var recipe in ReceipeDatabase.Receipes)
                    // Recipe for item
                    if (recipe._productItemID.Equals(itemId))
                    {
                        if (!bMax)
                            foreach (var ingredient in recipe._ingredients)
                            {
                                var nameIngredient = ItemDatabase.ItemById(ingredient._itemID)._name;
                                var nameItem = ItemDatabase.ItemById(itemId)._name;

                                // Needed to craft
                                var nNeeded = ingredient._amount * amount;

                                // In Inventory
                                var nInInventory = LocalPlayer.Inventory.AmountOf(ingredient._itemID);

                                Log.Write(
                                    $"Ingredient: {nameIngredient} for Item: {nameItem}. In Inventory: {nInInventory}, needed to Craft {amount} times: {nNeeded}");

                                // If enough available we can craft
                                if (nInInventory >= nNeeded)
                                {
                                    Log.Write(
                                        $"Added to Dictionary: Item: {ItemDatabase.ItemById(ingredient._itemID)._name}, Amount: {nNeeded}");
                                    dicIngredients.Add(ingredient._itemID, nNeeded);

                                    bCanCraft = true;
                                }
                                else
                                {
                                    return false;
                                }
                            }

                        break;
                    }

                Log.Write($"Can craft {ItemDatabase.ItemById(itemId)._name} ({amount}) times => {bCanCraft}");

                return bCanCraft;
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
                return false;
            }
        }

        private static void RemoveItemsFromInventory(ref Dictionary<int, int> dictionary)
        {
            try
            {
                // remove items / ingredients from inventory
                foreach (var ingredient in dictionary)
                foreach (var item in LocalPlayer.Inventory._possessedItems)
                    //ModAPI.Log.Write(string.Format("Possessed item: {0}, Item to remove: {1}", TheForest.Items.ItemDatabase.ItemById(item._itemId)._name, TheForest.Items.ItemDatabase.ItemById(ingredient.Key)._name));
                    if (item._itemId.Equals(ingredient.Key))
                    {
                        var success = item.Remove(ingredient.Value);
                        Log.Write(
                            $"Removed item: {ItemDatabase.ItemById(ingredient.Key)._name}, amount: {ingredient.Value} successfully = {success}");
                    }
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
        }
    }
}