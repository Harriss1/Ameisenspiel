using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Ant : Entity {
        protected bool isAlive;
        public Ant(int xPos, int yPos) : base(xPos, yPos){
            this.isAlive = true;
            this.entitySymbol = "@";
            this.x = xPos;
            this.y = yPos;
        }

        
    }
}
