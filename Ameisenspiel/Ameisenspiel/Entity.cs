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
        protected int age; //this will make the game crash if it runs too long, but only if we run it for longer than 58 billion years
        //protected int facingAngleDirection;
        //protected CoordinateDirection faceCoordinateDirection; //direction by x-y coordinate
        protected bool isDestroyable;
        protected bool updated;
        protected bool canMoveOnItsOwn;
        protected String entitySymbol;
        protected Color entityColor;

        public enum Color {
            Standard = 0,
            Red = 1,
            Green = 2,
            Blue = 3,
            Yellow = 4,
            DarkRed = 5,
            Cyan = 6,
            Magenta = 7,
            DarkMagenta = 8,
            White = 9,
            DarkYellow = 10,
            DarkGray = 11,
        }

        protected static Random rand = new Random();
        public Entity(int xPos, int yPos) { //we can only directly set x+y via constructor, not during game simulation
            this.x = xPos;
            this.oldX = xPos;
            this.y = yPos;
            this.oldY = yPos;
            this.updated = true;
            this.isDestroyable = false;
            this.canMoveOnItsOwn = false;
            this.age = 0;
            this.entityColor = Color.Standard;
            SetEntitySymbol(" ");
            //this.SetFacingAngleDirection(random.Next(1, 360));
        }
        

        private void SetPosition(int x, int y) {
            oldY = this.y;
            oldX = this.x;
            this.x = x;
            this.y = y;
        }
        
        virtual public void PassOneCycle() {
            //override this class inside inherited classes to make energy depleting happen or anything.
            //implement possible Entity relevant methods here
            IncreaseAge();
        }
        protected void IncreaseAge() {
            age++;
        }

        public void MoveOneLeft() {
            oldX = this.x;
            if (this.x != 1 && this.canMoveOnItsOwn) {
                this.x--;
            }
        }

        public void MoveOneRight() {
            oldX = this.x;
            if (this.y != World.GetWorldWidth() && this.canMoveOnItsOwn) {
                this.x++;
            }
        }

        public void MoveOneDown() {
            oldY = this.y;
            if (this.y != World.GetWorldHeight() && this.canMoveOnItsOwn) {
                this.y++;
            }
        }
        public void MoveOneUp() {
            oldY = this.y;
            if (this.y != 1 && this.canMoveOnItsOwn) {
                this.y--;
            }
        }

        protected void SetEmpty() {
            this.isDestroyable = true;
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

        virtual public void MoveOneIntelligent() {
            //static Entities dont move
        }
        public void SetDestroyable() {
            this.isDestroyable = true;
        }
        public bool GetDestroyable() {
            return this.isDestroyable;
        }

        public Color GetColor() {
            return this.entityColor;
        }
    }
}
