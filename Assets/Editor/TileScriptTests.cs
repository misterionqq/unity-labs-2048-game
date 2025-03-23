using NUnit.Framework;
using UnityEngine;
using FluentAssertions;

namespace Editor
{
    public class TileScriptTests
    {
        [Test]
        public void TileScript_Properties_CanBeSetAndRetrieved()
        {
            TileScript script = ScriptableObject.CreateInstance<TileScript>();
            Color backgroundColor = Color.blue;
            Color textColor = Color.yellow;

            script.backgroundColor = backgroundColor;
            script.textColor = textColor;

            script.backgroundColor.Should().Be(backgroundColor);
            script.textColor.Should().Be(textColor);

            ScriptableObject.DestroyImmediate(script);
        }
    }
}