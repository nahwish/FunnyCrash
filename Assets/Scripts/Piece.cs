using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Piece : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;

    public enum type
    {
        elephant,
        giraffe,
        hippo,
        monkye,
        parrot,
        pig,
        penguin,
        snake,
        rabbit,
        girafe,
        panda
    };

    public type pieceType;

    public void Setup(int x_, int y_, Board board_)
    {
        x = x_;
        y = y_;
        board = board_;
    }
    public void Move(int destinyX, int destinyY)
    {
        this.transform.DOMove(new Vector3(destinyX,destinyY, -6f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destinyX;
            y = destinyY;
        };
    }
    [ContextMenu("Test Move")]
    public void MoveTeste()
    {
        Move(0,0);
    }
}
