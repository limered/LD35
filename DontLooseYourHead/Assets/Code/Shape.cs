public struct Shape
{
    public int Width;
    public int Height;
    public bool[,] Blocks;
    public string Name;

    public Shape(string name, bool[,] blocks, int width, int height)
    {
        Name = name;
        Blocks = blocks;
        Width = width;
        Height = height;
    }

}