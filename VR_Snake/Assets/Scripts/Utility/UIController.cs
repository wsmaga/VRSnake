using Google.ProtocolBuffers;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Zachowanie kontrolera interfejsu użytkownika. W szczególności zarządzanie wyświetlaniem odpowiednich elementów UI i ładowanie wybranych map.
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField]
    Canvas mainMenu;  // Canvas z UI menu głównego

    [SerializeField]
    Canvas mapSelect;  // Canvas z UI menu wyboru map

    [SerializeField]
    GameObject world;  // Obiekt zawierający wszystkie widzialne obiekty wokół gracza dla łatwego obracania światem wokół gracza

    [SerializeField]
    GameObject camera;  // Kamera będąca de facto oczami gracza korzystającego z gogli VR

    private const float WORLDROTSPEED = 0.01f;  // Jak szybko świat obraca się gdy gracz zgubi menu z zasięgu wzroku

    public void Start()
    {
        ShowMainMenu();  // Na początku programu wyświetl graczowi menu główne
    }

    public void Update()
    {
        // Ustal wektory kierunkowe gracza oraz świata wokół niego:
        Vector3 worldDir = world.transform.forward;
        Vector3 playerDir = camera.transform.forward;

        if (Vector3.Angle(worldDir, playerDir) > 70)  // Jeżeli kąt między tymi wektorami jest zbyt duży...
        {
            // ...obróć świat w stronę gracza tak by ten nie stracił UI z oczu:
            Vector3 direction = Vector3.RotateTowards(worldDir, playerDir, WORLDROTSPEED, 1000);
            direction.y = 0;
            world.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    /// <summary>
    /// Wywoływana gdy gracz chce wyjść z aplikacji.
    /// </summary>
    public void ExitGame()
    {
        // Debug.Log("EXITING APPLICATION");
        Application.Quit(0);
    }

    /// <summary>
    /// Wywoływana gdy gracz chce wyświetlić menu wyboru map. Wyświetla odpowiednie canvasy.
    /// </summary>
    public void ShowMapSelect()
    {
        mainMenu.gameObject.SetActive(false);

        mapSelect.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wywoływana gdy gracz chce wyświetlić menu główne. Wyświetla odpowiednie canvasy.
    /// </summary>
    public void ShowMainMenu()
    {
        mapSelect.gameObject.SetActive(false);

        mainMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wywoływana gdy gracz wybierze konkretną mapę do załadowania.
    /// </summary>
    /// <param name="map">Nazwa sceny z wybraną mapą.</param>
    public void StartMap(string map)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(map, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
