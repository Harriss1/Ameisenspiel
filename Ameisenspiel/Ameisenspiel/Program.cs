using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Autor: Karl Klotz
//Datum: 29.03.2022
//Inhalt: Implementierung eines 2D Ameisenspiels nach Unterrichtsaufgabe vom 28.03.2022
//Ziele: Erlernen der Grundlagen der Objektorientierung (Vererbung, Abstraktion, Polymorphismus, Kapselung)
// - Im Verlauf der Implementierung sollen Interfaces umgesetzt werden
//Vereinfachter Aufbau:
// - Es gibt die Klasse "Entity" und die Klasse "World", und alle Spielobjekte werden aus
//   vererbten Klassen erzeugt
// - Nach Möglichkeit möchte ich die Klassen so aufbauen, dass zwei weitere Klassen dazu kommen können:
//          1) Speichern des Spielstands (ggf. Manipulation des Spiels von externer Schnittstelle aus)
//          2) Manipulation der Ameisen
//Die erste Implementierung wird keine komplizierteren Aufbauten haben in Hinsicht auf Safefile/Schnittstellen/History
//Es wird lediglich erstmal eine Entität, eine Ameise und eine Welt implementiert

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

                    //Lange Simulation, 20000 Zyklen (c.a. 2 Minuten)
                    case 2:
                        RunGame(Configuration.GameSettings.Mode.Double);
                        break;

                    //5 Ameisen
                    case 3:
                        RunGame(Configuration.GameSettings.Mode.Demo);
                        break;

                    //200 Ameisen, 999998 Zyklen
                    case 4:
                        RunGame(Configuration.GameSettings.Mode.Stress);
                        break;

                    case 5:
                        RunGame(Configuration.GameSettings.Mode.Bigworld);
                        break;
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
        }

        private static int SelectFromMainMenu() {
            Configuration config = new Configuration();
            Console.WriteLine();
            Console.WriteLine("Bitte drücken Sie eine Zahl zur Auswahl:");
            Console.WriteLine("(1) " + config.standard.title + ": " + config.standard.description + " (5000 entsprechen ca. 1 Minute).");
            Console.WriteLine("(2) " + config.doubleLength.title + ": " + config.doubleLength.description);
            Console.WriteLine("(3) " + config.demo.title + ": " + config.demo.description);
            Console.WriteLine("(4) " + config.stress.title + ": " + config.stress.description);
            Console.WriteLine("(5) " + config.bigworld.title + ": " + config.bigworld.description);
            Console.WriteLine("(0) Programm beenden");
            Console.WriteLine();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key) {

                //Durchfallen zum nächsten break; um Code-Doppelung zu vermeiden wäre möglich, diesmal nicht.
                //da return das break ersetzt braucht es hier keinen break-befehl

                //bestimmten Tag auslesen
                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.NumPad1:
                    return 1;

                //Jahresdurchschnitt
                case ConsoleKey.D2:
                    return 2;
                case ConsoleKey.NumPad2:
                    return 2;

                //Tag mit höchsten Niederschlag im Jahr
                case ConsoleKey.D3:
                    return 3;
                case ConsoleKey.NumPad3:
                    return 3;

                //Regenfall.txt generieren
                case ConsoleKey.D4:
                    return 4;
                case ConsoleKey.NumPad4:
                    return 4;

                //Regenfall.txt generieren
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
            //return -1;
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
