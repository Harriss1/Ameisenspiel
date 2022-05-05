using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Ant : Entity {
        protected bool isAlive;
        protected AntType antType;
        protected int energy;
        protected int hiveCoordinateX;
        protected int hiveCoordinateY;
        protected int maxAge;
        Log log = new Log("Ant.cs");
        public enum AntType {
            Standard = 0,
            Queen = 1,
            Worker = 2,
        }
        public Ant(int xPos, int yPos) : base(xPos, yPos){
            this.isAlive = true;
            this.entitySymbol = "@";
            this.x = xPos;
            this.y = yPos;
            this.hiveCoordinateX = xPos;
            this.hiveCoordinateY = yPos;
            this.antType = AntType.Standard;
            this.canMoveOnItsOwn = true;
            this.energy = GetRandomLevel(500, 5000);
            this.maxAge = GetRandomLevel(4000, 6000);
            this.speed = 40;
        }

        protected int GetRandomLevel(int minLevel, int maxLevel) {
            return rand.Next(minLevel, maxLevel);
        }

        virtual protected void HiveVisit() {
            ReplenishEnergy();
        }
        protected void ReplenishEnergy() {
            this.energy = GetRandomLevel(500, 5000);
            entityColor = Color.Green;
        }

        override public void PassOneCycle() {
            IncreaseAge();
            if (this.antType != AntType.Queen) {
                this.energy -= 1;
                if (this.age > this.maxAge) {
                    this.SetDestroyable();
                }
            }
            if (GetX() == hiveCoordinateX && GetY() == hiveCoordinateY) {
                HiveVisit();
            }
            if(energy < 1000 && energy >= 500) {
                entityColor = Color.DarkYellow;
            }
            if(energy < 500) {
                entityColor = Color.Red;
            }
        }

        public override void MoveOneIntelligent() {
            if(this.speedDistanceRemaining > 0) {
                this.speedDistanceRemaining -= this.speed;
                return;
            } else {
                this.speedDistanceRemaining = speedStartDistance;
            }

            if(energy < 500) {
                MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
                
            } else {
                MoveOneRandom();
            }
        }

        protected void MoveOneTowards(int destinationX, int destinationY) {

            //the ant simulates a shortest route with this algorithm
            int diffX = destinationX - this.x;
            int diffY = destinationY - this.y;
            if(Math.Abs(diffX) > Math.Abs(diffY)) {
                if(diffX > 0) {
                    this.MoveOneRight();
                } 
                if(diffX < 0){
                    this.MoveOneLeft();
                }
            } else {
                if(diffY > 0) {
                    this.MoveOneDown();
                } 
                if(diffY < 0){
                    this.MoveOneUp();
                }
            }
        }
    }
}
