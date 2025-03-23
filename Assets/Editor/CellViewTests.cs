using NUnit.Framework;
using UnityEngine;
using FluentAssertions;
using System;
using TMPro;

namespace Editor
{
    public class CellViewTests
{
    private CellView cellView;
    private Cell cell;
    private GameObject gameObject;
    private TextMeshProUGUI valueText;
    private SpriteRenderer spriteRenderer;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        cellView = gameObject.AddComponent<CellView>();
        valueText = gameObject.AddComponent<TextMeshProUGUI>();
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        
        cellView.valueText = valueText;
        var spriteRendererField = typeof(CellView).GetField("spriteRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        spriteRendererField.SetValue(cellView, spriteRenderer);

        cell = new Cell(Vector2Int.zero, 1);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void CellView_Init_SubscribesToCellEvents()
    {
        cellView.Init(cell);

        bool valueUpdated = false;
        bool positionUpdated = false;

        cellView.valueText = valueText;
        valueText.text = "";

        var spriteRendererField = typeof(CellView).GetField("spriteRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        spriteRendererField.SetValue(cellView, spriteRenderer);
        spriteRenderer.color = Color.black;

        cell.Value = 2;
        if (cellView.valueText.text == "4")
        {
            valueUpdated = true;
        }

        cell.SetPosition(new Vector2Int(1,1));
        if(cellView.transform.position == new Vector3(1,1,0))
        {
           positionUpdated = true;
        }

        valueUpdated.Should().BeTrue();
        positionUpdated.Should().BeTrue();
    }
    
    [Test]
    public void CellView_UpdateValue_UpdatesTextAndColor()
    {
        cellView.Init(cell);
        
        cell.Value = 3;

        cellView.valueText.text.Should().Be("8");
    }

    [Test]
    public void CellView_UpdatePosition_UpdatesTransformPosition()
    {
        cellView.Init(cell);
        Vector2Int newPosition = new Vector2Int(3, 2);
        
        cell.SetPosition(newPosition);
        
        cellView.transform.position.Should().Be(new Vector3(newPosition.x, newPosition.y, 0));
    }

    [Test]
    public void CellView_UpdateValue_LerpsColorCorrectly()
    {
        cellView.Init(cell);
        var startColorField = typeof(CellView).GetField("startColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var endColorField = typeof(CellView).GetField("endColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Color startColor = (Color)startColorField.GetValue(cellView);
        Color endColor = (Color)endColorField.GetValue(cellView);
        
        cell.Value = 5;

        float expectedT = Mathf.Clamp01((float)5 / 10f);
        Color expectedColor = Color.Lerp(startColor, endColor, expectedT);

        spriteRenderer.color.Should().Be(expectedColor);

    }
    [Test]
    public void CellView_Awake_InitializesSpriteRenderer()
    {

        var awakeMethod = typeof(CellView).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        awakeMethod.Invoke(cellView, null);
        
        var spriteRendererField = typeof(CellView).GetField("spriteRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var spriteRendererValue = spriteRendererField.GetValue(cellView);
        spriteRendererValue.Should().NotBeNull();
        spriteRendererValue.Should().Be(spriteRenderer);
    }
}
}