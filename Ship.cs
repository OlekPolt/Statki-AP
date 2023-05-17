public class Ship
{
    public int shipSize {get; set;}
    public bool IsWrecked {get; set;}

    public Ship(int size)
    {
        shipSize = size;
        IsWrecked = false;
    }

    public void Hit()
    {
        shipSize--;
        if (shipSize == 0)
        {
            IsWrecked = true;
        }
    }
}
