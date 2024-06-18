namespace day3;

internal class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        //string inputPath = "testInput.txt";
        string[] inputData = File.ReadAllLines(inputPath);
        int numRows = inputData.Length;
        int numCols = inputData[0].Length;
        char[,] grid = InitializeGrid(inputData, numRows, numCols);

        // Define the directions for adjacent cells
        CardinalDirection[] directions =
        [
            new CardinalDirection
            {
                Direction = "North",
                Row = -1,
                Col = 0
            },
            new CardinalDirection
            {
                Direction = "NorthEast",
                Row = -1,
                Col = 1
            },
            new CardinalDirection
            {
                Direction = "East",
                Row = 0,
                Col = 1
            },
            new CardinalDirection
            {
                Direction = "SouthEast",
                Row = 1,
                Col = 1
            },
            new CardinalDirection
            {
                Direction = "South",
                Row = 1,
                Col = 0
            },
            new CardinalDirection
            {
                Direction = "SouthWest",
                Row = 1,
                Col = -1
            },
            new CardinalDirection
            {
                Direction = "West",
                Row = 0,
                Col = -1
            },
            new CardinalDirection
            {
                Direction = "NorthWest",
                Row = -1,
                Col = -1
            },
        ];

        List<Symbol> symbolsWithAdjacentNumbers = FindSymbolsWithAdjacentNumbers(grid, directions);

        // Filter to get only gear symbols with adjacent numbers of length two
        List<Symbol> gearParts = symbolsWithAdjacentNumbers
            .Where(symbol => symbol.Type == GEAR && symbol.AdjacentNumbers.Count == 2)
            .ToList();

        // Calculate the sum of the numbers adjacent to a symbol
        // 527_446
        int sumAdjacentNumbers = symbolsWithAdjacentNumbers
            .Select(symbol => symbol.AdjacentNumbers.Sum())
            .Sum();

        // Calculate the gearRatio (gearPartA * gearPartB) for each set of gear parts and aggregate the total
        // 73_201_705
        int gearRatioSum = gearParts
            .Select(symbol =>
                symbol.AdjacentNumbers.ElementAt(0) * symbol.AdjacentNumbers.ElementAt(1)
            )
            .Sum();

        Console.WriteLine(
            $"The sum of the numbers that are adjacent to a symbol is: {sumAdjacentNumbers}"
        );

        Console.WriteLine($"The sum of the gear ratios is: {gearRatioSum}");
    }

    // Struct for the directions of adjacent cells
    public struct CardinalDirection
    {
        public string Direction { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public static readonly char GEAR = '*';

    public struct Symbol
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Type { get; set; }
        public HashSet<int> AdjacentNumbers { get; set; }
    }

    // Initializes 2D array of characters to represent the inputdata as a grid
    public static char[,] InitializeGrid(string[] data, int numRows, int numCols)
    {
        char[,] grid = new char[numRows, numCols];
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                grid[row, col] = data[row][col];
            }
        }
        return grid;
    }

    // Evaluates each symbol in the grid to find adjacent numbers
    // Returns a list of Symbols that contain the coordinates, the type of symbol, and a list of its adjacent numbers
    public static List<Symbol> FindSymbolsWithAdjacentNumbers(
        char[,] grid,
        CardinalDirection[] directions
    )
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);

        List<Symbol> symbolsWithAdjacentNumbers = [];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Make sure that the candidate is a symbol, otherwise go to the next tile in the grid
                char candidate = grid[row, col];
                if (!IsSymbol(candidate))
                {
                    continue;
                }

                // Check each adjacent tile to the candidate
                HashSet<int> adjacentNumbers = [];
                foreach (var direction in directions)
                {
                    int adjacentRow = row + direction.Row;
                    int adjacentCol = col + direction.Col;

                    // If not a valid adjacent tile, i.e. not within bounds or a digit, move on to the next adjacent tile
                    if (!IsValidAdjacentTile(grid, adjacentRow, adjacentCol))
                    {
                        continue;
                    }

                    // Extract the complete number from the digit and add it to the list of adjacent numbers
                    int number = ExtractNumber(grid, adjacentRow, adjacentCol);
                    adjacentNumbers.Add(number);
                }

                // Move on to the next tile if we did not find any adjacent numbers
                if (adjacentNumbers.Count == 0)
                {
                    continue;
                }

                // Create a new Symbol and add it to our list
                symbolsWithAdjacentNumbers.Add(
                    new Symbol
                    {
                        Row = row,
                        Col = col,
                        Type = candidate,
                        AdjacentNumbers = adjacentNumbers
                    }
                );
            }
        }

        return symbolsWithAdjacentNumbers;
    }

    // Helper method to evaluate if a character is a symbol
    public static bool IsSymbol(char c)
    {
        return c != '.' && !char.IsDigit(c);
    }

    // Helper method to evaluate if a candidate tile is valid, i.e. within bounds and a digit
    public static bool IsValidAdjacentTile(char[,] grid, int row, int col)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);
        bool inBounds = row >= 0 && row < numRows && col >= 0 && col < numCols;
        bool isDigit = char.IsDigit(grid[row, col]);
        return inBounds && isDigit;
    }

    // Finds the starting and ending coordinates of a number and returns it as an integer
    public static int ExtractNumber(char[,] grid, int row, int col)
    {
        int startCol = col;
        int endCol = col;

        // Move left through the grid to find the beginning of the number
        while (startCol > 0 && char.IsDigit(grid[row, startCol - 1]))
        {
            startCol--;
        }

        // Move right through the grid to find the end of the number
        while (endCol < grid.GetLength(1) - 1 && char.IsDigit(grid[row, endCol + 1]))
        {
            endCol++;
        }

        // Construct the resulting number as a string
        int numberLength = endCol - startCol + 1;
        string number = "";
        for (int i = 0; i < numberLength; i++)
        {
            number += grid[row, startCol + i];
        }

        return int.Parse(number);
    }
}
