//Daniel Rodriguez Irizarry
using System;
using System.Text.RegularExpressions;

public class Minesweeper
{
    /**************************************************************************\
    |* Game Constants                                                         *|
    \**************************************************************************/
    public const int ROWS = 5; //You can change the ROWS up to 10
    public const int COLS = 10; //You can change the COLS up to 26
    public const int MINES = 8; 
    public const string FLAG = "flag";
    public const string UNFLAG = "unflag";
    public const string SWEEP = "sweep";
    public const string FLAG_SYMBOL = "#";
    public const string MINE_SYMBOL = "*";
    /**************************************************************************\
    |* Game State                                                             *|
    \**************************************************************************/
    public bool isFirstSweep;
    public bool isGameLost;
    public GridCell[,] cells;

    /**************************************************************************\
    |* Other Game Data Objects and Components                                 *|
    \**************************************************************************/
    public class GridCell
    {
        public int mineCount { get; set; }
        public bool isFlagged { get; set; }
        public bool isSwept { get; set; }
        public bool isMine { get; set; }

        public GridCell()
        {
            mineCount = 0;
            isFlagged = false;
            isSwept = false;
            isMine = false;
        }
    }

    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public static void Main(string[] arg)
    {
        Minesweeper ms = new Minesweeper();
        ms.Start();
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public Minesweeper()
    {
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void Start()
    {
        string input;
        Init(); // 1. Initialize Variables
        ShowGameStartScreen(); // 2. Show Game Start Screen
        do
        {
            ShowBoard(); // 3. Show Board / Scene / Map
            do
            {
                ShowInputOptions(); // 4. Show Input Options
                input = GetInput(); // 5. Get Input
            }
            while (!IsValidInput(input)); // 6. Validate Input
            ProcessInput(input); // 7. Process Input
        }
        while (!IsGameOver()); // 8. Check for Termination Conditions
        ShowGameOverScreen(); // 9. Show Game Results
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void Init()
    {
        isFirstSweep = true;
        isGameLost = false;

        // Initializing Gridcell Array.
        cells = new GridCell[ROWS, COLS];
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j] = new GridCell(); // Calling default constructor.                                              
            }
        }

    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void ShowGameStartScreen()
    {
        Console.WriteLine("Welcome to Minesweeper!");
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void ShowBoard()
    {
        Console.Clear();
        string board = "   ";
        string line = "  -";

        //Showing the board with nested for loop.
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                if (i == 0)
                {
                    board += (char)(j + 'A');
                    board += " ";
                    line += "--";
                }
            }
        }
        board += Environment.NewLine;
        for (int i = 0; i < ROWS; i++)
        {
            board += line;
            board += Environment.NewLine;
            board += (char)(i + '0');

            for (int j = 0; j < COLS; j++)
            {
                if (j == 0)
                {
                    board += " ";
                    board += "|";
                }

                if (cells[i, j].isMine == false && cells[i, j].isSwept == true)
                {
                    board += cells[i, j].mineCount.ToString();
                }
                else if (cells[i, j].isFlagged == true && cells[i, j].isSwept == false)
                {
                    board += FLAG_SYMBOL;
                }
                else if (cells[i, j].isMine == true && isGameLost == true && cells[i, j].isSwept == true)
                {
                    board += MINE_SYMBOL;
                }
                else //(cells[i, j].IsSwept == false && cells[i, j].IsFlagged == false)
                {
                    board += " ";
                }

                board += "|";
                
                if (j == COLS - 1)
                {
                    board += " ";
                }
            }
            board += (char)(i + '0');
            board += Environment.NewLine;
            if (i == ROWS - 1)
            {
                board += line;
                board += Environment.NewLine;
            }
        }
        board += "   ";
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                if (i == 0)
                {
                    board += (char)(j + 'A');
                    board += " ";
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine(board);
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void ShowInputOptions()
    {
        Console.WriteLine();
        Console.Write($"Enter [{FLAG}|{UNFLAG}|{SWEEP}] [0-{ROWS - 1}] [A-{(char)(COLS - 1 + 'A')}]: ");      
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public string GetInput()
    {
        string input = Console.ReadLine().Trim().ToLower();
        return input;
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public bool IsValidInput(string input)
    {          
        try
        {
            string pattern = "\\s+"; // one or more white spaces     
            string[] tokens = Regex.Split(input, pattern); // This array retain first 3 value of the input.
            string action = tokens[0]; // flag, unflag, sweep
            int row = int.Parse(tokens[1]); // 0-4
            string col = tokens[2]; // A-J
            int letter = char.Parse(col);

            if (action != FLAG && action != UNFLAG && action != SWEEP)
            {
                Console.WriteLine("Input action must be either [flag|unflag|sweep].");
                return false;
            }
            else if (row < 0 || row > ROWS - 1)
            {
                Console.WriteLine($"Input row must be between[0 - {ROWS - 1}].");
                return false;
            }
            else if (letter < 'a' || letter > (char)(COLS - 1 + 'a'))
            {
                Console.WriteLine($"Input column must be between [A - {(char)(COLS - 1 + 'A')}].");
                return false;
            }
            else if (cells[row, letter - 97].isFlagged == true && action == FLAG)
            {
                Console.WriteLine($"Coordinate ({row}, {col.ToUpper()}) has already been flagged.");
                return false;
            }
            else if (cells[row, letter - 97].isSwept == true && action == FLAG)
            {
                Console.WriteLine($"You cannot flag coordinate ({row}, {col.ToUpper()}) because it has already been swept.");
                return false;
            }
            else if (cells[row, letter - 97].isFlagged == false && action == UNFLAG)
            {
                Console.WriteLine($"Coordinate ({row}, {col.ToUpper()}) has already been unflagged.");
                return false;
            }
            else if (cells[row, letter - 97].isFlagged == true && action == SWEEP)
            {
                Console.WriteLine($"Unflag first coordinate ({row}, {col.ToUpper()}).");
                return false;
            }
            else if ((cells[row, letter - 97].isSwept == true && action == SWEEP))
            {
                Console.WriteLine($"Coordinate ({row}, {col.ToUpper()}) has already been swept.");
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Invalid input format!");
            return false;
        }
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void ProcessInput(string input)
    {
        string pattern = "\\s+";
        string[] tokens = Regex.Split(input, pattern);
        string action = tokens[0]; //flag, unflag, sweep
        int row = int.Parse(tokens[1]); //0-4                                     
        string col = tokens[2]; //A-J
        int letter = char.Parse(col);

        if (action == FLAG && cells[row, letter - 97].isSwept == false)
        {
            cells[row, letter - 97].isFlagged = true; //Set a flag in the position selected by the player.
        }
        else if (action == UNFLAG && cells[row, letter - 97].isSwept == false) // Remove the flag in the position selected by the player.
        {
            cells[row, letter - 97].isFlagged = false;
        }
        else if (action == SWEEP)
        {
            if (isFirstSweep == true)
            {
                int rowNumber;
                int colNumber;
                Random ran = new Random();

                for (int i = 0; i < MINES; i++)
                {
                    rowNumber = ran.Next(0, ROWS);
                    colNumber = ran.Next(0, COLS);
                    if (row == rowNumber && letter - 97 == colNumber)
                    {
                        i--; // If the randomizer places a mine in the sweep location, subtract one and try again.
                    }
                    else
                    {
                        cells[rowNumber, colNumber].isMine = true; // Set mines randomly.
                    }
                }

                // This nested for loop search for mines and if is mine search for all 8 adjacent cells to increase the minecount by 1.
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLS; j++)
                    {
                        if (cells[i, j].isMine == true && i > 0 && j > 0)
                        {
                            if (cells[i - 1, j - 1].isMine == false)
                            {
                                cells[i - 1, j - 1].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && j > 0)
                        {
                            if (cells[i, j - 1].isMine == false)
                            {
                                cells[i, j - 1].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && i > 0)
                        {
                            if (cells[i - 1, j].isMine == false)
                            {
                                cells[i - 1, j].mineCount++;
                            }
                        }

                        if (cells[i, j].isMine == true && i < ROWS - 1)
                        {
                            if (cells[i + 1, j].isMine == false)
                            {
                                cells[i + 1, j].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && j < COLS - 1)
                        {
                            if (cells[i, j + 1].isMine == false)
                            {
                                cells[i, j + 1].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && i < ROWS - 1 && j < COLS - 1)
                        {
                            if (cells[i + 1, j + 1].isMine == false)
                            {
                                cells[i + 1, j + 1].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && i > 0 && j < COLS - 1)
                        {
                            if (cells[i - 1, j + 1].isMine == false)
                            {
                                cells[i - 1, j + 1].mineCount++;
                            }
                        }
                        if (cells[i, j].isMine == true && i < ROWS - 1 && j > 0)
                        {
                            if (cells[i + 1, j - 1].isMine == false )
                            {
                                cells[i + 1, j - 1].mineCount++;
                            }
                        }
                    }
                }
            }

            // If the player sweep a mine, this active the lose condition.
            if (cells[row, letter - 97].isMine == true && isFirstSweep == false)
            {
                
                isGameLost = true;
                for (int i = 0; i < ROWS; i++)
                {
                    for (int j = 0; j < COLS; j++)
                    {
                        if (cells[i, j].isSwept == false)
                        {
                            cells[i, j].isSwept = true;
                            cells[i, j].isFlagged = false;
                        }
                    }
                }
            }
            else
            {
                cells[row, letter - 97].isSwept = true; // Sweep the selected position.

                // If the position sweeped the minecount is 0, the nested for loop sweep all cell with 0 until a sweeped cell with minecount greater than 0.
                if (cells[row, letter - 97].mineCount == 0)
                {
                    for (int k = 0; k < COLS; k++) // This for loop is to repeat the nested for loop.
                    {
                        for (int i = 0; i < ROWS; i++)
                        {
                            for (int j = 0; j < COLS; j++)
                            {
                                // If minecount equals 0, sweep all 8 adjacent cells. 
                                if (cells[i, j].mineCount == 0 && row == i && letter - 97 == j && cells[i, j].isSwept == false)
                                {
                                    if (i > 0 && j > 0)
                                    {
                                        cells[i - 1, j - 1].isSwept = true;
                                    }                                   
                                    if (i > 0)
                                    {
                                        cells[i - 1, j].isSwept = true;
                                    }
                                    if (i > 0 && j < COLS - 1)
                                    {
                                        cells[i - 1, j + 1].isSwept = true;
                                    }
                                    if (j > 0)
                                    {
                                        cells[i, j - 1].isSwept = true;
                                    }
                                    if (j < COLS - 1)
                                    {
                                        cells[i, j + 1].isSwept = true;
                                    }
                                    if (i < ROWS - 1 && j > 0)
                                    {
                                        cells[i + 1, j - 1].isSwept = true;
                                    }
                                    if (i < ROWS - 1)
                                    {
                                        cells[i + 1, j].isSwept = true;
                                    }                                   
                                    if (i < ROWS - 1 && j < COLS - 1)
                                    {
                                        cells[i + 1, j + 1].isSwept = true;
                                    }                                  
                                }
                                else if (cells[i, j].mineCount == 0 && cells[i, j].isSwept == true && cells[i, j].isMine == false) // If the cell mineCount is 0, sweep all 8 adjacent cells recursively.
                                {
                                    if (cells[i, j].mineCount == 0 && i > 0 && j < COLS - 1)
                                    {
                                        cells[i - 1, j + 1].isSwept = true;
                                        cells[i - 1, j + 1].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && i > 0)
                                    {
                                        cells[i - 1, j].isSwept = true;
                                        cells[i - 1, j].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && i > 0 && j > 0)
                                    {
                                        cells[i - 1, j - 1].isSwept = true;
                                        cells[i - 1, j - 1].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && i < ROWS - 1)
                                    {
                                        cells[i + 1, j].isSwept = true;
                                        cells[i + 1, j].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && j > 0)
                                    {
                                        cells[i, j - 1].isSwept = true;
                                        cells[i, j - 1].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && j < COLS - 1)
                                    {
                                        cells[i, j + 1].isSwept = true;
                                        cells[i, j + 1].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && i < ROWS - 1 && j < COLS - 1)
                                    {
                                        cells[i + 1, j + 1].isSwept = true;
                                        cells[i + 1, j + 1].isFlagged = false;
                                    }
                                    if (cells[i, j].mineCount == 0 && i < ROWS - 1 && j > 0)
                                    {
                                        cells[i + 1, j - 1].isSwept = true;
                                        cells[i + 1, j - 1].isFlagged = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }           
            isFirstSweep = false;
        }
        else
        {
            Console.WriteLine("Something went really wrong! This is never supposed to happen!");
        }

    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public bool IsGameOver()
    {
        if(CheckLoss())
        {
            return true;
        }
        else
        {
            return CheckWin();
        }
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public bool CheckWin()
    {
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLS; j++)
            {
                if (cells[i, j].isSwept == false && cells[i, j].isMine == false)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public bool CheckLoss()
    {
        return isGameLost;
    }
    /**************************************************************************\
    |*                                                                        *|
    \**************************************************************************/
    public void ShowGameOverScreen()
    {
        ShowBoard();
        Console.WriteLine();
        Console.WriteLine("Game Over!");
        if (CheckLoss())
        {
            Console.WriteLine("You Lost! You exploded!");
        }
        else //if (CheckWin())
        {
            Console.WriteLine("Congrats! You Won! You managed to clear the minefield without getting blown to pieces!");
        }

    }
}
