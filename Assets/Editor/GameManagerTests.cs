using System.Collections;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Editor
{
    public class GameManagerTests
    {
        private GameManager gameManager;
        private GameObject gameObject;
        private TileBoard tileBoard;
        private CanvasGroup gameOverCanvasGroup;
        private TextMeshProUGUI scoreText;
        private TextMeshProUGUI hiscoreText;

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject();
            gameManager = gameObject.AddComponent<GameManager>();

            tileBoard = gameObject.AddComponent<TileBoard>();
            gameOverCanvasGroup = gameObject.AddComponent<CanvasGroup>();
            scoreText = gameObject.AddComponent<TextMeshProUGUI>();
            hiscoreText = gameObject.AddComponent<TextMeshProUGUI>();

            gameManager.board = tileBoard;
            gameManager.gameOver = gameOverCanvasGroup;
            gameManager.scoreText = scoreText;
            gameManager.hiscoreText = hiscoreText;
            
            tileBoard.grid = new GameObject().AddComponent<TileGrid>();
            tileBoard.tiles = new System.Collections.Generic.List<Tile>();
            tileBoard.tilePrefab = new GameObject().AddComponent<Tile>();
            tileBoard.tilePrefab.gameObject.AddComponent<UnityEngine.UI.Image>();
            tileBoard.tilePrefab.gameObject.AddComponent<TextMeshProUGUI>();
            var awakeMethod = typeof(Tile).GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(tileBoard.tilePrefab, null);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
            string path = Path.Combine(Application.persistentDataPath, "save.dat");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        [Test]
        public void NewGame_ResetsScoreAndCreatesTiles()
        {
            gameManager.NewGame();
            
            int.Parse(gameManager.scoreText.text).Should().Be(0);
            gameManager.gameOver.alpha.Should().Be(0f);
            gameManager.gameOver.interactable.Should().BeFalse();
            tileBoard.tiles.Count.Should().BeGreaterThan(0);
            tileBoard.enabled.Should().BeTrue();
        }

        [UnityTest]
        public IEnumerator GameOver_DisablesBoardAndShowsGameOverScreen()
        {
            gameManager.GameOver();

            tileBoard.enabled.Should().BeFalse();
            gameManager.gameOver.interactable.Should().BeTrue();
            yield return null;
            gameManager.gameOver.alpha.Should().BeGreaterThan(0f);
        }
        [Test]
        public void IncreaseScore_UpdatesScoreText()
        {
            int initialScore = int.Parse(gameManager.scoreText.text);
            int points = 10;

            gameManager.IncreaseScore(points);

            int.Parse(gameManager.scoreText.text).Should().Be(initialScore + points);
        }

        [Test]
        public void IncreaseScore_UpdatesHiscore_WhenNewHiscore()
        {
            gameManager.NewGame();
            int points = 100;

            gameManager.IncreaseScore(points);

            int.Parse(gameManager.hiscoreText.text).Should().Be(points);

        }

        [Test]
        public void IncreaseScore_DoesNotUpdateHiscore_WhenNotNewHiscore()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(200);
            int initialHiscore = int.Parse(gameManager.hiscoreText.text);
            int points = 50;

            gameManager.IncreaseScore(points);

            int.Parse(gameManager.hiscoreText.text).Should().Be(initialHiscore);
        }
        [Test]
        public void SaveHiScore_SavesScoreToFile()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(500);

            var saveHiScoreMethod = typeof(GameManager).GetMethod("SaveHiScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveHiScoreMethod.Invoke(gameManager, null);

            var loadHiScoreMethod = typeof(GameManager).GetMethod("LoadHiScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            int loadedHiScore = (int)loadHiScoreMethod.Invoke(gameManager, null);

            loadedHiScore.Should().Be(500);

        }

        [Test]
        public void LoadHiScore_LoadsScoreFromFile_IfExists()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(500);

            var saveHiScoreMethod = typeof(GameManager).GetMethod("SaveHiScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveHiScoreMethod.Invoke(gameManager, null);

            var loadHiScoreMethod = typeof(GameManager).GetMethod("LoadHiScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            int loadedHiScore = (int)loadHiScoreMethod.Invoke(gameManager, null);

            loadedHiScore.Should().Be(500);

        }
        [Test]
        public void LoadHiScore_ReturnsZero_IfFileDoesNotExist()
        {
            string path = Path.Combine(Application.persistentDataPath, "save.dat");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var loadHiScoreMethod = typeof(GameManager).GetMethod("LoadHiScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            int loadedHiScore = (int)loadHiScoreMethod.Invoke(gameManager, null);

            loadedHiScore.Should().Be(0);
        }

        [Test]
        public void ResetGame_CallsNewGame()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(100);
            gameManager.GameOver();
            gameManager.ResetGame();

            int.Parse(gameManager.scoreText.text).Should().Be(0);
            gameManager.gameOver.alpha.Should().Be(0f);
            gameManager.gameOver.interactable.Should().BeFalse();
            tileBoard.tiles.Count.Should().BeGreaterThan(0);
            tileBoard.enabled.Should().BeTrue();
        }
        [Test]
        public void SaveGame_SavesDataToFile()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(42);
            
            var saveGameMethod = typeof(GameManager).GetMethod("SaveGame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveGameMethod.Invoke(gameManager, null);

            string path = Path.Combine(Application.persistentDataPath, "save.dat");
            File.Exists(path).Should().BeTrue();

            var loadGameMethod = typeof(GameManager).GetMethod("LoadGame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            loadGameMethod.Invoke(gameManager, null);
            int.Parse(gameManager.scoreText.text).Should().Be(42);

        }

        [Test]
        public void LoadGame_LoadsDataFromFile()
        {
            gameManager.NewGame();
            gameManager.IncreaseScore(123);

            var saveGameMethod = typeof(GameManager).GetMethod("SaveGame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveGameMethod.Invoke(gameManager, null);
            var setScoreMethod = typeof(GameManager).GetMethod("SetScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            setScoreMethod.Invoke(gameManager, new object[] { 0 });
            var loadGameMethod = typeof(GameManager).GetMethod("LoadGame", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            loadGameMethod.Invoke(gameManager, null);

            int.Parse(gameManager.scoreText.text).Should().Be(123);
        }
    }
}