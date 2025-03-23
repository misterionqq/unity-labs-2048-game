using NUnit.Framework;
using UnityEngine;
using FluentAssertions;
using System;
using TMPro;

namespace Editor
{
    public class CellTests
    {
        [Test]
        public void Cell_Constructor_InitializesCorrectly()
        {
            Vector2Int testPosition = new Vector2Int(2, 3);
            int testValue = 2;
            
            Cell cell = new Cell(testPosition, testValue);
            
            cell.Position.Should().Be(testPosition);
            cell.Value.Should().Be(testValue);
        }

        [Test]
        public void Cell_SetValue_UpdatesValueAndTriggersEvent()
        {
            Cell cell = new Cell(Vector2Int.zero, 1);
            int newValue = 3;
            int? eventValue = null;
            cell.OnValueChanged += (value) => eventValue = value;

            cell.Value = newValue;

            cell.Value.Should().Be(newValue);
            eventValue.Should().Be(newValue);
        }

        [Test]
        public void Cell_SetValue_SameValue_DoesNotTriggerEvent()
        {

            Cell cell = new Cell(Vector2Int.zero, 2);
            int newValue = 2;
            int eventCount = 0;
            cell.OnValueChanged += (value) => eventCount++;

            cell.Value = newValue;

            cell.Value.Should().Be(newValue);
            eventCount.Should().Be(0);
        }

        [Test]
        public void Cell_SetPosition_UpdatesPositionAndTriggersEvent()
        {
            Cell cell = new Cell(Vector2Int.zero, 1);
            Vector2Int newPosition = new Vector2Int(1, 1);
            Vector2Int? eventPosition = null;
            cell.OnPositionChanged += (pos) => eventPosition = pos;

            cell.SetPosition(newPosition);

            cell.Position.Should().Be(newPosition);
            eventPosition.Should().Be(newPosition);
        }
    }
}