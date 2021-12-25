using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece
{
    public bool inPlay = true;

    public enum PieceColor { Black, White }
    public PieceColor color;
    public static BoardSquare[,] boardSquares;
    public BoardSquare BoardSquare { get { return boardSquares[gridPosition.x, gridPosition.y];  } }
    public GameObject GameObject { get { return gameObject; } }

    public Vector2Int gridPosition;

    public bool hasMoved = false;

    

    protected GameObject gameObject;

    /**
     * The theoretical moves a given piece can make. Does not account for 
     * a given king to be currently in check or moves causing a king to
     * be put in check
     */
    public abstract List<BoardSquare> PossibleMoves();

    public abstract void Move(Vector2Int gridPos);

    public void ResetGameObjectPosition()
    {
        gameObject.transform.position = boardSquares[gridPosition.x, gridPosition.y].squareGameObject.transform.position;
        GameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void SetPickedUp()
    {
        GameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }


    public bool CanAtackSquare(BoardSquare boardSquareToAttack)
    {
        foreach(BoardSquare boardSquare in PossibleMoves())
            if (boardSquare == boardSquareToAttack)
                return true;
        return false;
    }

    /**
     * Ignores piece on board square when looking for attack on sqaure
     */
    public bool CanAtackSquare(BoardSquare boardSquare, List<BoardSquare> boardSquaresToIgnore)
    {
        return false;
    }


    protected void ScanDirectionForMoves(Vector2Int direction, List<BoardSquare> legaMoves, int range)
    {
        Vector2Int cursor = new Vector2Int(gridPosition.x, gridPosition.y);
        BoardSquare currentSquare;
        try
        {
            for (int i = 0; i < range; ++i)
            {
                cursor += direction;
                currentSquare = boardSquares[cursor.x, cursor.y];
                // if there is a friendly chess piece return as there is no more possible moves.
                if (currentSquare.ChessPiece != null)
                {
                    // if peice is same color nothing more to do here
                    if (currentSquare.ChessPiece.color == color)
                    {
                        return;
                    }
                    // if enemy piece add sqaure and finish
                    else
                    {
                        legaMoves.Add(currentSquare);
                        return;
                    }
                }
                legaMoves.Add(currentSquare);
            }

        }
        catch { } // catches on out of bounds i.e. no more board sqaures to search
    }
}
