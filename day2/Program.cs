using System.Runtime.InteropServices;

namespace day2;

internal class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string inputData = File.ReadAllText(inputPath);

        var games = ParseInput(inputData);
        var minCubes = EvaluateMinimumSetOfCubes(games);
        var possibleGames = EvaluateGamePossibility(games);
        var powers = CalculatePowerOfCubes(minCubes);

        Console.WriteLine($"The sum of the possible gameIds is: {possibleGames.Sum()}"); //2683
        Console.WriteLine($"The sum of the powers is: {powers.Sum()}"); //49710
    }

    private enum CubeColor
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }

    private static readonly int COLOR_COUNT = Enum.GetValues(typeof(CubeColor)).Length;

    // Given set of cubes from the problem instructions
    private static readonly Dictionary<CubeColor, int> BagOfCubes =
        new()
        {
            { CubeColor.Red, 12 },
            { CubeColor.Green, 13 },
            { CubeColor.Blue, 14 }
        };

    // Parse inputData using LINQ
    // Returns a List of games where each game is a list of game rounds
    static List<List<int[]>> ParseInput(string inputData)
    {
        var games = inputData
            .Split("Game ", StringSplitOptions.RemoveEmptyEntries)
            // This splits the input into individual games:
            // [ "", "1: 1 Red, 2 Green, 3 Blue; 4 Red, 5 Green, 6 Blue; 7 Red, 8 Green, 9 Blue", "2: ..."]

            .Select(line => line.Split(':')[1])
            // For each line, split by ':' to get the game parts after "Game ".
            // [ " 1 Red, 2 Green, 3 Blue; 4 Red, 5 Green, 6 Blue; 7 Red, 8 Green, 9 Blue", " ..."]

            .Select(game =>
                game.Split(';', StringSplitOptions.RemoveEmptyEntries)
                    // Split each game part by ';' to get the game rounds.
                    // [ " 1 Red, 2 Green, 3 Blue", " 4 Red, 5 Green, 6 Blue", " 7 Red, 8 Green, 9 Blue" ]

                    .Select(gameRounds =>
                        gameRounds
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            // Split each round by ',' to get individual cubes.
                            // [ " 1 Red", " 2 Green", " 3 Blue" ]

                            .Select(cubes =>
                            {
                                var cube = cubes.Trim().Split(' ');
                                //Split each cube by ' ' to get the count and color.
                                //[ " 1", " Red" ]

                                return new
                                {
                                    Count = int.Parse(cube[0]),
                                    Color = (CubeColor)Enum.Parse(typeof(CubeColor), cube[1], true)
                                };

                                // Use Aggregate to accumulate cube counts into an array based on the color index.
                                // [ 1 (Red), 2 (Green), 3 (Blue) ]
                            })
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
            )
            .ToList();
        return games;
    }

    // Evaluate which games are possibile based on the given set of cubes
    // Returns a list of gameIds for each possible game given the set of cubes in the bag
    static List<int> EvaluateGamePossibility(List<List<int[]>> games)
    {
        List<int> possibleGames = [];
        int redCubesInBag = BagOfCubes[CubeColor.Red];
        int greenCubesInBag = BagOfCubes[CubeColor.Green];
        int blueCubesInBag = BagOfCubes[CubeColor.Blue];

        for (int gameId = 0; gameId < games.Count; gameId++)
        {
            List<int[]> gameRounds = games[gameId];
            bool possible = true;

            foreach (var gameRound in gameRounds)
            {
                if (
                    gameRound[(int)CubeColor.Red] > redCubesInBag
                    || gameRound[(int)CubeColor.Green] > greenCubesInBag
                    || gameRound[(int)CubeColor.Blue] > blueCubesInBag
                )
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                possibleGames.Add(gameId + 1);
            }
        }

        return possibleGames;
    }

    // Evaluates the minimum necessary cubes of each color in order for a game to be possible.
    // Returns a new list with the minimum set of cubes for each game.
    static List<int[]> EvaluateMinimumSetOfCubes(List<List<int[]>> games)
    {
        List<int[]> minCubes = games
            .Select(game =>
            {
                int[] minSet = new int[COLOR_COUNT];

                // Assign the max value for each color and store it in our minimum set
                minSet[(int)CubeColor.Red] = game.Max(gameRound => gameRound[(int)CubeColor.Red]);

                minSet[(int)CubeColor.Green] = game.Max(gameRound =>
                    gameRound[(int)CubeColor.Green]
                );

                minSet[(int)CubeColor.Blue] = game.Max(gameRound => gameRound[(int)CubeColor.Blue]);

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
            .Select(cubes =>
            {
                int power =
                    cubes[(int)CubeColor.Red]
                    * cubes[(int)CubeColor.Green]
                    * cubes[(int)CubeColor.Blue];
                return power;
            })
            .ToList();

        return powers;
    }
}
