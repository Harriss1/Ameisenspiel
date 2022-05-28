using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class AntEgg : Ant {
        Log log = new Log("AntEgg.cs");
        World world;
        int standardHatchTime = 2000;
        int hatchDelay;
        int hatchTime;
        public AntEgg(int xPos, int yPos, Nest nest, World world) : base(xPos, yPos, nest) {
            canMoveOnItsOwn = false;
            entitySymbol = ".";
            speed = 0;
            this.world = world;
            hatchDelay = GetRandomInteger(-250, 250);
            hatchTime = standardHatchTime + hatchDelay;
            //log.Add("Laying an Egg!!!");
        }

        public override void PassOneCycle() {
            base.PassOneCycle();

            if(age > hatchTime && !GetDestroyable()) {
                Hatch();
            }
        }

        private void Hatch() {
            //With an 20% chance we hatch a Worker Ant
            int probability = GetRandomInteger(1, 100);
            if (probability >= 80) {
                WorkerAnt ant = new WorkerAnt(GetX(), GetY(), antNest, world);
                world.AddEntityToQueue(ant);
                //log.Add("hatched a new Worker for the Ant God");
            }
            else {
                Ant ant = new Ant(GetX(), GetY(), antNest);
                world.AddEntityToQueue(ant);
                //log.Add("hatched a new citizen for the Ant God");
            }
            antNest.RemoveEgg(this);
            this.SetDestroyable();
            
        }
    }
}
