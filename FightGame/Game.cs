using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FightGame
{
    public class Game
    {
        public const int DefaultLives = 2;
        public const int DefaultPower = 10;

        public List<Player> Players { get; set; }

        private Random _random = new Random(DateTime.Now.Millisecond);
        private int _lastId = 0;

        public Game()
        {
            // Generador de arte ascii: http://patorjk.com/software/taag/#p=display&f=Graffiti&t=Fight%20Game

            ConsoleHelper.Write(@"___________.__       .__     __      ________                       
\_   _____/|__| ____ |  |___/  |_   /  _____/_____    _____   ____  
 |    __)  |  |/ ___\|  |  \   __\ /   \  ___\__  \  /     \_/ __ \ 
 |     \   |  / /_/  >   Y  \  |   \    \_\  \/ __ \|  Y Y  \  ___/ 
 \___  /   |__\___  /|___|  /__|    \______  (____  /__|_|  /\___  >
     \/      /_____/      \/               \/     \/      \/     \/  by Diego", ConsoleColor.Cyan);

            Players = new List<Player>
            {
                
                new Player
                {
                    Id = ++_lastId,
                    Name = "Cat Woman",
                    Gender = Gender.Female,
                    Lives = DefaultLives,
                    Power = DefaultPower
                },
                new Player
                {
                    Id = ++_lastId,
                    Name = "Lobezno",
                    Gender = Gender.Male,
                    Lives = DefaultLives,
                    Power = DefaultPower
                },
                new Player
                {
                    Id = ++_lastId,
                    Name = "Wonder Woman",
                    Gender = Gender.Female,
                    Lives = DefaultLives,
                    Power = DefaultPower
                },
                new Player
                {
                    Id = ++_lastId,
                    Name = "Batman",
                    Gender = Gender.Male,
                    Lives = DefaultLives,
                    Power = DefaultPower
                },
            };
        }

        public void Run()
        {
            Menu();

            while (true)
            {
                var option = Console.ReadKey(true);

                if (option.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n¡Hasta luego!");
                    Task.Run(async () => await Task.Delay(1500)).Wait();
                    break;
                }

                switch (option.KeyChar)
                {
                    case '0':
                        Menu();
                        break;

                    case '1':
                        AddPlayer();
                        break;

                    case '2':
                        Status();
                        break;

                    case '3':
                        Fight();
                        break;

                    case 'c':
                        Console.Clear();
                        break;
                }
            }
        }

        private void Menu()
        {
            Console.WriteLine("\n\nAyuda:\n");
            Console.WriteLine("0. Mostrar ayuda");
            Console.WriteLine("1. Añadir jugador");
            Console.WriteLine("2. Status");
            Console.WriteLine("3. Luchar");
            Console.WriteLine("c. Clear");
            Console.WriteLine("Esc. Salir\n");
        }

        public void AddPlayer()
        {
            string name = null;
            
            while(string.IsNullOrEmpty(name) || name.Length < 3)
            {
                Console.WriteLine("\n\nEscribe nombre del jugador (y presiona enter):");
                name = Console.ReadLine();
            }

            Gender? gender = null;

            while(gender == null)
            {
                Console.WriteLine("\nElige sexo:\n1. Femenino\n2. Masculino");
                var genderKey = Console.ReadKey(true);

                if (genderKey.KeyChar == '1')
                {
                    gender = Gender.Female;
                }
                else if (genderKey.KeyChar == '2')
                {
                    gender = Gender.Male;
                }
            }

            var player = new Player
            {
                Id = ++_lastId,
                Gender = gender.Value,
                Name = name,
                Power = DefaultPower,
                Lives = DefaultLives
            };

            Players.Add(player);

            ConsoleHelper.Write($"\n{player.Name} ha sido añadido", ConsoleColor.Yellow);
        }

        public void Fight()
        {
            var currentPlayers = Players
                .Where(x => x.Lives > 0)
                .ToList();

            // hay más de un jugador?
            if (currentPlayers.Count < 2)
            {
                ConsoleHelper.Write("\nNo hay suficientes jugadores", ConsoleColor.Red);
                return;
            }

            // elegir un player aleatoriamente
            var indexPlayer1 = _random.Next(0, currentPlayers.Count);
            var player1 = currentPlayers[indexPlayer1];

            // elegir el segundo player aleatoriamente pero que no se repita
            int indexPlayer2 = _random.Next(0, currentPlayers.Count);
            while (indexPlayer1 == indexPlayer2)
                indexPlayer2 = _random.Next(0, currentPlayers.Count);

            var player2 = currentPlayers[indexPlayer2];

            // quitamos power al player 2 (el nivel de daño será aleatorio entre 1 y 5)
            var damage = _random.Next(1, 5);
            player2.Power -= damage;

            ConsoleHelper.Write($"==> {player1.Name} ha zurrado a {player2.Name}", 
                ConsoleColor.Blue);

            if (player2.Power <= 0)
            {
                player2.Lives--;
                player2.Power = player2.Lives > 0 
                    ? DefaultPower 
                    : 0;

                if (player2.Lives > 0)
                {
                    ConsoleHelper.Write($"{player2.Name} ha perdido una vida", 
                        ConsoleColor.Yellow);
                }
                else
                {
                    player2.Gems = 0;
                    ConsoleHelper.Write($"{player2.Name} ha muerto", 
                        ConsoleColor.Red);
                }

                player1.Gems++;

                ConsoleHelper.Write($"{player1.Name} ha ganado una gema. " +
                    $"Ahora tiene {player1.Gems} en total.", 
                    ConsoleColor.Green);

                // cada 3 gemas le damos una vida
                if (player1.Gems == 3)
                {
                    player1.Lives++;
                    player1.Gems = 0;

                    ConsoleHelper.Write($"{player1.Name} ha ganado una VIDA!!",
                        ConsoleColor.Magenta);
                }

                // comprobar si hay ganador
                if(Players.Count(x => x.Lives > 0) == 1)
                {
                    Console.WriteLine("\n\n+============================================+");
                    Console.WriteLine("+============================================+");
                    Console.WriteLine("+============================================+");
                    ConsoleHelper.Write($"      {player1.Name} HA GANADO", ConsoleColor.Cyan);
                    Console.WriteLine("+============================================+");
                    Console.WriteLine("+============================================+");
                    Console.WriteLine("+============================================+");
                }
            }
        }

        public void Status()
        {
            if (Players.Count == 0)
            {
                Console.WriteLine("\nNo hay jugadores");
            }
            else
            {
                Console.WriteLine($"\nNombre\t\t\tId\tVidas\tPoder\tGemas\tSexo");
                Console.WriteLine($"--------------------------------------------------------");
                
                var ordered = Players
                    .OrderByDescending(player => player.Lives)
                    .ThenByDescending(x => x.Power)
                    .ThenByDescending(x => x.Gems);

                foreach (var player in ordered)
                {
                    player.Status();
                }
            }
        }
    }
}
