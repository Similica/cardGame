using System;
using System.Collections.Generic;
using System.Linq;

public class Game
{
    public int CalculateMinimumSteps(List<char> cards)
    {
        bool won = false;
        var player1Hand = cards.Take(5).OrderBy(card => card).ToList();
        var player2Hand = cards.Skip(5).Take(4).OrderBy(card => card).ToList();
        var player3Hand = cards.Skip(9).Take(4).OrderBy(card => card).ToList();

        Console.WriteLine($"Karte igrača 1: {string.Join("", player1Hand)}");
        Console.WriteLine($"Karte igrača 2: {string.Join("", player2Hand)}");
        Console.WriteLine($"Karte igrača 3: {string.Join("", player3Hand)}");

        var player1Missing = GetMissingCards(player1Hand);
        var player2Missing = GetMissingCards(player2Hand);
        var player3Missing = GetMissingCards(player3Hand);
        var currentPlayer = 1;
        var turnsSince2Passed = 0;
        var stepsTaken = 0;

        while (!won)
        {
            var currentHand = new List<char>();
            var currentMissing = new List<char>();
            var nextHand = new List<char>();
            var nextMissing = new List<char>();

            if(currentPlayer == 1)
            {
                currentHand = player1Hand;
                currentMissing = player1Missing;
                nextHand = player2Hand;
                nextMissing = player2Missing;
            }
            else if(currentPlayer == 2)
            {
                currentHand = player2Hand;
                currentMissing = player2Missing;
                nextHand = player3Hand;
                nextMissing = player3Missing;
            }
            else
            {
                currentHand = player3Hand;
                currentMissing = player3Missing;
                nextHand = player1Hand;
                nextMissing = player1Missing;
            }

            var cardToPass = GetCardToPass(currentHand, currentMissing, nextMissing, turnsSince2Passed);
            currentHand.Remove(cardToPass);
            currentHand.Sort();
            nextHand.Add(cardToPass);
            nextHand.Sort();
            stepsTaken++;
            currentMissing = GetMissingCards(currentHand);
            nextMissing = GetMissingCards(nextHand);

            if (currentMissing.Count == 0 && !currentHand.Contains('2') && currentHand.Count==4)
            {
                won = true;
                currentPlayer = currentPlayer == 0 ? 3 : currentPlayer;
                Console.WriteLine("Pobjednik je igrač " + currentPlayer);
                return stepsTaken;
            }
    
            currentPlayer = (currentPlayer + 1) % 3;
            turnsSince2Passed = (turnsSince2Passed + 1) % 3;
        }

        return stepsTaken;
    }

    private char GetCardToPass(List<char> hand, List<char> missing, List<char> nextMissing, int turnsSince2Passed)
    {
        var cardToPass = ' ';
        
        if(hand.Contains('2') && turnsSince2Passed == 2)
        {
            cardToPass = '2';
        }
        else if(hand.Contains('J') && missing.Contains('J') == false && nextMissing.Contains('J'))
        {
            cardToPass = 'J';
        }
        else if(hand.Contains('Q') && missing.Contains('Q') == false && nextMissing.Contains('Q'))
        {
            cardToPass = 'Q';
        }
        else if(hand.Contains('K') && missing.Contains('K') == false && nextMissing.Contains('K'))
        {
            cardToPass = 'K';
        }
        else 
        {
            hand.Remove('2');
            cardToPass = hand.First();
        }
        return cardToPass;
    }
    private List<char> GetMissingCards(List<char> hand)
    {
        var missingCards = new List<char>();

        var cards = new List<char>() { 'J', 'Q', 'K', '2' };
        var noOf_J = CountOccurences(hand, 'J');
        var noOf_Q = CountOccurences(hand, 'Q');
        var noOf_K = CountOccurences(hand, 'K');
        var noOf_2 = CountOccurences(hand, '2');

        List<int> countsOfElement = new List<int>() {
            noOf_J, noOf_Q, noOf_K, noOf_2
        };

        if (CountOccurences(countsOfElement, 4) == 1 && noOf_2 == 0)
        {
            return missingCards;
        }
        else if (CountOccurences(countsOfElement, 3) == 1 && CountOccurences(countsOfElement, 2) == 0)
        {
            int missingIndex = countsOfElement.IndexOf(3);
            missingCards = new List<char>() { cards[missingIndex] };
        }
        else if (CountOccurences(countsOfElement, 3) == 1 && CountOccurences(countsOfElement, 2) == 1)
        {
            int missingIndex = countsOfElement.IndexOf(3);
            int missingIndex2 = countsOfElement.IndexOf(2);
            missingCards = new List<char>() { cards[missingIndex], cards[missingIndex2] };
        }
        else if (CountOccurences(countsOfElement, 2) == 2)
        {
            int missingIndex = countsOfElement.IndexOf(2);
            int missingIndex2 = countsOfElement.LastIndexOf(2);
            missingCards.Add(cards[missingIndex]);
            missingCards.Add(cards[missingIndex2]);
        }
        else if (CountOccurences(countsOfElement, 2) == 1)
        {
            int missingIndex = countsOfElement.IndexOf(2);
            missingCards.Add(cards[missingIndex]);
        }
        else
        {
            missingCards = new List<char>() { 'J', 'Q', 'K' };
        }

        return missingCards;
    }
    public static int CountOccurences<T>(List<T> list, T item)
    {
        int count = 0;
        foreach (T element in list)
        {
            if (EqualityComparer<T>.Default.Equals(element, item))
            {
                count++;
            }
        }
        return count;
    }

public static void Main(string[] args)
{
var game = new Game();
var deck = new List<char>()
{
'J','J','J','J','Q','Q','Q','Q','K','K','K','K','2'
};
Random random = new Random();
List<char> shuffledDeck = deck.OrderBy(x => random.Next()).ToList();
int minSteps = game.CalculateMinimumSteps(shuffledDeck);
Console.WriteLine("Igra je završena nakon " + minSteps + " poteza.");
}

}