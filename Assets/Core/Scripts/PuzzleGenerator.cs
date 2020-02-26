using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PUZZLETYPE
{
    EP_RED,
    EP_PUPLE,
    EP_GREEN,
    EP_BLUE,
    EP_TELLOW,
}

public class PuzzleGenerator : MonoBehaviour
{
    public Texture2D[] puzzleSprite;
    PUZZLETYPE puzzleType;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
