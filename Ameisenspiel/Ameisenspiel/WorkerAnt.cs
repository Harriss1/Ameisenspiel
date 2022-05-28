using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class WorkerAnt : Ant {
        protected int carryStrength;
        Log log = new Log("WorkerAnt.cs");

        protected Food carriedFood;

        //Workerant requires a reference to the simulated world to find Food.
        World world;
        public WorkerAnt(int xPos, int yPos, Nest nest, World world) : base(xPos, yPos, nest) {
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
            this.antType = AntType.Worker;
            this.speed = 80;
            this.carryStrength = 2; //can carry 2 food
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
                return;
            }
            else {
                if (/*ownedFood.count() > carryStrength ||*/ carryLink != null) {
                    MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
                }
                else {
                    if (this.GetPathFindTarget() == null) {
                        //Seiteneffekt beachten: WorkerAnt bekommt ein Ziel gesetzt!
                        if (FindAndBindToClosestFood() == null) {
                            this.MoveOneRandom();
                        }
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
            if(this.GetCarryLink() != null || this.GetPathFindTarget() != null) {
                
                if (this.GetCarryLink() == null) {
                    
                    if (GetPathFindTarget().GetX() == this.GetX()
                        && GetPathFindTarget().GetY() == this.GetY()) {

                        MakeChainBetweenCarrier(GetPathFindTarget());
                        GetPathFindTarget().MakeChainBetweenCarrier(this);

                    } else {

                        MoveOneTowards(
                            GetPathFindTarget().GetX(), GetPathFindTarget().GetY()
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
                    && foodItem.GetPathFindTarget() == null  //the food is already targetet by someone
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
                this.AssoziatePathFindTowards(closestFood);
                closestFood.AssoziatePathFindTowards(this);
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
                this.UnsetCarryLink();
                //TODO refractor if we every make hives destroyable or have multiple
                Nest nest = (Nest)world.GetWorldNests().First();
                if (nest != null) {
                    //Make the food belong to the hive now
                    nest.AddFood( (Food)GetPathFindTarget() );
                }
                this.DisassoziatePathFindBindings();
            }
        }
    }
}
