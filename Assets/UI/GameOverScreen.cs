using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private HealthAndEnergy _healthAndEnergy;
    [SerializeField] private MainMenu mainMenu;
    public Text CaptionText;
    [SerializeField] private Text _menuButtonText;
    [SerializeField] private Text _restartButtonText;


    private void OnEnable()
    {
        MainMenu.IsGameStarted = false;
        Cursor.visible = true;

        float time = HealthAndEnergy.TimePassed;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        
        int best = PlayerPrefs.GetInt("BestTime", 0);
        if (time > best)
        {
            PlayerPrefs.SetInt("BestTime", (int) time);
        }

        if (_healthAndEnergy.health > 0)
        {
            string message;
            if (_healthAndEnergy.health >= 80) message = mainMenu.languageText.GetEntry("congrA");
            else if (_healthAndEnergy.health >= 40) message = mainMenu.languageText.GetEntry("congrB");
            else message = mainMenu.languageText.GetEntry("congrC");
            CaptionText.text = mainMenu.languageText.GetEntry("congrats") + message;
        }
        else 
        {
            string message;
            if (minutes <= 3) message = mainMenu.languageText.GetEntry("game_overA");
            else if (minutes <= 10) message = mainMenu.languageText.GetEntry("game_overB");
            else message = mainMenu.languageText.GetEntry("game_overC");
            CaptionText.text = mainMenu.languageText.GetEntry("game_over") + message;
            //$"Смэрть. Game over.\n\nYou held up for <color=#E7834F>{minutes}</color> minutes <color=#E7834F>{seconds}</color> seconds.\n{message}";
        }

        _menuButtonText.text = mainMenu.languageText.GetEntry("menu_button"); ;
        _restartButtonText.text = mainMenu.languageText.GetEntry("restart_button"); 

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Restart();
    }

    public void Restart()
    {
        MainMenu.IsGameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
