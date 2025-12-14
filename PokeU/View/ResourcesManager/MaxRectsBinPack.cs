using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class MaxRectsBinPack
{
    public int BinWidth;
    public int BinHeight;
    public List<Rectangle> FreeRectangles = new List<Rectangle>();

    public enum FreeRectChoiceHeuristic
    {
        BestShortSideFit
    }

    public MaxRectsBinPack(int width, int height)
    {
        BinWidth = width;
        BinHeight = height;
        FreeRectangles.Add(new Rectangle(0, 0, width, height));
    }

    public Rectangle Insert(int width, int height, FreeRectChoiceHeuristic method)
    {
        Rectangle bestRect = Rectangle.Empty;
        int bestShortSideFit = int.MaxValue;

        for (int i = 0; i < FreeRectangles.Count; i++)
        {
            int leftoverHoriz = System.Math.Abs(FreeRectangles[i].Width - width);
            int leftoverVert = System.Math.Abs(FreeRectangles[i].Height - height);
            int shortSideFit = System.Math.Min(leftoverHoriz, leftoverVert);

            if (FreeRectangles[i].Width >= width && FreeRectangles[i].Height >= height)
            {
                if (shortSideFit < bestShortSideFit)
                {
                    bestRect = new Rectangle(FreeRectangles[i].X, FreeRectangles[i].Y, width, height);
                    bestShortSideFit = shortSideFit;
                }
            }
        }

        if (bestRect != Rectangle.Empty)
            PlaceRect(bestRect);

        return bestRect;
    }

    private void PlaceRect(Rectangle node)
    {
        int i = 0;
        while (i < FreeRectangles.Count)
        {
            if (SplitFreeNode(FreeRectangles[i], ref node))
            {
                FreeRectangles.RemoveAt(i);
                i--;
            }
            i++;
        }
        FreeRectangles.Add(new Rectangle(node.X, node.Y, node.Width, node.Height));
    }

    private bool SplitFreeNode(Rectangle freeNode, ref Rectangle usedNode)
    {
        if (!freeNode.Intersects(usedNode))
            return false;

        if (usedNode.X < freeNode.Right && usedNode.Right > freeNode.X)
        {
            if (usedNode.Y > freeNode.Y && usedNode.Y < freeNode.Bottom)
            {
                Rectangle newNode = freeNode;
                newNode.Height = usedNode.Y - newNode.Y;
                FreeRectangles.Add(newNode);
            }

            if (usedNode.Bottom < freeNode.Bottom)
            {
                Rectangle newNode = freeNode;
                newNode.Y = usedNode.Bottom;
                newNode.Height = freeNode.Bottom - usedNode.Bottom;
                FreeRectangles.Add(newNode);
            }
        }

        if (usedNode.Y < freeNode.Bottom && usedNode.Bottom > freeNode.Y)
        {
            if (usedNode.X > freeNode.X && usedNode.X < freeNode.Right)
            {
                Rectangle newNode = freeNode;
                newNode.Width = usedNode.X - newNode.X;
                FreeRectangles.Add(newNode);
            }

            if (usedNode.Right < freeNode.Right)
            {
                Rectangle newNode = freeNode;
                newNode.X = usedNode.Right;
                newNode.Width = freeNode.Right - usedNode.Right;
                FreeRectangles.Add(newNode);
            }
        }
        return true;
    }
}