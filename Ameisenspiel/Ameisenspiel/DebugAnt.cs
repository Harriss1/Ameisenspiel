using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ameisenspiel {
    internal class DebugAnt : WorkerAnt {
        int id;
        static int idCount=0;
        public DebugAnt(int xPos, int yPos, World world) : base(xPos, yPos, world) {
            this.isAlive = true;
            //use another "a" symbol for carrying items, from this website:
            //https://www.rapidtables.com/code/text/unicode-characters.html
            /// â = \u00E2
            /// ã = \u00E3
            /// ä = \u00E4
            /// à = \u00E0
            /// å = \u00E5
            /// æ = \u00E6
            /// ¤ = \u00A4
            this.entitySymbol = "æ";
            id = DebugAnt.idCount;
            DebugAnt.idCount++;
        }


        public override void MoveOneIntelligent() {
            MoveOneToScreen(id);
        }

        /// <summary>
        /// Finde den Bildschirmrand
        /// </summary>
        /// <param name="direction">0=up, 1=down, 2=right, 3=left</param>
        public void MoveOneToScreen(int direction = 0) {
            if (direction == 0) {
                MoveOneUp();
            }
            if (direction == 1) {
                MoveOneDown();
            }
            if (direction == 2) {
                MoveOneRight();
            }
            if (direction == 3) {
                MoveOneLeft();
            }
        }
    }
}
