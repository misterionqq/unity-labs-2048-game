using NUnit.Framework;
using UnityEngine;
using FluentAssertions;
using System;
using TMPro;

namespace Editor
{
    public class GameFieldTests
{
    private GameField gameField;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject();
        gameField = go.AddComponent<GameField>();
        gameField.width = 4;
        gameField.height = 4;
    }
    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(gameField.gameObject);
    }

    [Test]
    public void GameField_GetEmptyPosition_ReturnsValidPosition()
    {
        var cellsField = typeof(GameField).GetField("cells", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cellsField.SetValue(gameField, new System.Collections.Generic.List<Cell>());
        
        Vector2Int emptyPosition = gameField.GetEmptyPosition();
        
        emptyPosition.x.Should().BeGreaterThanOrEqualTo(0);
        emptyPosition.x.Should().BeLessThan(gameField.width);
        emptyPosition.y.Should().BeGreaterThanOrEqualTo(0);
        emptyPosition.y.Should().BeLessThan(gameField.height);
    }

    [Test]
    public void GameField_GetEmptyPosition_ReturnsMinusOneWhenFull()
    {
        var cellsField = typeof(GameField).GetField("cells", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var cells = new System.Collections.Generic.List<Cell>();
        for (int x = 0; x < gameField.width; x++)
        {
            for (int y = 0; y < gameField.height; y++)
            {
                cells.Add(new Cell(new Vector2Int(x, y), 2));
            }
        }
        cellsField.SetValue(gameField, cells);

        Vector2Int emptyPosition = gameField.GetEmptyPosition();

        emptyPosition.Should().Be(new Vector2Int(-1, -1));
    }
    
    [Test]
    public void GameField_CreateCell_AddsCellToCellsList()
    {
        var cellsField = typeof(GameField).GetField("cells", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cellsField.SetValue(gameField, new System.Collections.Generic.List<Cell>());

        gameField.CreateCell();

        var cells = (System.Collections.Generic.List<Cell>)cellsField.GetValue(gameField);
        cells.Count.Should().Be(1);
    }

    [Test]
    public void GameField_CreateCell_DoesNotAddCellWhenFull()
    {
        var cellsField = typeof(GameField).GetField("cells", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var cells = new System.Collections.Generic.List<Cell>();
        for (int x = 0; x < gameField.width; x++)
        {
            for (int y = 0; y < gameField.height; y++)
            {
                cells.Add(new Cell(new Vector2Int(x, y), 2));
            }
        }
        cellsField.SetValue(gameField, cells);

        gameField.CreateCell();

        cells.Count.Should().Be(gameField.width * gameField.height);
    }

     [Test]
    public void GameField_CreateCell_CreatesCellWithValidValue()
    {
        var cellsField = typeof(GameField).GetField("cells", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cellsField.SetValue(gameField, new System.Collections.Generic.List<Cell>());
        
        gameField.CreateCell();
        
        var cells = (System.Collections.Generic.List<Cell>)cellsField.GetValue(gameField);
        
        (cells[0].Value == 2 || cells[0].Value == 4).Should().BeTrue();
    }
}
}