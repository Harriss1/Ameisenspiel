using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  


/* 
 * Autor: Karl Klotz 
 * Datum: 29. März 2022 bis 13. Mai 2022 
 * Inhalt: Implementierung eines 2D Ameisenspiels nach Unterrichtsaufgabe vom 28.03.2022 
 * Ziele: Erlernen der Grundlagen der Objektorientierung (Vererbung, Abstraktion, Polymorphismus, Kapselung) 
 * Vereinfachter Aufbau: 
 * - Es gibt die Klasse "Entity" und die Klasse "World", und alle Spielobjekte werden aus 
 * vererbten Klassen erzeugt: Die Welt wird mit Ameisen gefüllt, die Ameisen haben von Entity geerbt.
 * - Das "Füllen" des World-Objekts wird mittels der Game-Klasse erledigt, welche auch die Welt ausließt und grafisch darstellt.
 * Implementiert mit .Net 4.8 und C# 9 in Visual Studio 2022
*/


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
        Console.WriteLine();
        Console.WriteLine("@ = normale Ameise; a = Arbeiterameise; H = Ameisen-Hügel; Q = Queen (fast nie sichtbar)");
        Console.WriteLine();
        Console.WriteLine("weiß = Hügel noch nicht besucht; grün = Hügel mindestens einmal besucht und gegessen" +
            "\n orange = bald hungrig; rot = läuft zum Hügel um zu Essen");
        Console.WriteLine();
    }

    private static int SelectFromMainMenu() {
        Configuration config = new Configuration();
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
        Console.WriteLine();
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


/// ////////////////////////////////////////////////////////////////////////////////////// 
/// <summary> 
/// Zentrale Klasse um globale Spielparameter als Entwickler zu ändern. 
/// </summary> 
internal class Configuration {

    //Hier wird manuell festgelegt, ob wir die Development Settings nutzen 
    private static bool devSettingsActive = true;
    private static bool debugActive = false;
    private static bool devAutorun = false; //wird nur genutzt falls devSettingsActive=true;  

    public Configuration() {
        if (!devSettingsActive) {
            devAutorun = stdAutorun;
        }
        SetSettings();
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
    private static GameSettings doubleLength;
    private static GameSettings demo;
    private static GameSettings stress;
    private static GameSettings bigworld;

    private void SetSettings() {
        //Default Mode 
        standard = new GameSettings(true);
        //Double Time Length 
        doubleLength = new GameSettings(true);
        doubleLength.cycles *= 2;
        doubleLength.antCount = (int)((double)doubleLength.antCount * 1.5);
        doubleLength.title = "Doppelt soviel";
        doubleLength.description = doubleLength.MakeDescription();
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
    private static bool stdAutorun = false;          ///////////////////////////////////////////////////////////// 
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
        switch (mode) {
            case Configuration.GameSettings.Mode.Standard:
                return standard;
            case Configuration.GameSettings.Mode.Double:
                return doubleLength;
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


////////////////////////////////////////////////////////////////////////// 
///<summary> 
/// Die Spielsimmulation erfolgt durch diese Klasse. Verwantwortlich um das Objekt "World" zu instanziieren 
/// World wird mit Entity-Objekten verschiedener Typen (Ant, Queen, Hive) gefüllt. 
/// Jede Entität hat die Funktion "Bewegen" und "Zyklusfolgen berechnen (Alter und Energie verbrauchen)" welche im Main-Loop implementiert werden. 
/// Das Objekt "Game" ließt die Entity-Objekte in World aus, und übergibt diese an die Grafikausgabefunktionen: 
///     - UpdateDisplayContent (zum speichern des Anzeigeinhalts und Abgleich von Änderungen zum vorherigen Grafikausgabezyklus) 
///     - DrawDisplayContent (verantwortlich für die Grafikausgabe) 
/// </summary> 
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
    private class DisplayPoint {
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
                case Entity.Color.White:
                    this.symbolColor = ConsoleColor.White;
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
                default:
                    this.symbolColor = ConsoleColor.White;
                    break;
            }
        }
    }
    public void GameMenu() {
        Console.SetCursorPosition(1, settings.worldHeight + 3);
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
        }
        world = new World(settings.worldWidth, settings.worldHeight);
        //World mit Entitäten füllen 
        for (int antCount = 0; antCount < this.settings.antCount - 1; antCount++) {
            double percentage = ((double)antCount / (double)this.settings.antCount) * 100;
            if (percentage <= 80) {
                Ant ant = new Ant(40, 15);
                world.AddEntity(ant);
            }
            else {
                WorkerAnt ant = new WorkerAnt(40, 15);
                world.AddEntity(ant);
            }
            antsAlive++;
        }

        QueenAnt queen = new QueenAnt(40, 15);
        world.AddEntity(queen);
        antsAlive++;
        Nest hive = new Nest(40, 15);
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

        //Main-Loop 
        cyclesRemaining = settings.cyclesTotal;
        int loopDrawTimer = 5;                  //aller 5 Zyklen wird die Anzeige aktualisiert  
        for (int i = 0; i < settings.cyclesTotal; i++) {
            cyclesRemaining--;
            List<Entity> deletableEntities = new List<Entity>();
            //Ameisen simulieren 
            foreach (Entity entity in world.GetContent()) {
                if (entity.GetType().Name == typeof(Ant).Name
                    || entity.GetType().Name == typeof(WorkerAnt).Name
                    || entity.GetType().Name == typeof(QueenAnt).Name
                    || entity.GetType().Name == typeof(DebugAnt).Name
                    ) {
                    entity.MoveOneIntelligent();    //Bewegen 
                    entity.PassOneCycle();          //Berechnung der Folgen des Zyklus, z.B. Altern, Energie verlieren... 
                }
                if (entity.GetDestroyable()) {
                    deletableEntities.Add(entity);
                    antsAlive--;
                }
            }
            //Gestorbene Ameisen aus dem World-Objekt entfernen 
            foreach (Entity deleteEntity in deletableEntities) {
                world.DestroyEntity(deleteEntity);
            }
            //Anzeige aktualisieren 
            if (loopDrawTimer <= 0) {
                UpdateDisplayContent();
                //Listenforkeys 
                DrawGame();
                loopDrawTimer = 5;
            }
            else {
                loopDrawTimer--;
            }
            //Simulationsgeschwindigkeit verlangsamen 
            System.Threading.Thread.Sleep(40);
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
        if (newSettings.cyclesTotal > 0 && newSettings.cyclesTotal <= 9999999) {
            this.settings.cyclesTotal = newSettings.cyclesTotal;
            log.Add("ChangeSettings(): cycles changed to: " + this.settings.cyclesTotal);
        }
        else {
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
            displayContents.Add(new DisplayPoint(
                    entity.GetX() + 1,
                    entity.GetY(),
                    entity.GetEntitySymbol(),
                    entity.GetColor()));
        }
        //Statistics below the game frame 
        double remainingPercent = ((double)cyclesRemaining / (double)settings.cyclesTotal) * 100;
        displayContents.Add(new DisplayPoint(
            1,
            settings.worldHeight + 2,
            "Remaining: " + (int)remainingPercent + "% (" + settings.cyclesTotal + " cycles)",
            Entity.Color.Blue));
        displayContents.Add(new DisplayPoint(
            40,
            settings.worldHeight + 2,
            "Alive: " + antsAlive + " (" + (settings.antCount) + " start)",
            Entity.Color.Blue));
    }
    private void DrawGame() {
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
}


////////////////////////////////////////////////////////////////////////////// 
/// <summary> 
/// implementation of World size and keeping track of entities inside. 
/// </summary> 
internal class World {
    //Width and Height are used across the Entity-Objects and must have the same value across all generated objects. 
    //I am not using a Configuration-object, because this makes changes even harder later on. 
    protected static int width;
    protected static int height;
    private Log log;
    protected Random random;
    protected List<Entity> worldContent = new List<Entity>();

    //Standard World size declared here for reference 
    public World() : this(85, 25) {
    }

    public World(int width, int height) {
        World.width = width;
        World.height = height;
        this.random = new Random();
        log = new Log("World.cs");
        log.Add("Konstructor(): width=" + World.width + " height=" + World.height);
    }

    public static void SetWorldProperties(int width, int height) {
        World.width = width;
        World.height = height;
    }

    public static int GetWorldWidth() {
        return World.width;
    }

    public static int GetWorldHeight() {
        return World.height;
    }
    
    public bool AddEntity(Entity entity) {
        bool coordinateError = false;
        if (entity.GetX() > World.width || entity.GetX() < 0) {
            log.AddError("X coordinate is outside of world width. X=" + entity.GetX());
            coordinateError = true;
        }
        if (entity.GetY() > World.height || entity.GetY() < 0) {
            log.AddError("Y coordinate is outside of world height. Y=" + entity.GetY());
            coordinateError = true;
        }
        if (coordinateError) {
            return false;
        }
        this.worldContent.Add(entity);
        return true; //@todo: hier ist es möglich Kollision zu implementieren 
    }
    public List<Entity> GetContent() {
        return this.worldContent;
    }
    public void DestroyEntity(Entity entity) {
        this.worldContent.Remove(entity);
    }
}


/// /////////////////////////////////////////////////////////////////////////////////////////////
/// Zentrales Objekt aus dem Ameisen, Königinnen, Ameisenhügel und später Futter vererbt wird.
/// 
internal class Entity {

    protected int x;
    protected int y;
    protected int age; //this will make the game crash if it runs too long, but only if we run it for longer than 58 billion years 
    
    protected bool isDestroyable;
    protected bool canMoveOnItsOwn;
    protected String entitySymbol;
    protected Color entityColor;
    
    protected static Random rand = new Random();
    //speed calculation: 
    //example: start=100 -> start distance=100, speed=10, every cycle we remove 10 speed from distance, 
    //at distance=zero we move one square and reset distance to 100 
    protected int speed;
    protected int speedStartDistance;
    protected int speedDistanceRemaining;
    
    public enum Color {
        Standard = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        DarkRed = 5,
        Cyan = 6,
        Magenta = 7,
        DarkMagenta = 8,
        White = 9,
        DarkYellow = 10,
        DarkGray = 11,
    }
    public Entity(int xPos, int yPos) { //we can only directly set x+y via constructor, not during game simulation 
        this.x = xPos;
        this.y = yPos;
        this.isDestroyable = false;
        this.canMoveOnItsOwn = false;
        this.age = 0;
        this.entityColor = Color.Standard;
        this.speed = 0;
        this.speedStartDistance = 1000;
        this.speedDistanceRemaining = 0;
        SetEntitySymbol(" ");
    }
    virtual public void PassOneCycle() {
        //override this class inside inherited classes to make energy depleting happen or anything. 
        //implement possible Entity relevant methods here 
        IncreaseAge();
    }
    protected void IncreaseAge() {
        age++;
    }
    public void MoveOneLeft() {
        if (this.x != 1 && this.canMoveOnItsOwn) {
            this.x--;
        }
    }
    public void MoveOneRight() {
        if (this.x != World.GetWorldWidth() && this.canMoveOnItsOwn) {
            this.x++;
        }
    }
    public void MoveOneDown() {
        if (this.y != World.GetWorldHeight() && this.canMoveOnItsOwn) {
            this.y++;
        }
    }
    public void MoveOneUp() {
        if (this.y != 1 && this.canMoveOnItsOwn) {
            this.y--;
        }
    }
    public String GetEntitySymbol() {
        return this.entitySymbol;
    }
    protected void SetEntitySymbol(String entitySymbol) {
        this.entitySymbol = entitySymbol;
    }
    public int GetX() {
        return x;
    }
    public int GetY() {
        return y;
    }
    public void MoveOneRandom() {
        int selectRandom = rand.Next(1, 5);
        switch (selectRandom) {
            case 1:
                this.MoveOneRight();
                break;
            case 2:
                this.MoveOneLeft();
                break;
            case 3:
                this.MoveOneUp();
                break;
            case 4:
                this.MoveOneDown();
                break;
            case 5:
                //dont move 
                break;
            default:
                //report bug 
                break;
        }
    }
    virtual public void MoveOneIntelligent() {
        //static Entities dont move 
    }
    public void SetDestroyable() {
        this.isDestroyable = true;
    }
    public bool GetDestroyable() {
        return this.isDestroyable;
    }
    public Color GetColor() {
        return this.entityColor;
    }
}


/// <summary> 
/// Entität stellt einen Ameisenhügel dar 
/// </summary> 
internal class Nest : Entity {
    public Nest(int xPos, int yPos) : base(xPos, yPos) {
        this.entitySymbol = "H"; //H = Ameisen Hügel
        this.x = xPos;
        this.y = yPos;
    }
}


/// ///////////////////////////////////////////////////////////////////////////////////////////////////// 
/// eine normale Ameise welche in das Objekt "World" gelistet werden kann. 
/// normale Ameisen bewegen sich zufällig und haben keine weitere Aufgabe.
internal class Ant : Entity {

    protected bool isAlive;
    protected AntType antType;
    protected int energy;
    protected int hiveCoordinateX;
    protected int hiveCoordinateY;
    protected int maxAge;
    Log log = new Log("Ant.cs");
    public enum AntType {
        Standard = 0,
        Queen = 1,
        Worker = 2,
    }
    public Ant(int xPos, int yPos) : base(xPos, yPos) {
        this.isAlive = true;
        this.entitySymbol = "@";
        this.x = xPos;
        this.y = yPos;
        this.hiveCoordinateX = xPos;
        this.hiveCoordinateY = yPos;
        this.antType = AntType.Standard;
        this.canMoveOnItsOwn = true;
        this.energy = GetRandomLevel(500, 5000);
        this.maxAge = GetRandomLevel(4000, 6000);
        this.speed = 40;
    }
    protected int GetRandomLevel(int minLevel, int maxLevel) {
        return rand.Next(minLevel, maxLevel);
    }
    virtual protected void HiveVisit() {
        ReplenishEnergy();
    }
    protected void ReplenishEnergy() {
        this.energy = GetRandomLevel(500, 5000);
        entityColor = Color.Green;
    }
    override public void PassOneCycle() {
        IncreaseAge();
        if (this.antType != AntType.Queen) {
            this.energy -= 1;
            if (this.age > this.maxAge) {
                this.SetDestroyable();
            }
        }
        if (GetX() == hiveCoordinateX && GetY() == hiveCoordinateY) {
            HiveVisit();
        }
        if (energy < 1000 && energy >= 500) {
            entityColor = Color.DarkYellow;
        }
        if (energy < 500) {
            entityColor = Color.Red;
        }
    }
    public override void MoveOneIntelligent() {
        if (this.speedDistanceRemaining > 0) {
            this.speedDistanceRemaining -= this.speed;
            return;
        }
        else {
            this.speedDistanceRemaining = speedStartDistance;
        }
        if (energy < 500) {
            MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
        }
        else {
            MoveOneRandom();
        }
    }
    protected void MoveOneTowards(int destinationX, int destinationY) {
        //the ant simulates a shortest route with this algorithm 
        int diffX = destinationX - this.x;
        int diffY = destinationY - this.y;
        if (Math.Abs(diffX) > Math.Abs(diffY)) {
            if (diffX > 0) {
                this.MoveOneRight();
            }
            if (diffX < 0) {
                this.MoveOneLeft();
            }
        }
        else {
            if (diffY > 0) {
                this.MoveOneDown();
            }
            if (diffY < 0) {
                this.MoveOneUp();
            }
        }
    }
}    

//////////////////////////////////////////////////////////////////////////// 
///Die Königin unterscheidet sich von der Ameise in Lebensdauer und fehlender Fortbewegung
///Später wird hier das Legen von Eiern implementiert
internal class QueenAnt : Ant {
    public QueenAnt(int xPos, int yPos) : base(xPos, yPos) {
        this.isAlive = true;
        this.entitySymbol = "Q";
        this.x = xPos;
        this.y = yPos;
        this.hiveCoordinateX = xPos;
        this.hiveCoordinateY = yPos;
        this.antType = AntType.Queen;
        this.canMoveOnItsOwn = false;
        this.energy = GetRandomLevel(500, 5000);
        this.maxAge = GetRandomLevel(400000, 600000);
        this.speed = 0;
    }
}     

///////////////////////////////////////////////////////////////////////////////////////// 
///Arbeiterameisen zeichnen sich durch schnellere Bewegungsgeschwindigkeit und Traglast aus
///Später wird hier implementiert, dass sie konkrete Futter-Objekttypen zum Hügel zurück tragen
internal class WorkerAnt : Ant {

    protected int carryStrength;
    protected int carryCount;
    Log log = new Log("WorkerAnt.cs");
    
    public WorkerAnt(int xPos, int yPos) : base(xPos, yPos) {
        this.isAlive = true;
        //use another "a" symbol for carrying items, from this website: 
        //https://www.rapidtables.com/code/text/unicode-characters.html 
        /// â = \u00E2 
        /// ã = \u00E3 
        /// ä = \u00E4 
        /// à = \u00E0 
        /// å = \u00E5 
        /// æ = \u00E6 
        /// ¤ = \u00A4 
        this.entitySymbol = "a";
        this.antType = AntType.Worker;
        this.canMoveOnItsOwn = true;
        this.energy = GetRandomLevel(500, 5000);
        this.maxAge = GetRandomLevel(4000, 6000);
        this.speed = 80;
        this.carryStrength = 2; //can carry 2 food 
        this.carryCount = 0;
    }
    
    public override void MoveOneIntelligent() {
        if (this.speedDistanceRemaining > 0) {
            this.speedDistanceRemaining -= this.speed;
            return;
        }
        else {
            this.speedDistanceRemaining = speedStartDistance;
        }
        if (energy < 500) {
            MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
        }
        else {
            if (carryCount >= carryStrength) {
                MoveOneTowards(this.hiveCoordinateX, this.hiveCoordinateY);
            }
            else {
                MoveOneRandom();
                //MoveOneTowardsFood(); 
            }
        }
    }

    //Für spätere Implementierung vorgesehen
    private void MoveOneTowardsFood() {
    }

    override protected void HiveVisit() {
        base.HiveVisit();
        UnloadFood();
    }

    protected void UnloadFood() {
        this.carryCount = 0;
    }
}  


/// <summary> 
/// Für Debugzwecke haben diese Ameisen unnatürliche Verhaltensmuster, sie laufen direkt zum Weltrand
/// </summary> 
internal class DebugAnt : WorkerAnt {

    int id;
    static int idCount = 0;
    
    public DebugAnt(int xPos, int yPos) : base(xPos, yPos) {
        this.isAlive = true;
        this.entitySymbol = "æ";
        id = DebugAnt.idCount;
        DebugAnt.idCount++;
    }

    public override void MoveOneIntelligent() {
        MoveOneToScreen(id);
    }

    /// <summary> 
    /// Finde den Bildschirmrand 
    /// </summary> 
    /// <param name="direction">0=up, 1=down, 2=right, 3=left</param> 
    public void MoveOneToScreen(int direction = 0) {
        if (direction == 0) {
            MoveOneUp();
        }
        if (direction == 1) {
            MoveOneDown();
        }
        if (direction == 2) {
            MoveOneRight();
        }
        if (direction == 3) {
            MoveOneLeft();
        }
    }
}


/////////////////////////////////////////////////////////////////////////////////////////// 
/// <summary> 
/// Logging Klasse 
/// </summary> 
internal class Log {

    static List<string> messages = new List<string>();
    private static string logFileName = "antAppLog.txt";
    static bool newLog = true;

    enum MessageType {
        Information,
        Message,
        Warning,
        Error
    }

    private string sourceObjectDescription;
    
    public Log(string sourceObjectDescription) {
        this.sourceObjectDescription = sourceObjectDescription;
        if (newLog) {
            this.AddHeader();
            newLog = false;
        }
    }
    
    public void AddError(string message) {
        String text = GetTime() + "### ERROR [" + sourceObjectDescription + "]:" + message + "###";
        messages.Add(text);
        WriteToLogFile(text);
    }
    public void AddWarning(string message) {
        String text = GetTime() + "WARNING [" + sourceObjectDescription + "]:" + message;
        messages.Add(text);
        WriteToLogFile(text);
    }
    public void Add(string message) {
        String text = GetTime() + "Info [" + sourceObjectDescription + "]:" + message;
        messages.Add(text);
        WriteToLogFile(text);
    }
    private void AddHeader() {
        String text = "\n### Log (" + System.DateTime.Now.ToString() + ") ###\n";
        messages.Add(text);
        WriteToLogFile(text);
    }
    private string GetTime() {
        return "[" + System.DateTime.Now.TimeOfDay.ToString() + "] ";
    }
    private static string GetFile() {
        //Set Directory  
        // This will get the current WORKING directory (i.e. \bin\Debug) 
        string workingDirectory = Environment.CurrentDirectory;
        // or: Directory.GetCurrentDirectory() gives the same result  
        // This will get the current PROJECT bin directory (ie ../bin/) 
        string projectDirectoryA = Directory.GetParent(workingDirectory).Parent.FullName;
        // This will get the current PROJECT directory 
        string projectDirectoryB = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        String filepath = projectDirectoryA + "/" + logFileName;
        return filepath;
    }
    
    private static void WriteToLogFile(String text) {
        string file = GetFile();
        if (File.Exists(file)) {
            using (StreamWriter sw = File.AppendText(file)) {
                sw.WriteLine(text);
            }
        }
        else {
            using (StreamWriter sw = File.CreateText(file)) {
                sw.WriteLine(text);
            }
        }
    }
}

