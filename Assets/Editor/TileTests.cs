using FluentAssertions;
using NUnit.Framework;
using UnityEngine;

namespace Editor
{
    public class TileTests
    {
        private Tile tile;
        private GameObject gameObject;

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject();
            tile = gameObject.AddComponent<Tile>();
            gameObject.AddComponent<UnityEngine.UI.Image>();
            gameObject.AddComponent<TMPro.TextMeshProUGUI>();

            // FORCE AWAKE
            var awakeMethod = typeof(Tile).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(tile, null);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void Tile_SetState_UpdatesProperties()
        {
            TileScript state = ScriptableObject.CreateInstance<TileScript>();
            state.backgroundColor = Color.red;
            state.textColor = Color.white;
            int number = 4;

            tile.SetState(state, number);

            tile.state.Should().Be(state);
            tile.number.Should().Be(number);
            tile.GetComponent<UnityEngine.UI.Image>().color.Should().NotBe(Color.clear);
            tile.GetComponentInChildren<TMPro.TextMeshProUGUI>().text.Should().Be(number.ToString());

            Object.DestroyImmediate(state);
        }
        [Test]
        public void Tile_Spawn_SetsCellAndPosition()
        {
            TileCell cell = new GameObject().AddComponent<TileCell>();
            cell.coordinates = new Vector2Int(1, 2);

            tile.Spawn(cell);

            tile.cell.Should().Be(cell);
            cell.tile.Should().Be(tile);
            tile.transform.position.Should().Be((Vector3)cell.transform.position);
        }

        [Test]
        public void Tile_MoveTo_MovesTileToNewCell()
        {

            TileCell initialCell = new GameObject().AddComponent<TileCell>();
            TileCell newCell = new GameObject().AddComponent<TileCell>();
            initialCell.coordinates = Vector2Int.zero;
            newCell.coordinates = Vector2Int.one;

            tile.Spawn(initialCell);
            tile.MoveTo(newCell);

            tile.cell.Should().Be(newCell);
            newCell.tile.Should().Be(tile);
            initialCell.tile.Should().BeNull();

            tile.transform.position.Should().Be((Vector3)newCell.transform.position);
        }


        [Test]
        public void Tile_Merge_MergesTilesAndDestroysSource()
        {

            TileCell initialCell = new GameObject().AddComponent<TileCell>();
            TileCell targetCell = new GameObject().AddComponent<TileCell>();
            initialCell.coordinates = Vector2Int.zero;
            targetCell.coordinates = Vector2Int.one;
            Tile targetTile = new GameObject().AddComponent<Tile>();
            targetTile.gameObject.AddComponent<UnityEngine.UI.Image>();
            targetTile.gameObject.AddComponent<TMPro.TextMeshProUGUI>();
            var awakeMethod = typeof(Tile).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(targetTile, null);
            targetTile.Spawn(targetCell);


            tile.Spawn(initialCell);
            tile.Merge(targetCell);


            tile.cell.Should().BeNull();
            targetCell.tile.locked.Should().BeTrue();

            Object.DestroyImmediate(targetTile.gameObject);

        }
    }
}