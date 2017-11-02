using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blueprints
{
    class Stats : PlayerStats
    {
        protected override void Update()
        {
            if (Interactive.bOpened)
            {
                //this.IsBloody = false;
                //this.FireWarmth = true;
                //this.SunWarmth = true;
                //this.IsCold = false;
                //this.Health = 100f;
                //this.Armor = 400;
                //this.Fullness = 1f;
                this.Stamina = Interactive.fStamina;
                this.Energy = Interactive.fEnergy;
                //this.Hunger = 0;
                //this.Thirst = 0;
                //this.Starvation = 0;
            }
            else if (Recipes.bOpened)
            {
                this.Stamina = Recipes.fStamina;
                this.Energy = Recipes.fEnergy;
            }

            base.Update();
        }
    }
}
