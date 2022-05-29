using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//////////////////////////////////////////////////////////////////////////
///<summary>
/// Die Spielsimmulation erfolgt durch diese Klasse. Verwantwortlich um das Objekt "World" zu instanziieren
/// World wird mit Entity-Objekten verschiedener Typen (Ant, Queen, Nest) gefüllt.
/// Jede Entität hat die Funktion "Bewegen" und "Zyklusfolgen berechnen (Alter und Energie verbrauchen)" welche im Main-Loop implementiert werden.
/// Das Objekt "Game" ließt die Entity-Objekte in World aus, und übergibt diese an die Grafikausgabefunktionen:
///     - UpdateDisplayContent (zum speichern des Anzeigeinhalts und Abgleich von Änderungen zum vorherigen Grafikausgabezyklus)
///     - DrawDisplayContent (verantwortlich für die Grafikausgabe)
/// </summary>

namespace Ameisenspiel {
    internal class Game {
        Log log = new Log("Game.cs");
        private int cyclesRemaining;
        private World world;
        private List<DisplayPoint> displayContents;
        private List<DisplayPoint> oldDisplayContents;
        private Settings settings;
        private int antsAlive;
        ConsoleKey lastUserInputKey;
        public Game() {
            settings.cyclesTotal = 1;
            displayContents = new List<DisplayPoint>();
            oldDisplayContents = new List<DisplayPoint>();
            SetDefaultSettings();
        }
        public struct Settings {
            public int worldWidth;
            public int worldHeight;
            public int cyclesTotal;
            public int antCount;
        }

        private class DisplayPoint
            {
            public int x;
            public int y;
            public string symbol;
            public ConsoleColor symbolColor;
            public bool awaitsDrawing;
            public DrawPower drawPower = DrawPower.Default;
            //Draw Power for object draw hierarchy within the 2D Grafic Engine
            public enum DrawPower {
                Default = 0,
                Minus1 = -1,
                Minus2 = -2,
                Minus3 = -3,
                Minus4 = -4,
                Minus5 = -5,
                Plus1 = 1,
                Plus2 = 2,
                Plus3 = 3,
                Plus4 = 4,
                Plus5 = 5,
            }
            public DisplayPoint(int x, int y, string symbol, Entity.Color symbolColor, DrawPower drawPower) {
                this.x = x;
                this.y = y;
                this.symbol = symbol;
                this.awaitsDrawing = true;
                this.symbolColor = ConsoleColor.White;
                this.drawPower = drawPower;
                switch (symbolColor) {
                    case Entity.Color.DarkYellow:
                        this.symbolColor = ConsoleColor.DarkYellow;
                        break;
                    case Entity.Color.Red:
                        this.symbolColor = ConsoleColor.Red;
                        break; ;
                    case Entity.Color.Green:
                        this.symbolColor = ConsoleColor.Green;
                        break; ;
                    case Entity.Color.DarkGray:
                        this.symbolColor = ConsoleColor.DarkGray;
                        break; ;
                    case Entity.Color.Blue:
                        this.symbolColor = ConsoleColor.Blue;
                        break;
                    case Entity.Color.DarkMagenta:
                        this.symbolColor = ConsoleColor.DarkMagenta;
                        break;
                    case Entity.Color.White:
                        this.symbolColor= ConsoleColor.White;
                        break;
                    case Entity.Color.Magenta:
                        this.symbolColor = ConsoleColor.Magenta;
                        break;
                    case Entity.Color.Cyan:
                        this.symbolColor = ConsoleColor.Cyan;
                        break;
                    case Entity.Color.DarkRed:
                        this.symbolColor = ConsoleColor.DarkRed;
                        break;
                    case Entity.Color.Yellow:
                        this.symbolColor = ConsoleColor.Yellow;
                        break;

                    default: this.symbolColor = ConsoleColor.White;
                        break;
                }
            }
        }

