using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_console
{
    class sudoku
    {
        private int[,] map;

        // submap to define subsquares in sudoku grid
        private int[,] submap =
        {
            {1, 1, 1, 2, 2, 2, 3, 3, 3},
            {1, 1, 1, 2, 2, 2, 3, 3, 3},
            {1, 1, 1, 2, 2, 2, 3, 3, 3},
            {4, 4, 4, 5, 5, 5, 6, 6, 6},
            {4, 4, 4, 5, 5, 5, 6, 6, 6},
            {4, 4, 4, 5, 5, 5, 6, 6, 6},
            {7, 7, 7, 8, 8, 8, 9, 9, 9},
            {7, 7, 7, 8, 8, 8, 9, 9, 9},
            {7, 7, 7, 8, 8, 8, 9, 9, 9}
        };


        public int this[int i, int j]
        {
            get
            {
                return map[i, j];
            }
            set
            {
                map[i, j] = value;
            }
        }

        // default constructor
        public sudoku()
        {
            return;
        }

        public sudoku(sudoku s)
        {
            
        }

        // intializing, copying passed sudoku grid to map
        public sudoku(int[,] mp)
        {
            map = mp;
        }


        // solve the sudoku, solve as much as it can with single candidates, then if still not solved go with trySolve
        public int[,] solve()
        {
            if (unsolvable(map))
            {
                Console.WriteLine("This Sudoku is unsolvable!");
                return map;
            }
            singleCandidates(1, 20);

            if (!solved(map))
                trySolve(map, 0, 0);

            return map;
        }


        private bool unsolvable(int[,] tm)
        {
            int sols = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (tm[i, j] != 0)
                        sols++;
            if (sols <= 10)
                return true;
            else
                return false;
        }

        // check if the the current sudoku grid is solved
        private bool solved(int[,] tm)
        {
            int sols = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (tm[i, j] != 0)
                        sols++;
            if (sols == 81)
                return true;
            else
                return false;
        }

        // check if main sudoku grid is solved
        public bool solved()
        {
            return solved(map);
        }

        // solve the sudoku grid by trying all possible valid combinations after emitting non-valid paths
        private void trySolve(int[,] tm, int i, int j)
        {
            int[,] m = new int[9,9];
            System.Array.Copy(tm, m, 81);
            if (m[i, j] == 0)
            {
                int sols = 9;
                int[] ss = getSolutionSet(m, i, j, ref sols);

                if (sols > 0)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        if (ss[k] != 0)
                        {
                            m[i, j] = ss[k];

                            if (solved(m))
                            {
                                map = m;
                                break;
                            }

                            if (j < 8)
                                trySolve(m, i, j + 1);
                            else if (i < 8)
                                trySolve(m, i + 1, 0);

                        }
                    }
                }
            }
            else
            {
                if (j < 8)
                    trySolve(m, i, j + 1);
                else if (i < 8)
                    trySolve(m, i + 1, 0);

            }
        }

        // get all possible values for a certain slot
        private int[] getSolutionSet(int[,] cm, int i, int j, ref int sols)
        {

            sols = 9;
            int[] ss = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int l = 0; l < 9; l++)
            {
                for (int m = 0; m < 9; m++)
                {
                    if (submap[i, j] == submap[l, m])
                    {
                        if (cm[l, m] != 0 && ss[cm[l, m]] != 0)
                        {
                            ss[cm[l, m]] = 0;
                            sols--;
                        }
                    }
                }
            }

            for (int n = 0; n < 9; n++)
                if (cm[i, n] != 0 && ss[cm[i, n]] != 0) { ss[cm[i, n]] = 0; sols--; }
            for (int n = 0; n < 9; n++)
                if (cm[n, j] != 0 && ss[cm[n, j]] != 0) { ss[cm[n, j]] = 0; sols--; }

            return ss;
        }

        // solve by using single candidates technique for basic grids 
        private void singleCandidates(int level, int maxLevel)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (map[i, j] == 0)
                    {
                        int sols = 9;
                        int[] ss = getSolutionSet(map, i, j, ref sols);
                        if (sols == 1)
                            for (int n = 0; n < 10; n++)
                                if (ss[n] != 0) map[i, j] = ss[n];
                    }
                }
            }
            if (!solved(map) && level <= maxLevel)
                singleCandidates(level + 1, maxLevel);
        }

        ////////////////////////////////////////////////////////////
        ////////      GENERATING SUDOKO     ////// Incomplete //////
        ////////////////////////////////////////////////////////////

        // enum for sudoku difficulities
        public enum DIFFICULITY
        {
            EASY = 0,
            MEDIUM,
            HARD,
            ULTIMATE
        };

        private int[,] fillRandom()
        {
            int[,] temp = { {0} };
            Random r = new Random();
            
            for (int k = 0; k < 10; k++)
            {
                int i = r.Next(0, 8), j = r.Next(0, 8);
                if (temp[i, j] == 0) k--;
                temp[i, j] = r.Next(1, 9);
            }

            sudoku ts = new sudoku(temp);

            while(!ts.solved())
            {
                int i = r.Next(0, 8), j = r.Next(0, 8);
                if (ts[i, j] == 0) continue;
                ts[i, j] = r.Next(1, 9);
                ts.solve();
                //if (ts.solved)
            }


            return temp;
        }

        public void generate(DIFFICULITY difficulity)
        {
            int[,] filledMap = fillRandom();
        }

    }
}
