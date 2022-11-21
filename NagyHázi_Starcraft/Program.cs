using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NagyHázi_Starcraft
{
    
    class Playingfield
    {
        
        
        public char[,] StateOfThePF = new char[10,10]; // itt kódon belül beállítható a pályaméret, ennek meg kell egyezni a PfData pálya méretével. Ebben a táblában található karaktereket foglya megjeleníteni a renderer
        public char[,] PfData = new char[10, 10]; // Ez a tábla tárolja a pályán lévő egységek adatait: Melyik csapathoz tartoznak, 

        public Playingfield()
        {
            for (int i = 0; i < StateOfThePF.GetLength(0); i++)
            {

                for (int j = 0; j < StateOfThePF.GetLength(1); j++)
                {
                    StateOfThePF[i, j] = '•';
                }

            }
            for (int i = 0; i < StateOfThePF.GetLength(0); i++)
            {

                for (int j = 0; j < StateOfThePF.GetLength(1); j++)
                {
                    PfData[i, j] = '0';
                }

            }

        }

    }

    class Units
    {
        public int Lives;
        public int AttackStrength;
        public int Range;
        public int PopultaionSize;
        public int TimeToMake;
        
        public int MaxNumberOfUnits;
        public int[] Coordinates = new int[2];
        public int TimeTillNextUnit;
        public bool Visible = false;
    }

    class Base : Units
    {
        public Base(int x, int y)
        {
            Lives = 10;
            MaxNumberOfUnits = 1;
            Coordinates[0] = x;
            Coordinates[1] = y;

        }
        public static void BaseInitialisation(Base Zerg, Base Terran, Playingfield PF)
        {
            PF.StateOfThePF[Zerg.Coordinates[0], Zerg.Coordinates[1]] = 'Z';
            PF.StateOfThePF[Terran.Coordinates[0], Terran.Coordinates[1]] = 'T';
            PF.PfData[Zerg.Coordinates[0], Zerg.Coordinates[1]] = 'Z';
            PF.PfData[Terran.Coordinates[0], Terran.Coordinates[1]] = 'T';
        }
    }

    class Cursor
    {
        public int[] CursosCoordinates = new int[2];
        public char HiddenData;
        public Cursor()
        {
            CursosCoordinates[0] = 5;
            CursosCoordinates[1] = 4;
            HiddenData = '0';
        }
        public static void CursorMovement(Playingfield PF, Cursor cursor)
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[0] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[0] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        break;
                    }
                case ConsoleKey.LeftArrow:
                    {
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[1] -= 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        break;
                    }
                case ConsoleKey.RightArrow:
                    {
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = cursor.HiddenData;
                        cursor.CursosCoordinates[1] += 1;
                        cursor.HiddenData = PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]];
                        PF.PfData[cursor.CursosCoordinates[0], cursor.CursosCoordinates[1]] = 'X';
                        break;
                    }
            }
        }
    }
    
    class Frontend
    {
        public static void SlowPrint(string Text, int speed = 100)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                Console.Write(Text[i]);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }

        public static void FieldRender(Playingfield PF)
        {
            


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
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.ResetColor();
                                Console.Write(' ');
                                break;
                            }
                            
                        default:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(PF.StateOfThePF[i, j]);
                                Console.ResetColor();
                                Console.Write(' ');
                            }
                            break;



                    }
                    
                    
                    
                                        
                }
                Console.WriteLine("");
            }
        }
    }
    
    
    
    class Program
    {
        static void Main(string[] args)
        {
            //initialising Unicode characters.
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Field initialization
            Playingfield PF = new Playingfield();
            Cursor cursor = new Cursor();
            Base terranBase = new Base(0, 4);
            Base zergBase = new Base(9, 4);
            Base.BaseInitialisation(zergBase, terranBase, PF);
            Frontend.FieldRender(PF, cursor.CursosCoordinates);

            Units[] TerranUnits = new Units[5];
            for (int i = 0; i < TerranUnits.Length; i++)
            {
                TerranUnits[i] = new Units();
            }

            while(Console.ReadKey().Key != )
            
            
            Console.ReadLine();
        }
    }
}
