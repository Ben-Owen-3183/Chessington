using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public Pawn(PieceColor pieceColor, GameObject gameObject, Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
        this.color = pieceColor;
        this.gameObject = gameObject;
    }

    public override List<BoardSquare> PossibleMoves()
    {
        List<BoardSquare> legaMoves = new List<BoardSquare>();

        // check for diagonal attacks
        // make sure not off edge of board
        // check if attack is enemy king

        // check if off end of board

        int direction = 1;
        if (color == PieceColor.Black) direction = -1;
        BoardSquare boardSquare;
        // check moves for going fowards

        int maxSteps = 1;
        if (color == PieceColor.Black && gridPosition.y == 6
            || color == PieceColor.White && gridPosition.y == 1)
            maxSteps = 2;

        for (int i = 0; i < maxSteps; ++i)
        {
            try
            {
                boardSquare = boardSquares[gridPosition.x, gridPosition.y + (direction * (i + 1))];
                ChessPiece chessPiece = boardSquare.ChessPiece;
                if(chessPiece != null)
                    break; // we cannot move through a chess piece so stop looking
                else
                    legaMoves.Add(boardSquare);
            }
            catch{ } // should only be called when out of bounds so just ignore....probably should specifcally catch that
        }


        CheckForDiagonalMove(direction, - 1, legaMoves);
        CheckForDiagonalMove(direction, + 1, legaMoves);

        return legaMoves;
    }

    private BoardSquare CheckForDiagonalMove(int verticalDirection, int horizontal, List<BoardSquare> legaMoves)
    {
        try
        {
            BoardSquare boardSquare = boardSquares[gridPosition.x + horizontal, gridPosition.y + verticalDirection];
            ChessPiece chessPiece = boardSquare.ChessPiece;
            if (chessPiece != null && chessPiece.color != color)
                legaMoves.Add(boardSquare);
        }
        catch { } // should only be called when out of bounds so just ignore....probably should specifcally catch that

        return null;
    }

    public override void Move(Vector2Int gridPos)
    {
        throw new System.NotImplementedException();
    }
}