        public void GameMenu() {
            Console.SetCursorPosition(1,settings.worldHeight+3);
            Console.WriteLine("Beliebe Taste drücken um zum Hauptmenu zu gelangen.");

            ConsoleKey key = Console.ReadKey(true).Key;

        }
        public void RunGame() {

            //Bildschirm leeren und anpassen
            log.Add("RunGame(): SetwindowSize with worldWidth=" + settings.worldWidth + "worldHeight=" + settings.worldHeight);
            Console.SetWindowSize(settings.worldWidth + 5, settings.worldHeight + 5);
            Console.Clear();
            Console.CursorVisible = false;

            //Welt initialisieren als World-Objekt

            if (this.world != null) {
                this.world = null;
                //With this command, you set the respective reference to null,
                //if there is other references to the same object they will still point to the same object. 

                //anyhow, this notifies the garbage collector that there is no reference to the object-instance
                //anymore and it will get picked up faster like this.
            }
            world = new World(settings.worldWidth, settings.worldHeight);

            Nest nest = new Nest(40, 15, settings.antCount);
            world.AddEntity(nest);

            QueenAnt queen = new QueenAnt(40, 15, nest, world);
            world.AddEntity(queen);
            antsAlive++;

            //World mit Entitäten füllen
            for (int antsAdded = 0; antsAdded < settings.antCount; antsAdded++) {
                double percentage = ((double)antsAdded / (double)settings.antCount) * 100;
                if(percentage < 40) {

                    Food food = new Food();
                    world.AddEntity(food);

                }
                if (percentage < 80) {
                    Ant ant = new Ant(40, 15, nest);
                    world.AddEntity(ant);

                } else {
                    WorkerAnt ant = new WorkerAnt(40, 15, nest, world);
                    world.AddEntity(ant);

                }
                antsAlive++;
            }

            DebugAnt debugAnt1 = new DebugAnt(30, 10, nest, world);
            DebugAnt debugAnt2 = new DebugAnt(30, 10, nest, world);
            DebugAnt debugAnt3 = new DebugAnt(30, 10, nest, world);
            DebugAnt debugAnt4 = new DebugAnt(30, 10, nest, world);
            if (Configuration.GetDebugActive()) {
                world.AddEntity(debugAnt1);
                world.AddEntity(debugAnt2);
                world.AddEntity(debugAnt3);
                world.AddEntity(debugAnt4);
            }


            //Main-Loop
            cyclesRemaining = settings.cyclesTotal;
            int loopDrawTimer = 5;                  //aller 5 Zyklen wird die Anzeige aktualisiert
            
            while (cyclesRemaining != 0) {
                
                //infinite game if cyclesRemaining = -1
                if (settings.cyclesTotal >= 0) {
                    cyclesRemaining--;
                }

                //The actual game logic
                SimulateOneCycle();

                //Anzeige aktualisieren
                if (loopDrawTimer <= 0) {
                    UpdateDisplayContent();
                    //Listenforkeys
                    DrawGame();
                    loopDrawTimer = 5;
                } else {
                    loopDrawTimer--;
                }

                //Simulationsgeschwindigkeit verlangsamen
                System.Threading.Thread.Sleep(10);          //Empfehlung: 10, Lehrerforderung für 1 Sekunde: 40

                ReadUserInput();

                //Simulation beenden
                if (lastUserInputKey == ConsoleKey.D0) {
                    cyclesRemaining = 0;
                }
                //Simulation pausieren
                if (lastUserInputKey == ConsoleKey.D9) {
                    lastUserInputKey = ConsoleKey.NoName;
                    Console.ReadKey(true);
                }
            }

            //Ende des Spiels
            Console.CursorVisible = true;
            GameMenu();
        }

