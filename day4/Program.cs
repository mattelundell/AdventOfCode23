namespace day4;

internal class Program
{
    static void Main(string[] args)
    {
        //var filePath = "testInput.txt";
        var filePath = "inputData.txt";
        var inputData = File.ReadAllLines(filePath);
        List<Card> cards = ParseInput(inputData);
        var (totalPoints, scratchcardsWon) = CalculateTotalPoints(cards);

        Console.WriteLine($"The total points for all cards is: {totalPoints}");
        Console.WriteLine($"The total number of copies won is: {scratchcardsWon}");
    }

    // Struct used to construct cards based on the input data
    public struct Card
    {
        public int Id { get; set; }
        public List<int> WinningNumbers { get; set; }
        public List<int> OwnedNumbers { get; set; }
    }

    // Parses the input data to a list of Cards
    static List<Card> ParseInput(IEnumerable<string> input)
    {
        List<Card> cards = [];

        foreach (var line in input)
        {
            //Split the input on : to separate the card ID from the numbers
            // "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
            var lineParts = line.Split(':');

            // Trim and split the first linePart to extract the card ID
            // "Card 1"
            var cardId = lineParts[0].Trim();
            var cardIdPart = cardId.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var id = int.Parse(cardIdPart[1]);

            //Trim the second linePart and split it into two sets of numbers (winning numbers and owned numbers)
            // "41 48 83 86 17 | 83 86  6 31 17  9 48 53"
            var numbersPart = lineParts[1].Trim();
            var sets = numbersPart.Split('|', StringSplitOptions.RemoveEmptyEntries);

            //Split and parse the first set of numbers (winning numbers)
            // "41 48 83 86 17"
            // "83 86  6 31 17  9 48 53"
            var winningNumbers = sets[0]
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            //Split and parse the second set of numbers (owned numbers)
            // "83 86  6 31 17  9 48 53"
            var ownedNumbers = sets[1]
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            // Create a Card object with the parsed data
            // Add it to the list of cards
            cards.Add(
                new Card
                {
                    Id = id,
                    WinningNumbers = winningNumbers,
                    OwnedNumbers = ownedNumbers
                }
            );
        }

        return cards;
    }

    // Calculates the total points of a scratchcard from a list of cards and summarizes the number of scratchcards won.
    static (int totalPoints, int scratchcardsWon) CalculateTotalPoints(List<Card> cards)
    {
        int originalNumOfCards = cards.Count;
        List<int> cardCopies = new(new int[originalNumOfCards + 1]);
        List<int> matches = new(new int[originalNumOfCards + 1]);
        int totalPoints = 0;

        // Evaluate each card, store the number of matches, and add its points to the total
        for (int cardId = 1; cardId <= originalNumOfCards; cardId++)
        {
            Card currentCard = cards.First(card => card.Id == cardId);
            int numOfMatches = currentCard
                .OwnedNumbers.Intersect(currentCard.WinningNumbers)
                .Count();
            matches[cardId] = numOfMatches;

            // Calculate points
            int points = numOfMatches == 0 ? 0 : (int)Math.Pow(2, numOfMatches - 1);
            totalPoints += points;

            // Add one of each original card to our list of copies
            cardCopies[cardId] += 1;

            // If we have matches, add the number of copies of the current card to the following cardIds as per the game rule.
            if (matches[cardId] == 0)
            {
                continue;
            }
            for (int j = 1; j <= matches[cardId]; j++)
            {
                cardCopies[cardId + j] += cardCopies[cardId];
            }
        }

        // Sum the total number of copies
        int totalScratchcards = cardCopies.Sum();

        return (totalPoints, totalScratchcards);
    }
}
