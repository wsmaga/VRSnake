using Google.ProtocolBuffers;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Canvas mainMenu;

    [SerializeField]
    Canvas mapSelect;

    [SerializeField]
    GameObject world;

    [SerializeField]
    GameObject camera;

    private const float WORLDROTSPEED = 0.01f;

    public void Start()
    {
        ShowMainMenu();
    }

    public void Update()
    {
        Vector3 worldDir = world.transform.forward;
        Vector3 playerDir = camera.transform.forward;

        if (Vector3.Angle(worldDir, playerDir) > 70)
        {
            Vector3 direction = Vector3.RotateTowards(worldDir, playerDir, WORLDROTSPEED, 1000);
            direction.y = 0;
            world.transform.rotation = Quaternion.LookRotation(direction);
        }
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
