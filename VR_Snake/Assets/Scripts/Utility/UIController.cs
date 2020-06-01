using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Canvas mainMenu;

    [SerializeField]
    Canvas mapSelect;

    public void Start()
    {
        ShowMainMenu();
    }

    public void ExitGame()
    {
        Debug.Log("EXITING APPLICATION");
        Application.Quit(0);
    }

    public void ShowMapSelect()
    {
        mainMenu.gameObject.SetActive(false);

        mapSelect.gameObject.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mapSelect.gameObject.SetActive(false);

        mainMenu.gameObject.SetActive(true);
    }

    public void StartMap(string map)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(map, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
