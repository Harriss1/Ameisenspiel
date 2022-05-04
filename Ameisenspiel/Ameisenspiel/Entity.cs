using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Ameisenspiel {
    internal class Entity {
        //Wie gebe ich Entity beim Deklarieren eines Objekts auch die Art der Entity (Hinderniss/Ameise/Headquarter) mit?
        //->erstmal gar nicht...
        protected int x;
        protected int y;
        protected int oldX;
        protected int oldY;
        //protected int facingAngleDirection;
        //protected CoordinateDirection faceCoordinateDirection; //direction by x-y coordinate
        protected bool isEmpty;
        protected bool updated;
        protected String entitySymbol;

        protected static Random rand = new Random();
        public Entity(int xPos, int yPos) { //we can only directly set x+y via constructor, not during game simulation
            this.x = xPos;
            this.oldX = xPos;
            this.y = yPos;
            this.oldY = yPos;
            this.updated = true;
            this.isEmpty = false;
            SetEntitySymbol(" ");
            //this.SetFacingAngleDirection(random.Next(1, 360));
        }
        

        private void SetPosition(int x, int y) {
            oldY = this.y;
            oldX = this.x;
            this.x = x;
            this.y = y;
        }

        public void MoveOneLeft() {
            oldX = this.x;
            if (this.x != 1) {
                this.x--;
            }
        }


        public void MoveOneRight() {
            oldX = this.x;
            if (this.y != World.GetWorldWidth()) {
                this.x++;
            }
        }

        public void MoveOneDown() {
            oldY = this.y;
            if (this.y != World.GetWorldHeight()) {
                this.y++;
            }
        }
        public void MoveOneUp() {
            oldY = this.y;
            if (this.y != 1) {
                this.y--;
            }
        }

        protected void SetEmpty() {
            this.isEmpty = true;
        }
        public int GetOldX() {
            return this.oldX;
        }
        public int GetOldY() {
            return this.oldY;
        }

        public String GetEntitySymbol() {
            return this.entitySymbol;
        }

        protected void SetEntitySymbol(String entitySymbol) {
            this.entitySymbol = entitySymbol;
        }

        private void SetPosX(int x) {
            oldX = this.x;
            this.x = x;
        }
        public int GetX() {
            return x;
        }
        private void SetPosY(int y) {
            oldY = this.y;
            this.y = y;
        }
        public int GetY() {
            return y;
        }
        public void SetUpdated(bool updated) {
            this.updated = updated;
        }
        public bool GetUpdated() {
            return this.updated;
        }
        public void MoveOneRandom() {
            this.SetUpdated(true);
            int selectRandom = rand.Next(1, 5);
            switch (selectRandom) {
                case 1:
                    this.MoveOneRight();
                    break;
                case 2:
                    this.MoveOneLeft();
                    break;
                case 3:
                    this.MoveOneUp();
                    break;
                case 4:
                    this.MoveOneDown();
                    break;
                case 5:
                    //dont move
                    break;
                default:
                    //report bug
                    break;

            }
        }
    }
}
