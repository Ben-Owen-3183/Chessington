using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSelector : MonoBehaviour
{
    public Image queenImage, castleImage, horseImage, bishopImage;

    public Type selectedPieceType;
    public Pawn pawnToSwap;

    void Start()
    {

    }

    public void Reset()
    {
        selectedPieceType = null;
        pawnToSwap = null;
        gameObject.SetActive(false);
    }

    public void ChooseQueen() { selectedPieceType = typeof(Queen); }
    public void ChooseCastle() { selectedPieceType = typeof(Castle); }
    public void ChooseHorse() { selectedPieceType = typeof(Horse); }
    public void ChooseBishop() { selectedPieceType = typeof(Bishop); }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImages(Sprite queenSprite, Sprite castleSprite, Sprite horseSprite, Sprite bishopSprite)
    {
        queenImage.sprite = queenSprite;
        castleImage.sprite = castleSprite;
        horseImage.sprite = horseSprite;
        bishopImage.sprite = bishopSprite;
    }
}
