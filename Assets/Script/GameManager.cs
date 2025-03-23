using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    private int score;

    private void Start()
    {
        
        LoadGame();
        NewGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        SetScore(0);
        hiscoreText.text = LoadHiScore().ToString();

        gameOver.alpha = 0f;
        gameOver.interactable = false;

        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));

        int hiscore = LoadHiScore();
        if (score > hiscore)
        {
            SaveHiScore();
            hiscoreText.text = score.ToString();
        }
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        int hiscore = LoadHiScore();
        if (score > hiscore)
        {
            SaveHiScore();
            hiscoreText.text = score.ToString();
        }
    }

    public void SaveHiScore()
    {
        GameData data = new GameData
        {
            score = this.score,
           // bestScore = LoadHiScore(),
            bestScore = this.score,
            tiles = new List<GameData.TileData>()
        };

        foreach (var tile in board.tiles)
        {
            data.tiles.Add(new GameData.TileData
            {
                x = tile.cell.coordinates[0],
                y = tile.cell.coordinates[1],
                value = tile.number
            });
        }

        string path = Path.Combine(Application.persistentDataPath, "save.dat");
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public int LoadHiScore()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.dat");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                GameData data = (GameData)formatter.Deserialize(stream);
                return data.bestScore;
            }
        }
        return 0;
    }

    public void SaveGame()
    {
        GameData data = new GameData
        {
            score = this.score,
            bestScore = LoadHiScore(),
            tiles = new List<GameData.TileData>()
        };

        foreach (var tile in board.tiles)
        {
            data.tiles.Add(new GameData.TileData
            {
                x = tile.cell.coordinates[0],
                y = tile.cell.coordinates[1],
                value = tile.number
            });
        }

        string path = Path.Combine(Application.persistentDataPath, "save.dat");
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.dat");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                GameData data = (GameData)formatter.Deserialize(stream);
                SetScore(data.score);
                hiscoreText.text = data.bestScore.ToString();

                board.ClearBoard();
                foreach (var tileData in data.tiles)
                {
                    Tile tile = Instantiate(board.tilePrefab, board.grid.transform);
                    int value = tileData.value;
                    tile.SetState(board.tileStates[0], value);
                    tile.Spawn(board.grid.GetCell(tileData.x, tileData.y));
                    board.tiles.Add(tile);
                }
            }
        }
    }

    public void ResetGame()
    {
        SaveHiScore();
        NewGame();
    }
}
/*
public class SwipeInput : MonoBehaviour
{
    public float swipeThreshold = 50f; // Минимальная длина свайпа, чтобы считался движением
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (!isSwiping)
            {
                startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isSwiping = true;
            }
        }
        else if (isSwiping)
        {
            endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            DetectSwipe();
            isSwiping = false;
        }

        // Добавляем обработку свайпов мышью
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startTouchPosition = Mouse.current.position.ReadValue();
            isSwiping = true;
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endTouchPosition = Mouse.current.position.ReadValue();
            DetectSwipe();
            isSwiping = false;
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < swipeThreshold) return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0) MoveRight();
            else MoveLeft();
        }
        else
        {
            if (swipeDelta.y > 0) MoveUp();
            else MoveDown();
        }
    }

    private void MoveUp() => Debug.Log("Swipe Up");
    private void MoveDown() => Debug.Log("Swipe Down");
    private void MoveLeft() => Debug.Log("Swipe Left");
    private void MoveRight() => Debug.Log("Swipe Right");
}
*/
[Serializable]
public class GameData
{
    public int score;
    public int bestScore;
    public List<TileData> tiles;

    [Serializable]
    public class TileData
    {
        public int x;
        public int y;
        public int value;
    }
}