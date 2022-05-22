using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Hive : Entity {

        Log log = new Log("Hive.cs");
        List<Food> foodList = new List<Food>();
        List<Ant> ants = new List<Ant>();
        List<AntEgg> eggs = new List<AntEgg>();
        protected int targetMaxAnts;

        public Hive(int xPos, int yPos, int targetMaxAnts) : base(xPos, yPos) {
            this.entitySymbol = "H";
            this.x = xPos;
            this.y = yPos;
            this.targetMaxAnts = targetMaxAnts;
        }

        //Add Food To Hive
        public void AddFood(Food food) {
            //Added Food has as Owner the Hive so it does not get Searched anymore
            food.SetTargetOrOwner(this);
            foodList.Add(food);
            log.Add("Hive got Food id(" + food.GetEntityId() + ") - Food count =" + foodList.Count());
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
            if (ant.GetHive() == this) {
                ants.Add(ant);
            }
        }

        public void RemoveOwnedAnt(Ant ant) {
            if (ant.GetHive() == this) {
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
            if (egg.GetHive() == this) {
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