        //Spieleinstellungen ändern
        public void ChangeSettings(Configuration.GameSettings.Mode mode = Configuration.GameSettings.Mode.Standard) {
            //Problem für später: Falls man vom Konstruktor aus eine World initialisiert wöllte, würde diese mit
            //der Standardgröße erstellt. ChangeSettings() ändert diese Größe nicht nachträglich. (@todo)
            //Deshalb muss das World-Objekt z.B. in RunGame() initialisiert werden, und darf nicht im Konstruktor erstellt werden.
            //Dadurch dass ich aber World nicht im Konstruktor initialisiere, kann ich außerhalb von RunGame() diese nicht ändern!
            //Ich kann also nicht vor einem Spiel Entitäten manuell hinzufügen, z.B. innerhalb des Konstruktors.
            //Eventuell wirft es auch Probleme auf, falls ich gespeicherte Spielstände in das initialisierte Game-Objekt laden möchte.

            Settings newSettings = new Settings();

            newSettings.worldWidth = Configuration.GetGameSettings(mode).worldWidth;
            newSettings.worldHeight = Configuration.GetGameSettings(mode).worldHeight;
            newSettings.antCount = Configuration.GetGameSettings(mode).antCount;
            newSettings.cyclesTotal = Configuration.GetGameSettings(mode).cycles;

            //cycles
            if (newSettings.cyclesTotal >= -1 && newSettings.cyclesTotal <= 9999999) {
                this.settings.cyclesTotal = newSettings.cyclesTotal;
                log.Add("ChangeSettings(): cycles changed to: " + this.settings.cyclesTotal);
            } else {
                log.AddWarning("ChangeSettings(): cycles not changed, out of range: " + newSettings.cyclesTotal);
            }

            //antCount
            if (newSettings.antCount > 0 && newSettings.antCount < 10000) {
                this.settings.antCount = newSettings.antCount;
                log.Add("ChangeSettings(): antCount changed to: " + this.settings.antCount);
            }
            else {
                log.AddWarning("ChangeSettings(): antCount not changed, out of range: " + newSettings.antCount);
            }

            //WindowWidth
            if (newSettings.worldWidth > 20 && newSettings.worldWidth < 500) {
                this.settings.worldWidth = newSettings.worldWidth;
                log.Add("ChangeSettings(): windowWidth changed to: " + this.settings.worldWidth);
            }
            else {
                log.AddWarning("ChangeSettings(): windowWidth not changed, out of range: " + newSettings.worldWidth);
            }

            //WindowHeight
            if (newSettings.worldHeight > 20 && newSettings.worldHeight < 500) {
                this.settings.worldHeight = newSettings.worldHeight;
                log.Add("ChangeSettings(): windowHeight changed to: " + this.settings.worldHeight);
            }
            else {
                log.AddWarning("ChangeSettings(): windowHeight not changed, out of range: " + newSettings.worldHeight);
            }
        }
        public void SetDefaultSettings() {
            settings.cyclesTotal = Configuration.GetSdtCycles();
            settings.antCount = Configuration.GetStdAntCount();
            settings.worldWidth = Configuration.GetStdWorldWidth();
            settings.worldHeight = Configuration.GetStdWorldHeight();
        }

        //Speichert zu schreibende Punkte als DisplayPoint-Objekt
        //Speichert bereits geschriebene Punkte in der Liste oldDisplayContents
        //Falls ein Punkt aktualisiert wurde, teilt der Wahrheitswert "awaitsDrawing" mit,
        //dass der Punkt neu geschrieben werden soll
        private void UpdateDisplayContent() {

            oldDisplayContents.Clear();
            oldDisplayContents.TrimExcess();

            foreach (DisplayPoint content in this.displayContents) {
                oldDisplayContents.Add(content);
            }

            displayContents.Clear();
            displayContents.TrimExcess();

            foreach (Entity entity in world.GetContent()) {
                
                DisplayPoint.DrawPower drawPower = DisplayPoint.DrawPower.Default;

                if (entity.GetType().Name == typeof(Ant).Name
                || entity.GetType().IsSubclassOf(typeof(Ant))) {
                    drawPower = DisplayPoint.DrawPower.Plus1;
                }
                if (entity.GetType().IsSubclassOf(typeof(Ant))) {
                    drawPower = DisplayPoint.DrawPower.Plus2;
                }
                if (entity.GetType().Name == typeof(QueenAnt).Name) {
                    drawPower = DisplayPoint.DrawPower.Plus3;
                }
                if (entity.GetType().Name == typeof(Nest).Name) {
                    drawPower = DisplayPoint.DrawPower.Plus3;
                }

                displayContents.Add( new DisplayPoint(
                        entity.GetX()+1,
                        entity.GetY(),
                        entity.GetEntitySymbol(),
                        entity.GetColor(),
                        drawPower
                        ));
            }

            //Statistics below the game frame
            double remainingPercent = ( (double)cyclesRemaining / (double)settings.cyclesTotal ) * 100;
            displayContents.Add(new DisplayPoint(
                1,
                settings.worldHeight+2,
                "Remaining: " + (int)remainingPercent + "% (" +settings.cyclesTotal +" cycles)",
                Entity.Color.Blue,
                DisplayPoint.DrawPower.Default
                ));
            displayContents.Add(new DisplayPoint(
                33, 
                settings.worldHeight + 2, 
                "Alive: " + world.GetAnts().Count() + " (" + (settings.antCount) + " start)",
                Entity.Color.Blue,
                DisplayPoint.DrawPower.Default
                ));
            displayContents.Add(new DisplayPoint(
                57,
                settings.worldHeight + 2,
                "Food (Nest/World): " + ((Nest)world.GetWorldNests().First()).GetOwnedFoodCount() + " / " + world.GetFood().Count() + " ",
                Entity.Color.Blue,
                DisplayPoint.DrawPower.Default
                ));
            displayContents.Add(new DisplayPoint(
                1,
                settings.worldHeight + 3,
                "Eggs (Nest/Hatched): " + ((Nest)world.GetWorldNests().First()).GetOwnedAntEggCount() + " / " + world.GetHatchedAntsCounter() + " ",
                Entity.Color.Blue,
                DisplayPoint.DrawPower.Default
                ));

            displayContents.Add(new DisplayPoint(
                            30,
                            settings.worldHeight + 3,
                            "Key: " + lastUserInputKey.ToString() + "                           "
                            ,
                            Entity.Color.Blue,
                DisplayPoint.DrawPower.Default
                ));

        }

