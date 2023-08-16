using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject titleObject;
    public float cameraSizeOffset;
    public float cameraVerticalOffset;

    public GameObject[] availablePieces;
    Tile[,] Tiles;
    Piece[,] Pieces;

    Tile startTile;
    Tile endTile;

    bool swappingPices = false;

    // Start is called before the first frame update
    void Start()
    {
        Tiles = new Tile[width, height];
        Pieces = new Piece[width, height];
        SetupBoard();
        PositionCamera();
        SetupPieces();
    }

    private void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++) 
            { 
                var newPiece = CreatePieceAt(x,y);
            }
        }
    }

    private Piece CreatePieceAt(int x, int y)
    {
        var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
        var temporalObject = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
        temporalObject.transform.parent = this.transform;
        Pieces[x, y] = temporalObject.GetComponent<Piece>();
        Pieces[x, y].Setup(x, y, this);
    }

    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;
        Camera.main.transform.position = new Vector3(
            newPosX - 0.5f,
            newPosY - 0.5f + cameraVerticalOffset,
            -10f
        );
        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        Camera.main.orthographicSize =
            horizontal > vertical ? horizontal + cameraSizeOffset : vertical + cameraSizeOffset;
    }

    private void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var temporalObject = Instantiate(
                    titleObject,
                    new Vector3(x, y, -5),
                    Quaternion.identity
                );
                temporalObject.transform.parent = this.transform;
                Tiles[x, y] = temporalObject.GetComponent<Tile>();
                Tiles[x, y]?.Setup(x, y, this);
            }
        }
    }

    public void TileDown(Tile tile_)
    {
        startTile = tile_;
    }

    public void TileOver(Tile tile_)
    {
        endTile = tile_;
    }

    public void TileUp(Tile tile_)
    {
        if (startTile != null && endTile != null && IsCloseTo(startTile, endTile))
        {
            swappingPices = true;
            StartCoroutine(SwapTiles());
        }
    }

    IEnumerator SwapTiles()
    {
        var StartPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StartPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StartPiece;

        yield return new WaitForSeconds(0.6f);

        bool foundMatch = false;
        var startMatches = GetMatchByPiece(StartPiece.x, startTile.y, 3);
        var endMatches = GetMatchByPiece(endTile.x, endTile.y, 3);

        startMatches.ForEach(pieces =>
        {
            foundMatch = true;
            Pieces[pieces.x, pieces.y] = null;
            Destroy(pieces.gameObject);
        });
        endMatches.ForEach(pieces =>
        {
            foundMatch = true;
            Pieces[pieces.x, pieces.y] = null;
            Destroy(pieces.gameObject);
        });

        if (!foundMatch)
        {
            StartPiece.Move(startTile.x, startTile.y);
            EndPiece.Move(endTile.x, endTile.y);
            Pieces[startTile.x, endTile.y] = StartPiece;
            Pieces[endTile.x, startTile.y] = EndPiece;
        }
        startTile = null;
        endTile = null;
        swappingPices = false;

        yield return null;
    }

    public bool IsCloseTo(Tile start, Tile end)
    {
        if (Math.Abs(start.x - end.x) == 1 && start.y == end.y)
        {
            return true;
        }
        if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x)
        {
            return true;
        }
        return false;
    }

    public List<Piece> GetMatchByDirection(int xpos, int ypos, Vector2 direction, int minPieces = 3)
    {
        List<Piece> matches = new List<Piece>();
        Piece startPiece = Pieces[xpos, ypos];
        matches.Add(startPiece);

        int nextX;
        int nextY;
        int maxVal = width > height ? width : height;

        for (int i = 1; i < maxVal; i++)
        {
            nextX = xpos + ((int)direction.x * i);
            nextY = ypos + ((int)direction.y * i);
            if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height)
            {
                var nextPiece = Pieces[nextX, nextY];
                if (nextPiece != null && nextPiece.pieceType == startPiece.pieceType)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }
        if (matches.Count >= minPieces)
        {
            return matches;
        }
        return null;
    }

    public List<Piece> GetMatchByPiece(int xpos, int ypos, int minPieces = 3)
    {
        var upMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, 1), 2);
        var downMatchs = GetMatchByDirection(xpos, ypos, new Vector2(0, -1), 2);
        var rightMatchs = GetMatchByDirection(xpos, ypos, new Vector2(1, 0), 2);
        var leftMatchs = GetMatchByDirection(xpos, ypos, new Vector2(-1, 0), 2);

        if (upMatchs == null)
            upMatchs = new List<Piece>();
        if (downMatchs == null)
            downMatchs = new List<Piece>();
        if (rightMatchs == null)
            rightMatchs = new List<Piece>();
        if (leftMatchs == null)
            leftMatchs = new List<Piece>();

        var verticalMatches = upMatchs.Union(downMatchs).ToList();
        var horizontalMatches = leftMatchs.Union(rightMatchs).ToList();

        var foundMatches = new List<Piece>();

        if (verticalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(verticalMatches).ToList();
        }
        if (horizontalMatches.Count >= minPieces)
        {
            foundMatches = foundMatches.Union(horizontalMatches).ToList();
        }
        return foundMatches;
    }
}