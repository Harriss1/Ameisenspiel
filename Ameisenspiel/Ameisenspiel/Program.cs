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
            

            if (DevelopmentGameSettings.GetAutorun()) {
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
                        RunGame(2);
                        break;

                    //5 Ameisen
                    case 3:
                        RunGame(3);
                        break;

                    //200 Ameisen, 999998 Zyklen
                    case 4:
                        RunGame(4);
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
            Console.WriteLine();
            Console.WriteLine("Bitte drücken Sie eine Zahl zur Auswahl:");
            Console.WriteLine("(1) Standard Spiel - 100 Ameisen mit 15000 Zyklen = 150 Sekunden");
            Console.WriteLine("(2) Längere Simulation - 150 Ameisen, 25000 Zyklen (c.a. 4 Minuten)");
            Console.WriteLine("(3) 5 Ameisen, 100 Zyklen");
            Console.WriteLine("(4) 200 Ameisen, 999.998 Zyklen");
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

        private static void RunGame(int gameMode = 1) {
            Game game = new Game();
            
            Game.Settings settings = new Game.Settings();
            if(gameMode == 1) {
                //Standard Settings siehe Game.cs
            }
            if (gameMode == 2) {
                //Lange Simulation, 2.000 Zyklen (c.a. 2 Minuten)
                settings.cycles = 10000;
                settings.antCount = 150;
            }
            if (gameMode == 3) {
                //5 Ameisen
                settings.antCount = 5;
                settings.cycles = 2000;
            }
            if (gameMode == 4) {
                //200 Ameisen, 999998 Zyklen
                settings.antCount = 200;
                settings.cycles = 999998;
            }

            game.ChangeSettings(settings);
            game.RunGame();
        }
    }
}
