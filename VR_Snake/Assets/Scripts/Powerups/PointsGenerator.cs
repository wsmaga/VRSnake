using System.Collections.Generic;
using UnityEngine;

public class PointsGenerator : MonoBehaviour
{

    //warunek mniejszości losowych współrzędnych (x<, z<)
    [SerializeField] Vector3 bottomLeftConstraint;

    // //warunek mniejszości losowych współrzędnych (x>, z>)
    [SerializeField] Vector3 topRightConstraint;


    [SerializeField] uint pointsCount;

    //wysokość, na której znajduje się punkt początkowy raycast-a (współrzędna y punktu
    [SerializeField] float raycastBeginPointHeight;

    //szablon punktu, który będzie spawnowany
    [SerializeField] List<GameObject> pointTemplates;

    //lista zawierająca tagi colliderów, na których może się punkt zespawnować
    [SerializeField] List<string> allowedSurfaces;

    [SerializeField] int PointsGenerationInvervalSeconds;

    //przesunięcie, o które przesunięty jest punkt spawnowania względem punktu-kandydata
    Vector3 offset = new Vector3(.0f, 0.7f, .0f);

    //rzeczywisty punkt znajdujący się na planszy
    private GameObject point;

    private GameObject[] points;

    private float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        points = new GameObject[pointsCount];
    }

    // Update is called once per frame
    void Update()
    {
        //sprawdzenie czy licznik doliczył do 
        if(PointsGenerationInvervalSeconds - timeCounter < 0.001)
        {
            for (int i = 0; i < pointsCount; i++)
            {
                if (points[i] == null)
                {
                    points[i] = generatePowerUp();
                    break;
                }
            }

            timeCounter = 0;
        }

        timeCounter += Time.deltaTime;
    }

    private GameObject generatePowerUp()
    {
        //wylosowanie typu powerup'a do zrespienia
        int templateIndex = UnityEngine.Random.Range(0, pointTemplates.Count);


        //wylosowanie wektora   3.0f - odległość od ścian mapy
        float xRand = UnityEngine.Random.Range(bottomLeftConstraint.x + 3.0f, topRightConstraint.x - 3.0f);
        float zRand = UnityEngine.Random.Range(bottomLeftConstraint.z + 3.0f, topRightConstraint.z - 3.0f);

        //punkt początkowy raycast
        Vector3 rayCastBeginPoint = new Vector3(xRand, raycastBeginPointHeight, zRand);

        RaycastHit[] hits;
        Ray pointGenerationRay = new Ray(rayCastBeginPoint, Vector3.down);

        //Wykonanie spherecast
        hits = Physics.SphereCastAll(rayCastBeginPoint, pointTemplates[0].GetComponent<SphereCollider>().radius, Vector3.down, raycastBeginPointHeight * 2.0f);

        var goodHits = new List<RaycastHit>();

        foreach (var hit in hits)
        {
            foreach (string surf in allowedSurfaces)    //wykluczenie "złych" trafień
            {
                if (hit.collider.tag == surf)
                {
                    goodHits.Add(hit);
                    break;
                }
            }
        }

        //wygenerowanie punktu na losowej wysokości
        return Instantiate(pointTemplates[templateIndex], goodHits[UnityEngine.Random.Range(0, goodHits.Count)].point + offset, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
    }

}

