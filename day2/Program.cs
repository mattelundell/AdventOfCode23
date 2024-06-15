using System.IO.Compression;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace day2;

internal class Program
{

    public enum CubeColor
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }

    const int COLOR_COUNT = 3;

    static void Main(string[] args)
    {
        /* string testData =
            "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" +
            "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue" +
            "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red" +
            "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red" +
            "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"; */
        //Dictionary<int, List<int[]>> games = ParseInput(testData);

        string inputPath = "input.txt";
        string inputData = File.ReadAllText(inputPath);
        
        Dictionary<int, List<int[]>> games = ParseInput(inputData);
        PrintGames(games);
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

