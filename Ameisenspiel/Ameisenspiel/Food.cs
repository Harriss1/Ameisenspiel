using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Food : Entity {

        //Possible generalisation: An item can have an owner.
        //Attention: Owning Food has to stay a one-way relationship.
        //How do I implement, that Food belongs to a nest and is not available on the map?
        Ant ownerAnt;
        //Food either belongs to a nest or to an ant.
        Nest ownerNest;

        private Food(int xPos, int yPos) : base(xPos, yPos) {
            ownerAnt = null;
            ownerNest = null;
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

        public Entity GetOwnerAnt() {
            return ownerAnt;
        }

        public void SetOwnerAnt(Ant owner) {
            if (ownerNest == null) {
                this.ownerAnt = owner;
            }
        }

        public void SetOwnerNest(Nest nest) {
            this.ownerAnt = null;
            this.ownerNest = nest;
        }

    }
}
