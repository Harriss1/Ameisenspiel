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
        private static int stdWorldWidth = 85;
        private static int stdWorldHeight = 25;
        private static int stdCycles = 5000; // = 1.05 Minuten > entspricht 50 Sekunden plus Rechenzeit 15 Sekunden (core i7)
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
        private static GameSettings infiniteStandard;
        private static GameSettings demo;
        private static GameSettings stress;
        private static GameSettings bigworld;

        private static void SetSettings() {
            //Default Mode
            standard = new GameSettings(true);

            //Infinite Duration
            infiniteStandard = new GameSettings(true);
            infiniteStandard.cycles = -1;
            infiniteStandard.antCount = (int)((double)infiniteStandard.antCount * 1.0);
            infiniteStandard.title = "Unendlich";
            infiniteStandard.description = infiniteStandard.MakeDescription();

            //Demonstrate a few ants
            demo = new GameSettings(true);
            demo.cycles = 2000;
            demo.antCount = 10;
            demo.title = "Kleine Demo";
            demo.description = demo.MakeDescription();

            //Stresstest
            stress = new GameSettings(true);
            stress.cycles = 9999999; //entspricht 10 Stunden
            stress.antCount = 400;
            stress.title = "Stresstest";
            stress.description = stress.MakeDescription();

            //Resize Screen
            bigworld = new GameSettings(true);
            bigworld.worldWidth = 110;
            bigworld.worldHeight = 40;
            bigworld.cycles *= 2;
            bigworld.antCount *= 2;
            bigworld.title = "Größerer Bildschirm und verdoppelte Simulation";
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
                    return infiniteStandard;
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
