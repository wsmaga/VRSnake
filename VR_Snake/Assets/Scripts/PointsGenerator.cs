using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsGenerator : MonoBehaviour
{

    //warunek mniejszości losowych współrzędnych (x<, z<)
    [SerializeField] Vector3 bottomLeftConstraint;

    // //warunek mniejszości losowych współrzędnych (x>, z>)
    [SerializeField] Vector3 topRightConstraint;

    //wysokość, na której znajduje się punkt początkowy raycast-a (współrzędna y punktu
    [SerializeField] float raycastBeginPointHeight;

    //szablon punktu, który będzie spawnowany
    [SerializeField] GameObject pointTemplate;

    //lista zawierająca tagi colliderów, na których może się punkt zespawnować
    [SerializeField] List<string> allowedSurfaces;

    //przesunięcie, o które przesunięty jest punkt spawnowania względem punktu-kandydata
    Vector3 offset = new Vector3(.0f, 0.5f, .0f);

    //rzeczywisty punkt znajdujący się na planszy
    private GameObject point;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!point)
        {
                //wylosowanie wektora
                float xRand = Random.Range(bottomLeftConstraint.x, topRightConstraint.x);
                float zRand = Random.Range(bottomLeftConstraint.z, topRightConstraint.z);

                //punkt początkowy raycast
                Vector3 rayCastBeginPoint = new Vector3(xRand, raycastBeginPointHeight, zRand);

                RaycastHit hit;
                Ray pointGenerationRay = new Ray(rayCastBeginPoint, Vector3.down);

                
                if (Physics.Raycast(pointGenerationRay, out hit))
                {
                    //sprawdzenie czy punkt-kandydat znajduje się na odpowiedniej płaszczyźnie i zespawnowanie go
                    foreach (string it in allowedSurfaces)
                    {
                        if (hit.collider.tag == it)
                        {
                            point = Instantiate(pointTemplate, hit.point + offset, Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                            break;
                        }
                    }
                }

       }
    }
}
