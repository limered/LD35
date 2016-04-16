﻿using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator
{
    private readonly Dictionary<string, Shape> cache = new Dictionary<string, Shape>();
    private readonly Game game;
    private readonly System.Random rng = new System.Random();

    public ShapeCreator()
    {
        game = IoC.Resolve<Game>();
    }


    public Shape GenerateNextShape()
    {
        var texture = game.ValidShapes[rng.Next(0, game.ValidShapes.Length)];
        var pixels = texture.GetPixels();
        var name = texture.name;
        var width = texture.width;
        var height = texture.height;

        Shape shape;
        if (!cache.TryGetValue(name, out shape))
        {
            var blocks = new bool[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    blocks[w, h] = pixels[h*width + w] != Color.black;
                }
            }

            shape = new Shape(blocks, width, height);
            cache.Add(name, shape);
        }
        return shape;
    }
}
