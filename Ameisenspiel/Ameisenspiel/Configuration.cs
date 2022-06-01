using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// //////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Zentrale Klasse um globale Spielparameter als Entwickler zu ändern.
/// </summary>
namespace Ameisenspiel {
    internal class Configuration {
        //Hier wird manuell festgelegt, ob wir die Development Settings nutzen
        private static bool devSettingsActive = true;
        private static bool debugActive = false;
        private static bool devAutorun = false; //wird nur genutzt falls devSettingsActive=true;

        public Configuration() {
            if (!devSettingsActive) {
                devAutorun = stdAutorun;
            }
            Configuration.SetSettings();
        }

        //Standard Game Settings
        private static int stdWorldWidth = 110;
        private static int stdWorldHeight = 40;
        private static int stdCycles = -1; // -1 = unendlich / 5000= 1.05 Minuten > entspricht 50 Sekunden plus Rechenzeit 15 Sekunden (core i7)
        private static int stdAntCount = 100;

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

            //Default = Standard Settings
            public GameSettings(bool useStandard) {
                this.type = Mode.Standard;
                this.cycles = stdCycles;
                this.antCount = stdAntCount;
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
        private static GameSettings standard;
        private static GameSettings infiniteMaxAnts;
        private static GameSettings demo;
        private static GameSettings stress;
        private static GameSettings bigworld;

        private static void SetSettings() {
            //Default Mode
            standard = new GameSettings(true);

            //Mehr Ameisen
            infiniteMaxAnts = new GameSettings(true);
            infiniteMaxAnts.antCount = (int)((double)infiniteMaxAnts.antCount * 3.0);
            infiniteMaxAnts.title = "Dreimal so viele";
            infiniteMaxAnts.description = infiniteMaxAnts.MakeDescription();

            //Demonstrate a few ants
            demo = new GameSettings(true);
            demo.cycles = 10000;
            demo.antCount = 20;
            demo.worldWidth = 85;
            demo.worldHeight = 25;
            demo.title = "2 Minuten Demo in Miniwelt";
            demo.description = demo.MakeDescription();

            //Stresstest
            stress = new GameSettings(true);
            stress.cycles = -1;
            stress.antCount = 600; //Bei 600 Ameisen verlangsamt sich die Rechenzeit deutlich.
            bigworld.worldWidth = 150;
            bigworld.worldHeight = 43;
            stress.title = "Stresstest";
            stress.description = stress.MakeDescription();

            //Resize Screen
            bigworld = new GameSettings(true);
            bigworld.worldWidth = 150;
            bigworld.worldHeight = 43;
            bigworld.cycles = -1;
            bigworld.antCount *= 3;
            bigworld.title = "Größere Welt, Dreimal so viele";
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
            return devAutorun;
        }

        public static bool GetDebugActive() {
            return debugActive;
        }

        public static int GetStdWorldWidth() {
            return stdWorldWidth;
        }

        public static int GetStdWorldHeight() {
            return stdWorldHeight;
        }

        public static int GetSdtCycles() {
            return stdCycles;
        }

        public static int GetStdAntCount() {
            return stdAntCount;
        }

        public static GameSettings GetGameSettings(GameSettings.Mode mode) {
            Configuration.SetSettings();
            switch (mode) {
                case Configuration.GameSettings.Mode.Standard:
                    return standard;
                case Configuration.GameSettings.Mode.Double:
                    return infiniteMaxAnts;
                case Configuration.GameSettings.Mode.Demo:
                    return demo;
                case Configuration.GameSettings.Mode.Stress:
                    return stress;
                case Configuration.GameSettings.Mode.Bigworld:
                    return bigworld;
                default: return standard;
            }
        }
    }
}
