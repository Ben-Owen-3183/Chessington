using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public King(PieceColor pieceColor, GameObject gameObject, Vector2Int gridPosition)
    {
        this.color = pieceColor;
        this.gameObject = gameObject;
        this.gridPosition = gridPosition;
    }

    public override List<BoardSquare> PossibleMoves()
    {
        List<BoardSquare> legaMoves = new List<BoardSquare>();
        ScanDirectionForMoves(new Vector2Int(-1, -1), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(1, -1), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(-1, 1), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(1, 1), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(-1, 0), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(0, -1), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(1, 0), legaMoves, 1);
        ScanDirectionForMoves(new Vector2Int(0, 1), legaMoves, 1);

        if(hasMoved == false)
        {
            Debug.Log("Castling not fully implemented!!!");

            /*
            1. Neither the king nor the rook may have moved from their starting
            position at any time in the game prior to castling.

            2. There must be no pieces in between the rook and the king of any 
            color.

            3. The king must not be in check.

            4. None of the squares that the king passes through, including the 
            starting and finishing square, may be under attack by any of the
            opponent’s pieces during the time of castling.
            
             */

            ScanForCastle(legaMoves, new Vector2Int(-1, 0));
            ScanForCastle(legaMoves, new Vector2Int(1, 0));
        }


        return legaMoves;
    }

    private void ScanForCastle(List<BoardSquare> legaMoves, Vector2Int direction)
    {
        Vector2Int cursor = new Vector2Int(gridPosition.x, gridPosition.y);
        BoardSquare currentSquare;
        try
        {
            for (int i = 0; i < 8; ++i)
            {
                cursor += direction;
                currentSquare = boardSquares[cursor.x, cursor.y];
                if (currentSquare.ChessPiece != null)
                {
                    if(typeof(Castle) == currentSquare.ChessPiece.GetType())
                    {
                        Vector2Int position = gridPosition + (direction * 2);
                        currentSquare = boardSquares[position.x, position.y];
                        legaMoves.Add(currentSquare);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }

        }
        catch { } // catches on out of bounds i.e. no more board sqaures to search
    }

    public override void Move(Vector2Int gridPos)
    {
        throw new System.NotImplementedException();
    }
}
