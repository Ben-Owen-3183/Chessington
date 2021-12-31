using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMove
{
    public enum Type { STANDARD_MOVE, TAKES_PIECE, CASTLING }
    public enum Validity { LEGAL, CAUSES_CHECK }

    public BoardSquare from, to, castleBoardSquare, castleToSquare;
    public Type type;
    public Validity validity;

    private ChessPiece takenPiece;
    private King king;

    public ChessMove(BoardSquare squareFrom, BoardSquare squareTo)
    {
        this.from = squareFrom;
        this.to = squareTo;
        ValidateMove();
    }

    public ChessMove(BoardSquare squareFrom, BoardSquare squareTo, BoardSquare castleBoardSquare, BoardSquare castleToSquare)
    {
        this.from = squareFrom;
        this.to = squareTo;
        this.castleBoardSquare = castleBoardSquare;
        this.castleToSquare = castleToSquare;
        this.type = Type.CASTLING;
        this.validity = Validity.LEGAL;
    }

    public bool IsValid()
    {
        return validity == Validity.LEGAL;
    }

    public void UndoMove()
    {
        // Need to keep track of pieces hasMoved bool and reset them...
        // use intialSetPiece when undoing
    }

    public ChessPiece Execute()
    {
        if(type == Type.STANDARD_MOVE)
        {
            to.SetChessPiece(from.TakeChessPiece());
        }
        else if(type == Type.TAKES_PIECE)
        {
            takenPiece = to.TakeChessPiece();
            to.SetChessPiece(from.TakeChessPiece());
        }
        else if(type == Type.CASTLING)
        {
            to.SetChessPiece(from.TakeChessPiece());
            castleToSquare.SetChessPiece(castleBoardSquare.TakeChessPiece());
            castleToSquare.ChessPiece.ResetGameObjectPosition();
        }

        return takenPiece;
    }

    private ChessPiece GetPiece<T>(ChessPiece.PieceColor pieceColor)
    {
        List<ChessPiece> pieces = (pieceColor == ChessPiece.PieceColor.White ? ChessBoard.whitePieces : ChessBoard.blackPieces);

        foreach (ChessPiece chessPiece in pieces)
            if (typeof(T) == chessPiece.GetType())
                return chessPiece;
        return null;
    }

    private bool KingInCheck(ChessPiece.PieceColor pieceColor)
    {
        List<ChessPiece> pieces = (pieceColor == ChessPiece.PieceColor.White ? ChessBoard.blackPieces : ChessBoard.whitePieces);
        King king = (King)GetPiece<King>(pieceColor);
        BoardSquare kingsBoardSquare = king.BoardSquare;

        foreach (ChessPiece chessPiece in pieces)
            if (chessPiece.inPlay && chessPiece.CanAtackSquare(kingsBoardSquare))
                return true;
        return false;
    }

    private void ValidateMove()
    {
        ChessPiece playerPiece = from.ChessPiece;

        if (to.ChessPiece != null)
        {
            if (playerPiece.color == to.ChessPiece.color)
                throw new System.Exception("Cannot take pieces of same color");

            type = Type.TAKES_PIECE;

            ChessPiece takenPiece = to.TakeChessPiece();
            to.SetInitialChessPiece(from.TakeChessPiece());
            bool kingInCheck = KingInCheck(playerPiece.color);

            if (!kingInCheck)
                validity = Validity.LEGAL;
            else
                validity = Validity.CAUSES_CHECK;

            from.SetInitialChessPiece(to.TakeChessPiece());
            to.SetInitialChessPiece(takenPiece);
        }
        else
        {
            type = Type.STANDARD_MOVE;

            Debug.Log("current");
            Debug.Log(from.GridPosition);
            Debug.Log(to.GridPosition);
            Debug.Log(playerPiece);

            to.SetInitialChessPiece(from.TakeChessPiece());
            bool kingInCheck = KingInCheck(playerPiece.color);

            if (!kingInCheck)
                validity = Validity.LEGAL;
            else
                validity = Validity.CAUSES_CHECK;

            from.SetInitialChessPiece(to.TakeChessPiece());
        }
    }

}
