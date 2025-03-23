using NUnit.Framework;
using UnityEngine;
using FluentAssertions;
using System.Linq;


namespace Editor
{
    public class TileGridTests
{
    private TileGrid tileGrid;
    private GameObject gameObject;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        tileGrid = gameObject.AddComponent<TileGrid>();

        TileRow[] rows = new TileRow[4];
        TileCell[] cells = new TileCell[16];
        for (int i = 0; i < 4; i++)
        {
            rows[i] = new GameObject().AddComponent<TileRow>();
            rows[i].transform.SetParent(tileGrid.transform);
            for (int j = 0; j < 4; j++)
            {
                cells[i * 4 + j] = new GameObject().AddComponent<TileCell>();
                cells[i * 4 + j].transform.SetParent(rows[i].transform);
                cells[i * 4 + j].coordinates = new Vector2Int(j,i);
            }
            typeof(TileRow).GetProperty("cells").SetValue(rows[i], cells.Skip(i*4).Take(4).ToArray());
        }
        typeof(TileGrid).GetProperty("rows").SetValue(tileGrid, rows);
        typeof(TileGrid).GetProperty("cells").SetValue(tileGrid, cells);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameObject);
    }
     [Test]
    public void TileGrid_Awake_InitializesRowsAndCells()
    {
        tileGrid.rows.Should().NotBeNull().And.HaveCount(4);
        tileGrid.cells.Should().NotBeNull().And.HaveCount(16);
    }

    [Test]
    public void Start_SetsCellCoordinates() {
        var startMethod = typeof(TileGrid).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        startMethod.Invoke(tileGrid, null);

        for (int y = 0; y < tileGrid.rows.Length; y++)
        {
            for (int x = 0; x< tileGrid.rows[y].cells.Length; x++)
            {
                tileGrid.rows[y].cells[x].coordinates.Should().Be(new Vector2Int(x,y));
            }
        }
    }


    [Test]
    public void TileGrid_GetCell_WithValidCoordinates_ReturnsCorrectCell()
    {
        TileCell cell = tileGrid.GetCell(2, 1);
        cell.Should().Be(tileGrid.rows[1].cells[2]);
    }

    [Test]
    public void TileGrid_GetCell_WithInvalidCoordinates_ReturnsNull()
    {
        TileCell cell = tileGrid.GetCell(5, 1);
        cell.Should().BeNull();

        cell = tileGrid.GetCell(1, 5);
        cell.Should().BeNull();

        cell = tileGrid.GetCell(-1, 1);
        cell.Should().BeNull();
    }
        [Test]
    public void GetCell_WithVector2Int_ReturnsCorrectCell()
    {
        Vector2Int coords = new Vector2Int(1, 2);

        TileCell cell = tileGrid.GetCell(coords);

        cell.Should().Be(tileGrid.rows[2].cells[1]);
    }

    [Test]
    public void TileGrid_GetAdjacentCell_ReturnsCorrectAdjacentCell()
    {
        TileCell cell = tileGrid.GetCell(1, 1);
        TileCell adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.right);
        adjacent.Should().Be(tileGrid.GetCell(2, 1));

        adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.left);
        adjacent.Should().Be(tileGrid.GetCell(0, 1));

        adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.up);
        adjacent.Should().Be(tileGrid.GetCell(1, 0));

        adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.down);
        adjacent.Should().Be(tileGrid.GetCell(1, 2));
    }
    [Test]
    public void TileGrid_GetAdjacentCell_WithInvalidCoordinates_ReturnsNull() {
        TileCell cell = tileGrid.GetCell(0, 0); // Top left corner
        TileCell adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.up); //Try to get cell above
        adjacent.Should().BeNull();

        cell = tileGrid.GetCell(3,3); //Bottom Right
        adjacent = tileGrid.GetAdjacentCell(cell, Vector2Int.down);
        adjacent.Should().BeNull();
    }

    [Test]
    public void TileGrid_GetRandomEmptyCell_ReturnsEmptyCell()
    {
        tileGrid.GetCell(0, 0).tile = new GameObject().AddComponent<Tile>();

        TileCell emptyCell = tileGrid.GetRandomEmptyCell();
        emptyCell.Should().NotBeNull();
        emptyCell.empty.Should().BeTrue();
        Object.DestroyImmediate(tileGrid.GetCell(0,0).tile.gameObject);
    }

    [Test]
    public void TileGrid_GetRandomEmptyCell_ReturnsNullWhenGridFull()
    {
        foreach (var cell in tileGrid.cells)
        {
            cell.tile = new GameObject().AddComponent<Tile>();
        }

        TileCell emptyCell = tileGrid.GetRandomEmptyCell();
        emptyCell.Should().BeNull();

        foreach (var cell in tileGrid.cells)
        {
             Object.DestroyImmediate(cell.tile.gameObject);
        }
    }

    [Test]
    public void Size_ReturnsCorrectValue()
    {
        tileGrid.size.Should().Be(16);
    }
     [Test]
    public void Height_ReturnsCorrectValue()
    {
        tileGrid.height.Should().Be(4);
    }

    [Test]
    public void Width_ReturnsCorrectValue()
    {
        tileGrid.width.Should().Be(4);
    }
}
}