        private void DrawGame() {
            //Sort display points according to their draw power
            //possible later addition: alternate drawing of objects with equal draw power (e.g. Queen and Nest)
            displayContents.Sort(delegate (DisplayPoint x, DisplayPoint y)
            {
                if (x.drawPower == y.drawPower) return 0;
                else if (x.drawPower < y.drawPower) return -1;
                else if (y.drawPower < x.drawPower) return 1;
                else return x.drawPower.CompareTo(y.drawPower);
            });

            oldDisplayContents.Sort(delegate (DisplayPoint x, DisplayPoint y) {
                if (x.drawPower == y.drawPower) return 0;
                else if (x.drawPower < y.drawPower) return -1;
                else if (y.drawPower < x.drawPower) return 1;
                else return x.drawPower.CompareTo(y.drawPower);
            });

            //this loop checks if an already drawn position shall be overdrawn, because it either is empty or has another symbol
            foreach (DisplayPoint oldContent in oldDisplayContents) {
                bool foundNewContent = false;
                foreach (DisplayPoint content in displayContents) {
                    if (oldContent.x == content.x && oldContent.y == content.y) {
                        foundNewContent = true;
                        if (content.awaitsDrawing) {
                            WriteSymbol(content);
                            content.awaitsDrawing = false;
                        }
                    }
                }
                if (!foundNewContent) {
                    Console.SetCursorPosition(oldContent.x, oldContent.y);
                    Console.Write(" ");
                }
            }

            //draw new content that was not part of the overdrawing of already used positions in the upper loop
            foreach (DisplayPoint content in displayContents) {
                if (content.awaitsDrawing) {
                    WriteSymbol(content);
                    content.awaitsDrawing = false;
                }
            }
        }
        private void WriteSymbol(DisplayPoint content) {
            Console.SetCursorPosition(content.x, content.y);
            Console.ForegroundColor = content.symbolColor;
            Console.Write(content.symbol);
            Console.ResetColor();
        }

        private void SimulateOneCycle() {
            List<Entity> deletableEntities = new List<Entity>();

            //Ameisen simulieren
            foreach (Entity entity in world.GetContent()) {
                if (entity.GetType().Name == typeof(Ant).Name
                    || entity.GetType().Name == typeof(WorkerAnt).Name
                    || entity.GetType().Name == typeof(QueenAnt).Name
                    || entity.GetType().Name == typeof(DebugAnt).Name
                    || entity.GetType().Name == typeof(Food).Name
                    || entity.GetType().Name == typeof(AntEgg).Name
                    ) {
                    entity.MoveOneIntelligent();
                    entity.PassOneCycle();
                }
                if (entity.GetDestroyable()) {
                    deletableEntities.Add(entity);
                    antsAlive--;
                }

            }

            if (world.GetAnts().Count() < settings.antCount * 0.9) {
                log.Add("new food!");
                if ((double)world.GetFood().Count() < settings.antCount * 0.2) {
                    Food food = new Food();
                    world.AddEntity(food);
                }
            }

            world.HandleQueue();

            //Gestorbene Ameisen aus dem World-Objekt entfernen
            foreach (Entity deleteEntity in deletableEntities) {
                world.DestroyEntity(deleteEntity);
            }
        }

        private void ReadUserInput() {
            if (Console.KeyAvailable) {
                lastUserInputKey = Console.ReadKey(true).Key;
            }
        }
    }
}
