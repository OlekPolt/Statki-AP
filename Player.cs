using System;

public class Player
{
    public bool endOfGamePlayer { get; set; }
    public bool endOfGameComputer { get; set; }
    Board yourBoard;
    Board shootBoard;
    int shipNumber;
    const int MaxShipNumber = 10;
    public Ship[] yourShips;
    public Ship[] computerShips;
    public Player(Board yourBoard)
    {
        this.yourBoard = yourBoard;
        shootBoard = new Board();
        yourShips = new Ship[MaxShipNumber];
        computerShips = new Ship[MaxShipNumber];
        shipNumber = 0;

    }
    public bool CheckDirection(int direction)
    {
        return direction == 1 || direction == 2 || direction == 3 || direction == 4;
    }
    public bool CheckShot(int row, int column)
    {
        return row >= 0 && row < 10 && column >= 0 && column < 10 && shootBoard.shootBoard[row, column] == ' ';
    }
    public void PlaceShips(Board targetBoard)
    {
        int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        for (int i = 0; i < 11; i++)
        {
            Console.WriteLine($"Ustaw statek o rozmiarze {shipSizes[i]}.");
            bool goodPlacement = false;

            while (!goodPlacement)
            {
                Console.Write("Podaj kolumne (A-J)");
                string columnStr = Console.ReadLine().ToUpper();
                Console.Write("Podaj wiersz (0-9)");
                int row = GetInt();

                int column = "ABCDEFGHIJ".IndexOf(columnStr);

                Console.Write("Podaj kierunek 1.Góra  2.Dół  3.Prawo  4.Lewo ");
                int direction = GetInt();
                Console.WriteLine();

                if (targetBoard.CheckPosition(row, column) && CheckDirection(direction))
                {
                    if (targetBoard.CanPlaceShip(row, column, shipSizes[i], direction))
                    {
                        targetBoard.PlaceShip(row, column, shipSizes[i], direction);
                        goodPlacement = true;

                        yourShips[shipNumber] = new Ship(shipSizes[i]);
                        shipNumber++;
                    }
                    else
                    {
                        Console.WriteLine("Zła pozycja");
                    }
                }
                else
                {
                    Console.WriteLine("Złe dane");
                }
            }
            Console.Clear();
            Console.WriteLine("Twoja plansza:");
            yourBoard.ShowYourBoard();
            Console.WriteLine("Plansza do ataku:");
            shootBoard.ShowShootBoard();

            if (shipNumber == 10)
            {
                Console.WriteLine("Postawiłeś wszystkie statki");
                break;
            }
        }
    }
    public void ComputerPlaceShips(Board targetBoard)
    {
        Random random = new Random();
        int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        int shipCount = 0;

        while (shipCount < 10)
        {
            int shipSize = shipSizes[shipCount];
            bool goodPlacement = false;

            while (!goodPlacement)
            {
                int row = random.Next(10);
                int column = random.Next(10);

                int direction;

                if (random.Next(4) == 0)
                {
                    direction = 1;
                }
                else if (random.Next(4) == 1)
                {
                    direction = 2;
                }
                else if (random.Next(4) == 2)
                {
                    direction = 3;
                }
                else
                {
                    direction = 4;
                }

                if (targetBoard.CanPlaceShip(row, column, shipSize, direction))
                {
                    targetBoard.PlaceShip(row, column, shipSize, direction);
                    goodPlacement = true;
                    computerShips[shipCount] = new Ship(shipSize);
                    shipCount++;
                }
            }
        }
    }
    public void Shoot(Board targetBoard, Player player)
    {
        Console.Write("Podaj kolumne strzału (A-J)");
        string columnStr = Console.ReadLine().ToUpper();

        Console.Write("Podaj wiersz strzału (0-9)");
        int row = GetInt();

        int column = "ABCDEFGHIJ".IndexOf(columnStr);

        if (targetBoard.CheckPosition(row, column))
        {
            if (CheckShot(row, column))
            {
                char result = targetBoard.takeHitComputer(targetBoard, row, column, player);

                if (result == 'S')
                {
                    if (targetBoard.AreComputerShipsWrecked(player))
                    {
                        Console.WriteLine("Wszystkie statki wroga zniszczone, wygrałeś !!!");
                    }
                    else
                    {
                        Console.WriteLine("Trafiłeś, strzel jeszcze raz!");
                        Shoot(targetBoard, player);
                    }
                }
                else
                {
                    Console.WriteLine("Pudło!");
                }
            }
            else
            {
                Console.WriteLine("Zła pozycja");
            }
        }
        else
        {
            Console.WriteLine("Złe dane");
        }
    }
    public bool[,] memory = new bool[10, 10];
    public void ComputerShoot(Board targetBoard, Player player)
    {
        Random random = new Random();

        int row;
        int column;
        bool validAttack;

        do
        {
            int[] position = GetRandomUnused();
            row = position[0];
            column = position[1];
            validAttack = CheckShot(row, column);

            memory[row, column] = true;

        } while (!validAttack); ;

            char result = targetBoard.takeHitYou(targetBoard, row, column, player);

            if (result == 'S')
            {
                if (!targetBoard.ArePlayerShipsWrecked(player))
                {
                Console.WriteLine("Komputer trafił, kolejny strzał");
                ComputerShoot(targetBoard, player);
                }
            }
    }
    public bool IsUsed(int row, int column)
    {
        return memory[row, column] = true;
    }
    int[] GetRandomUnused()
    {
        Random random = new Random();
        int row, column;

        do
        {
            row = random.Next(10);
            column = random.Next(10);
        }
        while (memory[row, column]);

        return new int[] { row, column };
    }

    public int GetInt()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int value))
            {
                return value;
            }
            Console.WriteLine("Złą wartość");
        }
    }

    
}
