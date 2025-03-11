using UnityEngine;
using System.Collections.Generic;

public class GameField : MonoBehaviour
{
    public int width = 4;
    public int height = 4;
    private List<Cell> cells = new List<Cell>();
    private int score;
    
    public Vector2Int GetEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!cells.Exists(c => c.Position == new Vector2Int(x, y)))
                {
                    emptyPositions.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyPositions.Count > 0 ? emptyPositions[Random.Range(0, emptyPositions.Count)] : new Vector2Int(-1, -1);
    }
    
    public void CreateCell()
    {
        Vector2Int pos = GetEmptyPosition();
        if (pos.x != -1)
        {
            int value = Random.value < 0.8f ? 2 : 4;
            Cell newCell = new Cell(pos, value);
            cells.Add(newCell);
            UpdateScore();
        }
    }
    
    private void UpdateScore()
    {
        score = 0;
        foreach (var cell in cells)
        {
            score += cell.Value;
        }
        Debug.Log("Score: " + score);
    }
}