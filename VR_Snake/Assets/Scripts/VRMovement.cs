using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRMovement : MonoBehaviour
{
    public float mSpeed = 3.0f; //prędkość poruszania
    public float gravity = 2.0f; //siła gravitacji
    private bool isMoving = true; //zmienna przechowująca informacje czy gracz się posrusza

    private CharacterController characterController;
    [SerializeField] private Transform cameraTransform; //pozycja i rotacja kamery
    [SerializeField] private Transform snakeHead; //pozycja i rotacja głowy węża
    [SerializeField] private GameObject trailGenerator; //obiekt generujący ogon węża
    [SerializeField] private Material deadMaterial;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //ustaw rotację w osi y głowy węża na rotację kamery
            snakeHead.rotation = Quaternion.Euler(0, cameraTransform.localEulerAngles.y, 0);
            //oblicz wektor do przodu  na podstawie głowy węża
            Vector3 vForward = snakeHead.transform.forward;
            //zastosuj siłę grawitacji
            vForward.y = -gravity;
            //przesuń gracza o wypadkowy wektor * prędkość
            characterController.Move(vForward*mSpeed * Time.deltaTime);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (trailGenerator.GetComponent<TrailGenerator>().IsGenerating())
        {
            Debug.Log("Detected collision with [" + other.gameObject.tag + "]");
            KillPlayer();
        }
    }
    
    //obsługa kolizji kontrolera gracza
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //na potrzeby testów przy zderzeniu z ogonem przestań generować ogon
        if (hit.gameObject.tag == "Trail")
        {
            if (trailGenerator.GetComponent<TrailGenerator>().IsGenerating())
            {
                Debug.Log("Collided with Trail");
                KillPlayer();
            }
        }
        //else if (hit.gameObject.tag != "Map")
           // Debug.Log("Collided with " + hit.gameObject.tag);
    }
    private void KillPlayer()
    {
        trailGenerator.GetComponent<TrailGenerator>().StopGenerating();
        GameObject playerHead = this.transform.Find("HeadObject").gameObject;
        GameObject temp = Instantiate(playerHead, playerHead.transform.position, playerHead.transform.rotation);
        temp.layer = 0;
        temp.GetComponent<MeshRenderer>().material = deadMaterial;
        Handheld.Vibrate();
    }


}
