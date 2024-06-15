namespace day2;

internal class Program
{

    public enum CubeColor
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }

    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string testData =
            "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\n" +
            "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\n" +
            "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\n" +
            "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\n" +
            "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

        Dictionary<int, List<int[]>> games = ParseInput(testData);

        PrintGames(games);

    }

    static Dictionary<int, List<int[]>> ParseInput(string inputData)
    {
        var games = new Dictionary<int, List<int[]>>();

        // Delimiters
        char partDelimiter = ':';
        char cubeDelimiter = ',';
        char setDelimiter = ';';
        char gameDelimiter = '\n';

        // Split input data into a list of strings
        string[] gameData = inputData.Split(gameDelimiter);

        // Loop through each game
        foreach (string game in gameData)
        {
            // Split each game entry into two parts, e.g. {["Game 1"], ["3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"]}
            string[] parts = game.Split(partDelimiter);

            // Splitting the first part and fetch the current gameId
            int gameId = int.Parse(parts[0].Split(' ')[1]);

            // Split the current game into separate sets, e.g. {["3 blue, 4 red"], ["1 red, 2 green, 6 blue"], ["2 green"]]
            string[] setData = parts[1].Split(setDelimiter);

            // Store each set in a list of int arrays, e.g. {[3, 4, 1], [2, 1, 2], [0, 1, 2]}.
            // Each index represents a cube color by mapping with the enums CubeColor
            var sets = new List<int[]>();

            // Loop through each set
            foreach (string set in setData)
            {
                // A list of integers of length 3, i.e. the number of colors in our enum, initalized to [0,0,0]
                // Each index represents a cube color, e.g. CubeColor.Red = index 0
                var gameRound = new int[Enum.GetValues(typeof(CubeColor)).Length];

                // Split the current set into the different cubes, e.g. {["3 blue"], ["4 red"]}
                string[] cubesInSet = set.Split(cubeDelimiter);

                foreach (string cubes in cubesInSet)
                {
                    // Trim any excess whitespace and split into two parts, e.g. ["6", "blue"]
                    string trimmedCubes = cubes.Trim();
                    string[] splitCubes = trimmedCubes.Split(' ');

                    // Convert the number of cubes to an integer and parse the color using the enums to get the correct index
                    int count = int.Parse(splitCubes[0]);
                    CubeColor color = (CubeColor)Enum.Parse(typeof(CubeColor), splitCubes[1], true);

                    // Store the number of cubes in the correct index position based on its color
                    gameRound[(int) color] = count;
                }
                // Add each game round to our list of sets, e.g. [6, 2, 1] = 6 red cubes, 2 green cubes, 1 blue cube
                sets.Add(gameRound);
            }
            // Add the sets to the list of games in the current gameId's position
            games[gameId] = sets;
        }

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

