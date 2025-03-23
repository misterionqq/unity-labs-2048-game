using NUnit.Framework;
using UnityEngine;
using FluentAssertions;


namespace Editor
{
    public class TileCellTests
    {
        [Test]
        public void TileCell_Empty_ReturnsTrueWhenNoTile()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            cell.tile = null;
            cell.empty.Should().BeTrue();
        }

        [Test]
        public void TileCell_Empty_ReturnsFalseWhenHasTile()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            cell.tile = new GameObject().AddComponent<Tile>();
            cell.empty.Should().BeFalse();
            Object.DestroyImmediate(cell.tile.gameObject);
            Object.DestroyImmediate(cell.gameObject);
        }

        [Test]
        public void TileCell_Occupied_ReturnsTrueWhenHasTile()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            cell.tile = new GameObject().AddComponent<Tile>();
            cell.occupied.Should().BeTrue();
            Object.DestroyImmediate(cell.tile.gameObject);
            Object.DestroyImmediate(cell.gameObject);
        }

        [Test]
        public void TileCell_Occupied_ReturnsFalseWhenNoTile()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            cell.tile = null;
            cell.occupied.Should().BeFalse();
            Object.DestroyImmediate(cell.gameObject);
        }
        [Test]
        public void TileCell_Coordinates_AreSetCorrectly()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            Vector2Int coords = new Vector2Int(3, 1);
            cell.coordinates = coords;
            cell.coordinates.Should().Be(coords);
            Object.DestroyImmediate(cell.gameObject);
        }
    }
}