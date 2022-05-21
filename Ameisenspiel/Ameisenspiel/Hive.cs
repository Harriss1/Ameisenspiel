using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Hive : Entity {

        Log log = new Log("Hive.cs");
        List<Food> foodList = new List<Food>();
        public Hive(int xPos, int yPos) : base(xPos, yPos) {
            this.entitySymbol = "H";
            this.x = xPos;
            this.y = yPos;
        }

        //Add Food To Hive
        public void AddFood(Food food) {
            //Added Food has as Owner the Hive so it does not get Searched anymore
            food.SetTargetOrOwner(this);
            foodList.Add(food);
            log.Add("Hive got Food id(" + food.GetEntityId() + ") - Food count =" + foodList.Count());
        }
    }

}
