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

    //zmienna stanu zawierająca 
    private bool mExists;

    //referencja reprezentująca punkt do zebrania
    [SerializeField] GameObject pointTemplate;

    private GameObject point;


    // Start is called before the first frame update
    void Start()
    {
        mExists = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mExists)
        {

                float xRand = Random.Range(bottomLeftConstraint.x, topRightConstraint.x);
                float zRand = Random.Range(bottomLeftConstraint.z, topRightConstraint.z);

                Vector3 rayCastBeginPoint = new Vector3(xRand, raycastBeginPointHeight, zRand);

                Vector3 offset = new Vector3(.0f, 0.5f, .0f);

                RaycastHit hit;
                Ray pointGenerationRay = new Ray(rayCastBeginPoint, Vector3.down);

                if (Physics.Raycast(pointGenerationRay, out hit))
                {
                    if (hit.collider.tag == "Map")
                    {
                        point = Instantiate(pointTemplate,hit.point+offset,Quaternion.Euler(new Vector3(90.0f,0.0f,0.0f)));
                        mExists = true;
                    }
                }

       }
    }
}
