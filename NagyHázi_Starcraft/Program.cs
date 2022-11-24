using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

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
            for (int i = 0; i < 25; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }

        public static void FieldRender(Playingfield PF, int mode = 2)
        {
            FastConsoleClear();
            if (PF.WhosTurn == "Terran") Console.ForegroundColor = ConsoleColor.Blue;
            if (PF.WhosTurn == "Zerg") Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(PF.WhosTurn + "'s Turn!");
            if (mode == 1) Console.Write(" -- To Save game status press [Enter] -- To Exit press [Escape]");
            Console.ResetColor();
            Console.SetCursorPosition(0, 2);
            for (int i = 0; i < 10; i++)
            {
                Console.Write("             ");
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
                        case 'M':
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
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

    [Serializable]
    internal class Playingfield
    {
        public char[,] StateOfThePF = new char[10, 10]; // itt kódon belül beállítható a pályaméret, ennek meg kell egyezni a PfData pálya méretével. Ebben a táblában található karaktereket foglya megjeleníteni a renderer
        public char[,] PfData = new char[10, 10]; // Ez a tábla tárolja a pályán lévő egységek adatait: Melyik csapathoz tartoznak,
        public string WhosTurn;
        public int[,] lives = new int[10, 10];
        public int[,] range = new int[10, 10];
        public int[,] strength = new int[10, 10];

        public Playingfield()
        {
            for (int i = 0; i < StateOfThePF.GetLength(0); i++)
            {
                for (int j = 0; j < StateOfThePF.GetLength(1); j++)
                {
                    StateOfThePF[i, j] = '•';
                    PfData[i, j] = '0';
                    lives[i, j] = 0;
                    range[i, j] = 0;
                    strength[i, j] = 0;
                }
            }
        }
    }

    [Serializable]
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

    [Serializable]
    internal class Marauders : Units
    {
        public Marauders()
        {
            Lives = 6;
            AttackStrength = 3;
            Range = 3;
            PopultaionSize = 3;
            TimeToMake = 3;
            TimeTillNextUnit = 0;
            UnitID = 'M';
        }
    }

    [Serializable]
    internal class CacoDemons : Units
    {
        public CacoDemons()
        {
            Lives = 4;
            AttackStrength = 5;
            Range = 3;
            PopultaionSize = 5;
            TimeToMake = 4;
            TimeTillNextUnit = 0;
            UnitID = 'C';
        }
    }

    [Serializable]
    internal class SnakesWithKnives : Units
    {
        public SnakesWithKnives()
        {
            Lives = 2;
            AttackStrength = 2;
            Range = 1;
            PopultaionSize = 2;
            TimeToMake = 1;
            TimeTillNextUnit = 0;
            UnitID = 'S';
        }
    }

    [Serializable]
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

    [Serializable]
    internal class Interacrions
    {
        private readonly int[] CursorCoordinates = new int[2];
        public char HiddenData;
        public int[] SelectedCoordinates = new int[2];
        public int Mode; //1. mode movement 2.mode attack

        public Interacrions()
        {
            CursorCoordinates[0] = 5;
            CursorCoordinates[1] = 4;
            HiddenData = '0'; // data hidden by the cursor
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

        public static void RoundHasPassed(Players player)
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

        private static void FileSave(Playingfield PF, Players Zerg, Players Terran, Interacrions cursor)
        {
            Console.WriteLine();
            Frontend.SlowPrint("Saving Gamestate...");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream pfdata = File.Create("PF.dat");
            FileStream zergdata = File.Create("Zerg.dat");
            FileStream terrandata = File.Create("Terran.dat");
            FileStream cursordata = File.Create("cursor.dat");

            bf.Serialize(pfdata, PF);
            bf.Serialize(zergdata, Zerg);
            bf.Serialize(terrandata, Terran);
            bf.Serialize(cursordata, cursor);
            pfdata.Close();
            zergdata.Close();
            terrandata.Close();
            cursordata.Close();
        }

        public static void FileLoad(ref Playingfield PF, ref Players Zerg, ref Players Terran, ref Interacrions cursor)
        {
            Console.WriteLine();
            Frontend.SlowPrint("Loading save...");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream pfdata = File.Open("PF.dat", FileMode.Open);
            FileStream zergdata = File.Open("Zerg.dat", FileMode.Open);
            FileStream terrandata = File.Open("Terran.dat", FileMode.Open);
            FileStream cursordata = File.Open("cursor.dat", FileMode.Open);

            Playingfield tempPF = (Playingfield)bf.Deserialize(pfdata); pfdata.Close();
            Players tempZerg = (Players)bf.Deserialize(zergdata); zergdata.Close();
            Players tempTerran = (Players)bf.Deserialize(terrandata); terrandata.Close();
            Interacrions tempCursor = (Interacrions)bf.Deserialize(cursordata); cursordata.Close();

            PF = tempPF;
            Zerg = tempZerg;
            Terran = tempTerran;
            cursor = tempCursor;
        }

        public static void CursorMovement(Playingfield PF, Interacrions cursor, ref bool pass, Players player, int range, Players Zerg, Players Terran, int mode)
        {
            if (player.CurrentActtionsRemaining == 0) pass = true;
            if (!pass) switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursor.CursorCoordinates[0] - 1 < 0 || cursor.CursorCoordinates[0] - 1 < cursor.SelectedCoordinates[0] - range) break;
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = cursor.HiddenData;
                        cursor.CursorCoordinates[0] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]];
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 'X';
                        break;

                    case ConsoleKey.DownArrow:
                        if (cursor.CursorCoordinates[0] + 1 == 10 || cursor.CursorCoordinates[0] + 1 > cursor.SelectedCoordinates[0] + range) break;
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = cursor.HiddenData;
                        cursor.CursorCoordinates[0] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]];
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 'X';

                        break;

                    case ConsoleKey.LeftArrow:
                        if (cursor.CursorCoordinates[1] - 1 < 0 || cursor.CursorCoordinates[1] - 1 < cursor.SelectedCoordinates[1] - range) break;
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = cursor.HiddenData;
                        cursor.CursorCoordinates[1] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]];
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 'X';

                        break;

                    case ConsoleKey.RightArrow:
                        if (cursor.CursorCoordinates[1] + 1 == 10 || cursor.CursorCoordinates[1] + 1 > cursor.SelectedCoordinates[1] + range) break;
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = cursor.HiddenData;
                        cursor.CursorCoordinates[1] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]];
                        PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 'X';

                        break;

                    case ConsoleKey.Backspace:
                        pass = true;
                        Console.Clear();

                        break;

                    case ConsoleKey.P:
                        if (mode != 1) break;
                        if (cursor.HiddenData != '0') break;
                        Frontend.FieldRender(PF);
                        if (cursor.CursorCoordinates[0] < player.Base.BaseCoordinates[0] + 4 && cursor.CursorCoordinates[0] > player.Base.BaseCoordinates[0] - 4)
                        {
                            UnitPlacementOptions(PF, player, cursor.CursorCoordinates[0], cursor.CursorCoordinates[1], cursor);
                        }
                        break;

                    case ConsoleKey.M:
                        if (mode != 1) break;
                        if (PF.StateOfThePF[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] == player.TeamID) break; //you cant move your base
                        if (cursor.HiddenData != player.TeamID) break; //you cant move the enemies units
                        UnitMovementOptions(PF, cursor, player, ref pass);
                        break;

                    case ConsoleKey.A:
                        if (mode != 1) break;
                        if (PF.StateOfThePF[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] == player.TeamID) break; //you cant attack with your base
                        if (cursor.HiddenData != player.TeamID) break; //you cant attack with the enemies unit
                        UnitAttackOptions(PF, cursor, player, ref pass);
                        break;

                    case ConsoleKey.S:
                        if (mode != 2) break;
                        if (cursor.HiddenData != '0') break;
                        UnitMovementEffective(PF, cursor);
                        player.CurrentActtionsRemaining -= 1;
                        pass = true;
                        break;

                    case ConsoleKey.T:
                        if (mode != 3) break;
                        if (cursor.HiddenData == player.TeamID || cursor.HiddenData == '0') break; // cant attack your own uit, cant attack empty field
                        pass = UnitAttackEffective(PF, cursor, player);
                        break;

                    case ConsoleKey.Enter:
                        if (mode != 1) break;
                        FileSave(PF, Zerg, Terran, cursor);
                        break;

                    case ConsoleKey.Escape:
                        if (mode != 1) break;
                        PF.lives[0, 4] = 0;
                        PF.lives[9, 4] = 0;
                        pass = true;
                        break;

                    default:
                        {
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            break;
                        }
                }
            if (PF.lives[0, 4] < 1 || PF.lives[9, 4] < 1) pass = true;
        }

        public static void Options(Playingfield PF, Interacrions cursor, Players player)
        {
            Console.WriteLine("\nYour current population: " + player.CurrentPopulation + " (max population: " + player.MaxPopulation + " )");
            Console.Write("\nYou have " + player.CurrentActtionsRemaining + " Actions remaining -- Press ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Backspace]");
            Console.ResetColor();
            Console.Write(" to pass");

            switch (PF.StateOfThePF[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]])
            {
                case '•':
                    {
                        if (cursor.CursorCoordinates[0] < player.Base.BaseCoordinates[0] + 4 && cursor.CursorCoordinates[0] > player.Base.BaseCoordinates[0] - 4)
                        {
                            Console.Write("\nPress ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("[P]"); Console.ResetColor();
                            Console.Write(" to select this field to place a new unit!");
                        }
                        else
                        {
                            Console.WriteLine("\nYou must be in your Base's range to place a new unit!");
                        }

                        break;
                    }
                case 'Z':
                case 'T':
                    Console.WriteLine("\nHealth of this unit is: " + PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]]);
                    break;

                default:
                    Console.WriteLine("\nHealth of this unit is: " + PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]]);
                    if (cursor.HiddenData == player.TeamID)
                    {
                        Console.Write("\nPress ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[M]"); Console.ResetColor();
                        Console.Write(" to Move with this unit --- Press ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("[A]"); Console.ResetColor();
                        Console.Write(" to attack with this Unit");
                    }
                    break;
            }
        }

        private static void UnitMovementOptions(Playingfield PF, Interacrions cursor, Players player, ref bool pass)
        {
            cursor.SelectedCoordinates[0] = cursor.CursorCoordinates[0];
            cursor.SelectedCoordinates[1] = cursor.CursorCoordinates[1];
            while (!pass)
            {
                Frontend.FieldRender(PF);
                Console.Write("\nUse the ARROW KEYS to select an area, then press ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[S]"); Console.ResetColor();
                Console.Write(" to finalize your selection\nor Press ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[BackSpace]"); Console.ResetColor();
                Console.Write(" to exit selection ");
                CursorMovement(PF, cursor, ref pass, player, 1, player, player, 2);
            }
            pass = false;
            Frontend.FieldRender(PF);
        }

        private static void UnitMovementEffective(Playingfield PF, Interacrions cursor)
        {
            PF.StateOfThePF[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = PF.StateOfThePF[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = PF.PfData[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = PF.lives[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            PF.strength[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = PF.strength[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            PF.range[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = PF.range[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            cursor.HiddenData = PF.PfData[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
            PF.StateOfThePF[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]] = '•';
            PF.PfData[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]] = '0';
            PF.lives[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]] = 0;
            PF.strength[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]] = 0;
            PF.range[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]] = 0;
        }

        private static void UnitPlacementOptions(Playingfield PF, Players player, int x, int y, Interacrions cursor)
        {
            Console.SetCursorPosition(0, 14);
            Console.WriteLine("Rounds till you can place your next Marauders: " + player.marauders.TimeTillNextUnit + "                                  ");
            Console.WriteLine("Rounds till you can place your next Snakes with Knives: " + player.snakes.TimeTillNextUnit);
            Console.WriteLine("Rounds till you can place your next CacoDemons: " + player.cacoDemons.TimeTillNextUnit);
            Console.Write("\nPress "); Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[M]"); Console.ResetColor();
            Console.Write(" to place maruders --- Press ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[C]"); Console.ResetColor();
            Console.Write(" to place Cacodemons --- Press ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[S]"); Console.ResetColor();
            Console.Write(" to place Snakes With Knives\n\nPress ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[BackSpace]"); Console.ResetColor();
            Console.Write(" to quit Placement selection");

            switch (Console.ReadKey(true).Key)
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

                case ConsoleKey.Backspace:
                    break;

                default:
                    UnitPlacementOptions(PF, player, x, y, cursor);
                    break;
            }
        }

        private static void TryToPlaceUnit(Playingfield PF, Players player, Units unit, int x, int y, Interacrions cursor)
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
                Frontend.FieldRender(PF);
                Console.SetCursorPosition(0, 20);
                if (player.MaxPopulation < player.CurrentPopulation + unit.PopultaionSize)
                {
                    Console.WriteLine("\nCan't place that Unit due to your max population size, choose a nother one");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
                else if (unit.TimeTillNextUnit != 0)
                {
                    Console.WriteLine("\nThat Unit isn't ready, choose a nother one!");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
                else if (player.MaxNumberOfUnits < player.CurrentNumberOfUnits + 1)
                {
                    Console.WriteLine("\nYou can't have that many units, try moving or attacking with one of your units, or pass to the other player by pressing ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[BackSpace]"); Console.ResetColor();
                    Console.Write("!");
                    UnitPlacementOptions(PF, player, x, y, cursor);
                }
            }
        }

        public static void PlaceUnit(Playingfield PF, Units unit, int x, int y, Interacrions cursor)
        {
            PF.StateOfThePF[x, y] = unit.UnitID;
            PF.lives[x, y] = unit.Lives;
            PF.strength[x, y] = unit.AttackStrength;
            PF.range[x, y] = unit.Range;
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

        private static void UnitAttackOptions(Playingfield PF, Interacrions cursor, Players player, ref bool pass)
        {
            cursor.SelectedCoordinates[0] = cursor.CursorCoordinates[0];
            cursor.SelectedCoordinates[1] = cursor.CursorCoordinates[1];

            Console.Write("\nUse the ARROW KEYS to select an area, then press ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[T]"); Console.ResetColor();
            Console.Write(" to finalize your selection\nor Press ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Backspace]"); Console.ResetColor();
            Console.Write(" to exit selection");

            while (!pass)
            {
                Frontend.FieldRender(PF);
                Console.Write("\nUse the ARROW KEYS to select an area, then press ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[T]"); Console.ResetColor();
                Console.Write(" to finalize your selection\nor Press ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[BackSpace]"); Console.ResetColor();
                Console.Write(" to exit selection ");
                CursorMovement(PF, cursor, ref pass, player, PF.range[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]], player, player, 3);
            }
            pass = false;
        }

        private static bool UnitAttackEffective(Playingfield PF, Interacrions cursor, Players player)
        {
            bool pass;
            if (cursor.HiddenData == player.TeamID || cursor.HiddenData == '0')
            {
                Console.WriteLine("You can't attack there!");
                pass = true;
            }
            else
            {
                PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] -= PF.strength[cursor.SelectedCoordinates[0], cursor.SelectedCoordinates[1]];
                if (PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] < 1)
                {
                    PF.StateOfThePF[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = '•';
                    PF.PfData[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = '0';
                    PF.lives[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 0;
                    PF.strength[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 0;
                    PF.range[cursor.CursorCoordinates[0], cursor.CursorCoordinates[1]] = 0;
                    cursor.HiddenData = '0';
                }
                player.CurrentActtionsRemaining -= 1;
                pass = true;
            }

            return pass;
        }
    }

    [Serializable]
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

    internal class Menu
    {
        private static void Rules()
        {
            Frontend.FastConsoleClear();
            string text = System.IO.File.ReadAllText("Rules.txt");
            Frontend.SlowPrint(text, 10);
            Console.WriteLine("\nPress ANY KEY to head back to the Menu");
            Console.ReadKey(true);
        }

        private static void Cretids()
        {
            Frontend.FastConsoleClear();
            string text = System.IO.File.ReadAllText("Credits.txt");
            Frontend.SlowPrint(text, 10);
            Console.WriteLine("\nPress ANY KEY to head back to the Menu");
            Console.ReadKey(true);
        }

        public static void MenuRender(ref bool load, ref int selector)
        {
            Frontend.FastConsoleClear();
            Console.WriteLine("   _____ __             ______           ______ " +
                            "\n  / ___// /_____ ______/ ____/________ _/ __/ /_" +
                            "\n  \\__ \\/ __/ __ `/ ___/ /   / ___/ __ `/ /_/ __/" +
                            "\n ___/ / /_/ /_/ / /  / /___/ /  / /_/ / __/ /_  " +
                            "\n/____/\\__/\\__,_/_/   \\____/_/   \\__,_/_/  \\__/" +
                            "\n V_1.0");
            if (selector == 1) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n\n              Start a New Game");
            Console.ResetColor();
            if (selector == 2) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n            Load a previous Game");
            Console.ResetColor();
            if (selector == 3) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n                   Rules");
            Console.ResetColor();
            if (selector == 4) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n                  Credits");
            Console.ResetColor();
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.DownArrow:
                    if (selector < 4) selector += 1;
                    MenuRender(ref load, ref selector);
                    break;

                case ConsoleKey.UpArrow:
                    if (selector > 1) selector -= 1;
                    MenuRender(ref load, ref selector);
                    break;

                case ConsoleKey.Enter:
                    switch (selector)
                    {
                        case 1:
                            load = false;
                            break;

                        case 2:
                            load = true;
                            break;

                        case 3:

                            Rules();
                            MenuRender(ref load, ref selector);
                            break;

                        case 4:
                            Cretids();
                            MenuRender(ref load, ref selector);
                            break;

                        default:
                            MenuRender(ref load, ref selector);
                            break;
                    }
                    break;

                default:
                    MenuRender(ref load, ref selector);
                    break;
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
            Console.WindowHeight = 27;
            Console.WindowWidth = 106;
            bool ingame = true;
            while (ingame)
            {
                bool load = false;
                int menuselector = 1;
                Console.CursorVisible = false;
                Menu.MenuRender(ref load, ref menuselector);

                //initialising Unicode characters.
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                //menu:
                //Game initialization
                Playingfield PF = new Playingfield();
                Interacrions cursor = new Interacrions();
                Players Zerg = new Players("Zerg", 'Z');
                Players Terran = new Players("Terran", 'T');
                PF.WhosTurn = Zerg.name;
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
                PF.PfData[5, 4] = 'X';
                //in a game
                while (PF.lives[0, 4] > 0 && PF.lives[9, 4] > 0)
                {
                    while (!Pass) //one round
                    {
                        if (load)
                        {
                            Interacrions.FileLoad(ref PF, ref Zerg, ref Terran, ref cursor);
                            load = false;
                        }
                        Frontend.FieldRender(PF, 1);
                        Interacrions.Options(PF, cursor, Interacrions.WhosTurnItIs(PF, Zerg, Terran));
                        Interacrions.CursorMovement(PF, cursor, ref Pass, Interacrions.WhosTurnItIs(PF, Zerg, Terran), 100, Zerg, Terran, 1);
                    }
                    //end of a player's round
                    Interacrions.RoundHasPassed(Interacrions.WhosTurnItIs(PF, Zerg, Terran));
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
                //Game end
                if (PF.lives[0, 4] < 1 && PF.lives[9, 4] < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Thanks for playing!");
                }
                else
                {
                    if (PF.lives[0, 4] < 1) Console.WriteLine("Terran WIN!");
                    else Console.WriteLine("Zerg WIN!");
                    Console.WriteLine("Thanks for playing!");
                }
                Console.WriteLine("\nPress Enter to go back to the Menu!\nTo Exit the application press ANY BUTTON");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        ingame = true;
                        break;

                    default:
                        ingame = false;
                        break;
                }
            }
        }
    }
}