public struct Shape
{
    public int Width;
    public int Height;
    public bool[,] Blocks;

    public Shape(bool[,] blocks, int width, int height)
    {
        Blocks = blocks;
        Width = width;
        Height = height;
    }

}