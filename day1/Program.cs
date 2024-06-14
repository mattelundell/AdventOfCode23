namespace day1;

internal class Program
{
    // Dictionary used to map strings to their corresponding digit. 
    private static readonly Dictionary<string, int> textToDigit = new(){
        {"zero", 0}, {"one", 1}, {"two", 2},
        {"three", 3}, {"four", 4}, {"five", 5},
        {"six", 6}, {"seven", 7}, {"eight", 8},
        {"nine", 9}
    };
    static void Main(string[] args)
    {
        string inputPath = "input.txt";

        List<string> inputWords = ReadWordsFromFile(inputPath);
        //List<string> testWords1 = ["1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet"];
        //List<string> testWords2 = ["oneight", "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen"];

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

    static string ConvertWordToNumber(string word)
    {
        string result = "";
        int i = 0;

        while (i < word.Length)
        {
            bool foundMatchingKeyWord = false;

            foreach (var entry in textToDigit)
            {
                // Ensures that the remaining substring of the evaluated word is at least as long as the key we are trying to match
                if (word.Length - i >= entry.Key.Length)
                {
                    // The remaining substring of the evaluated word
                    string subString = word.Substring(i, entry.Key.Length);

                    // Checks to see if the beginning of the remaining substring matches with the key, ignoring casing of letters
                    if (subString.Equals(entry.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        // If we have a match, we add the digit to our result string, and jump to the next point in the evaluated word 
                        result += entry.Value;

                        // Have to start the next matching with the last character of the matched key to catch edge cases such as "oneight" -> 18
                        i += entry.Key.Length - 1;
                        foundMatchingKeyWord = true;
                        break;
                    }
                }
            }

            // If we didn't match the beginning of the remaining substring to a key, the substring could begin with a digit
            if (!foundMatchingKeyWord)
            {
                // Ensure that the starting character of the remaining substring is in fact a digit, add it to our result string, move on to the next character
                if (char.IsDigit(word[i]))
                {
                    result += word[i];
                }
                i++;
            }
        }

        return result;
    }

    static int FirstAndLastDigits(string word)
    {
        char? firstDigit = null;
        char? lastDigit = null;

        foreach (char c in word)
        {
            if (char.IsDigit(c))
            {

                // if firstDigit is null, we want to assign the value of c to it as c is the first digit we encountered
                firstDigit ??= c;

                // For each consecutive digit we find, we update the value of lastDigit to ensure that we use the last encountered digit
                lastDigit = c; // 
            }
        }

        // return 0 if no digit vas found
        if (firstDigit == null || lastDigit == null)
        {
            return 0;
        }

        // Construct the resulting value from the first and last digit and parse it as an integer
        string numberString = $"{firstDigit}{lastDigit}";
        int result = int.Parse(numberString);

        return result;
    }
}

