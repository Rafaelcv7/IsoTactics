using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6,
        TopLeft = 7,
        BottomLeft = 8,
        UpFinish = 9,
        DownFinish = 10,
        LeftFinish = 11,
        RightFinish = 12,
    }

    public ArrowDirection TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        var pastDirection = previousTile != null
            ? currentTile.grid2DLocation - previousTile.grid2DLocation
            : new Vector2Int(0, 0);
        
        var futureDirection = futureTile != null
            ? futureTile.grid2DLocation - currentTile.grid2DLocation
            : new Vector2Int(0, 0);

        var direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if (direction == new Vector2Int(0, 1) && !isFinal)
        {
            return ArrowDirection.Up;
        }
        if (direction == new Vector2Int(0, -1) && !isFinal)
        {
            return ArrowDirection.Down;
        }
        if (direction == new Vector2Int(1, 0) && !isFinal)
        {
            return ArrowDirection.Right;
        }
        if (direction == new Vector2Int(-1, 0) && !isFinal)
        {
            return ArrowDirection.Left;
        }
        
        if (direction == new Vector2Int(1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomLeft;
            else
                return ArrowDirection.TopRight;
        }
        
        if (direction == new Vector2Int(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomRight;
            else
                return ArrowDirection.TopLeft;
        }
        
        if (direction == new Vector2Int(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopLeft;
            else
                return ArrowDirection.BottomRight;
        }
        
        if (direction == new Vector2Int(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopRight;
            else
                return ArrowDirection.BottomLeft;
        }
        
        if (direction == new Vector2Int(0, 1) && isFinal)
        {
            return ArrowDirection.UpFinish;
        }
        if (direction == new Vector2Int(0, -1) && isFinal)
        {
            return ArrowDirection.DownFinish;
        }
        if (direction == new Vector2Int(1, 0) && isFinal)
        {
            return ArrowDirection.RightFinish;
        }
        if (direction == new Vector2Int(-1, 0) && isFinal)
        {
            return ArrowDirection.LeftFinish;
        }

        return ArrowDirection.None;
    }
}
