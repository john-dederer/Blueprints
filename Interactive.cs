using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace Blueprints
{
    class Interactive : MonoBehaviour
    {
        
        // Members
        //protected Texture2D myTexture = null;
        public static bool bOpened = false;
        protected Dictionary<int, TheForest.Items.Craft.Receipe> dicItemIdToReceipe;
        protected string sInput = "Search for item...";
        protected string sCount = "1";
        public static float fStamina = 0f;
        public static float fEnergy = 0f;

        [ModAPI.Attributes.ExecuteOnGameStart]
        private static void Init()
        {
            GameObject go = new GameObject("__Interactive__");
            go.AddComponent<Interactive>();
            ModAPI.Log.Write("Interactive added to scene");
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

        private void OnGUI()
        {
            try
            {
               
                if (bOpened)
                {
                    //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
                    //LocalPlayer.Inventory.Block();

                    //TheForest.Utils.LocalPlayer.Inventory.StashEquipedWeapon(false);

                    float cY = 30f;

                    GUI.skin = ModAPI.Gui.Skin;

                    Matrix4x4 bkpMatrix = GUI.matrix;

                    //GUI.DrawTexture(new Rect(100, 100, 700, 700), myTexture);
                    sInput = GUI.TextField(new Rect(110, 110, 1000, 30), sInput);

                    foreach (var item in TheForest.Items.ItemDatabase.Items)
                    {
                        if (item._name.ToUpper().Contains(sInput.ToUpper()))
                        {
                            if (sInput.Equals(string.Empty)) { continue; }
                            //ModAPI.Log.Write(string.Format("{0} found for item {1}", sInput, item._name));
                            //if (!item._type.Equals(TheForest.Items.Item.Types.)) { continue; }
                            bool bfound = false;
                            string sRecipe = GetRecipeString(item._id, ref bfound);

                            GUI.TextField(new Rect(110f, 120f + cY, 620f, 30f), sRecipe);

                            sCount = GUI.TextField(new Rect(750f, 120f + cY, 120f, 30f), sCount);

                            if (GUI.Button(new Rect(880f, 120f + cY, 100f, 30f), "Add"))
                            {
                                ModAPI.Log.Write(string.Format("Craft {0} items", sCount));

                                var count = Convert.ToInt32(sCount);

                                Dictionary<int, int> dictionary = new Dictionary<int, int>();

                                for (int i = 0; i < count; i++)
                                {
                                    if (CheckInventory(item._id, 1, ref dictionary))
                                    {
                                        RemoveItemsFromInventory(ref dictionary);

                                        // add items to inventory
                                        LocalPlayer.Inventory.AddItem(item._id);
                                        LocalPlayer.Inventory.CurrentView.ToString();

                                        dictionary = new Dictionary<int, int>();
                                    }
                                    else
                                    {
                                        // maximum crafted
                                        break;
                                    }
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
                                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                            
                                int max = TheForest.Items.ItemDatabase.ItemById(item._id)._maxAmount;
                            
                                for (int i = 0; i < max; i++)
                                {
                                    if (CheckInventory(item._id, 1, ref dictionary))
                                    {
                                        RemoveItemsFromInventory(ref dictionary);

                                        // add items to inventory
                                        LocalPlayer.Inventory.AddItem(item._id);
                                        LocalPlayer.Inventory.CurrentView.ToString();

                                        dictionary = new Dictionary<int, int>();
                                    }
                                    else
                                    {
                                        // maximum crafted
                                        break;
                                    }
                                }                                
                            }

                            cY += 30f;
                        }
                    }

                    GUI.matrix = bkpMatrix;

                   
                }
            }
            catch (Exception e)
            {
                ModAPI.Log.Write(e.Message);
            }
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
            catch (Exception e)
            {
                ModAPI.Log.Write(e.Message);
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

                if (sRecipe == string.Empty)
                {
                    sRecipe = TheForest.Items.ItemDatabase.ItemById(dicItemIdToReceipe[id]._productItemID)._name;
                }

                bNoEntry = true;
                //foreach (var ingredient in dicItemIdToReceipe[id]._ingredients)
                //{
                //    sRecipe += string.Format(", [Item: {0} - Amount: {1}]", TheForest.Items.ItemDatabase.ItemById(ingredient._itemID)._name, ingredient._amount);
                //}

                return sRecipe;
            }
            catch (Exception e)
            {
                ModAPI.Log.Write(e.Message);
                return string.Empty;
            }
        }


        private void Update()
        {
            if (ModAPI.Input.GetButtonDown("Interactive", "Blueprints"))
            {
                //TheForest.Utils.LocalPlayer.Inventory.CancelNextChargedAttack = true;
                //LocalPlayer.Inventory.Block();
                

                if (bOpened)
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                    sInput = "Search for item...";
                    //LocalPlayer.Inventory.UnBlock();
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

        private bool CheckInventory(int itemID, int amount, ref Dictionary<int, int> dicIngredients, bool bMax = false)
        {
            try
            {
                bool bCanCraft = false;

                if (TheForest.Items.ItemDatabase.ItemById(itemID)._maxAmount.Equals(LocalPlayer.Inventory.AmountOf(itemID)))
                {
                    ModAPI.Log.Write(string.Format("Maximum amount of item {0} in Inventory.", TheForest.Items.ItemDatabase.ItemById(itemID)._name));
                    return false;
                }

                foreach (var recipe in TheForest.Items.Craft.ReceipeDatabase.Receipes)
                {
                    // Recipe for item
                    if (recipe._productItemID.Equals(itemID))
                    {

                        if (!bMax)
                        {
                            // Get ingredients when no max item requested
                            foreach (var ingredient in recipe._ingredients)
                            {
                                string nameIngredient = TheForest.Items.ItemDatabase.ItemById(ingredient._itemID)._name;
                                string nameItem = TheForest.Items.ItemDatabase.ItemById(itemID)._name;

                                // Needed to craft
                                int nNeeded = ingredient._amount * amount;

                                // In Inventory
                                int nInInventory = LocalPlayer.Inventory.AmountOf(ingredient._itemID);

                                ModAPI.Log.Write(string.Format("Ingredient: {0} for Item: {1}. In Inventory: {2}, needed to Craft {3} times: {4}", nameIngredient, nameItem, nInInventory, amount, nNeeded));

                                // If enough available we can craft
                                if (nInInventory >= nNeeded)
                                {
                                    ModAPI.Log.Write(string.Format("Added to Dictionary: Item: {0}, Amount: {1}", TheForest.Items.ItemDatabase.ItemById(ingredient._itemID)._name, nNeeded));
                                    dicIngredients.Add(ingredient._itemID, nNeeded);

                                    bCanCraft = true;
                                }
                                else
                                {
                                    bCanCraft = false;                                   
                                    return bCanCraft;
                                }
                            }
                        }
                        else
                        {
                            //// calculate max items craftable
                            //foreach (var ingredient in recipe._ingredients)
                            //{
                            //    // Needed to craft
                            //    double nNeeded = ingredient._amount;
                            //
                            //    // In Inventory
                            //    double nInInventory = TheForest.Utils.LocalPlayer.Inventory.AmountOf(ingredient._itemID);
                            //
                            //    int nCraftable = Convert.ToInt32(Math.Floor((nInInventory / nNeeded)));
                            //
                            //    // If enough available we can craft
                            //
                            //    ModAPI.Log.Write(string.Format("Added to Dictionary: ItemID: {0}, AMount: {1}", ingredient._itemID, nNeeded));
                            //    dicIngredients.Add(ingredient._itemID, nCraftable * Convert.ToInt32(nNeeded));
                            //
                            //    bCanCraft = true;
                            //}
                        }

                        break;
                    }
                }

                ModAPI.Log.Write(string.Format("Can craft {0} ({1}) times => {2}", TheForest.Items.ItemDatabase.ItemById(itemID)._name, amount, bCanCraft));

                return bCanCraft;
            }
            catch(Exception e)
            {
                ModAPI.Log.Write(e.Message);
                return false;
            }
        }

        private void RemoveItemsFromInventory(ref Dictionary<int, int> dictionary)
        {
            try
            {
                // remove items / ingredients from inventory
                foreach (var ingredient in dictionary)
                {
                    foreach (var item in LocalPlayer.Inventory._possessedItems)
                    {
                        //ModAPI.Log.Write(string.Format("Possessed item: {0}, Item to remove: {1}", TheForest.Items.ItemDatabase.ItemById(item._itemId)._name, TheForest.Items.ItemDatabase.ItemById(ingredient.Key)._name));
                        if (item._itemId.Equals(ingredient.Key))
                        {
                            var success = item.Remove(ingredient.Value);
                            ModAPI.Log.Write(string.Format("Removed item: {0}, amount: {1} successfully = {2}", TheForest.Items.ItemDatabase.ItemById(ingredient.Key)._name, ingredient.Value, success));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                ModAPI.Log.Write(e.Message);
            }
        }
        
    }
}
