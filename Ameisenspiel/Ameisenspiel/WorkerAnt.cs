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
            carriedFood = null;
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
                /* 1) Check if we already carry food
                 *      -> go back to hive for unloading in this case
                 * 2) Check if we find food on the map and bind to it
                 * 3) If the Ant reached the food it has targetted it forget its pathing target and collects the food
                 */
                if (carriedFood != null ) {
                    MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
                    carriedFood.CarryByAnt(this);
                }
                else {
                    if (this.GetPathFindTarget() == null) {
                        //Seiteneffekt beachten: WorkerAnt bekommt ein Ziel gesetzt!
                        Food food = (Food)SearchClosestUnownedFood();
                        if ( food == null) {
                            this.MoveOneRandom();
                        } else {
                            this.AssoziatePathFindTowards(food);
                        }
                    }
                    else {
                        if (PathFindDestinationReached()) {

                            carriedFood = (Food)GetPathFindTarget();

                        }
                        else {
                            MoveOneTowardsFood();
                        }
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

            if(this.carriedFood != null) {
                return;
            }

            if (this.GetPathFindTarget() == null) {
                log.AddWarning("MoveOneTowardsFood(): Not possibe - no pathfind target.");
                return;
            }
            
            MoveOneTowards(
                            GetPathFindTarget().GetX(), GetPathFindTarget().GetY()
                            );
            return; 
        }

        private Entity SearchClosestUnownedFood() {
            //Find a suitable target
            Entity closestFood = null;
            double smallestDistance = 100000;

            foreach (Entity foodItem in world.GetFood()) {
                Food food = foodItem as Food;
                if (
                    food.GetStoredInNest() == null            //the food must not already be stored inside a nest
                    && food.GetPathFindFollower() == null     //the food must not be already targeted by another ant
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

            return closestFood;
        }

        override protected void HiveVisit() {
            base.HiveVisit();
            UnloadFood();
        }

        protected void UnloadFood() {
            if (this.carriedFood != null) {

                //The food follows the Ant by one cycle difference
                if (carriedFood.GetX() == hiveCoordinateX && carriedFood.GetY() == hiveCoordinateY) {


                    //TODO refractor if we every make hives destroyable or have multiple
                    Nest nest = (Nest)world.GetWorldNests().First();
                    
                    //Make the food belong to the hive now
                    nest.AddFood(carriedFood);

                    this.carriedFood = null;
                    this.DisassoziatePathFindBindings();
                }
            }
        }
    }
}
