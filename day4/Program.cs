namespace day4;

internal class Program
{
    static void Main(string[] args)
    {
        //var filePath = "testInput.txt";
        var filePath = "inputData.txt";
        var inputData = File.ReadAllLines(filePath);
        List<Card> cards = ParseInput(inputData);
        var totalPoints = CalculateTotalPoints(cards);
        var totalScratchcardsWon = CalculateScratchcardsWon(cards);
        Console.WriteLine($"The total points for all cards is: {totalPoints}");
        Console.WriteLine($" You have won: {totalScratchcardsWon} scratchcards");
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

    // Calculates the total points of a scratchcard from a list of cards.
    static int CalculateTotalPoints(List<Card> cards)
    {
        int totalPoints = 0;

        // Evaluate each card and add its points to the total
        foreach (var card in cards)
        {
            int matches = card.OwnedNumbers.Intersect(card.WinningNumbers).Count();
            int points = matches == 0 ? 0 : (int)Math.Pow(2, matches - 1);
            totalPoints += points;
        }

        return totalPoints;
    }

    // Calculates the number of scratchcards, original set and copies won, from a list of cards.
    static int CalculateScratchcardsWon(List<Card> cards)
    {
        // Initialize a queue with the original cards
        int numberOfScratchcards = cards.Count;
        Queue<(int cardId, int copy)> cardsToScratch = new();
        for (int i = 0; i < numberOfScratchcards; i++)
        {
            cardsToScratch.Enqueue((i + 1, 1));
        }

        // Keep scratching cards as long as there are cards in the queue.
        while (cardsToScratch.Count > 0)
        {
            // Take the first item in the queue, match it with a cardId
            // Count the number of matching / winning numbers
            var (currentCardId, copy) = cardsToScratch.Dequeue();
            Card currentCard = cards.First(card => card.Id == currentCardId);
            int matches = currentCard.OwnedNumbers.Intersect(currentCard.WinningNumbers).Count();

            // add the number of matches, i.e. number of cards won to total
            numberOfScratchcards += matches;

            // Add each copy won to the queue
            for (int i = 1; i <= matches; i++)
            {
                int wonCardId = currentCardId + i;
                if (wonCardId > numberOfScratchcards)
                {
                    break;
                }
                cardsToScratch.Enqueue((wonCardId, copy));
            }
        }

        return numberOfScratchcards;
    }
}
