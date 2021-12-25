using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public Bishop(PieceColor pieceColor, GameObject gameObject, Vector2Int gridPosition)
    {
        this.color = pieceColor;
        this.gameObject = gameObject;
        this.gridPosition = gridPosition;
    }

    public override List<BoardSquare> PossibleMoves()
    {
        List<BoardSquare> legaMoves = new List<BoardSquare>();
        ScanDirectionForMoves(new Vector2Int(-1, -1), legaMoves, 8);
        ScanDirectionForMoves(new Vector2Int(1, -1), legaMoves, 8);
        ScanDirectionForMoves(new Vector2Int(-1, 1), legaMoves, 8);
        ScanDirectionForMoves(new Vector2Int(1, 1), legaMoves, 8);

        return legaMoves;
    }

    public override void Move(Vector2Int gridPos)
    {
        throw new System.NotImplementedException();
    }
}
