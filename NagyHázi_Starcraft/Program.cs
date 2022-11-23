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
        public int[] BaseCoordinates = new int[2];

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
        public int[] SelectedCoordinates = new int[2];

        public Interacrions()
        {
            CursosCoordinates[0] = 5;
            CursosCoordinates[1] = 4;
            HiddenData = '0'; // data hidden by the cursor
        }

        public static void CursorMovement(Playingfield PF, Interacrions cursor, ref bool pass, Players player)
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

                case ConsoleKey.P:
                    UnitPlacementOptions(PF, player, cursor.CursosCoordinates[0], cursor.CursosCoordinates[1], cursor);
                    break;

                case ConsoleKey.U:

                default:
                    {
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        break;
                    }
            }
        }

        public static Players WhosTurnItIs(Playingfield PF, Players Zerg, Players Terran)
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

        public static void PlaceUnit(Playingfield PF, Units unit, int x, int y, Interacrions cursor)
        {
            PF.StateOfThePF[x, y] = unit.UnitID;
            PF.lives[x, y] = unit.Lives;
            if (PF.WhosTurn == "Zerg")
            {
                PF.PfData[x, y] = 'Z';
                cursor.HiddenData = 'Z';
            }
            else
            {
                PF.PfData[x, y] = 'T';
                cursor.HiddenData = 'T';
            }
        }

        public static void TryToPlaceUnit(Playingfield PF, Players player, Units unit, int x, int y, Interacrions cursor)
        {
            if (unit.TimeTillNextUnit == 0 && player.MaxPopulation >= player.CurrentPopulation + unit.PopultaionSize && player.MaxNumberOfUnits >= player.CurrentNumberOfUnits + 1)
            {
                PlaceUnit(PF, unit, x, y, cursor);
                unit.TimeTillNextUnit = unit.TimeToMake;
                player.CurrentActtionsRemaining -= 1;
                player.CurrentNumberOfUnits += 1;
                player.CurrentPopulation += unit.PopultaionSize;
                Frontend.FieldRender(PF);
            }
            else
            {
                if (player.MaxPopulation < player.CurrentPopulation + unit.PopultaionSize)
                {
                    Console.WriteLine("\nCan't place that unit due to your max population size, choose a nother one");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
                if (player.marauders.TimeTillNextUnit != 0)
                {
                    Console.WriteLine("\nTnhat unit isn't ready, choose a nother one!");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
                if (player.MaxNumberOfUnits < player.CurrentNumberOfUnits + 1)
                {
                    Console.WriteLine("\nYou can't have that many units, try moving or attacking with one of your units, or pass to the other player by pressing Backspace!");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
            }
        }

        public static void UnitPlacementOptions(Playingfield PF, Players player, int x, int y, Interacrions cursor)
        {
            Console.SetCursorPosition(0, 12);
            Console.WriteLine("\nRounds till you can place your next Marauders: " + player.marauders.TimeTillNextUnit);
            Console.WriteLine("Rounds till you can place your next Snakes with Knives: " + player.snakes.TimeTillNextUnit);
            Console.WriteLine("Rounds till you can place your next CacoDemons: " + player.cacoDemons.TimeTillNextUnit);
            Console.WriteLine("\nPress M to place maruders --- Press C to place Cacodemons --- Press S to place Snakes With Knives\n\nPress anything else to quit Placement selection");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.M:
                    TryToPlaceUnit(PF, player, player.marauders, x, y, cursor);
                    break;

                case ConsoleKey.C:
                    TryToPlaceUnit(PF, player, player.cacoDemons, x, y, cursor);
                    break;

                case ConsoleKey.S:
                    TryToPlaceUnit(PF, player, player.snakes, x, y, cursor);
                    break;

                default:
                    break;
            }
        }

        public static void RoundHsPassed(Players player)
        {
            player.cacoDemons.TimeTillNextUnit -= 1;
            player.marauders.TimeTillNextUnit -= 1;
            player.snakes.TimeTillNextUnit -= 1;
            if (player.cacoDemons.TimeTillNextUnit < 0)
            {
                player.cacoDemons.TimeTillNextUnit = 0;
            }
            if (player.marauders.TimeTillNextUnit < 0)
            {
                player.marauders.TimeTillNextUnit = 0;
            }
            if (player.snakes.TimeTillNextUnit < 0)
            {
                player.snakes.TimeTillNextUnit = 0;
            }
            player.CurrentActtionsRemaining = player.MaxActions;
        }

        public static void Options(Playingfield PF, Interacrions cursor, Players player, ref bool pass)
        {
            Console.WriteLine("\nYour current population: " + player.CurrentPopulation + " (max population: " + player.MaxPopulation + " )");
            Console.WriteLine("\nYou have " + player.CurrentActtionsRemaining + " Actions remaining");
            switch (PF.StateOfThePF[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]])
            {
                case '•':
                    {
                        if (cursor.CursosCoordinates[0] < player.Base.BaseCoordinates[0] + 4 && cursor.CursosCoordinates[0] > player.Base.BaseCoordinates[0] - 4)
                        {
                            Console.WriteLine("\npress P to select this field to place a new unit!");
                        }
                        else
                        {
                            Console.WriteLine("\nYou must be in your Base's range to place a new unit!");
                        }

                        break;
                    }
                case 'Z':
                case 'T':
                    Console.WriteLine("Health of this unit is: " + PF.lives[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]]);
                    break;

                default:
                    {
                        Console.WriteLine("Health XDXDof this unit is: " + PF.lives[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]]);

                        break;
                    }
            }
            if (player.CurrentActtionsRemaining == 0) pass = true;
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
        public char TeamID;
        public Marauders marauders = new Marauders();
        public CacoDemons cacoDemons = new CacoDemons();
        public SnakesWithKnives snakes = new SnakesWithKnives();
        public Base Base = new Base();

        public Players(string Name, char teamid)
        {
            name = Name;
            TeamID = teamid;
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

            Players Zerg = new Players("Zerg", 'Z');
            Players Terran = new Players("Terran", 'T');
            PF.WhosTurn = Zerg.name;
            bool InGame = true;
            bool Pass = false;
            Interacrions.PlaceUnit(PF, Interacrions.WhosTurnItIs(PF, Zerg, Terran).Base, 0, 4, cursor);
            PF.StateOfThePF[0, 4] = 'Z';
            Zerg.Base.BaseCoordinates[0] = 0; Zerg.Base.BaseCoordinates[1] = 4;
            PF.WhosTurn = Terran.name;
            Interacrions.PlaceUnit(PF, Interacrions.WhosTurnItIs(PF, Zerg, Terran).Base, 9, 4, cursor);
            PF.StateOfThePF[9, 4] = 'T';
            Terran.Base.BaseCoordinates[0] = 9; Terran.Base.BaseCoordinates[1] = 4;
            PF.WhosTurn = Zerg.name;
            cursor.HiddenData = '0';
            while (InGame)
            {
                while (!Pass)
                {
                    Frontend.FieldRender(PF);
                    Interacrions.Options(PF, cursor, Interacrions.WhosTurnItIs(PF, Zerg, Terran), ref Pass);
                    Interacrions.CursorMovement(PF, cursor, ref Pass, Interacrions.WhosTurnItIs(PF, Zerg, Terran));
                }
                Interacrions.RoundHsPassed(Interacrions.WhosTurnItIs(PF, Zerg, Terran));
                Pass = false;
                if (PF.WhosTurn == Zerg.name)
                {
                    PF.WhosTurn = Terran.name;
                }
                else
                {
                    PF.WhosTurn = Zerg.name;
                }
            }

            Console.ReadLine();
        }
    }
}