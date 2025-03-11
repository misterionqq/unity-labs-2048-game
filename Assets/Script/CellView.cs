using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private Cell cell;
    public TextMeshProUGUI valueText;
    private Color startColor = Color.grey;
    private Color endColor = Color.red;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Init(Cell cell)
    {
        this.cell = cell;
        cell.OnValueChanged += UpdateValue;
        cell.OnPositionChanged += UpdatePosition;
        UpdateValue(cell.Value);
        UpdatePosition(cell.Position);
    }
    
    private void UpdateValue(int newValue)
    {
        valueText.text = Mathf.Pow(2, newValue).ToString();
        float t = Mathf.Clamp01((float)newValue / 10);
        spriteRenderer.color = Color.Lerp(startColor, endColor, t);
    }
    
    private void UpdatePosition(Vector2Int newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);
    }
}