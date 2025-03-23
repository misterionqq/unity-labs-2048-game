using NUnit.Framework;
using UnityEngine;
using FluentAssertions;


namespace Editor
{
    public class TileRowTests
    {
        [Test]
        public void TileRow_Awake_InitializesCells()
        {
            GameObject go = new GameObject();
            TileRow row = go.AddComponent<TileRow>();

            TileCell[] cells = new TileCell[4];
            for (int i = 0; i < 4; i++)
            {
                cells[i] = new GameObject().AddComponent<TileCell>();
            }
            typeof(TileRow).GetProperty("cells").SetValue(row, cells);
            var awakeMethod = typeof(TileRow).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(row, null);

            row.cells.Should().NotBeNull().And.HaveCount(4);
            for (int i = 0; i < 4; i++)
            {
                Object.DestroyImmediate(cells[i].gameObject);
            }

            Object.DestroyImmediate(go);

        }
    }
}