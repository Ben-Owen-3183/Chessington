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

    protected override List<BoardSquare> PossibleMoves()
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

        return legaMoves;
    }

    public override List<ChessMove> AllMoves()
    {
        List<ChessMove> pieceMoves = new List<ChessMove>();
        foreach (BoardSquare possibleMove in PossibleMoves())
            pieceMoves.Add(new ChessMove(BoardSquare, possibleMove));


        if (hasMoved == false && KingInCheck() == false)
        {
            Debug.Log("King and castle have not move");
            /*
            DONE    1. Neither the king nor the rook may have moved from their starting
            position at any time in the game prior to castling.

            DONE    2. There must be no pieces in between the rook and the king of any 
            color.

            DONE    3. The king must not be in check.

            DONE    4. None of the squares that the king passes through, including the 
            starting and finishing square, may be under attack by any of the
            opponent’s pieces during the time of castling.
            
             */

            BoardSquare castleBoardSquare;

            castleBoardSquare = ScanForCastle(new Vector2Int(-1, 0));
            if (castleBoardSquare != null)
            {
                ChessMove chessMove = TryToCreateCastlingMove(castleBoardSquare);
                pieceMoves.Add(chessMove);
            }

            castleBoardSquare = ScanForCastle(new Vector2Int(1, 0));
            if (castleBoardSquare != null)
            {
                ChessMove chessMove = TryToCreateCastlingMove(castleBoardSquare);
                pieceMoves.Add(chessMove);
            }
        }

        return pieceMoves;
    }

    private bool KingInCheck()
    {
        List<ChessPiece> pieces = (color == ChessPiece.PieceColor.White ? ChessBoard.blackPieces : ChessBoard.whitePieces);
        foreach (ChessPiece chessPiece in pieces)
            if (chessPiece.inPlay && chessPiece.CanAtackSquare(BoardSquare))
                return true;
        return false;
    }

    private ChessMove TryToCreateCastlingMove(BoardSquare castleBoardSquare)
    {
        if (castleBoardSquare.ChessPiece.hasMoved) return null;

        BoardSquare moveThroughSquare;
        BoardSquare moveToSquare;
        if (castleBoardSquare.GridPosition.x < this.BoardSquare.GridPosition.x)
        {
            moveThroughSquare = ChessBoard.boardSquares[this.BoardSquare.GridPosition.x - 1, this.BoardSquare.GridPosition.y];
            moveToSquare = ChessBoard.boardSquares[this.BoardSquare.GridPosition.x - 2, this.BoardSquare.GridPosition.y];
        }
        else
        {
            moveThroughSquare = ChessBoard.boardSquares[this.BoardSquare.GridPosition.x + 1, this.BoardSquare.GridPosition.y];
            moveToSquare = ChessBoard.boardSquares[this.BoardSquare.GridPosition.x + 2, this.BoardSquare.GridPosition.y];
        }

        List<ChessPiece> enemyPieces = color == PieceColor.White ? ChessBoard.blackPieces : ChessBoard.whitePieces;
        
        foreach (ChessPiece enemyPiece in enemyPieces)
        {
            if (enemyPiece.CanAtackSquare(moveThroughSquare))
                return null;
        }

        return new ChessMove(this.BoardSquare, moveToSquare, castleBoardSquare, moveThroughSquare); 
    }

    private BoardSquare ScanForCastle(Vector2Int direction)
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
                        // Vector2Int position = gridPosition + (direction * 2);
                        // currentSquare = boardSquares[position.x, position.y];
                        return currentSquare;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        catch { } // catches on out of bounds i.e. no more board sqaures to search
        return null;
    }

    public override void Move(Vector2Int gridPos)
    {
        throw new System.NotImplementedException();
    }
}
