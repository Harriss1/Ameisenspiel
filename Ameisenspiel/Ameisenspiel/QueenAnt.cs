using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class QueenAnt : Ant{

        World world;
        Log log = new Log("QueenAnt.cs");
        int eggAbility = 0;

        public QueenAnt(int xPos, int yPos, Nest nest, World world) : base(xPos, yPos, nest) {
            this.world = world;
            this.isAlive = true;
            this.entitySymbol = "Q";
            this.x = xPos;
            this.y = yPos;
            this.hiveCoordinateX = xPos;
            this.hiveCoordinateY = yPos;
            this.antType = AntType.Queen;
            this.canMoveOnItsOwn = false;
            this.energy = GetRandomInteger(500, 5000);
            this.maxAge = GetRandomInteger(400000, 600000);
            this.speed = 0;
        }

        public override void PassOneCycle() {
            base.PassOneCycle();
            if (eggAbility < 50) {
                Eat();
            }
            Reproduce();
        }

        protected void Reproduce() {
            //are there enough Ants in the colony?
            //We only lay a new Egg, if we dont already have enough Eggs+Ants
            double maxAntQualifier = antNest.GetTargetMaxAnts() * 1.5;
            double minAntQualifier = (double)(antNest.GetOwnedAntCount() + antNest.GetOwnedAntEggCount());
            int eggCount = antNest.GetOwnedAntEggCount();
            if ( minAntQualifier < maxAntQualifier && eggAbility > 0) {
                //log.Add("We start reproduction. MinQuali=" + minAntQualifier + " maxAntQuali=" + maxAntQualifier + " eggCount=" + eggCount);
                AntEgg egg = new AntEgg(GetX(), GetY(), antNest, world);
                GetNest().AddOwnedEgg(egg);
                world.AddEntityToQueue(egg);
                eggAbility--;
            }
        }

        protected void Eat() {
            Food food = antNest.EatFood();
            if(food != null) {
                eggAbility += 3;
                food.SetDestroyable();
            }
        }
    }
}
