using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _scoreText;

    public Text GameOverText { get => _gameOverText; set => _gameOverText = value; }
    public Text LevelText { get => _levelText; set => _levelText = value; }
    public Text ScoreText { get => _scoreText; set => _scoreText = value; }

    public void ShowText(Text textToShow) => textToShow.enabled = true;
    public void HideText(Text textToHide) => textToHide.enabled = false;
    public void SetText(Text textToSet, string value) => textToSet.text = value;
}
