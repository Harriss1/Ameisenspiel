using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Responsible for handling Input/Output and Savegames
/// The goal is to have a class, that can be changed
/// GameSimulation and related subclasses should not be changed if I use other interfaces
/// change of plan: I am not outsourcing IO, the code gets too complicated 
///     (draw/remove single ant requires observers or long chains)

/// </summary>

namespace Ameisenspiel {
    internal class Game {
        Log log = new Log("Game.cs");
        private int cyclesRemaining;
        private Ant ant;
        private int windowWidth;
        private int windowHeight;
        private World world;
        private List<DisplayContent> displayContents;
        private List<DisplayContent> oldDisplayContents;
        private Settings settings;
        public Game() {
            this.cyclesRemaining = 1;
            this.displayContents = new List<DisplayContent>();
            this.oldDisplayContents = new List<DisplayContent>();
            SetDefaultSettings();
            Initialise();
            
        }
        public struct Settings {
            public int windowWidth;
            public int windowHeight;
            public int cycles;
            public int antCount;
        }

        public class DisplayContent
            {
            public int x;
            public int y;
            public string symbol;
            public ConsoleColor symbolColor;
            public bool awaitsDrawing;
            public DisplayContent(int x, int y, string symbol, Entity.Color symbolColor) {
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
                    default: this.symbolColor = ConsoleColor.White;
                        break;
                }
            }
        }


        private void Initialise() {

            //adapt window size
            windowHeight = DevelopmentGameSettings.GetWindowHeight();
            windowWidth = DevelopmentGameSettings.GetWindowWidth();
            Console.SetWindowSize(windowWidth+3, windowHeight+3);
            Console.Clear();
            if(this.world != null) {
                this.world = null;
                //With this command, you set the car reference to null,
                //if there is other references to the same object they will still point to the same object. 

                //anyhow, this notifies the garbage collector that there is no reference to the object-instance
                //anymore and it will get picked up faster like this.
            }
            World.SetWorldProperties(windowWidth, windowHeight);
            this.world = new World();
        }

        public void GameMenu() {
            Console.SetCursorPosition(0,this.windowHeight+1);
            Console.WriteLine("Beliebe Taste drücken um zum Hauptmenu zu gelangen.");

            ConsoleKey key = Console.ReadKey(true).Key;

        }
        public void RunGame() {

            //Settings
            this.cyclesRemaining = settings.cycles;
            this.windowHeight = settings.windowHeight;
            this.windowWidth = settings.windowWidth;

            for (int antCount = 0; antCount < this.settings.antCount; antCount++) {
                double percentage = ((double)antCount / (double)this.settings.antCount) * 100;
                log.Add("percentage = " + percentage);
                if (percentage <= 80) {
                    Ant ant = new Ant(40, 15);
                    world.AddEntity(ant);

                } else {
                    WorkerAnt ant = new WorkerAnt(40, 15);
                    world.AddEntity(ant);
                    log.Add("added worker");
                }
            }

            QueenAnt queen = new QueenAnt(40, 15);
            Hive hive = new Hive(40, 15);
            world.AddEntity(queen);
            world.AddEntity(hive);

            //MainLoop
            Console.CursorVisible = false;
            int count = 0;
            for (int i = 0; i < this.cyclesRemaining; i++) {
                List<Entity> deletableEntities = new List<Entity>();
                foreach(Entity entity in this.world.GetContent()) {
                    
                    if (entity.GetType().Name == typeof(Ant).Name
                        || entity.GetType().Name == typeof(WorkerAnt).Name
                        || entity.GetType().Name == typeof(QueenAnt).Name
                        ) {
                        entity.MoveOneIntelligent();
                        entity.PassOneCycle();
                    }
                    if (entity.GetDestroyable()) {
                        deletableEntities.Add(entity);
                    }
                }
                foreach (Entity deleteEntity in deletableEntities) {
                    this.world.DestroyEntity(deleteEntity);
                }
                if (count >= 5) {
                    UpdateDisplayContent();
                    //Listenforkeys
                    DrawDisplayContent();
                    count = 0;
                } else {
                    count++;
                }
                System.Threading.Thread.Sleep(10);

            }
            Console.CursorVisible = true;
            GameMenu();
        }
        public void ChangeSettings(Settings newSettings) {

            //cycles
            if (newSettings.cycles > 0 && newSettings.cycles < 1000000) {
                this.settings.cycles = newSettings.cycles;
                log.Add("ChangeSettings(): cycles changed to: " + settings.cycles);
            } else {
                log.AddWarning("ChangeSettings(): cycles not changed, out of range: " + newSettings.cycles);
            }

            //antCount
            if (newSettings.antCount > 0 && newSettings.antCount < 10000) {
                this.settings.antCount = newSettings.antCount;
                log.Add("ChangeSettings(): antCount changed to: " + settings.antCount);
            }
            else {
                log.AddWarning("ChangeSettings(): antCount not changed, out of range: " + newSettings.antCount);
            }

            //WindowWidth
            if (newSettings.windowWidth > 20 && newSettings.windowWidth < 500) {
                this.settings.windowWidth = newSettings.windowWidth;
                log.Add("ChangeSettings(): windowWidth changed to: " + settings.windowWidth);
            }
            else {
                log.AddWarning("ChangeSettings(): windowWidth not changed, out of range: " + newSettings.windowWidth);
            }

            //WindowHeight
            if (newSettings.windowHeight > 20 && newSettings.windowHeight < 500) {
                this.settings.windowHeight = newSettings.windowHeight;
                log.Add("ChangeSettings(): windowHeight changed to: " + settings.windowHeight);
            }
            else {
                log.AddWarning("ChangeSettings(): windowHeight not changed, out of range: " + newSettings.windowHeight);
            }
        }
        public void SetDefaultSettings() {
            this.settings.cycles = 1000;
            this.settings.antCount = 100;
            this.settings.windowWidth = 85;
            this.settings.windowHeight = 25;
        }

        private void UpdateDisplayContent() {
            oldDisplayContents.Clear();
            oldDisplayContents.TrimExcess();
            foreach (DisplayContent content in this.displayContents) {
                oldDisplayContents.Add(content);
            }
            displayContents.Clear();
            displayContents.TrimExcess();
            foreach (Entity entity in world.GetContent()) {
                displayContents.Add(new DisplayContent(entity.GetX(), entity.GetY(), entity.GetEntitySymbol(), entity.GetColor()));

                //Console.SetCursorPosition(entity.GetX(), entity.GetY());
                //Console.Write(" ");
            }

            //the displayContent gets told: draw at this point xy symbol Z
            //if we change an entities position we search the displaycontent for its old position and change that entry
        
        }

        private void DrawDisplayContent() {
            foreach (DisplayContent oldContent in this.oldDisplayContents) {
                bool foundNewContent = false;
                foreach (DisplayContent content in this.displayContents) {
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
            foreach (DisplayContent content in this.displayContents) {
                if (content.awaitsDrawing) {
                    WriteSymbol(content);
                    content.awaitsDrawing = false;
                }
            }
        }
        private void WriteSymbol(DisplayContent content) {
            Console.SetCursorPosition(content.x, content.y);
            Console.ForegroundColor = content.symbolColor;
            Console.Write(content.symbol);
            Console.ResetColor();
        }
        //keep for idea
        public void SetAntRandomly() {
            Random rand = new Random();
            int randX = rand.Next(1, this.windowWidth);
            int randY = rand.Next(1, this.windowHeight);
            this.ant = new Ant(randX, randY);
        }
    }
}
