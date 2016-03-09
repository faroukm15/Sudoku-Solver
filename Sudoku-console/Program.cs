using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku_console;

namespace Sudoku_console
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] mp = new int[9,9];
            for (int i = 0; i < 9; i++)
            {
                string[] token = Console.ReadLine().Split();
                for (int j = 0; j < 9; j++)
                    mp[i, j] = int.Parse(token[j]);
            }

            sudoku sud = new sudoku(mp);
            sud.solve();

            Console.Clear();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(sud[i, j].ToString() + " ");
                Console.Write("\n");
            }
            Console.ReadLine();
        }
    }
}
