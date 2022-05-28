using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// A nest of ants or another insect.
/// </summary>
namespace Ameisenspiel {
    internal class Nest : Entity {

        Log log = new Log("Nest.cs");
        List<Food> foodList = new List<Food>();
        List<Ant> ants = new List<Ant>();
        List<AntEgg> eggs = new List<AntEgg>();
        protected int targetMaxAnts;

        public Nest(int xPos, int yPos, int targetMaxAnts) : base(xPos, yPos) {
            this.entitySymbol = "H";
            this.x = xPos;
            this.y = yPos;
            this.targetMaxAnts = targetMaxAnts;
        }

        //Add Food To Nest
        public void AddFood(Food food) {
            //Added Food has as Owner the Nest so it does not get Searched anymore
            food.AssoziatePathFindTowards(this);
            foodList.Add(food);
            log.Add("Nest got Food id(" + food.GetEntityId() + ") - Food count =" + foodList.Count());
        }

        public Food EatFood() {
            if (foodList.Count() > 0) {
                Food food = foodList.First();
                foodList.Remove(food);
                foodList.TrimExcess();
                return food;
            }
            return null;
        }

        public int GetTargetMaxAnts() {
            return targetMaxAnts;
        }

        public void AddOwnedAnt(Ant ant) {
            //we only Ants that belong to this hive
            if (ant.GetNest() == this) {
                ants.Add(ant);
            }
        }

        public void RemoveOwnedAnt(Ant ant) {
            if (ant.GetNest() == this) {
                ants.Remove(ant);
                ants.TrimExcess();
            }
        }

        public int GetOwnedAntCount() {
            if(ants == null) {
                return 0;
            }
            ants.TrimExcess();
            return ants.Count();
        }

        public void AddOwnedEgg(AntEgg egg) {
            if (egg.GetNest() == this) {
                eggs.Add(egg);
            }
        }

        public int GetOwnedAntEggCount() {
            
            return eggs.Count();
        }

        public void RemoveEgg(AntEgg egg) {
            eggs.Remove(egg);
            eggs.TrimExcess();
        }

        public int GetOwnedFoodCount() {
            return foodList.Count();
        }
    }

}
