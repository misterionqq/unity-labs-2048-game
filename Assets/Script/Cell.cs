using UnityEngine;
using System;

public class Cell
{
    public Vector2Int Position { get; private set; }
    private int value;
    public int Value
    {
        get => value;
        set
        {
            if (this.value != value)
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
    
    public event Action<int> OnValueChanged;
    public event Action<Vector2Int> OnPositionChanged;
    
    public Cell(Vector2Int position, int value)
    {
        Position = position;
        Value = value;
    }
    
    public void SetPosition(Vector2Int newPosition)
    {
        Position = newPosition;
        OnPositionChanged?.Invoke(newPosition);
    }
}