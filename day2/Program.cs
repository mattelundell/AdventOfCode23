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

    private static readonly List<int> BagOfCubes = [12, 13, 14];

    static void Main(string[] args)
    {
        /* string inputPath = "test.txt";
        string testData = File.ReadAllText(inputPath);
        Dictionary<int, List<int[]>> games = ParseInput(testData); */

        string inputPath = "input.txt";
        string inputData = File.ReadAllText(inputPath);
        Dictionary<int, List<int[]>> games = ParseInput(inputData);

        /*
        * TODO: Evaluate each game to find the minimum set of cubes -> We need to find the highest r, g, b from all rounds of a game
        * TODO: Calculate the power of the minimum set of cubes for each game -> r * g * b = power
        * TODO: Calculate the sum of all the powers
        */

        //PrintGames(games);
        List<int> possibleGames = EvaluateGamePossibility(games);
        Console.WriteLine($"The gameIds of the possible games are = {string.Join(", ", possibleGames)}");
        Console.WriteLine($"The sum of the possible gameIds = {possibleGames.Sum()}");
    }

    //Parse inputData using LINQ processing 
    static Dictionary<int, List<int[]>> ParseInput(string inputData)
    {
        var games = inputData
            // Splitting games
            // first {[""], [1: "3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"]}
            // then {["1"], ["3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"]}
            .Split("Game ", StringSplitOptions.RemoveEmptyEntries)
            .Select(game => game.Split(':'))

            // Converts the result to a dictionary
            .ToDictionary(
                // Key = gameId
                parts => int.Parse(parts[0].Split(' ')[0]),

                // Value: sets of game rounds
                parts => parts[1]

                    // Splits sets by ';' and cubes by ','
                    // set is e.g. {["3 blue, 4 red"], ["1 red, 2 green, 6 blue"]}, and cube is e.g. ["3 blue", "4 red"]
                    .Split(';')
                    .Select(set => set
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)

                        // Processing cubes
                        // A cube gets a count and a color, e.g. cube.Count = 3, cube.Color = Blue
                        .Select(cube =>
                        {
                            // Trims and splits each cube, parsing the count and color
                            var splitCubes = cube.Trim().Split(' ');
                            return new
                            {
                                Count = int.Parse(splitCubes[0]),
                                Color = (CubeColor)Enum.Parse(typeof(CubeColor), splitCubes[1], true)
                            };

                        })

                        // Aggregate the cube counts into an array where each index corresponds to a color
                        // 4 red, 3 blue is e.g. [4, 0, 3]
                        .Aggregate(new int[COLOR_COUNT], (cubes, cube) =>
                        {
                            cubes[(int)cube.Color] = cube.Count;
                            return cubes;
                        }))
                    .ToList()
            );
        return games;
    }

    static List<int> EvaluateGamePossibility(Dictionary<int, List<int[]>> games)
    {
        List<int> possibleGames = [];

        foreach (var game in games)
        {
            int gameId = game.Key;
            List<int[]> rounds = game.Value;
            bool possible = true;

            // Check if the round had a color count higher than what's in the bag, and if so, the game was not possible.
            foreach (var round in rounds)
            {

                if (round[(int)CubeColor.Red] > BagOfCubes[(int)CubeColor.Red] ||
                    round[(int)CubeColor.Green] > BagOfCubes[(int)CubeColor.Green] ||
                    round[(int)CubeColor.Blue] > BagOfCubes[(int)CubeColor.Blue])
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


    static void PrintGames(Dictionary<int, List<int[]>> games)
    {
        foreach (var game in games)
        {
            Console.WriteLine($"Game {game.Key}:");
            foreach (var round in game.Value)
            {
                Console.WriteLine($"  Round:");
                Console.WriteLine($"    Red: {round[(int)CubeColor.Red]}");
                Console.WriteLine($"    Green: {round[(int)CubeColor.Green]}");
                Console.WriteLine($"    Blue: {round[(int)CubeColor.Blue]}");
            }
        }
    }


}

