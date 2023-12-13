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

            Random random = new Random();

            Game solitaire = new Game(new CardDeck(random));

            int delimiterLenght = 25;

            char delimiterSymbol = '=';

            bool isOpen = true;

            solitaire.AddPlayer(new Player("Josh"));
            solitaire.AddPlayer(new Player("Sam"));
            solitaire.SelectPlayer();

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"Меню.\n{CommandTakeCard} - Сколько карт взять.\n" +
                                  $"{CommandShowCards} - Показать карты.\n{CommandExit} - Выход.");
                Console.WriteLine(new string(delimiterSymbol, delimiterLenght));
                Console.WriteLine($"Игрок: {solitaire.CurrentPlayer.Name}. Количество карт: {solitaire.CurrentPlayer.CountCards()}");

                Console.Write("\nВыберите номер действия: ");
                if (int.TryParse(Console.ReadLine(), out int numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandTakeCard:
                            solitaire.CurrentPlayer.TakeCards(solitaire);
                            break;

                        case CommandShowCards:
                            solitaire.CurrentPlayer.ShowHand();
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

    class Game
    {
        private CardDeck _playingDeck;
        List<Player> _players = new List<Player>();
        Player _currentPlayer;

        public Game(CardDeck deck)
        {
            _playingDeck = deck;
        }

        public int DeckSize => _playingDeck.Count;

        public Player CurrentPlayer => _currentPlayer;

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public Card GiveCard()
        {
            return _playingDeck.GiveCard();
        }

        public void SelectPlayer()
        {
            bool isFind = true;

            while (isFind)
            {
                Console.Clear();
                Console.WriteLine($"На выбор {_players.Count} игрока(ов):");

                for (int i = 0; i < _players.Count; i++)
                    Console.WriteLine($"{i + 1} - {_players[i].Name}.");

                Console.Write("Выберите номер игрока: ");
                int numPlayers = GetFormatInput() - 1;

                if (numPlayers < _players.Count && numPlayers >= 0)
                {
                    _currentPlayer = _players[numPlayers];
                    isFind = false;
                }
                else
                {
                    Error.Show();
                }

                Console.ReadKey(true);
            }
        }

        private int GetFormatInput()
        {
            if (int.TryParse(Console.ReadLine(), out int userInput))
                return userInput;
            else
                return -1;
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
        Random _random;
        private Stack<Card> _cardDeck = new Stack<Card>();

        public CardDeck(Random random)
        {
            _random = random;
            FormDeck();
        }

        public int Count => _cardDeck.Count;

        public Card GiveCard()
        {
            return _cardDeck.Pop();
        }

        private void FormDeck()
        {
            for (int i = 0; i < (int)CardSuit.Max; i++)
                for (int j = 0; j < (int)CardMeaning.Max; j++)
                    _cardDeck.Push(new Card((CardSuit)i, (CardMeaning)j));

            for (int i = 0; i < _cardDeck.Count; i++)
                Shuffle(_cardDeck);
        }

        private void Shuffle(Stack<Card> deck)
        {
            Card[] tempDeck = new Card[deck.Count];

            for (int i = 0; i < tempDeck.Length; i++)
                tempDeck[i] = deck.Pop();

            int countCycles = _random.Next(0, int.MaxValue) % tempDeck.Length;
            int numElement = 0;
            Card tempCard;

            for (int i = 0; i < countCycles; i++)
            {
                for (int j = 0; j < tempDeck.Length; j++)
                {
                    if (numElement == j)
                        numElement = _random.Next(0, tempDeck.Length - 1);

                    tempCard = tempDeck[numElement];
                    tempDeck[numElement] = tempDeck[j];
                    tempDeck[j] = tempCard;
                }
            }

            for (int i = 0; i < tempDeck.Length; i++)
                deck.Push(tempDeck[i]);
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
            Name = name;
        }

        public string Name { get; private set; }

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

        public void TakeCards(Game solitaire)
        {
            bool isNotCorrect = true;

            while (isNotCorrect)
            {
                Console.Clear();
                Console.Write("Сколько карт вы хотите взять?: ");

                if (int.TryParse(Console.ReadLine(), out int countCards))
                {
                    if (countCards >= 0 && countCards < solitaire.DeckSize)
                    {
                        for (int i = 0; i < countCards; i++)
                            _hand.Add(solitaire.GiveCard());

                        isNotCorrect = false;
                    }
                    else
                    {
                        Error.Show();
                    }
                }
                else
                {
                    Error.Show();
                }
            }
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
