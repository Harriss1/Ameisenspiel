using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Ameisenspiel {
    internal class Entity {
        protected int x;
        protected int y;
        protected int age; //this will make the game crash if it runs too long, but only if we run it for longer than 58 billion years
        protected bool isDestroyable;
        protected bool canMoveOnItsOwn;
        protected String entitySymbol;
        protected Color entityColor;
        
        private Entity pathFindTarget;
        private Entity pathFindFollower;
        protected Entity carryLink;

        protected static Random rand = new Random();
        private static uint entityIdCounter = 0;
        private uint entityId;

        //speed calculation:
        //example: start=100 -> start distance=100, speed=10, every cycle we remove 10 speed from distance,
        //at distance=zero we move one square and reset distance to 100
        protected int speed;
        protected int speedStartDistance;
        protected int speedDistanceRemaining;  

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

        public Entity(int xPos, int yPos) { //we can only directly set x+y via constructor, not during game simulation
            this.x = xPos;
            this.y = yPos;
            entityIdCounter++;
            this.entityId = entityIdCounter;
            this.isDestroyable = false;
            this.canMoveOnItsOwn = false;
            this.age = 0;
            this.entityColor = Color.Standard;
            this.speed = 0;
            this.speedStartDistance = 1000; 
            this.speedDistanceRemaining = 0;
            SetEntitySymbol(" ");
            //rand = new Random(); GROSER FEHLER
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
            if (this.x != 1 && this.canMoveOnItsOwn) {
                this.x--;
            }
        }

        public void MoveOneRight() {
            if (this.x != World.GetWorldWidth() && this.canMoveOnItsOwn) {
                this.x++;
            }
        }

        public void MoveOneDown() {
            if (this.y != World.GetWorldHeight() && this.canMoveOnItsOwn) {
                this.y++;
            }
        }
        public void MoveOneUp() {
            if (this.y != 1 && this.canMoveOnItsOwn) {
                this.y--;
            }
        }

        public String GetEntitySymbol() {
            return this.entitySymbol;
        }

        protected void SetEntitySymbol(String entitySymbol) {
            this.entitySymbol = entitySymbol;
        }

        public int GetX() {
            return x;
        }
        public int GetY() {
            return y;
        }
        public void MoveOneRandom() {
            rand.Next();
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

        protected int GetRandomInteger(int min, int max) {

            return rand.Next(min, max);
        }
        virtual public void MoveOneIntelligent() {
            //static Entities dont move
        }
        public virtual void SetDestroyable() {
            this.isDestroyable = true;
        }
        public bool GetDestroyable() {
            return this.isDestroyable;
        }
        public Color GetColor() {
            return this.entityColor;
        }
        public void SetRandomPosition(int distanceFromHive = 0) {
            x = GetRandomInteger(1, World.GetWorldWidth());
            y = GetRandomInteger(1, World.GetWorldHeight());
        }

        public virtual void DestroyChainLinks() {
            DisassoziatePathFindBindings();
            UnsetCarryLink();
        }

        public void MakeChainBetweenCarrier(Entity carrierToChainTo, bool unsetOldCarrier=false) {
            if (carryLink == null || unsetOldCarrier) {
                this.carryLink = carrierToChainTo;
                carrierToChainTo.MakeChainBetweenCarrier(this);
            }
        }

        
        public Entity GetCarryLink() {
            return carryLink;
        }

        protected void UnsetCarryLink() {
            if (carryLink != null) {
                carryLink.carryLink=null;
                carryLink = null;
            }
        }
        /// <summary>
        /// Binds together target and follower Entity for pathfinding
        /// Example: WorkerAnt gets the target=Food, and the Food gets the follower=WorkerAnt
        /// Defaults to setting target.target=null, and this.follower=null
        /// </summary>
        /// <param name="target">Target Entity</param>
        /// <param name="targetEachOther">Use if booth shall walk at the same time towards each other.</param>
        public void AssoziatePathFindTowards(Entity target, bool targetEachOther=false) {

            this.pathFindTarget = target;
            target.pathFindFollower = this;


            if (targetEachOther) {
                target.pathFindTarget = this;

                this.pathFindFollower = target;
            }
            else {
                target.pathFindTarget = null;

                this.pathFindFollower = null;
            }
        }
        public Entity GetPathFindTarget() {
            return pathFindTarget;
        }

        public Entity GetPathFindFollower() {
            return pathFindFollower;
        }
        public void DisassoziatePathFindBindings() {
            pathFindTarget.pathFindTarget=null;
            pathFindTarget.pathFindFollower=null;
            this.pathFindFollower = null;
            this.pathFindTarget = null;
        }

        public uint GetEntityId() {
            return entityId;
        }
    }
}
