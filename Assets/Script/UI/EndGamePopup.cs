using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//class que controla o popup de end game
public class EndGamePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("End Game Messages")]
    [SerializeField] private string defeatMessage = "You were captured!!";
    [SerializeField] private string winMessage = "You were won!!";

    [Header("Sets next Scene")]
    [SerializeField] private string nextLevelName;

    //Adiciona um metodo para ser chamado no click dos botoes
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

    //Metodo que configura o end game popup para o caso de vitoria ou derrota
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
