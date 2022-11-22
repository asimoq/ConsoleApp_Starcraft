using System;

namespace NagyHázi_Starcraft
{
    internal class Frontend
    {
        protected Frontend()
        {
        }

        public static void SlowPrint(string Text, int speed = 100)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                Console.Write(Text[i]);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }

        public static void FastConsoleClear()
        {
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }

        public static void FieldRender(Playingfield PF)
        {
            FastConsoleClear();
            Console.WriteLine(PF.WhosTurn + "'s turn!");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (PF.PfData[i, j])
                    {
                        case '0':
                            {
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.Write(' ');
                                break;
                            }

                        case 'Z':
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.ResetColor();
                                Console.Write(' ');
                                break;
                            }

                        case 'T':
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.ResetColor();
                                Console.Write(' ');
                                break;
                            }

                        case 'X':
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.ResetColor();
                                Console.Write(' ');
                                break;
                            }

                        default:
                            {
                                Console.ResetColor();
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.Write(' ');
                                break;
                            }
                    }
                }
                Console.WriteLine("");
            }
        }

        public static void Options(Playingfield PF, Interacrions cursor, Players player, ref bool pass)
        {
            Console.WriteLine("Your current population: " + player.CurrentPopulation + " (max population: " + player.MaxPopulation + " )");
            Console.WriteLine("You have " + player.CurrentActtionsRemaining + "Actions remaining");
            switch (PF.StateOfThePF[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]])
            {
                case '•':
                    {
                        if (cursor.CursosCoordinates[0] == 0)
                            Console.WriteLine("M - place Mandruders" +
                                "\nC - place Cacodemons" +
                                "\nS - place Snakes With Knives");
                        Console.WriteLine("move the selection with the arrow keys or press backspace to pass");

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (player.CurrentActtionsRemaining == 0) pass = true;
        }
    }

    internal class Playingfield
    {
        public char[,] StateOfThePF = new char[10, 10]; // itt kódon belül beállítható a pályaméret, ennek meg kell egyezni a PfData pálya méretével. Ebben a táblában található karaktereket foglya megjeleníteni a renderer
        public char[,] PfData = new char[10, 10]; // Ez a tábla tárolja a pályán lévő egységek adatait: Melyik csapathoz tartoznak,
        public string WhosTurn;
        public int[,] lives = new int[10, 10];

        public Playingfield()
        {
            for (int i = 0; i < StateOfThePF.GetLength(0); i++)
            {
                for (int j = 0; j < StateOfThePF.GetLength(1); j++)
                {
                    StateOfThePF[i, j] = '•';
                    PfData[i, j] = '0';
                    lives[i, j] = 0;
                }
            }
        }
    }

    internal class Units
    {
        public int Lives;
        public int AttackStrength;
        public int Range;
        public int PopultaionSize;
        public int TimeToMake;
        public int TimeTillNextUnit;
        public char UnitID;
    }

    internal class Marauders : Units
    {
        public Marauders()
        {
            Lives = 5;
            AttackStrength = 3;
            Range = 1;
            PopultaionSize = 3;
            TimeToMake = 3;
            TimeTillNextUnit = 0;
            UnitID = 'M';
        }
    }

    internal class CacoDemons : Units
    {
        public CacoDemons()
        {
            Lives = 3;
            AttackStrength = 5;
            Range = 3;
            PopultaionSize = 5;
            TimeToMake = 4;
            TimeTillNextUnit = 0;
            UnitID = 'C';
        }
    }

    internal class SnakesWithKnives : Units
    {
        public SnakesWithKnives()
        {
            Lives = 2;
            AttackStrength = 1;
            Range = 1;
            PopultaionSize = 2;
            TimeToMake = 1;
            TimeTillNextUnit = 0;
            UnitID = 'S';
        }
    }

    internal class Base : Units
    {
        public Base()
        {
            Lives = 10;
            AttackStrength = 0;
            Range = 0;
            PopultaionSize = 0;
            TimeToMake = 0;
            TimeTillNextUnit = 0;
            UnitID = 'S';
        }
    }

    internal class Interacrions
    {
        public int[] CursosCoordinates = new int[2];
        public char HiddenData;

        public Interacrions()
        {
            CursosCoordinates[0] = 5;
            CursosCoordinates[1] = 4;
            HiddenData = '0'; // data hidden by the cursor
        }

        public static void Interaction(Playingfield PF, Interacrions cursor, ref bool pass)
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    if (cursor.CursosCoordinates[0] - 1 < 0) break;
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                    cursor.CursosCoordinates[0] -= 1;
                    cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                    break;

                case ConsoleKey.DownArrow:
                    if (cursor.CursosCoordinates[0] + 1 == 10) break;
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                    cursor.CursosCoordinates[0] += 1;
                    cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';

                    break;

                case ConsoleKey.LeftArrow:
                    if (cursor.CursosCoordinates[1] - 1 < 0) break;
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                    cursor.CursosCoordinates[1] -= 1;
                    cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';

                    break;

                case ConsoleKey.RightArrow:
                    if (cursor.CursosCoordinates[1] + 1 == 10) break;
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                    cursor.CursosCoordinates[1] += 1;
                    cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                    PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';

                    break;

                case ConsoleKey.Backspace:
                    pass = true;
                    Console.Clear();

                    break;

                default:
                    {
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        break;
                    }
            }
        }

        public static Players PlayerSelector(Playingfield PF, Players Zerg, Players Terran)
        {
            if (PF.WhosTurn == Zerg.name)
            {
                return Zerg;
            }
            else
            {
                return Terran;
            }
        }

        public static void PlaceUnit(Playingfield PF, Units unit, int x, int y)
        {
            PF.StateOfThePF[x, y] = unit.UnitID;
            PF.lives[x, y] = unit.Lives;
            if (PF.WhosTurn == "Zerg")
            {
                PF.PfData[x, y] = 'Z';
            }
            else
            {
                PF.PfData[x, y] = 'T';
            }
        }
    }

    internal class Players
    {
        public int MaxPopulation = 20;
        public int CurrentPopulation = 0;
        public int MaxNumberOfUnits = 6;
        public int CurrentNumberOfUnits = 0;
        public int MaxActions = 3;
        public int CurrentActtionsRemaining = 3;
        public string name;
        public Marauders marauders = new Marauders();
        public CacoDemons cacoDemons = new CacoDemons();
        public SnakesWithKnives snakes = new SnakesWithKnives();
        public Base Base = new Base();

        public Players(string Name)
        {
            name = Name;
        }
    }

    internal class Program
    {
        protected Program()
        {
        }

        private static void Main()
        {
            Console.CursorVisible = false;
            //initialising Unicode characters.
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Field initialization
            Playingfield PF = new Playingfield();
            Interacrions cursor = new Interacrions();
            //Player initialisation
            Players Zerg = new Players("Zerg");
            Players Terran = new Players("Terran");
            PF.WhosTurn = Zerg.name;
            bool InGame = true;
            bool Pass = false;
            Interacrions.PlaceUnit(PF, Interacrions.PlayerSelector(PF, Zerg, Terran).Base, 0, 4);
            PF.StateOfThePF[0, 4] = 'Z';
            PF.WhosTurn = Terran.name;
            Interacrions.PlaceUnit(PF, Interacrions.PlayerSelector(PF, Zerg, Terran).Base, 9, 4);
            PF.StateOfThePF[9, 4] = 'T';
            PF.WhosTurn = Zerg.name;
            while (InGame)
            {
                while (!Pass)
                {
                    Frontend.FieldRender(PF);
                    Frontend.Options(PF, cursor, Interacrions.PlayerSelector(PF, Zerg, Terran), ref Pass);
                    Interacrions.Interaction(PF, cursor, ref Pass);
                }
                Pass = false;
                if (PF.WhosTurn == Zerg.name)
                {
                    PF.WhosTurn = Terran.name;
                    Terran.CurrentActtionsRemaining = Terran.MaxActions;
                }
                else
                {
                    PF.WhosTurn = Zerg.name;
                    Zerg.CurrentActtionsRemaining = Zerg.MaxActions;
                }
            }

            Console.ReadLine();
        }
    }
}