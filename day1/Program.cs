namespace day1;

internal class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";

        List<string> inputWords = ReadWordsFromFile(inputPath);

        // Select transforms each word in the list of words to the result of the CalibrateValue-method and stores it in a list of integers
        List<int> calibratedValues = inputWords.Select(CalibrateValue).ToList();

        int sum = calibratedValues.Sum(); // 54968
        Console.WriteLine($"The sum of the calibrated values is: {sum}.");
    }

    static List<string> ReadWordsFromFile(string filePath)
    {
        return [.. File.ReadAllLines(filePath)];
    }

    static int CalibrateValue(string word)
    {
        char? firstDigit = null;
        char? lastDigit = null;

        foreach (char c in word)
        {
            if (char.IsDigit(c))
            {

                // if firstDigit is null, we want to assign the value of c to it as c is the first digit we encountered
                firstDigit ??= c;

                // For each consecutive digit we find, we update the value of lastDigit to ensure that we use the last one
                lastDigit = c; // 
            }
        }

        // return 0 if no digit vas found
        if (firstDigit == null || lastDigit == null)
        {
            return 0;
        }

        // Construct the calibrated value from the first and last digit and parse it as an integer
        string numberString = $"{firstDigit}{lastDigit}";
        int calibratedValue = int.Parse(numberString);

        return calibratedValue;
    }
}

