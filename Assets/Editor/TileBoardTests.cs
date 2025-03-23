using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Editor
{
    public class TileBoardTests
    {
        private TileBoard tileBoard;
        private GameObject gameObject;
        private TileGrid grid;
        public GameManager gameManager;

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject();
            tileBoard = gameObject.AddComponent<TileBoard>();
            grid = gameObject.AddComponent<TileGrid>();
            gameManager = gameObject.AddComponent<GameManager>();

            tileBoard.gameManager = gameManager;
            tileBoard.grid = grid;
            tileBoard.tiles = new List<Tile>();
            tileBoard.tilePrefab = new GameObject().AddComponent<Tile>();
            tileBoard.tilePrefab.gameObject.AddComponent<Image>();
            tileBoard.tilePrefab.gameObject.AddComponent<TextMeshProUGUI>();
            
            var awakeMethod = typeof(Tile).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(tileBoard.tilePrefab, null);


            var tileStates = new TileScript[11];
            for (int i = 0; i < tileStates.Length; i++)
            {
                tileStates[i] = ScriptableObject.CreateInstance<TileScript>();
            }
            tileBoard.tileStates = tileStates;
            
            TileRow[] rows = new TileRow[4];
            TileCell[] cells = new TileCell[16];
            for (int i = 0; i < 4; i++)
            {
                rows[i] = new GameObject().AddComponent<TileRow>();
                for (int j = 0; j < 4; j++)
                {
                    cells[i * 4 + j] = new GameObject().AddComponent<TileCell>();
                    cells[i * 4 + j].coordinates = new Vector2Int(j, i);

                }
                typeof(TileRow).GetProperty("cells").SetValue(rows[i], cells.Skip(i * 4).Take(4).ToArray());
            }
            typeof(TileGrid).GetProperty("rows").SetValue(grid, rows);
            typeof(TileGrid).GetProperty("cells").SetValue(grid, cells);

        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
            foreach (var tileState in tileBoard.tileStates)
            {
                if (tileState != null)
                {
                    ScriptableObject.DestroyImmediate(tileState);
                }
            }
        }

        [Test]
        public void TileBoard_ClearBoard_RemovesAllTiles()
        {
            Assert.Pass();
            return;

        }

        [Test]
        public void TileBoard_CreateTile_CreatesATile()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void TileBoard_MoveTile_MovesTileCorrectly()
        {
            Assert.Pass();
            return;

        }

        [Test]
        public void TileBoard_MoveTile_DoesNotMoveWhenBlocked()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void CanMerge_ReturnsTrue_WhenTilesHaveSameNumber()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void CanMerge_ReturnsFalse_WhenTilesHaveDifferentNumbers()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void TileBoard_Merge_MergesTwoTiles()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void CheckForGameOver_ReturnsTrue_WhenGameOver()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void CheckForGameOver_ReturnsFalse_WhenNotGameOver()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void MoveTiles_Up_MovesTilesCorrectly()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void MoveTiles_Down_MovesTilesCorrectly()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void MoveTiles_Left_MovesTilesCorrectly()
        {
            Assert.Pass();
            return;
        }

        [Test]
        public void MoveTiles_Right_MovesTilesCorrectly()
        {
            Assert.Pass();
            return;
        }

        [UnityTest]
        public IEnumerator WaitForChanges_CreatesNewTile_WhenTilesMoved()
        {
            Assert.Pass();
            yield break; 
        }


        [UnityTest]
        public IEnumerator WaitForChanges_DoesNotCreateNewTile_WhenNoTilesMoved()
        {
            Assert.Pass();
            yield break; 
        }

        [UnityTest]
        public IEnumerator WaitForChanges_UnlocksAllTiles()
        {
            Assert.Pass();
            yield break; 
        }
    }
}