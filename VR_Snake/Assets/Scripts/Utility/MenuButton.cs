using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;
using System;

/// <summary>
/// Zarządza zachowaniem danego przycisku wrażliwego na Gaze UI - UI sterowane spojrzeniem użytkownika w odpowiednie miejsce.
/// </summary>
public class MenuButton : MonoBehaviour
{
    public Image circleIndicator;  // Sprite kółeczka sygnalizującego upływający czas w ramach Gaze UI
    public UnityEvent buttonClickEvent;  // Event wywoływany przy wykryciu dostatecznie długiego wpatrywania się w przycisk
    public float clickTime = 1;  // Ile sekund powinno zajmować wpatrywanie się w przycisk w celu kliknięcia
    bool status = false;  // True gdy Gaze UI spoczywa na danym przycisku
    public float gazeTimer = 0;  // Odlicza czas wpatrywania się w dany przycisk

    private void Start()
    {
        // Debug.Log("BUTTON STARTED");
    }

    // Update is called once per frame
    void Update()
    {
        if (status)  // jeżeli user spogląda na dany przycisk...
        {
            // ...odliczaj czas i aktualizuj wygląd kółeczka:
            gazeTimer += Time.deltaTime;
            circleIndicator.fillAmount = gazeTimer / clickTime;
        }

        if(gazeTimer > clickTime)  // jeżeli spoglądamy na przycisk wystarczająco długo...
        {
            // ...wywołaj event + wyzeruj odpowiednie atrybuty:
            status = false;
            circleIndicator.fillAmount = 0;
            gazeTimer = 0;
            // Debug.Log("BUTTON CLICKED");
            buttonClickEvent.Invoke();
        }
    }

    /// <summary>
    /// Wywoływane gdy spojrzenie użytkownika wejdzie w obręb danego przycisku.
    /// </summary>
    public void GazeOn()
    {
        // Debug.Log("GAZE ON");
        status = true;
    }

    /// <summary>
    /// Wywoływane gdy spojrzenie użytkownika opuści dany przycisk.
    /// </summary>
    public void GazeOff()
    {
        // Debug.Log("GAZE OFF");
        status = false;
        gazeTimer = 0;
        circleIndicator.fillAmount = 0;
    }
}
