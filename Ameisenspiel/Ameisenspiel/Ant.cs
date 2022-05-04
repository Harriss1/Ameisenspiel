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
        private int hiveCoordinateX;
        private int hiveCoordinateY;
        private int maxAge;
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
            this.energy = GetRandomLevel(50, 500);
            this.maxAge = GetRandomLevel(400, 600);
        }

        public void SetQueen() {
            this.antType = AntType.Queen;
            this.canMoveOnItsOwn = false;
            this.maxAge = GetRandomLevel(40000, 60000);
        }

        private int GetRandomLevel(int minLevel, int maxLevel) {
            return rand.Next(minLevel, maxLevel);
        }

        private void ReplenishEnergy() {
            this.energy = GetRandomLevel(50, 500);
        }

        override public void PassOneCycle() {
            IncreaseAge();
            if (this.antType != AntType.Queen) {
                this.energy--;
                if (this.age > this.maxAge) {
                    this.SetDestroyable();
                }
            }
            if (GetX() == hiveCoordinateX && GetY() == hiveCoordinateY) {
                ReplenishEnergy();
                entityColor = Color.Green;
            }
            if(energy < 100 && energy >= 50) {
                entityColor = Color.DarkYellow;
            }
            if(energy < 50) {
                entityColor = Color.Red;
            }
        }

        public override void MoveOneIntelligent() {
            if(energy < 50) {
                MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
            } else {
                MoveOneRandom();
            }
        }

        private void MoveOneTowards(int destinationX, int destinationY) {

            //the ant simulates a shortest route with this algorithm
            int diffX = destinationX - this.x;
            int diffY = destinationY - this.y;
            if(Math.Abs(diffX) > Math.Abs(diffY)) {
                if(diffX > 0) {
                    this.MoveOneRight();
                } else {
                    this.MoveOneLeft();
                }
            } else {
                if(diffY > 0) {
                    this.MoveOneDown();
                } else {
                    this.MoveOneUp();
                }
            }
        }
    }
}
