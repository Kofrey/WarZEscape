using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangButton : MonoBehaviour
{
    [SerializeField] private Button _thisButton;
    [SerializeField] private MainMenu mainMenu;

    public void OnClick()
    {
        mainMenu.languageText.SetLanguage("English");
        _thisButton.gameObject.SetActive(false);
    }
}
