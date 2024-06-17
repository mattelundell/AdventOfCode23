namespace day4;

internal class Program
{
    static void Main(string[] args) { }
}


// TODO: Parse input data.
//    Dictionary with cardId as key?
//    [{winingNumbers}, {ownedNumbers}]?

// TODO: count the number of owned numbers that appear in list of winning numbers

// TODO: Calculate the points for each card (1p first match, then doubble)
//      If numMatches == 0 -> points = 0
//      otherwise: 2^(number of matches - 1)
//      1 match = 2^0 = 1 point, 2 = 2^1 = 2 points, 3 = 2^2 = 4

// TODO: Calculate the sum of the total points