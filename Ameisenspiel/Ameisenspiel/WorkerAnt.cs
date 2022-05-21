using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class WorkerAnt : Ant {
        protected int carryStrength;
        protected int carryCount;
        Log log = new Log("WorkerAnt.cs");

        //Workerant requires a reference to the simulated world to find Food.
        World world;
        public WorkerAnt(int xPos, int yPos, World world) : base(xPos, yPos) {
            this.isAlive = true;
            this.world = world;
            //use another "a" symbol for carrying items, from this website:
            //https://www.rapidtables.com/code/text/unicode-characters.html
            /// â = \u00E2
            /// ã = \u00E3
            /// ä = \u00E4
            /// à = \u00E0
            /// å = \u00E5
            /// æ = \u00E6
            /// ¤ = \u00A4
            this.entitySymbol = "a";
            this.x = xPos;
            this.y = yPos;
            this.hiveCoordinateX = xPos;
            this.hiveCoordinateY = yPos;
            this.antType = AntType.Worker;
            this.canMoveOnItsOwn = true;
            this.energy = GetRandomInteger(500, 5000);
            this.maxAge = GetRandomInteger(4000, 6000);
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
                if (carryCount >= carryStrength || carryLink != null) {
                    MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
                }
                else {
                    if (this.GetTargetOrSeeker() == null) {
                        this.FindAndBindToClosestFood();
                    }
                    else {
                        MoveOneTowardsFood();
                    }
                }
            }
        }


        /// <summary>
        /// Vorüberlegung:
        /// [ ]- Natürlich sollte Nahrung welche nah (7 Punkte) am Hügel spawnt zielstrebig aufgesammelt werden
        /// [ ] - Weiter entfernte Nahrung sollte jedoch nur zufällig angesteuert werden.
        /// [X] - Eine Ameise sollte nur eine Nahrung ansteuern (target-follower?)
        /// [ ] - Eine Ameise sollte mehr als ein Entity tragen können?
        ///         -> Keine Liste, nur "Itemslots" was feste Attribute bedeutet, z.B. CarrySlot4 etc.
        ///
        /// </summary>
        private void MoveOneTowardsFood() {
            if(this.GetCarryLink() != null || this.GetTargetOrSeeker() != null) {
                
                if (this.GetCarryLink() == null) {
                    
                    if (GetTargetOrSeeker().GetX() == this.GetX()
                        && GetTargetOrSeeker().GetY() == this.GetY()) {

                        MakeChainBetweenCarrier(GetTargetOrSeeker());
                        GetTargetOrSeeker().MakeChainBetweenCarrier(this);

                    } else {

                        MoveOneTowards(
                            GetTargetOrSeeker().GetX(), GetTargetOrSeeker().GetY()
                            );
                    }
                }
                return;
            }

            
        }

        private Entity FindAndBindToClosestFood() {
            //Find a suitable target
            Entity closestFood = null;
            double smallestDistance = 100000;

            foreach (Entity foodItem in world.GetFood()) {
                if (
                    foodItem.GetCarryLink() == null          //the food already is carried by someone
                    && foodItem.GetTargetOrSeeker() == null  //the food is already targetet by someone
                    ) {
                    int xDiff = foodItem.GetX() - this.GetX();
                    int yDiff = foodItem.GetY() - this.GetY();
                    double distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                    if (distance < smallestDistance) {
                        smallestDistance = distance;
                        closestFood = foodItem;
                    }
                }
            }
            
            if (closestFood != null) {
                //log.Add("WorkerAnt id(" + this.GetEntityId() + ") found a target id(" + closestFood.GetEntityId() + ") and should follow it now.");
                this.SetTargetOrOwner(closestFood);
                closestFood.SetTargetOrOwner(this);
            }

            return closestFood;
        }

        override protected void HiveVisit() {
            base.HiveVisit();
            if (this.carryLink != null) {
                UnloadFood();
            }
        }

        protected void UnloadFood() {
            //The food follows the Ant by one cycle difference
            if (carryLink.GetX() == hiveCoordinateX && carryLink.GetY() == hiveCoordinateY) {
                this.carryCount = 0;
                this.UnsetCarryLink();
                //TODO refractor if we every make hives destroyable or have multiple
                Hive hive = (Hive)world.GetWorldHives().First();
                if (hive != null) {
                    //Make the food belong to the hive now
                    hive.AddFood((Food)GetTargetOrSeeker());
                }
                this.UnsetTarget();
            }
        }
    }
}
