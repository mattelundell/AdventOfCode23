namespace day1;

internal class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        List<string> lines = ReadLinesFromFile(inputPath);

        // Evaluate and convert each line from input.txt to numbers
        List<string> convertedLines = [];
        foreach (string line in lines)
        {
            string numbers = ConvertLineToNumbers(line);
            convertedLines.Add(numbers);
        }

        // Transform each line to the result of the FirstAndLastDigits-method
        // I.e. return a list with the first and last digit four each line
        List<int> calibratedValues = convertedLines.Select(FirstAndLastDigits).ToList();
        int sum = calibratedValues.Sum();
        Console.WriteLine($"The sum of the calibrated values is: {sum}.");
    }

    // Dictionary used to map strings to their corresponding digit.
    private static readonly Dictionary<string, int> StringToDigit =
        new()
        {
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

    // Reads a text file and returns a List<string> with all the input lines
    static List<string> ReadLinesFromFile(string filePath)
    {
        return [.. File.ReadAllLines(filePath)];
    }

    // Iterates over a word and converts text representation of numbers to digits
    // Returns a string of the number, e.g. "1oneight" -> "118"
    static string ConvertLineToNumbers(string line)
    {
        string result = "";
        int i = 0;

        while (i < line.Length)
        {
            bool digitFound = char.IsDigit(line[i]);
            if (digitFound)
            {
                result += line[i];
                i++;
                continue;
            }

            bool matchedWithKey = false;

            foreach (var entry in StringToDigit)
            {
                // Ensures that the remaining substring is at least as long as the key
                bool notEnoughCharsForKey = line.Length - i < entry.Key.Length;
                if (notEnoughCharsForKey)
                {
                    continue;
                }

                // Try to match the remainder with the key
                var remainingChars = line.Substring(i, entry.Key.Length);
                bool charsMatchedWithKey = remainingChars.Equals(
                    entry.Key,
                    StringComparison.OrdinalIgnoreCase
                );

                // If match, add the digit and jump to the next point in the line
                // Next point is the last character of the matched key to catch edge cases such as "oneight" -> 18
                if (charsMatchedWithKey)
                {
                    result += entry.Value;
                    i += entry.Key.Length - 1;
                    matchedWithKey = true;
                    break;
                }
            }

            // If no match, move to the next char
            if (!matchedWithKey)
            {
                i++;
            }
        }
        return result;
    }

    // Fetches the first and last digits and returns the new number
    // "1820" -> 10
    static int FirstAndLastDigits(string line)
    {
        char firstDigit = line.First();
        char lastDigit = line.Last();

        // Parse the first and last digit to an integer
        string numberString = $"{firstDigit}{lastDigit}";
        int result = int.Parse(numberString);

        return result;
    }
}
