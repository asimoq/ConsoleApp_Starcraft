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
        public char WhosTurn;
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
            WhosTurn = 'T';
        }
    }

    internal class Units
    {
        public int Lives;
        public int AttackStrength;
        public int Range;
        public int PopultaionSize;
        public int TimeToMake;

        public int MaxNumberOfUnits;
        public int[] Coordinates = new int[2];
        public int TimeTillNextUnit;
        public int UnitID;
    }

    internal class Marauders : Units
    {
        public Marauders(int x, int y, int unitid)
        {
            Coordinates[0] = x;
            Coordinates[1] = y;
            Lives = 5;
            AttackStrength = 3;
            Range = 1;
            PopultaionSize = 3;
            TimeToMake = 3;
            MaxNumberOfUnits = 2;
            UnitID = unitid;
        }
    }

    internal class CacoDemons : Units
    {
        public CacoDemons(int x, int y, int unitid)
        {
            Coordinates[0] = x;
            Coordinates[1] = y;
            Lives = 3;
            AttackStrength = 5;
            Range = 3;
            PopultaionSize = 5;
            TimeToMake = 4;
            MaxNumberOfUnits = 1;
            UnitID = unitid;
        }
    }

    internal class SnakesWithKnives : Units
    {
        public SnakesWithKnives(int x, int y, int unitid)
        {
            Coordinates[0] = x;
            Coordinates[1] = y;
            Lives = 2;
            AttackStrength = 1;
            Range = 1;
            PopultaionSize = 2;
            TimeToMake = 1;
            MaxNumberOfUnits = 5;
            UnitID = unitid;
        }
    }

    internal class Cursor
    {
        public int[] CursosCoordinates = new int[2];
        public char HiddenData;

        public Cursor()
        {
            CursosCoordinates[0] = 5;
            CursosCoordinates[1] = 4;
            HiddenData = '0'; // data hidden by the cursor
        }

        public static void CursorMovement(Playingfield PF, Cursor cursor)
        {
            bool exit = true;
            while (exit)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursor.CursosCoordinates[0] - 1 < 0) break;
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[0] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        Frontend.FieldRender(PF);
                        Cursor.Selection(PF, cursor);
                        break;

                    case ConsoleKey.DownArrow:
                        if (cursor.CursosCoordinates[0] + 1 == 10) break;
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[0] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        Frontend.FieldRender(PF);
                        Cursor.Selection(PF, cursor);
                        break;

                    case ConsoleKey.LeftArrow:
                        if (cursor.CursosCoordinates[1] - 1 < 0) break;
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[1] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        Frontend.FieldRender(PF);
                        Cursor.Selection(PF, cursor);
                        break;

                    case ConsoleKey.RightArrow:
                        if (cursor.CursosCoordinates[1] + 1 == 10) break;
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[1] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        Frontend.FieldRender(PF);
                        Cursor.Selection(PF, cursor);
                        break;

                    case ConsoleKey.Backspace:
                        exit = false;
                        Console.Clear();
                        Console.WriteLine("Press enter to exit the application.");
                        break;

                    default:
                        {
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.Write("Press the Backspace for exit, or use the arrow keys to move the cursor");
                            Console.SetCursorPosition(0, Console.CursorTop);
                            break;
                        }
                }
            }
        }

        public static void Selection(Playingfield PF, Cursor cursor)
        {
            switch (PF.StateOfThePF[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]])
            {
                case '•':
                    {
                        Console.SetCursorPosition(0, 10);
                        Console.WriteLine("M - place Mandruders" +
                            "\nC - place Cacodemons" +
                            "\nS - place Snakes With Knives");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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
            Cursor cursor = new Cursor();
            Cursor.CursorMovement(PF, cursor);

            Console.ReadLine();
        }
    }
}