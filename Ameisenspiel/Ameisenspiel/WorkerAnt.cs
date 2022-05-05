using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class WorkerAnt : Ant {
        private int carryStrength;
        private int carryCount;
        Log log = new Log("WorkerAnt.cs");
        public WorkerAnt(int xPos, int yPos) : base(xPos, yPos) {
            this.isAlive = true;
            this.entitySymbol = "a";
            this.x = xPos;
            this.y = yPos;
            this.hiveCoordinateX = xPos;
            this.hiveCoordinateY = yPos;
            this.antType = AntType.Worker;
            this.canMoveOnItsOwn = true;
            this.energy = GetRandomLevel(500, 5000);
            this.maxAge = GetRandomLevel(4000, 6000);
            this.speed = 80;
            this.carryStrength = 2; //can carry 2 food
            this.carryCount = 0;
        }

        public override void MoveOneIntelligent() {
            if (this.speedDistanceRemaining > 0) {
                this.speedDistanceRemaining -= this.speed;
                return;
            }
            else {
                this.speedDistanceRemaining = speedStartDistance;
            }

            if (energy < 500) {
                MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);

            }
            else {
                if (carryCount >= carryStrength) {
                    MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
                }
                else {
                    MoveOneRandom();
                    //MoveOneTowardsFood();
                }
            }
        }

        private void MoveOneTowardsFood() {

        }

        override protected void HiveVisit() {
            base.HiveVisit();
            UnloadFood();
        }

        protected void UnloadFood() {
            this.carryCount = 0;
        }
    }
}
