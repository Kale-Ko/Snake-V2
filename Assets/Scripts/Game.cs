/**
    MIT License
    Copyright (c) 2021 Kale Ko
    See https://kaleko.ga/license.txt
*/

using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text scoreText;

    public GameObject snakePrefab;
    public GameObject foodPrefab;

    public GameObject pauseMenuPrefab;
    public GameObject gameOverPrefab;

    private GameObject snake;

    private GameObject menu = null;

    void Start()
    {
        this.SpawnFood();

        this.snake = Instantiate(snakePrefab);
        this.snake.name = "Snake";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) this.Pause();

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!Debug.developerConsoleVisible) { Debug.LogError("Showing Console"); Debug.developerConsoleVisible = true; }
            else Debug.developerConsoleVisible = false;
        }

        scoreText.text = "Score: " + (this.snake.GetComponent<Snake>().length - 1);
    }

    public void SpawnFood()
    {
        GameObject food = Instantiate(foodPrefab);
        food.name = "Food";
        food.transform.position = new Vector3(((float)Random.Range(-20, 21) / 2), ((float)Random.Range(-10, 12) / 2) - 0.25f, 0);
    }

    public void Pause()
    {
        if (menu == null)
        {
            this.snake.GetComponent<Snake>().started = false;

            menu = Instantiate(pauseMenuPrefab);
        }
        else
        {
            Destroy(menu);

            menu = null;

            this.snake.GetComponent<Snake>().started = true;
        }
    }

    public void GameOver()
    {
        Instantiate(gameOverPrefab);
    }
}
