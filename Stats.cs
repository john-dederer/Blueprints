namespace Blueprints
{
    internal class Stats : PlayerStats
    {
        protected override void Update()
        {
            if (Interactive.BOpened)
            {
                //this.IsBloody = false;
                //this.FireWarmth = true;
                //this.SunWarmth = true;
                //this.IsCold = false;
                //this.Health = 100f;
                //this.Armor = 400;
                //this.Fullness = 1f;
                Stamina = Interactive.FStamina;
                Energy = Interactive.FEnergy;
                //this.Hunger = 0;
                //this.Thirst = 0;
                //this.Starvation = 0;
            }
            else if (Recipes.BOpened)
            {
                Stamina = Recipes.FStamina;
                Energy = Recipes.FEnergy;
            }

            base.Update();
        }
    }
}