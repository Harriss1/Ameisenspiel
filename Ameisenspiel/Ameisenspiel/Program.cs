using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Autor: Karl Klotz
//Datum: 29. März 2022 bis 13. Mai 2022
//Abgabeversion für SDM, bisher ist noch kein Futterspawning und Nahrungshaushalt (Nest besitzt Futter o.ä.) umgesetzt.
//Inhalt: Implementierung eines 2D Ameisenspiels nach Unterrichtsaufgabe vom 28.03.2022
//Ziele: Erlernen der Grundlagen der Objektorientierung (Vererbung, Abstraktion, Polymorphismus, Kapselung)
//Vereinfachter Aufbau:
// - Es gibt die Klasse "Entity" und die Klasse "World", und alle Spielobjekte werden aus
//   vererbten Klassen erzeugt: Die Welt wird mit Ameisen gefüllt.
// - Das "Füllen" des World-Objekts wird mittels der Game-Klasse erledigt, welche auch die Welt ausließt und grafisch darstellt.
//Besonderheit: Es gibt normale Ameisen "@" und Arbeiterameisen "a", Arbeiterameisen sind etwas schneller als normale Ameisen.

namespace Ameisenspiel {
    internal class Program {
        static void Main(string[] args) {
            
            if (Configuration.GetAutorun()) {
                DevelopmentAutorun();
            }

            //Hauptprogrammschleife
            int eventSelection = -1;
            while (eventSelection != 0) {

                Console.Clear();

                //Programm-Begrüßung, Programmziel für User erklären
                ShowMainGreeting();
                
                //Anzeige einer Menüauswahl, Zugriff durch Ziffern, inkl. Beenden bei "0"
                eventSelection = SelectFromMainMenu();

                switch (eventSelection) {

                    //Standard Ameisenspiel
                    case 1:
                        RunGame();
                        break;
                    //Weitere Optionen
                    case 2:
                        RunGame(Configuration.GameSettings.Mode.Double);
                        break;

                    case 3:
                        RunGame(Configuration.GameSettings.Mode.Demo);
                        break;

                    case 4:
                        RunGame(Configuration.GameSettings.Mode.Stress);
                        break;

                    case 5:
                        RunGame(Configuration.GameSettings.Mode.Bigworld);
                        break;

                    //@to-do Individualparamter ermöglichen

                    //Programmende
                    case 0:
                        Console.WriteLine("Programmende.");
                        break;

                    //ungültige Auswahl/Fehleingabe
                    default:
                        Console.WriteLine("Keine gültige Auswahl, bitte erneut wählen.");
                        break;
                }
            }
        }

        static void ShowMainGreeting() {
            Console.WriteLine("~~~ ### ### ### ### ### ### ### ### ### ### ### ### ~~~");
            Console.WriteLine("~~~            Das Spiel der Ameisen                ~~~");
            Console.WriteLine("~~~ ### ### ### ### ### ### ### ### ### ### ### ### ~~~");
            Console.WriteLine();
            Console.WriteLine("@ = normale Ameise; a = Arbeiterameise; H = Ameisen-Hügel; Q = Queen (fast nie sichtbar)");
            Console.WriteLine();
            Console.WriteLine("weiß = Hügel sofort nach Geburt verlassen; grün = Hügel mindestens einmal besucht und gegessen" +
                "\n orange = bald hungrig; rot = läuft zum Hügel um zu Essen");
            Console.WriteLine();
            Console.WriteLine("Ingame Hotkeys: [9] = Pause [0] = Ende");

        }

        private static int SelectFromMainMenu() {
            Console.WriteLine();
            Console.WriteLine("Bitte drücken Sie eine Zahl zur Auswahl:\n");

            Console.WriteLine("(1) " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Standard).title 
                + ": " + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Standard).description + " (5000 entsprechen ca. 1 Minute).");
            
            Console.WriteLine("(2) " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Double).title 
                + ": " + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Double).description);
            
            Console.WriteLine("(3) " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Demo).title 
                + ": " + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Demo).description);

            Console.WriteLine("(4) " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Stress).title 
                + ": " + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Stress).description);

            Console.WriteLine("(5) " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Bigworld).title + ": " 
                + Configuration.GetGameSettings(Configuration.GameSettings.Mode.Bigworld).description);

            Console.WriteLine("(0) Programm beenden");
            Console.WriteLine();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key) {
                //da return das break ersetzt braucht es hier keinen break-befehl

                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.NumPad1:
                    return 1;

                case ConsoleKey.D2:
                    return 2;
                case ConsoleKey.NumPad2:
                    return 2;

                case ConsoleKey.D3:
                    return 3;
                case ConsoleKey.NumPad3:
                    return 3;

                case ConsoleKey.D4:
                    return 4;
                case ConsoleKey.NumPad4:
                    return 4;

                case ConsoleKey.D5:
                    return 5;
                case ConsoleKey.NumPad5:
                    return 5;

                //Programmende
                case ConsoleKey.D0:
                    return 0;
                case ConsoleKey.NumPad0:
                    return 0;

                //default: keine Menüauswahl (falsche Eingabe) entspricht -1
                default:
                    return -1;
            }
            //return -1; redundant, weil im switch-Block alle Fälle behandelt wurden
        }

        private static void DevelopmentAutorun() {
            Console.WriteLine("Autorun aktiv. Starte Spiel in\n3...");
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("2...");
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("1...");

            //Aufzurufende Funktion
            RunGame();
        }

        private static void RunGame(Configuration.GameSettings.Mode gameMode = Configuration.GameSettings.Mode.Standard) {
            Game game = new Game();
            
            game.ChangeSettings(gameMode);
            game.RunGame();
        }
    }
}
