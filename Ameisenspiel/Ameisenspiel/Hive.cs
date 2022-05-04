using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class Hive : Entity {
        public Hive(int xPos, int yPos) : base(xPos, yPos) {
            this.entitySymbol = "H";
            this.x = xPos;
            this.y = yPos;
        }
    }
}
