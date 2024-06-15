namespace day2;

internal class Program
{
    private enum CubeColor
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }

    private static readonly int COLOR_COUNT = 3;

    private static readonly Dictionary<CubeColor, int> BagOfCubes =
        new()
        {
            { CubeColor.Red, 12 },
            { CubeColor.Green, 13 },
            { CubeColor.Blue, 14 }
        };

    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string inputData = File.ReadAllText(inputPath);
        Dictionary<int, List<int[]>> games = ParseInput(inputData);

        List<int[]> minCubes = EvaluateMinimumSetOfCubes(games);
        List<int> possibleGames = EvaluateGamePossibility(games);
        List<int> powers = CalculatePowerOfCubes(minCubes);

        Console.WriteLine($"The sum of the possible gameIds is: {possibleGames.Sum()}");
        Console.WriteLine($"The sum of the powers is: {powers.Sum()}");
    }

    // Parse inputData using LINQ
    // Returns a Dictionary with gameIds as keys and a list of game rounds as values
    static Dictionary<int, List<int[]>> ParseInput(string inputData)
    {
        var games = inputData
            .Split("Game ", StringSplitOptions.RemoveEmptyEntries)
            // Process games
            .Select(game => game.Split(':'))
            .ToDictionary(
                parts => int.Parse(parts[0].Split(' ')[0]),
                parts =>
                    parts[1]
                        .Split(';')
                        // Process sets
                        .Select(set =>
                            set.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                // Process cubes
                                .Select(cube =>
                                {
                                    var splitCubes = cube.Trim().Split(' ');
                                    return new
                                    {
                                        Count = int.Parse(splitCubes[0]),
                                        Color = (CubeColor)
                                            Enum.Parse(typeof(CubeColor), splitCubes[1], true)
                                    };
                                })
                                // Store cube count in designated color index
                                .Aggregate(
                                    new int[COLOR_COUNT],
                                    (cubes, cube) =>
                                    {
                                        cubes[(int)cube.Color] = cube.Count;
                                        return cubes;
                                    }
                                )
                        )
                        .ToList()
            );
        return games;
    }

    // Evaluate which games are possibile based on the given set of cubes
    // Returns a list of gameIds for each possible game given the set of cubes in the bag
    static List<int> EvaluateGamePossibility(Dictionary<int, List<int[]>> games)
    {
        List<int> possibleGames = [];
        int redCubesInBag = BagOfCubes[CubeColor.Red];
        int greenCubesInBag = BagOfCubes[CubeColor.Green];
        int blueCubesInBag = BagOfCubes[CubeColor.Blue];

        foreach (var game in games)
        {
            int gameId = game.Key;
            List<int[]> rounds = game.Value;
            bool possible = true;

            // Check if the current round had a color count higher than what's in the bag
            foreach (var round in rounds)
            {
                if (
                    round[(int)CubeColor.Red] > redCubesInBag
                    || round[(int)CubeColor.Green] > greenCubesInBag
                    || round[(int)CubeColor.Blue] > blueCubesInBag
                )
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                possibleGames.Add(gameId);
            }
        }

        return possibleGames;
    }

    // Evaluates the minimum necessary cubes of each color in order for a game to be possible.
    // Returns a new list with the minimum set of cubes for each game.
    static List<int[]> EvaluateMinimumSetOfCubes(Dictionary<int, List<int[]>> games)
    {
        List<int[]> minCubes = games
            .Select(game =>
            {
                var rounds = game.Value;
                int[] minSet = new int[COLOR_COUNT];

                // Assign the max value for each color and store it in our minimum set
                minSet[(int)CubeColor.Red] = rounds.Max(round => round[(int)CubeColor.Red]);

                minSet[(int)CubeColor.Green] = rounds.Max(round => round[(int)CubeColor.Green]);

                minSet[(int)CubeColor.Blue] = rounds.Max(round => round[(int)CubeColor.Blue]);

                return minSet;
            })
            .ToList();

        return minCubes;
    }

    // Calculates the power of each minimum set of cubes, i.e. multiplies the value of each color
    // Returns a list of powers for each game.
    static List<int> CalculatePowerOfCubes(List<int[]> minCubes)
    {
        List<int> powers = minCubes
            .Select(cubeSet =>
            {
                int power =
                    cubeSet[(int)CubeColor.Red]
                    * cubeSet[(int)CubeColor.Green]
                    * cubeSet[(int)CubeColor.Blue];
                return power;
            })
            .ToList();

        return powers;
    }
}
