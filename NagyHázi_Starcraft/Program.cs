using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NagyHázi_Starcraft
{
    
    class Playingfield
    {
        
        public static char[,] CreateDefaultPF(ref char[,] PF)
        {
            for (int i = 0; i < PF.GetLength(0); i++)
            {
                
                for (int j = 0; j < PF.GetLength(1); j++)
                {
                    PF[i, j] = '•';
                }
                
            }
            return PF;
        }
        public char[,] StateOfThePF = new char[9,9]; // itt kódon belül beállítható a pályaméret

        public Playingfield()
        {
            StateOfThePF = Playingfield.CreateDefaultPF(ref StateOfThePF);
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

        public static void FieldRender(char[,] PF)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(PF[i, j]);
                    Console.Write(' ');                    
                }
                Console.WriteLine("");
            }
        }
    }
    
    
    
    class Program
    {
        static void Main(string[] args)
        {
            //Unicode characters.
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Field initialization
            Playingfield PF = new Playingfield();
            
            Frontend.FieldRender(PF.StateOfThePF);
            
            Console.ReadLine();
        }
    }
}
