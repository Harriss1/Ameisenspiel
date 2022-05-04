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
            public bool awaitsDrawing;
            public DisplayContent(int x, int y, string symbol) {
                this.x = x;
                this.y = y;
                this.symbol = symbol;
                this.awaitsDrawing = true;
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
            //this.cyclesRemaining = 200; //@todo where to put

            this.cyclesRemaining = settings.cycles;
            this.windowHeight = settings.windowHeight;
            this.windowWidth = settings.windowWidth;

            for (int antCount = 0; antCount < this.settings.antCount; antCount++) {
                Ant ant = new Ant(40,15);
                world.AddEntity(ant);
            }

            //MainLoop
            Console.CursorVisible = false;
            for (int i = 0; i < this.cyclesRemaining; i++) {

                foreach(Entity entity in this.world.GetContent()) {
                    if (entity.GetEntitySymbol() == "@") {
                        entity.MoveOneRandom();
                    }
                }
                UpdateDisplayContent();
                //Listenforkeys
                DrawDisplayContent();
                System.Threading.Thread.Sleep(100);

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
            this.settings.cycles = 100;
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
                displayContents.Add(new DisplayContent(entity.GetX(), entity.GetY(), entity.GetEntitySymbol()));

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
                            Console.SetCursorPosition(content.x, content.y); //content.y == -1 ?!?!?!?
                            Console.Write(content.symbol);
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
                    Console.SetCursorPosition(content.x, content.y); //content.y == -1 ?!?!?!?
                    Console.Write(content.symbol);
                    content.awaitsDrawing = false;
                }
            }
        }
        public void SetAntRandomly() {
            Random rand = new Random();
            int randX = rand.Next(1, 80);
            int randY = rand.Next(1, 25);
            this.ant = new Ant(15, 15);
        }
        
        public void DrawDevWorld() {
            this.cyclesRemaining = 20;
            for (int i = 0; i < this.cyclesRemaining; i++) {
                //Console.Clear(); //kann zu Problem führen, da man später alles löscht, und Screen flackert.
                ant.MoveOneRandom();
                Log log = new Log("Game");
                log.Add("Moved Randomly");
                Console.SetCursorPosition(ant.GetX(), ant.GetY());
                Console.Write(ant.GetEntitySymbol());
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
