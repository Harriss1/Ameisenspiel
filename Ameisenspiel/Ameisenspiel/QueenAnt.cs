using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class QueenAnt : Ant{

        public QueenAnt(int xPos, int yPos) : base(xPos, yPos) {
            this.isAlive = true;
            this.entitySymbol = "Q";
            this.x = xPos;
            this.y = yPos;
            this.hiveCoordinateX = xPos;
            this.hiveCoordinateY = yPos;
            this.antType = AntType.Queen;
            this.canMoveOnItsOwn = false;
            this.energy = GetRandomLevel(500, 5000);
            this.maxAge = GetRandomLevel(400000, 600000);
            this.speed = 0;
        }
    }
}
