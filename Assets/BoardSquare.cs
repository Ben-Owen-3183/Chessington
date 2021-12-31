using UnityEngine;
using System;

public class BoardSquare
{
    public GameObject squareGameObject;
    public enum IconState { SELECTED, MOVE, ATTACK };

    private BoxCollider2D collider;

    public ChessPiece ChessPiece { get; private set; }
    private GameObject selectIcon;
    private SpriteRenderer selectIconSpriteRenderer;
    private static Sprite selectedSpriteIcon, moveSpriteIcon, attackSpriteIcon;

    private Vector2Int gridPosition;
    public Vector2Int GridPosition { get { return gridPosition; } }

    public BoardSquare(GameObject gameObject, Vector2Int gridPosition)
    {
        if (BoardSquare.selectedSpriteIcon == null) CreateSprites();

        this.gridPosition = gridPosition;

        squareGameObject = gameObject;
        RectTransform rectTransform = squareGameObject.GetComponent<RectTransform>();
        this.collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = rectTransform.sizeDelta;

        selectIcon = new GameObject();
        selectIcon.transform.parent = gameObject.transform;
        selectIcon.transform.position = gameObject.transform.position;
        selectIconSpriteRenderer = selectIcon.AddComponent<SpriteRenderer>();
        selectIconSpriteRenderer.sprite = selectedSpriteIcon;
        selectIconSpriteRenderer.color = new Color(1, 0, 0);
        selectIconSpriteRenderer.sortingOrder = 1;
        selectIcon.SetActive(false);
    }

    private static void CreateSprites()
    {
        Texture2D texture = new Texture2D(8, 8, TextureFormat.ARGB32, false);
        texture.Reinitialize(320, 320);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        selectedSpriteIcon = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(320, 320)), new Vector2(0.5f, 0.5f));
        moveSpriteIcon = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(100, 100)), new Vector2(0.5f, 0.5f));
        attackSpriteIcon = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(320, 320)), new Vector2(0.5f, 0.5f));

    }

    public Vector2 Position()
    {
        return squareGameObject.transform.position;
    }

    public bool Intersects(Vector2 position)
    {
        return collider.bounds.Contains(position);
    }

    /*
     * Same as SetChessPiece() but does not count as an actual move
     */
    public void SetInitialChessPiece(ChessPiece chessPiece)
    {
        chessPiece.gridPosition = gridPosition;
        this.ChessPiece = chessPiece;
        chessPiece.inPlay = true;
    }

    public void SetChessPiece(ChessPiece chessPiece) {
        chessPiece.gridPosition = gridPosition;
        if (this.ChessPiece != null)
            throw new Exception("There is already a piece on this square!!!!");
        this.ChessPiece = chessPiece;
        chessPiece.hasMoved = true;
        chessPiece.inPlay = true;
    }

    public ChessPiece TakeChessPiece()
    {
        ChessPiece removedChessPiece = ChessPiece;
        removedChessPiece.inPlay = false;
        ChessPiece = null;
        return removedChessPiece;
    }
    public void Select(IconState iconState)
    {
        switch (iconState)
        {
            case IconState.ATTACK:
                selectIconSpriteRenderer.color = new Color(1, 0, 0);
                selectIconSpriteRenderer.sprite = attackSpriteIcon;
                break;
            case IconState.MOVE:
                selectIconSpriteRenderer.color = new Color(0, 0, 0.5f);
                selectIconSpriteRenderer.sprite = moveSpriteIcon;
                break;
            case IconState.SELECTED:
                selectIconSpriteRenderer.color = new Color(0, 1, 0);
                selectIconSpriteRenderer.sprite = selectedSpriteIcon;
                break;
            default:
                throw new Exception("Not a valid state"); // can this be called ??????
        }
        selectIcon.SetActive(true);
    }

    public void Unselect()
    {
        selectIcon.SetActive(false);
    }
}
