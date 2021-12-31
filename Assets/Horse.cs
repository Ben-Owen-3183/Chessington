using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : ChessPiece
{

    private static List<Vector2Int> possibleMoveOffsets;

    public Horse(PieceColor pieceColor, GameObject gameObject, Vector2Int gridPosition)
    {
        this.color = pieceColor;
        this.gridPosition = gridPosition;
        this.gameObject = gameObject;

        if (possibleMoveOffsets == null)
        {
            possibleMoveOffsets = new List<Vector2Int>();
            possibleMoveOffsets.Add(new Vector2Int(-1, 2));
            possibleMoveOffsets.Add(new Vector2Int(-2, 1));
            possibleMoveOffsets.Add(new Vector2Int(-2, -1));
            possibleMoveOffsets.Add(new Vector2Int(-1, -2));

            possibleMoveOffsets.Add(new Vector2Int(2, 1));
            possibleMoveOffsets.Add(new Vector2Int(2, -1));
            possibleMoveOffsets.Add(new Vector2Int(1, 2));
            possibleMoveOffsets.Add(new Vector2Int(1, -2));
        }

        
    }

    protected override List<BoardSquare> PossibleMoves()
    {
        List<BoardSquare> legaMoves = new List<BoardSquare>();

        foreach (Vector2Int moveOffset in possibleMoveOffsets)
        {
            int x = gridPosition.x + moveOffset.x;
            int y = gridPosition.y + moveOffset.y;


            try
            {
                if (boardSquares[x, y].ChessPiece == null || boardSquares[x, y].ChessPiece.color != color)
                    legaMoves.Add(boardSquares[x, y]);

            }
            catch { }
        }

        return legaMoves;
    }

    public override void Move(Vector2Int gridPos)
    {
       
    }
}
