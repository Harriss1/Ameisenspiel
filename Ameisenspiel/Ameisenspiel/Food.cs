using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Food : Entity {

        public Food(int xPos, int yPos) : base(xPos, yPos) {
        }

        //Making a new Food Entity without coodinates spawns it randomly
        public Food()
            : this(-1,-1){
            SetRandomPosition();
            entitySymbol = "\'";
        }


        public override void MoveOneIntelligent() {
            if (carryLink == null) {
                //Food always follows the carrier
                return;
            } else {
                x = carryLink.GetX();
                y = carryLink.GetY();
            }
        }

    }
}
