// Tile.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class Tile : MonoBehaviour
{
    public TileScript state { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; private set; }

    public bool locked { get; set; }

    private Image background;
    private TextMeshProUGUI text;


    private Color startColor = Color.grey;
    private Color endColor = Color.red;

    private void Awake()
    {
        background = GetComponent<Image>(); // Get the Image component
        text = GetComponentInChildren<TextMeshProUGUI>(); // Get the TextMeshPro component
    }

    public void SetState(TileScript state, int number)
    {
        this.state = state;
        this.number = number;
        float t = Mathf.Clamp01((Mathf.Log(number, 2) - 1) / 10);

         // THIS WAS THE LINE CAUSING THE PROBLEM:
        background.color = Color.Lerp(startColor, endColor, t); // Line 48
        text.text = number.ToString();
    }
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;
        StartCoroutine(Animate(cell.transform.position, true));

    }

    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging)
        {
            Object.DestroyImmediate(gameObject);
        }
    }

}