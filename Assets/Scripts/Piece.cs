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
        panda,
    };

    public type pieceType;

    public void Setup(int x_, int y_, Board board_)
    {
        x = x_;
        y = y_;
        board = board_;

        transform.localScale = Vector3.one * 0.35f;
        transform.DOScale(Vector3.one, 0.35f);
    }
    public void Move(int destinyX, int destinyY)
    {
        this.transform.DOMove(new Vector3(destinyX,destinyY, -6f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destinyX;
            y = destinyY;
        };
    }

    public void Remove(bool animated)
    {
        if(animated)
        {
            transform.DORotate(new Vector3(0, 0, -120f), 0.12f);
            transform.DOScale(Vector3.one * 1.2f, 0.0085f).onComplete = () =>
            {
                transform.DOScale(Vector3.zero, 0.1f).onComplete = () =>
                {
                    Destroy(gameObject);
                };
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
