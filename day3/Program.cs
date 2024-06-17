namespace day3;

internal class Program
{
    // Struct for the directions of adjacent cells
    public struct CardinalDirection
    {
        public string Direction { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public static readonly char Gear = '*';

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

        List<int> adjacentNumbers = FindAdjacentNumbers(grid, directions);
        List<HashSet<int>> gearParts = FindGearParts(grid, directions);

        // Calculate the gearRatio (gearPartA * gearPartB) for each set of gearParts and aggregate the total
        int gearRatioSum = gearParts
            .Select(parts => parts.Aggregate(1, (gearRatio, part) => gearRatio * part))
            .Sum();

        Console.WriteLine(
            $"The sum of the numbers that are adjacent to a symbol is: {adjacentNumbers.Sum()}"
        );

        Console.WriteLine($"The sum of the gear ratios is: {gearRatioSum}");
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

    // Returns a List of all the numbers that are adjacent to a symbol
    public static List<int> FindAdjacentNumbers(char[,] grid, CardinalDirection[] directions)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);
        List<int> adjacentNumbers = [];

        // Loop through the grid and find a symbol
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (IsSymbol(grid[row, col]))
                {
                    HashSet<int> candidateNumbers = [];
                    // Check all adjacent tiles in the grid
                    foreach (CardinalDirection direction in directions)
                    {
                        int candidateRow = row + direction.Row;
                        int candidateCol = col + direction.Col;

                        // Check if the candidate tile is valid, i.e. in bounds and a digit
                        // If valid, extract the number and store it in a HashSet to avoid duplicates
                        if (IsValidCandidate(grid, candidateRow, candidateCol))
                        {
                            candidateNumbers.Add(ExtractNumber(grid, candidateRow, candidateCol));
                        }
                    }

                    // Add the valid candidates to the list of adjacent numbers
                    adjacentNumbers.AddRange(candidateNumbers);
                }
            }
        }

        return adjacentNumbers;
    }

    // Returns a List with a HashSet containing the gearParts-combo
    public static List<HashSet<int>> FindGearParts(char[,] grid, CardinalDirection[] directions)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);

        List<HashSet<int>> gearParts = [];

        // Loop through the grid to find a Gear ('*')
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (IsGear(grid[row, col]))
                {
                    HashSet<int> candidateNumbers = [];
                    // Check all adjacent tiles in the grid
                    foreach (CardinalDirection direction in directions)
                    {
                        int candidateRow = row + direction.Row;
                        int candidateCol = col + direction.Col;

                        // Check if the candidate tile is valid, i.e. in bounds and a digit
                        // If valid, extract the number and store it in a HashSet to avoid duplicates
                        if (IsValidCandidate(grid, candidateRow, candidateCol))
                        {
                            candidateNumbers.Add(ExtractNumber(grid, candidateRow, candidateCol));
                        }
                    }

                    // Add the candidates to the list of gearParts if they are gearParts
                    if (IsGearPart(candidateNumbers))
                    {
                        gearParts.Add(candidateNumbers);
                    }
                }
            }
        }

        return gearParts;
    }

    // Helper function to evaluate that a candidate set is gear parts (has exactly two numbers)
    public static Boolean IsGearPart(HashSet<int> gearSet)
    {
        return gearSet.Count == 2;
    }

    // Helper method to evaluate if a character is a symbol
    public static bool IsSymbol(char c)
    {
        return c != '.' && !char.IsDigit(c);
    }

    public static bool IsGear(char c)
    {
        return c == Gear;
    }

    // Helper method to evaluate if a candidate tile is valid, i.e. within bounds and a digit
    public static bool IsValidCandidate(char[,] grid, int row, int col)
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
