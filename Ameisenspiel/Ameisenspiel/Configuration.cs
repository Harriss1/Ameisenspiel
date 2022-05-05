using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Zentraler Ort um globale Spielparameter als Developer zu ändern.

namespace Ameisenspiel {
    internal class Configuration {
        //Hier wird zentral festgelegt, ob wir die Development Settings nutzen
        private static bool settingsActive = true;

        public Configuration() {
            if (!settingsActive) {
                autorun = stdAutorun;
            }
            SetSettings();
        }

        //Autorun
        private static bool autorun = false;

        //Standard World Width+Height
        private static int stdWorldWidth = 85;
        private static int stdWorldHeight = 25;

        //Game Settings for each Mode
        public struct GameSettings {
            public enum Mode {
                None = 0,
                Standard = 1,
                Double = 2,
                Demo = 3,
                Stress = 4,
                Bigworld = 5,
            }
            public Mode type;
            public int cycles;
            public int antCount;
            public int worldWidth;
            public int worldHeight;
            public string description;
            public string title;

            //Default Settings
            public GameSettings(bool useStandard) {
                this.type = Mode.Standard;
                this.cycles = 5000; // = 1.05 Minuten > entspricht 50 Sekunden plus Rechenzeit 15 Sekunden
                this.antCount = 100;
                this.worldWidth = stdWorldWidth;
                this.worldHeight = stdWorldHeight;
                this.title = "Standard";
                this.description = antCount + " Ameisen, " + cycles + " Zyklen";
            }
            public String MakeDescription() {
                return antCount + " Ameisen, " + cycles + " Zyklen";
            }
        }

        //Modes
        public GameSettings standard;
        public GameSettings doubleLength;
        public GameSettings demo;
        public GameSettings stress;
        public GameSettings bigworld;

        private void SetSettings() {
            //Default Mode
            standard = new GameSettings(true);

            //Double Time Length
            doubleLength = new GameSettings(true);
            doubleLength.cycles *= 2;
            doubleLength.antCount = (int)((double)doubleLength.antCount * 1.5);
            doubleLength.title = "Doppelt soviel";
            doubleLength.description = doubleLength.MakeDescription();

            //Demonstrate a few ants
            demo = new GameSettings(true);
            demo.cycles = 2000;
            demo.antCount = 10;
            demo.title = "Kleine Demo";
            demo.description = demo.MakeDescription();

            //Stresstest
            stress = new GameSettings(true);
            stress.cycles = 9999999; //entspricht 10 Stunden
            stress.antCount = 200;
            stress.title = "Stresstest";
            stress.description = stress.MakeDescription();

            //Resize Screen
            bigworld = new GameSettings(true);
            bigworld.worldWidth = 100;
            bigworld.worldHeight = 30;
            bigworld.cycles *= 2;
            bigworld.antCount *= 2;
            bigworld.title = "Größerer Bildschirm und verdoppelte Simulation (fehlerhaft)";
            bigworld.description = bigworld.MakeDescription();


        }



        ////////////////////////////////////////////////////////
        //Standard Werte = Werte die genutzt werden falls Devmode=false
        // oder falls individuelle Werte nicht genutzt werden sollen
        // - werden nur implementiert falls notwendig
        private static bool stdAutorun = false;
        

        /////////////////////////////////////////////////////////////
        //GETTER (es darf keine Setter geben)
        public static bool GetAutorun() {
            return autorun;
        }

        public static int GetWorldWidth() {
            return stdWorldWidth;
        }

        public static int GetWorldHeight() {
            return stdWorldHeight;
        }
    }
}
