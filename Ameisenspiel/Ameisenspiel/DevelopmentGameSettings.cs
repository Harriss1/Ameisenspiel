using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Zentraler Ort um globale Spielparameter als Developer zu ändern.

namespace Ameisenspiel {
    internal class DevelopmentGameSettings {
        //Hier wird zentral festgelegt, ob wir die Development Settings nutzen
        private static bool settingsActive=true;

        public DevelopmentGameSettings() {
            if (!settingsActive) {
                autorun = stdAutorun;
            }
        }
        
        //Autorun
        private static bool autorun=false;

        //Console Width+Height
        private static int windowWidth = 80;
        private static int windowHeight = 25;


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

        public static int GetWindowWidth() {
            return windowWidth;
        }

        public static int GetWindowHeight() {
            return windowHeight;
        }
    }
}
