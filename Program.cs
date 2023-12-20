using System;
using System.Collections.Generic;
using System.Linq;

namespace L41_cardBlock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game solitaire = new Game(new Random());

            solitaire.Run();
        }
    }

    class Game
    {
        private const int CommandPlayerTakeCard = 1;
        private const int CommandPlayerShowCards = 2;
        private const int CommandGameExit = 3;

        private Deck _deck;
        private Player _currentPlayer;
        private List<Player> _players = new List<Player>();

        public Game(Random random)
        {
            _deck = new Deck(random);

            AddPlayers();
            SelectPlayer();
        }

        public void Run()
        {
            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"Меню.\n{CommandPlayerTakeCard} - Сколько карт взять.\n" +
                                  $"{CommandPlayerShowCards} - Показать карты.\n{CommandGameExit} - Выход.");
                Console.WriteLine(new string(FormatOutput.DelimiterSymbol, FormatOutput.DelimiterLenght));
                Console.WriteLine($"Игрок: {_currentPlayer.Name}. Количество карт: {_currentPlayer.NumberCards}");

                Console.Write("\nВыберите номер действия: ");
                if (int.TryParse(Console.ReadLine(), out int numberMenu))
                {
                    Console.Clear();

                    switch (numberMenu)
                    {
                        case CommandPlayerTakeCard:
                            DealCards();
                            break;

                        case CommandPlayerShowCards:
                            _currentPlayer.ShowHand();
                            break;

                        case CommandGameExit:
                            isOpen = false;
                            break;

                        default:
                            Error.Show();
                            break;
                    }
                }
                else
                {
                    Error.Show();
                }

                Console.ReadKey(true);
            }
        }

        private void DealCards()
        {
            bool isNotCorrect = true;

            while (isNotCorrect)
            {
                Console.Clear();
                Console.Write("Сколько карт вы хотите взять?: ");

                if (int.TryParse(Console.ReadLine(), out int cardsCount))
                {
                    if (cardsCount >= 0 && cardsCount < _deck.Count)
                    {
                        for (int i = 0; i < cardsCount; i++)
                            if (_deck.TryGiveCard(out Card card))
                                _currentPlayer.TakeCard(card);

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

        private void SelectPlayer()
        {
            bool isFind = true;

            while (isFind)
            {
                Console.Clear();
                Console.WriteLine($"На выбор {_players.Count} игрока(ов):");

                for (int i = 0; i < _players.Count; i++)
                    Console.WriteLine($"{i + 1} - {_players[i].Name}.");

                Console.Write("Выберите номер игрока: ");

                if (int.TryParse(Console.ReadLine(), out int playerNumber))
                {
                    playerNumber--;

                    if (playerNumber < _players.Count && playerNumber >= 0)
                    {
                        _currentPlayer = _players[playerNumber];
                        isFind = false;
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

                Console.ReadKey(true);
            }
        }

        private void AddPlayers()
        {
            _players.Add(new Player("Josh"));
            _players.Add(new Player("Sam"));
        }
    }

    class FormatOutput
    {
        private static int _delimiterLenght = 35;
        private static char _delimiterSymbol = '=';

        public static int DelimiterLenght => _delimiterLenght;

        public static char DelimiterSymbol => _delimiterSymbol;
    }

    class Error
    {
        public static void Show()
        {
            Console.WriteLine("Вы ввели некорректное значение.\nДля продолжения нажмите любую клавишу...");
        }
    }

    class Deck
    {
        private Random _random;
        private Stack<Card> _cards = new Stack<Card>();

        public Deck(Random random)
        {
            _random = random;
            Fill();
        }

        public int Count => _cards.Count;

        public bool TryGiveCard(out Card card)
        {
            if (_cards.Count > 0)
            {
                card = _cards.Pop();
                return true;
            }
            else
            {
                card = null;
                return false;
            }
        }

        private void Fill()
        {
            List<Card> tempDeck = new List<Card>();
            int suitsCount = Enum.GetNames(typeof(CardSuit)).Length;
            int numberValues = Enum.GetNames(typeof(CardMeaning)).Length;

            for (int i = 0; i < suitsCount; i++)
                for (int j = 0; j < numberValues; j++)
                    tempDeck.Add(new Card((CardSuit)i, (CardMeaning)j));

            Shuffle(tempDeck);
        }

        private void Shuffle(List<Card> tempDeck)
        {
            for (int i = 0; i < tempDeck.Count; i++)
            {
                Card temp = tempDeck[0];
                tempDeck.RemoveAt(0);
                tempDeck.Insert(_random.Next(tempDeck.Count), temp);
            }

            for (int i = 0; i < tempDeck.Count; i++)
                _cards.Push(tempDeck[i]);
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

        public int NumberCards => _hand.Count;

        public void ShowHand()
        {
            if (_hand.Count == 0)
                Console.WriteLine("У вас нет карт.");
            else
                foreach (var card in _hand)
                    card.ShowInfo();
        }


        public void TakeCard(Card card)
        {
            if (card != null)
                _hand.Add(card);
        }
    }

    enum CardSuit
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs,
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
    }
}
