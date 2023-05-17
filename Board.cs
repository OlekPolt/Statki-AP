using System.Data;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

public class Board
{
    public char[,] yourBoard;
    public char[,] shootBoard;

    public Board()
    {
        yourBoard = new char[10, 10];
        shootBoard = new char[10, 10];
        CreateBoards();
    }
    public void CreateBoards()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                yourBoard[i, j] = ' ';
                shootBoard[i, j] = ' ';
            }
        }
    }
    public void ShowYourBoard()
    {
        Console.WriteLine("  A B C D E F G H I J");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < 10; j++)
            {
                Console.Write(yourBoard[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
    public void ShowShootBoard()
    {
        Console.WriteLine("  A B C D E F G H I J");
        for (int i = 0; i < 10; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < 10; j++)
            {
                Console.Write(shootBoard[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
    public bool ArePlayerShipsWrecked(Player player)
    {
        foreach (Ship ship in player.yourShips)
        {
            if (ship != null)
            {
                if (!ship.IsWrecked)
                {
                    return false;
                }
            }
        }
        return true;

    }
    public bool AreComputerShipsWrecked(Player player)
    {
        foreach (Ship ship in player.computerShips)
        {
            if (ship != null)
            {
                if (!ship.IsWrecked)
                {
                    return false;
                }
            }
        }
        return true;

    }
    public bool CheckPosition(int row, int column)
    {
        if (row < 0 || row > 9 || column < 0 || column > 9)
            return false;

        return row >= 0 && row < 10 && column >= 0 && column < 10;
    }
    public bool CanPlaceShip(int row, int column, int size, int direction)
    {
        int lastRow = row;
        int lastColumn = column;

        if (direction == 1)
        {
            lastRow -= size + 1;
        }
        if (direction == 2)
        {
            lastRow += size - 1;
        }
        if (direction == 3)
        {
            lastColumn += size - 1;
        }
        if (direction == 4)
        {
            lastColumn -= size + 1;
        }


        if (lastRow >= 10 || lastColumn >= 10)
        {
            return false;
        }

        for (int i = row; i <= lastRow; i++)
        {
            for (int j = column; j <= lastColumn; j++)
            {
                if (i < 0 || i >= 10 || j < 0 || j >= 10 || yourBoard[i, j] != ' ' || !IsSurrounded(i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public bool IsSurrounded(int row, int column)
    {
        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if (i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if (yourBoard[i, j] == 'S')
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public void PlaceShip(int row, int column, int size, int direction)
    {
        for (int i = 0; i < size; i++)
        {
            if (direction == 1)
            {      
              if(CheckPosition(row - i, column))
                {
                    yourBoard[row - i, column] = 'S';
                }
            }
            else if (direction == 2)
            {
                if (CheckPosition(row + i, column))
                {
                    yourBoard[row + i, column] = 'S';
                }
            }
            else if (direction == 3)
            {
                if (CheckPosition(row, column + i))
                {
                    yourBoard[row, column + i] = 'S';
                }
            }
            else if (direction == 4)
            {
                if(CheckPosition(row, column - i))
                {
                    yourBoard[row, column - i] = 'S';
                }
            }
        }
    }
    public char takeHitYou(Board targetBoard, int row, int column, Player player)
    {
        char target = targetBoard.yourBoard[row, column];

        if (target == 'S')
        {
            yourBoard[row, column] = 'X';
            shootBoard[row, column] = 'X';

            foreach (Ship ship in player.yourShips)
            {
                if (ship != null)
                {
                    if (!ship.IsWrecked)
                    {
                        if (ship.shipSize > 0)
                        {
                            ship.Hit();
                            break;
                        }
                    }
                    else
                    { 
                        if (ArePlayerShipsWrecked(player))
                        {
                            player.endOfGamePlayer = true;
                        }
                    }
                }
                
            }
            return 'S';
        }
        else
        {
            shootBoard[row, column] = 'O';
            return ' ';
        }
    }
    public char takeHitComputer(Board targetBoard, int row, int column, Player computer)
    {
        char target = targetBoard.yourBoard[row, column];

        if (target == 'S')
        {

            yourBoard[row, column] = 'X';
            shootBoard[row, column] = 'X';

            foreach (Ship ship in computer.computerShips)
            {
                if (ship != null)
                {
                    if (!ship.IsWrecked)
                    {
                        if (ship.shipSize > 0)
                        {
                            ship.Hit();
                            break;
                        }
                    }
                    else
                    {
                        if (AreComputerShipsWrecked(computer))
                        {
                            computer.endOfGameComputer = true;
                        }
                    }
                }

            }
            return 'S';
        }
        else
        {
            shootBoard[row, column] = 'O';
            return ' ';
        }
    }
}
