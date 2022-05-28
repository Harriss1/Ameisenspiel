using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Food : Entity {

        //Possible generalisation: An item can have an nest it belongs to.
        //Food has never an reference to any destroyable entity!
        Nest storedInNest;

        private Food(int xPos, int yPos) : base(xPos, yPos) {
            storedInNest = null;
        }

        //Making a new Food Entity without coodinates spawns it randomly
        public Food()
            : this(-1,-1){
            SetRandomPosition();
            entitySymbol = "\'";
        }

        /*
        public override void MoveOneIntelligent() {
            if (ownerAnt == null) {
                //Food always follows the carrier
                return;
            }

            x = ownerAnt.GetX();
            y = ownerAnt.GetY();
        }*/

        public void CarryByAnt(Ant ant){
            x = ant.GetX();
            y = ant.GetY();
        }


        public void SetStoredInNest(Nest nest) {
            this.storedInNest = nest;
        }

        public Nest GetStoredInNest() {
            return storedInNest;
        }

    }
}
