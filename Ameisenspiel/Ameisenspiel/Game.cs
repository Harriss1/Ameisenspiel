using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Responsible for handling Input/Output and game logic
/// Optional Goal: GameSimulation and related subclasses should not be changed if I use another interfaces
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

        public class DisplayPoint
            {
            public int x;
            public int y;
            public string symbol;
            public ConsoleColor symbolColor;
            public bool awaitsDrawing;
            public DisplayPoint(int x, int y, string symbol, Entity.Color symbolColor) {
                this.x = x;
                this.y = y;
                this.symbol = symbol;
                this.awaitsDrawing = true;
                this.symbolColor = ConsoleColor.White;
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

            if (this.world != null) {
                this.world = null;
                //With this command, you set the car reference to null,
                //if there is other references to the same object they will still point to the same object. 

                //anyhow, this notifies the garbage collector that there is no reference to the object-instance
                //anymore and it will get picked up faster like this.
            }
            this.world = new World(settings.worldWidth, settings.worldHeight);

            log.Add("RunGame(): SetwindowSize<> worldWidth=" + settings.worldWidth + "worldHeight=" + settings.worldHeight);
            Console.SetWindowSize(settings.worldWidth + 5, settings.worldHeight + 5);
            Console.Clear();

            this.cyclesRemaining = settings.cyclesTotal;

            for (int antCount = 0; antCount < this.settings.antCount; antCount++) {
                double percentage = ((double)antCount / (double)this.settings.antCount) * 100;
                if (percentage <= 80) {
                    Ant ant = new Ant(40, 15);
                    world.AddEntity(ant);

                } else {
                    WorkerAnt ant = new WorkerAnt(40, 15);
                    world.AddEntity(ant);
                }
                antsAlive++;
            }

            QueenAnt queen = new QueenAnt(40, 15);
            world.AddEntity(queen);
            antsAlive++;

            Hive hive = new Hive(40, 15);
            world.AddEntity(hive);

            DebugAnt debugAnt1 = new DebugAnt(30, 10);
            DebugAnt debugAnt2 = new DebugAnt(30, 10);
            DebugAnt debugAnt3 = new DebugAnt(30, 10);
            DebugAnt debugAnt4 = new DebugAnt(30, 10);
            if (Configuration.GetDebugActive()) {
                world.AddEntity(debugAnt1);
                world.AddEntity(debugAnt2);
                world.AddEntity(debugAnt3);
                world.AddEntity(debugAnt4);
            }

            //MainLoop
            Console.CursorVisible = false;
            int loopDrawTimer = 5;
            for (int i = 0; i < settings.cyclesTotal; i++) {
                this.cyclesRemaining--;
                List<Entity> deletableEntities = new List<Entity>();
                foreach(Entity entity in this.world.GetContent()) {
                    
                    if (entity.GetType().Name == typeof(Ant).Name
                        || entity.GetType().Name == typeof(WorkerAnt).Name
                        || entity.GetType().Name == typeof(QueenAnt).Name
                        || entity.GetType().Name == typeof(DebugAnt).Name
                        ) {
                        entity.MoveOneIntelligent();
                        entity.PassOneCycle();
                    }
                    if (entity.GetDestroyable()) {
                        deletableEntities.Add(entity);
                        antsAlive--;
                    }
                }
                foreach (Entity deleteEntity in deletableEntities) {
                    this.world.DestroyEntity(deleteEntity);
                }
                if (loopDrawTimer <= 0) {
                    UpdateDisplayContent();
                    //Listenforkeys
                    DrawGame();
                    loopDrawTimer = 5;
                } else {
                    loopDrawTimer--;
                }
                System.Threading.Thread.Sleep(10);

            }
            Console.CursorVisible = true;
            GameMenu();
        }

        //wirft folgendes Problem auf: Falls man vom Konstruktor aus eine World initialisiert wird diese mit
        //der Standardgröße erstellt. Changesettings() ändert diese Größe nicht nachträglich.
        //Deshalb muss das World-Objekt z.B. in RunGame() initialisiert werden, und darf nicht im Konstruktor erstellt werden.
        //Dadurch dass ich aber World nicht im Konstruktor initialisiere, kann ich außerhalb von RunGame() diese nicht ändern!
        //Ich kann also nicht vor einem Spiel Entitäten manuell hinzufügen, z.B. innerhalb des Konstruktors.
        //auch ein Gespeicherter Spielstand kann somit nicht von außerhalb geladen werden durch den Konstruktor...
        public void ChangeSettings(Configuration.GameSettings.Mode mode = Configuration.GameSettings.Mode.Standard) {
            Settings newSettings = new Settings();
            Configuration config = new Configuration();
            switch (mode) {
                case Configuration.GameSettings.Mode.Standard:
                    newSettings.antCount = config.standard.antCount;
                    newSettings.cyclesTotal = config.standard.cycles;
                    newSettings.worldHeight = config.standard.worldHeight;
                    newSettings.worldWidth = config.standard.worldWidth;
                    break;
                case Configuration.GameSettings.Mode.Double:
                    newSettings.antCount = config.doubleLength.antCount;
                    newSettings.cyclesTotal = config.doubleLength.cycles;
                    newSettings.worldHeight = config.doubleLength.worldHeight;
                    newSettings.worldWidth = config.doubleLength.worldWidth;
                    break;
                case Configuration.GameSettings.Mode.Demo:
                    newSettings.antCount = config.demo.antCount;
                    newSettings.cyclesTotal = config.demo.cycles;
                    newSettings.worldHeight = config.demo.worldHeight;
                    newSettings.worldWidth = config.demo.worldWidth;
                    break;
                case Configuration.GameSettings.Mode.Stress:
                    newSettings.antCount = config.stress.antCount;
                    newSettings.cyclesTotal = config.stress.cycles;
                    newSettings.worldHeight = config.stress.worldHeight;
                    newSettings.worldWidth = config.stress.worldWidth;
                    break;
                case Configuration.GameSettings.Mode.Bigworld:
                    newSettings.antCount = config.bigworld.antCount;
                    newSettings.cyclesTotal = config.bigworld.cycles;
                    newSettings.worldHeight = config.bigworld.worldHeight;
                    newSettings.worldWidth = config.bigworld.worldWidth;
                    break;
            }

            //cycles
            if (newSettings.cyclesTotal > 0 && newSettings.cyclesTotal <= 9999999) {
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
            this.settings.cyclesTotal = Configuration.GetSdtCycles();
            this.settings.antCount = Configuration.GetStdAntCount();
            this.settings.worldWidth = Configuration.GetStdWorldWidth();
            this.settings.worldHeight = Configuration.GetStdWorldHeight();
        }

        private void UpdateDisplayContent() {
            //the displayContent gets told: draw at this point xy symbol Z
            //if we change an entities position we search the displaycontent for its old position and change that entry

            oldDisplayContents.Clear();
            oldDisplayContents.TrimExcess();

            foreach (DisplayPoint content in this.displayContents) {
                oldDisplayContents.Add(content);
            }

            displayContents.Clear();
            displayContents.TrimExcess();

            foreach (Entity entity in world.GetContent()) {
                displayContents.Add( new DisplayPoint(
                        entity.GetX()+1,
                        entity.GetY(),
                        entity.GetEntitySymbol(),
                        entity.GetColor()));
            }

            //Statistics below the game frame
            double remainingPercent = ( (double)cyclesRemaining / (double)settings.cyclesTotal ) * 100;
            displayContents.Add(new DisplayPoint(
                1,
                settings.worldHeight+2,
                "Remaining: " + (int)remainingPercent + "% (" +settings.cyclesTotal +" cycles)",
                Entity.Color.Blue));
            displayContents.Add(new DisplayPoint(
                40, 
                settings.worldHeight + 2, 
                "Alive: " + antsAlive + " (" + (settings.antCount+1) + " start)",
                Entity.Color.Blue));
        }

        private void DrawGame() {
            foreach (DisplayPoint oldContent in this.oldDisplayContents) {
                bool foundNewContent = false;
                foreach (DisplayPoint content in this.displayContents) {
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
            foreach (DisplayPoint content in this.displayContents) {
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
        //keep for idea
        public void SetAntRandomly() {
            Random rand = new Random();
            int randX = rand.Next(1, settings.worldWidth);
            int randY = rand.Next(1, settings.worldHeight);
            
            Ant ant = new Ant(randX, randY);
        }
    }
}
