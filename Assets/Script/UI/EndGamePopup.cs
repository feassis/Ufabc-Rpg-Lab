using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePopup : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private string defeatMessage = "You were captured!!";
    [SerializeField] private string winMessage = "You were won!!";
    [SerializeField] private string nextLevelName;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        });

        nextLevelButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(nextLevelName);
            Time.timeScale = 1f;
        });
    }

    public void Setup(bool hasWon)
    {
        Time.timeScale = 0f;
        if (hasWon)
        {
            playAgainButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
            messageText.text = winMessage;
        }
        else
        {
            playAgainButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
            messageText.text = defeatMessage;
        }

    }
}
