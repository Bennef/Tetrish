using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text gameOverText;

    public Text GameOverText { get => gameOverText; set => gameOverText = value; }
    public Text LevelText { get => levelText; set => levelText = value; }
    public Text ScoreText { get => scoreText; set => scoreText = value; }

    void Start()
    {
        ScoreText = GameObject.Find("Score Text").GetComponent<Text>();
        LevelText = GameObject.Find("Level Text").GetComponent<Text>();
        GameOverText = GameObject.Find("Game Over Text").GetComponent<Text>();
    }

    public void ShowText(Text textToShow)
    {
        textToShow.enabled = true;
    }

    public void HideText(Text textToHide)
    {
        textToHide.enabled = false;
    }

    public void SetText(Text textToSet, string value)
    {
        textToSet.text = value;
    }
}
