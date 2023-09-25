using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;
    private GameObject startScreen;
    private Button button;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startScreen = GameObject.Find("GameStartScreen");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDifficulty()
    {
        startScreen.SetActive(false);
        gameManager.StartGame(difficulty);
    }
}
