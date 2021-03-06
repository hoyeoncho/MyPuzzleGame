﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    
    public static MovePieces instance;
    Match3 game;

    NodePiece moving;
    Point newIndex;
    Vector2 mouseStart;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<Match3>();
        
    }

    void Update()
    {
        if(moving != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.clone(moving.index);
            Point add = Point.zero;
            if(dir.magnitude > 50)  //마우스가 시작점부터 50이상 떨어졌다면 
            {
                // if eles if 와 삼항연산자를 이용해서 4개의 경우의 수를 가려내고 
                if (aDir.x > aDir.y)
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                else if(aDir.y > aDir.x)
                    add = (new Point(0,(nDir.y > 0) ? -1 : 1));
            }
            newIndex.add(add);

            Vector2 pos = game.getPositionFromPoint(moving.index);
            //50만큼 이동 
            if (!newIndex.Equals(moving.index))
                pos += Point.mult(new Point(add.x, - add.y), 25).ToVector();
            moving.MovePositionTo(pos);
        }
    }

    public void MovePiece(NodePiece piece)
    {
        if(moving != null)  return;
        
        moving = piece;
        mouseStart = Input.mousePosition;
    }

    //매치되면 실행되는 부분
    public void DropPiece()
    {
        if (moving == null) return;        
        Debug.Log("Dropped");
        //Flip the pieces around int the game board
        //Reset the piece back to origin spot

        if (!newIndex.Equals(moving.index))
            game.FlipPieces(moving.index, newIndex, true);
        else game.ResetPiece(moving);

        moving = null;
    }
}
