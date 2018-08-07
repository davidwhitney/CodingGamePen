using System;
using System.Collections.Generic;
using System.Linq;
using CodeingGamePen.Fakes;
using NUnit.Framework;

namespace CodeingGamePen
{
    [TestFixture]
    public class War
    {
        public FakeConsole Console { get; set; }

        [SetUp]
        public void SetUp() => Console = new FakeConsole();

        [Test]
        public void Play_()
        {
            var game = new WarGame(new List<string> {"AD", "KC", "QC"}, new List<string> {"KH", "QS", "JC"});
            var result = game.Play();

            Assert.That(result.ToString(), Is.EqualTo("1 3"));
        }

        public void TestRunnerForWebsite()
        {
            var cards1 = new List<string>();
            int n = int.Parse(Console.ReadLine()); // the number of cards for player 1
            for (int i = 0; i < n; i++)
            {
                string cardp1 = Console.ReadLine(); // the n cards of player 1
                cards1.Add(cardp1);
            }

            var cards2 = new List<string>();
            int m = int.Parse(Console.ReadLine()); // the number of cards for player 2
            for (int i = 0; i < m; i++)
            {
                string cardp2 = Console.ReadLine(); // the m cards of player 2
                cards2.Add(cardp2);
            }
            
            var game = new WarGame(cards1, cards2);

            var result = game.Play();
            Console.WriteLine(result);
        }
    }

    public class WarGame
    {
        public List<Player> Players { get; set; } = new List<Player>();

        public WarGame(IEnumerable<string> p1Cards, IEnumerable<string> p2Cards)
        {
            Players.Add(new Player(1, p1Cards));
            Players.Add(new Player(2, p2Cards));
        }

        public GameResult Play()
        {
            int rounds = 0;
            while (Players.All(player => player.Hand.Any()))
            {
                var onTheTable = new List<Pick>();
                var warChest = new List<Pick>();

                Players.ForEach(p => onTheTable.Add(p.DrawCard()));

                if (onTheTable.All(scores => scores.Score == onTheTable.First().Score))
                {
                    warChest.AddRange(onTheTable);
                    onTheTable.Clear();

                    foreach (var player in Players)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            if (player.Hand.Count > 0)
                            {
                                var drawCard = player.DrawCard();
                                warChest.Add(drawCard);
                            }
                        }
                    }
                }
                else
                {
                    var winningCard = onTheTable.Single(x => x.Score == onTheTable.Max(s=>s.Score));
                    var currentWinner = winningCard.Player;
                    onTheTable.ForEach(c => currentWinner?.Hand.Enqueue(c.Card));
                    onTheTable.Clear();
                }

                rounds++;
            }

            var winner = Players.Single(x => x.Hand.Any());
            return new GameResult
            {
                WinningPlayer = winner.Id,
                GameRounds = rounds
            };
        }
    }

    public class Player
    {
        public int Id { get; set; }
        public Queue<string> Hand { get; set; }

        public Player(int id, IEnumerable<string> cards)
        {
            Id = id;
            Hand = new Queue<string>(cards);
        }

        public Pick DrawCard()
        {
            var card = Hand.Dequeue();
            var cardScore = 0;
            var cardSortOrder = new List<string> {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};

            for (int value = 0; value < cardSortOrder.Count; value++)
            {
                var val = cardSortOrder[value];
                if (card.StartsWith(val))
                {
                    cardScore = value;
                }
            }

            return new Pick
            {
                Player = this,
                Card = card,
                Score = cardScore
            };
        }
    }

    public class Pick
    {
        public string Card { get; set; }
        public int Score { get; set; }
        public Player Player { get; set; }
    }

    public class GameResult
    {
        public int? WinningPlayer { get; set; }
        public int GameRounds { get; set; }
        public override string ToString() => WinningPlayer.HasValue ? WinningPlayer + " " + GameRounds : "PAT";
    }
}
