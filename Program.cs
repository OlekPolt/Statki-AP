class Program
{
    static void Main()
    {
        Board yourBoard = new Board();
        Board computerBoard = new Board();

        Player player = new Player(yourBoard);
        Player computer = new Player(computerBoard);

        bool yourTurn = true;
        bool endOfGame = false;

        Console.WriteLine("Grasz w statki, ustaw swoje");

        player.PlaceShips(yourBoard);

        Console.WriteLine("Teraz komputer");
        computer.ComputerPlaceShips(computerBoard);

        Console.WriteLine("Start gry");

        while (!endOfGame)
        {
            Console.WriteLine("Plansza strzałów komputera");
            yourBoard.ShowShootBoard();
            Console.WriteLine("Plansza twoich strzałów");
            computerBoard.ShowShootBoard();

            if (yourTurn)
            {
                Console.WriteLine("Teraz ty");
                player.Shoot(computerBoard, player);
                if (player.endOfGamePlayer) 
                {
                    endOfGame = true;
                    Console.WriteLine("Twoje statki zostały zniszczone, przegrałeś :((((");
                }
            }
            else
            {
                Console.WriteLine("Teraz komputer");
                computer.ComputerShoot(yourBoard, computer);
                if (computer.endOfGameComputer)
                {
                    endOfGame = true;
                    Console.WriteLine("Wszystkie statki przeciwnika zniszczone, wygrałeś :))))");
                }

            }
            Console.Clear();
            yourTurn = !yourTurn;
        }

        Console.Clear();
        Console.WriteLine("Plansza strzałów komputera");
        yourBoard.ShowShootBoard();
        Console.WriteLine("Plansza twoich strzałów");
        computerBoard.ShowYourBoard();

        Console.WriteLine("GAME OVER");
    }
}
