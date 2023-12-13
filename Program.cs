using System;
using System.Collections.Generic;
using System.Linq;

namespace L41_cardBlock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int CommandTakeCard = 1;
            const int CommandShowCards = 2;
            const int CommandExit = 3;

            Player[] players = { new Player("Josh"), new Player("Sam") };
            Player currentPlayer = Player.SelectPlayer(players);

            CardDeck cardDeck = new CardDeck();

            int delimiterLenght = 25;

            char delimiterSymbol = '=';

            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"Меню.\n{CommandTakeCard} - Сколько карт взять.\n" +
                                  $"{CommandShowCards} - Показать карты.\n{CommandExit} - Выход.");
                Console.WriteLine(new string(delimiterSymbol, delimiterLenght));
                Console.WriteLine($"Игрок: {currentPlayer.Name}. Количество карт: {currentPlayer.CountCards()}");

                Console.Write("\nВыберите номер действия: ");
                if (int.TryParse(Console.ReadLine(), out int numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandTakeCard:
                            currentPlayer.TakeCards(cardDeck);
                            break;

                        case CommandShowCards:
                            currentPlayer.ShowHand();
                            break;

                        case CommandExit:
                            isOpen = false;
                            break;

                        default:
                            Error.Show();
                            break;
                    }
                }

                Console.ReadKey(true);
            }
        }
    }

    class Error
    {
        public static void Show()
        {
            Console.WriteLine("Вы ввели некорректное значение.\nДля продолжения нажмите любую клавишу...");
            Console.ReadKey(true);
        }
    }

    class CardDeck
    {
        private Stack<Card> _cardDeck = new Stack<Card>();

        public CardDeck()
        {
            for (int i = 0; i < (int)CardSuit.Max; i++)
                for (int j = 0; j < (int)CardMeaning.Max; j++)
                    _cardDeck.Push(new Card((CardSuit)i, (CardMeaning)j));

            for (int i = 0; i < _cardDeck.Count; i++)
                Shuffle(_cardDeck);
        }

        public int Count
        {
            get
            {
                return _cardDeck.Count;
            }
        }

        public void Shuffle(Stack<Card> deck)
        {
            Random random = new Random();
            Card[] tempDeck = new Card[deck.Count];

            for (int i = 0; i < tempDeck.Length; i++)
                tempDeck[i] = deck.Pop();

            int countCycles = random.Next(0, int.MaxValue) % tempDeck.Length;
            int numElement = 0;
            Card tempCard;

            for (int i = 0; i < countCycles; i++)
            {
                for (int j = 0; j < tempDeck.Length; j++)
                {
                    if (numElement == j)
                        numElement = random.Next(0, tempDeck.Length - 1);

                    tempCard = tempDeck[numElement];
                    tempDeck[numElement] = tempDeck[j];
                    tempDeck[j] = tempCard;
                }
            }

            for (int i = 0; i < tempDeck.Length; i++)
                deck.Push(tempDeck[i]);
        }

        public Card GiveCard()
        {
            return _cardDeck.Pop();
        }
    }

    class Card
    {
        private CardSuit _suit;
        private CardMeaning _meaning;

        public Card(CardSuit suit, CardMeaning meaning)
        {
            _suit = suit;
            _meaning = meaning;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Масть: {_suit}, значение: {_meaning}");
        }
    }

    class Player
    {
        private List<Card> _hand = new List<Card>();

        public Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static Player SelectPlayer(Player[] players)
        {
            Player player = null;
            bool isFind = true;

            while (isFind)
            {
                Console.Clear();
                Console.WriteLine($"На выбор {players.Length} игрока(ов):");

                for (int i = 0; i < players.Length; i++)
                    Console.WriteLine($"{i + 1} - {players[i].Name}.");

                Console.Write("Выберите номер игрока: ");
                int numPlayers = GetFormatInput() - 1;

                if (numPlayers < players.Length && numPlayers >= 0)
                {
                    player = players[numPlayers];
                    isFind = false;
                }
                else
                {
                    Error.Show();
                }

                Console.ReadKey(true);
            }

            return player;
        }

        public void TakeCards(CardDeck deck)
        {
            bool isNotCorrect = true;

            while (isNotCorrect)
            {
                Console.Clear();
                Console.Write("Сколько карт вы хотите взять?: ");
                int countCards = GetFormatInput();

                if (countCards >= 0 && countCards < deck.Count)
                {
                    for (int i = 0; i < countCards; i++)
                        _hand.Add(deck.GiveCard());

                    isNotCorrect = false;
                }
                else
                {
                    Error.Show();
                }
            }
        }

        public void ShowHand()
        {
            if (_hand.Count == 0)
                Console.WriteLine("У вас нет карт.");
            else
                foreach (var card in _hand)
                    card.ShowInfo();
        }

        public int CountCards()
        {
            return _hand.Count();
        }

        static int GetFormatInput()
        {
            if (int.TryParse(Console.ReadLine(), out int userInput))
                return userInput;
            else
                return -1;
        }
    }

    enum CardSuit
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs,
        Max,
    }

    enum CardMeaning
    {
        Joker,
        Ace,
        King,
        Queen,
        Jack,
        Ten,
        Nine,
        Eight,
        Seven,
        Six,
        Five,
        Four,
        Three,
        Two,
        Max,
    }
}
