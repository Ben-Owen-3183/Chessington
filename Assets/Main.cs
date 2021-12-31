using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private GameObject chessBoardVisual;
    private Vector2 boardSize;
    private BoardSquare selectedBoardSquare;
    private ChessPiece heldChessPiece;
    private bool selectingPiece = false;

    private BoardSquare lastMoveFrom, lastMoveTo;

    private ChessPiece.PieceColor currentPlayerColor;

    private GameObject takenWhitesRow, takenBlacksRow;
    private int whitesTaken = 0, blacksTaken = 0;

    public bool IgnoreCheckOnPeiceMove;
    public Sprite blackCastle, blackHorse, blackBishop, blackKing, blackQueen, blackPawn, whiteCastle, whiteHorse, whiteBishop, whiteKing, whiteQueen, whitePawn;

    public PieceSelector pieceSelector;

    private void Awake()
    {
        currentPlayerColor = ChessPiece.PieceColor.White;

        CreateChessBoardSprite();

        Bounds newCameraBounds = chessBoardVisual.GetComponent<Renderer>().bounds;
        Camera.main.transform.position = new Vector3(
            newCameraBounds.center.x,
            newCameraBounds.center.y,
            -20f
        );
        Camera.main.orthographicSize = newCameraBounds.size.y / 2f;

        takenWhitesRow = new GameObject();
        CreateTakenPieceRow(takenWhitesRow, 1);
        takenBlacksRow = new GameObject();
        CreateTakenPieceRow(takenBlacksRow, -1);



        ChessBoard.boardSquares = new BoardSquare[8, 8];

        for (int x = 0; x < 8; ++x)
        {
            for (int y = 0; y < 8; ++y)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.parent = chessBoardVisual.gameObject.transform;

                RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(boardSize.x / 8, boardSize.y / 8);
                gameObject.transform.position = new Vector2((boardSize.x / 8 * (x + 1)) - boardSize.x / 16, (boardSize.y / 8 * y) + boardSize.y / 16);
                ChessBoard.boardSquares[x, y] = new BoardSquare(gameObject, new Vector2Int(x, y));
            }
        }

        // create last move highlight sqaures
        GameObject gameObjectLastMove = new GameObject();
        gameObjectLastMove.AddComponent<RectTransform>();
        gameObjectLastMove.transform.parent = chessBoardVisual.transform;
        lastMoveFrom = new BoardSquare(gameObjectLastMove, new Vector2Int(-1, -1));
        gameObjectLastMove = new GameObject();
        gameObjectLastMove.AddComponent<RectTransform>();
        gameObjectLastMove.transform.parent = chessBoardVisual.transform;
        lastMoveTo = new BoardSquare(gameObjectLastMove, new Vector2Int(-1, -1));

        ChessBoard.blackPieces = new List<ChessPiece>(16);
        CreatePeices(blackCastle, blackHorse, blackBishop, blackKing, blackQueen, blackPawn, ChessPiece.PieceColor.Black, ChessBoard.blackPieces);
        ChessBoard.whitePieces = new List<ChessPiece>(16);
        CreatePeices(whiteCastle, whiteHorse, whiteBishop, whiteKing, whiteQueen, whitePawn, ChessPiece.PieceColor.White, ChessBoard.whitePieces);
    }

    // Creates the game object row for the taken pieces to go in
    private void CreateTakenPieceRow(GameObject gameObject, int direction)
    {
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(
            boardSize.x,
            boardSize.y / 16
        );
        Vector2 position = new Vector2(
            chessBoardVisual.transform.position.x + boardSize.x / 1.92f,
            chessBoardVisual.transform.position.y + boardSize.y / 2
                + (direction * (boardSize.y/2 + rectTransform.sizeDelta.y/2)) 
        );
        gameObject.transform.position = position;
        gameObject.transform.parent = chessBoardVisual.transform;
    }

    private void CreatePeices(Sprite castle, Sprite horse, Sprite bishop, Sprite king, Sprite queen, Sprite pawn, ChessPiece.PieceColor pieceColor, List<ChessPiece> piecesInPlay)
    {
        ChessPiece.boardSquares = ChessBoard.boardSquares;
        BoardSquare[,] boardSquares = ChessBoard.boardSquares;

        int y = pieceColor == ChessPiece.PieceColor.White ? 1 : 8;

        GameObject gameObject = CreateChessPeice(castle, GridToWorldPositon(1, y));
        boardSquares[0, y - 1].SetInitialChessPiece(new Castle(pieceColor, gameObject, new Vector2Int(0, y - 1)));
        piecesInPlay.Add(boardSquares[0, y - 1].ChessPiece);

        gameObject = CreateChessPeice(horse, GridToWorldPositon(2, y));
        boardSquares[1, y - 1].SetInitialChessPiece(new Horse(pieceColor, gameObject, new Vector2Int(1, y - 1)));
        piecesInPlay.Add(boardSquares[1, y - 1].ChessPiece);


        gameObject = CreateChessPeice(bishop, GridToWorldPositon(3, y));
        boardSquares[2, y - 1].SetInitialChessPiece(new Bishop(pieceColor, gameObject, new Vector2Int(2, y - 1)));
        piecesInPlay.Add(boardSquares[2, y - 1].ChessPiece);


        gameObject = CreateChessPeice(queen, GridToWorldPositon(4, y));
        boardSquares[3, y - 1].SetInitialChessPiece(new Queen(pieceColor, gameObject, new Vector2Int(3, y - 1)));
        piecesInPlay.Add(boardSquares[3, y - 1].ChessPiece);


        gameObject = CreateChessPeice(king, GridToWorldPositon(5, y));
        boardSquares[4, y - 1].SetInitialChessPiece(new King(pieceColor, gameObject, new Vector2Int(4, y - 1)));
        piecesInPlay.Add(boardSquares[4, y - 1].ChessPiece);


        gameObject = CreateChessPeice(bishop, GridToWorldPositon(6, y));
        boardSquares[5, y - 1].SetInitialChessPiece(new Bishop(pieceColor, gameObject, new Vector2Int(5, y - 1)));
        piecesInPlay.Add(boardSquares[5, y - 1].ChessPiece);

        gameObject = CreateChessPeice(horse, GridToWorldPositon(7, y));
        boardSquares[6, y - 1].SetInitialChessPiece(new Horse(pieceColor, gameObject, new Vector2Int(6, y - 1)));
        piecesInPlay.Add(boardSquares[6, y - 1].ChessPiece);

        gameObject = CreateChessPeice(castle, GridToWorldPositon(8, y));
        boardSquares[7, y - 1].SetInitialChessPiece(new Castle(pieceColor, gameObject, new Vector2Int(7, y - 1)));
        piecesInPlay.Add(boardSquares[7, y - 1].ChessPiece);

        for (int i = 0; i < 8; ++i)
        {
            int pawnY = pieceColor == ChessPiece.PieceColor.White ? 2 : 7;
            gameObject = CreateChessPeice(pawn, GridToWorldPositon(i + 1, pawnY));
            boardSquares[i, pawnY - 1].SetInitialChessPiece(new Pawn(pieceColor, gameObject, new Vector2Int(i, pawnY - 1)));
            piecesInPlay.Add(boardSquares[i, pawnY - 1].ChessPiece);
        }
    }

    private Vector2 GridToWorldPositon(int x, int y)
    {
        return new Vector2(
            (boardSize.x / 8 * x) - boardSize.x / 16,
            (boardSize.y / 8 * y) - boardSize.x / 16
        );
    }

    private GameObject CreateChessPeice(Sprite sprite, Vector2 position)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = chessBoardVisual.transform;
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        gameObject.transform.position = position;
        spriteRenderer.sortingOrder = 2;
        return gameObject;
    }

    private void CreateChessBoardSprite()
    {
        chessBoardVisual = new GameObject("Chess Board");
        SpriteRenderer renderer = chessBoardVisual.AddComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(8, 8, TextureFormat.ARGB32, false);

        Color colorA = new Color(0.2f, 0.6f, 0.3f);     // Black
        Color colorB = new Color(0.8f, 0.9f, 0.8f);     // White

        Color[] pixels = texture.GetPixels();
        pixels[0] = colorA;
        for (int i = 1; i < pixels.Length; ++i)
        {
            Color color = pixels[i - 1] == colorB ? colorA : colorB;
          
            if (i % 8 == 0)
                pixels[i] = color == colorB ? colorA : colorB;
            else
                pixels[i] = color;
        }
        
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(pixels);
        texture.Apply();

        renderer.sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(8, 8)), Vector2.zero);

        chessBoardVisual.transform.localScale = new Vector3(320, 320);

        RectTransform rectTransform = chessBoardVisual.AddComponent<RectTransform>();
        boardSize = renderer.bounds.size;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UnselectAllSquares()
    {
        foreach(BoardSquare boardSquare in ChessBoard.boardSquares)
            boardSquare.Unselect();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (selectingPiece)
        {
            if (pieceSelector.selectedPieceType != null)
            {
                Type type = pieceSelector.selectedPieceType;
                ChessPiece newChessPiece;
                Pawn pawnToSwap = pieceSelector.pawnToSwap;
                Sprite sprite;
                if(type == typeof(Queen))
                {
                    sprite = pawnToSwap.color == ChessPiece.PieceColor.Black ? blackQueen : whiteQueen;
                    GameObject gameObject = CreateChessPeice(sprite, pawnToSwap.GameObject.transform.position);
                    newChessPiece = new Queen(pawnToSwap.color, gameObject, pawnToSwap.gridPosition);
                }
                else if (type == typeof(Castle))
                {
                    sprite = pawnToSwap.color == ChessPiece.PieceColor.Black ? blackCastle : whiteCastle;
                    GameObject gameObject = CreateChessPeice(sprite, pawnToSwap.GameObject.transform.position);
                    newChessPiece = new Castle(pawnToSwap.color, gameObject, pawnToSwap.gridPosition);
                }
                else if (type == typeof(Horse))
                {
                    sprite = pawnToSwap.color == ChessPiece.PieceColor.Black ? blackHorse : whiteHorse;
                    GameObject gameObject = CreateChessPeice(sprite, pawnToSwap.GameObject.transform.position);
                    newChessPiece = new Horse(pawnToSwap.color, gameObject, pawnToSwap.gridPosition);
                }
                else
                {
                    sprite = pawnToSwap.color == ChessPiece.PieceColor.Black ? blackBishop : whiteBishop;
                    GameObject gameObject = CreateChessPeice(sprite, pawnToSwap.GameObject.transform.position);
                    newChessPiece = new Bishop(pawnToSwap.color, gameObject, pawnToSwap.gridPosition);
                }

                int x = pawnToSwap.gridPosition.x;
                int y = pawnToSwap.gridPosition.y;

                ChessBoard.boardSquares[x, y].TakeChessPiece();

                ChessBoard.boardSquares[x, y].SetChessPiece(newChessPiece);

                if (pawnToSwap.color == ChessPiece.PieceColor.Black)
                {
                    ChessBoard.blackPieces.Remove(pawnToSwap);
                    ChessBoard.blackPieces.Add(newChessPiece);
                }
                else
                {
                    ChessBoard.whitePieces.Remove(pawnToSwap);
                    ChessBoard.whitePieces.Add(newChessPiece);
                }

                Destroy(pawnToSwap.GameObject);

                ToggleCurrentPlayerColor();
                selectingPiece = false;
                pieceSelector.Reset();
            }


            return; // dont allow game to continue unless piece choosen
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (heldChessPiece != null)
            heldChessPiece.GameObject.transform.position = mousePosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            BoardSquare boardSquare = GetIntersectionBoardSquare(mousePosition);
         
            if (boardSquare != null 
                && boardSquare.ChessPiece != null 
                && (IgnoreCheckOnPeiceMove || currentPlayerColor == boardSquare.ChessPiece.color))
            {
                heldChessPiece = boardSquare.ChessPiece;
                heldChessPiece.SetPickedUp();

                if (selectedBoardSquare != null) UnselectAllSquares();

                boardSquare.Select(BoardSquare.IconState.SELECTED);

                foreach(ChessMove move in heldChessPiece.AllMoves())
                {
                    if(move.validity == ChessMove.Validity.LEGAL)
                    {
                        if (move.type == ChessMove.Type.STANDARD_MOVE || move.type == ChessMove.Type.CASTLING)
                            move.to.Select(BoardSquare.IconState.MOVE);
                        else if (move.type == ChessMove.Type.TAKES_PIECE)
                            move.to.Select(BoardSquare.IconState.ATTACK);
                    }
                }

                selectedBoardSquare = boardSquare;
            }
            else
            {
                if (selectedBoardSquare != null)
                    UnselectAllSquares();
                selectedBoardSquare = null;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // if player holding chess piece
            if (heldChessPiece != null)
                TryToPlaceHeldPiece(mousePosition);
        }
    }

    private void AddPieceToTakenRow(ChessPiece chessPiece)
    {
        chessPiece.GameObject.transform.localScale = chessPiece.GameObject.transform.localScale / 2;

        GameObject row;
        int offset = 0;

        if (chessPiece.color == ChessPiece.PieceColor.Black)
        {
            offset = blacksTaken++;
            row = takenBlacksRow;
        }
        else
        {
            offset = whitesTaken++;
            row = takenWhitesRow;
        }
        
        float x = row.transform.position.x
            - row.GetComponent<RectTransform>().rect.width / 2
            + (offset * (boardSize.x / 16));

        chessPiece.GameObject.transform.position = new Vector2(
            x,
            row.transform.position.y
        );
    }

    /**
     * gets first piece found of given color
     */
    private ChessPiece GetPiece<T>(ChessPiece.PieceColor pieceColor)
    {
        List<ChessPiece> pieces = (pieceColor == ChessPiece.PieceColor.White ? ChessBoard.whitePieces : ChessBoard.blackPieces);

        foreach (ChessPiece chessPiece in pieces)
            if (typeof(T) == chessPiece.GetType())
                return chessPiece;
        return null;
    }
    
    private void ToggleCurrentPlayerColor()
    {
        Vector3 position = chessBoardVisual.GetComponent<Renderer>().bounds.center;
        chessBoardVisual.transform.RotateAround(position, Vector3.forward, 180f);

        List<ChessPiece> chessPieces = new List<ChessPiece>();
        chessPieces.AddRange(ChessBoard.blackPieces);
        chessPieces.AddRange(ChessBoard.whitePieces);

        foreach (ChessPiece chessPiece in chessPieces)
            chessPiece.GameObject.transform.Rotate(0, 0, 180f);

        currentPlayerColor = currentPlayerColor == ChessPiece.PieceColor.Black ? ChessPiece.PieceColor.White : ChessPiece.PieceColor.Black;
    }

    private void TryToPlaceHeldPiece(Vector2 mousePosition)
    {
        BoardSquare selectedBoardSquare = GetIntersectionBoardSquare(mousePosition);

        if(heldChessPiece.BoardSquare == selectedBoardSquare)
        {
            ResetHeldPiece();
            return;
        }

        // DEBUG MODE 
        if (IgnoreCheckOnPeiceMove)
        {
            BoardSquare heldPieceBoardSqaure = heldChessPiece.BoardSquare;
            if (selectedBoardSquare.ChessPiece != null)
            {
                ChessPiece takenPiece = selectedBoardSquare.TakeChessPiece();
                selectedBoardSquare.SetChessPiece(heldPieceBoardSqaure.TakeChessPiece());
                AddPieceToTakenRow(takenPiece);
                ToggleCurrentPlayerColor();
                UnselectAllSquares();
            }
            else
            {
                heldPieceBoardSqaure.TakeChessPiece();
                selectedBoardSquare.SetChessPiece(heldChessPiece);
                ToggleCurrentPlayerColor();
                UnselectAllSquares();
            }

            ResetHeldPiece();
            return;
        }


        ChessMove move = FindChessMoveBySqaure(selectedBoardSquare);
        if(move != null)
        {
            if(move.IsValid())
            {
                ChessPiece takenPiece = move.Execute();

                if (takenPiece != null)
                    AddPieceToTakenRow(takenPiece);

                if (move.to.ChessPiece.GetType() == typeof(Pawn)
                    && CheckIfPawnReachedOtherSide((Pawn)move.to.ChessPiece))
                {
                    selectingPiece = true;
                }
                else
                {
                    if (ColorInCheckMate(currentPlayerColor))
                    {
                        Debug.Log("Checkmate!!!!!!");
                    }

                    ToggleCurrentPlayerColor();
                }

                lastMoveFrom.squareGameObject.transform.position = move.from.squareGameObject.transform.position;
                lastMoveTo.squareGameObject.transform.position = move.to.squareGameObject.transform.position;
                lastMoveFrom.Select(BoardSquare.IconState.SELECTED);
                lastMoveTo.Select(BoardSquare.IconState.SELECTED);
            }
            else if(move.validity == ChessMove.Validity.CAUSES_CHECK)
            {
                King king = (King)GetPiece<King>(heldChessPiece.color);

                ResetHeldPiece();// actually updates the visuals...probs should change name
                UnselectAllSquares();
                king.BoardSquare.Select(BoardSquare.IconState.ATTACK);
                return;
            }

            UnselectAllSquares();
        }

        ResetHeldPiece();// actually updates the visuals...probs should change name
    }

    private bool CheckIfPawnReachedOtherSide(Pawn pawn)
    {
        if (pawn.ReachedOtherSide())
        {
            if (pawn.color == ChessPiece.PieceColor.White)
                pieceSelector.SetImages(whiteQueen, whiteCastle, whiteHorse, whiteBishop);
            else
                pieceSelector.SetImages(blackQueen, blackCastle, blackHorse, blackBishop);

            pieceSelector.pawnToSwap = pawn;
            pieceSelector.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private bool ColorInCheckMate(ChessPiece.PieceColor pieceColor)
    {
        List<ChessPiece> chessPieces = (pieceColor == ChessPiece.PieceColor.White ? ChessBoard.blackPieces : ChessBoard.whitePieces);
        foreach (ChessPiece chessPiece in chessPieces)
        {
            if (chessPiece.inPlay)
            {
                List<ChessMove> chessMoves = chessPiece.AllMoves();
                if (chessMoves.Count > 0)
                {
                    foreach (ChessMove chessMove in chessMoves)
                        if (chessMove.IsValid())
                            return false;
                }
            }
        }
        return true;
    }

    private ChessMove FindChessMoveBySqaure(BoardSquare boardSquare)
    {
        foreach (ChessMove move in heldChessPiece.AllMoves())
            if (move.to == boardSquare)
                return move;
        return null;
    }

    private void ResetHeldPiece()
    {
        heldChessPiece.ResetGameObjectPosition();
        heldChessPiece = null;
    }

    private BoardSquare GetIntersectionBoardSquare(Vector2 position)
    {
        foreach (BoardSquare boardSquare in ChessBoard.boardSquares)
            if (boardSquare.Intersects(position))
                return boardSquare;
        return null;
    }
}
