namespace day1;

internal class Program
{
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

    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        List<string> inputWords = ReadWordsFromFile(inputPath);

        // Evaluate and convert each word from input.txt
        List<string> convertedWords = [];
        foreach (var word in inputWords)
        {
            string number = ConvertWordToNumber(word);
            convertedWords.Add(number);
        }

        //Transform each word in the list to the result of the FirstAndLastDigits-method
        // The list calibratedValues contains the first and last digit found in each converted word
        List<int> calibratedValues = convertedWords.Select(FirstAndLastDigits).ToList();
        int sum = calibratedValues.Sum(); // 54094
        Console.WriteLine($"The sum of the calibrated values is: {sum}.");
    }

    // Reads a text file and returns a List<string> with all the input words
    static List<string> ReadWordsFromFile(string filePath)
    {
        return [.. File.ReadAllLines(filePath)];
    }

    // Iterates over a word and converts text representation of numbers to digits
    // Returns a string of the number, e.g. "1oneight" -> "118"
    static string ConvertWordToNumber(string word)
    {
        string result = "";
        int i = 0;

        while (i < word.Length)
        {
            bool foundMatchingKeyWord = false;

            foreach (var entry in StringToDigit)
            {
                // Ensures that the remaining substring of the evaluated word is at least as long as the key we are trying to match
                if (word.Length - i >= entry.Key.Length)
                {
                    // The remaining characters of the word
                    string remainingWord = word.Substring(i, entry.Key.Length);

                    // Checks to see if the beginning matches with the key
                    // If match: add the digit and jump to the next point in the evaluated word
                    // Have to start the next matching with the last character of the matched key to catch edge cases such as "oneight" -> 18
                    if (remainingWord.Equals(entry.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        result += entry.Value;
                        i += entry.Key.Length - 1;
                        foundMatchingKeyWord = true;
                        break;
                    }
                }
            }

            // If no match, check if it is a digit, add it to the result, move to next character
            if (!foundMatchingKeyWord)
            {
                if (char.IsDigit(word[i]))
                {
                    result += word[i];
                }
                i++;
            }
        }
        return result;
    }

    // Iterates over a string representation of a number and returns an integer of the first and last digit
    // "1820" -> 10
    static int FirstAndLastDigits(string word)
    {
        char? firstDigit = null;
        char? lastDigit = null;

        foreach (char c in word)
        {
            if (char.IsDigit(c))
            {
                // if firstDigit is null, we assign c as it is the first digit
                firstDigit ??= c;

                // update lastDigit each time we encounter a new digit
                lastDigit = c; //
            }
        }

        // return 0 if no digit vas found
        if (firstDigit == null || lastDigit == null)
        {
            return 0;
        }

        // Parse the first and last digit to an integer
        string numberString = $"{firstDigit}{lastDigit}";
        int result = int.Parse(numberString);

        return result;
    }
}
