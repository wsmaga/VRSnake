using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum utworzony żeby ułatwić odwoływanie się do tablic markerów
//poszczególne wartości odwołują się do poszczególnych markerów
//UL - upper left
//UR - upper right
//BL - bottom left
//BR - bottom right

//aby odwołać się trzeba zrzutować enum na inta 
//np. tablica[(int)d.UL]
enum d{ UL=0,UR=1,BL=2,BR=3}

public class TrailGenerator : MonoBehaviour
{
    
    private Mesh mesh;
    private MeshCollider meshCollider; 
    private Transform[] headTransforms; //aktualne współrzędne markerów
    private Vector3[] lastPoints; //poprzednie współrzędne markerów
    private List<Vector3> vertices; //współrzędne wierzchołków
    private List<int> triangles; //numeracja wierzchołków
    private int currTriangleNo; //aktualny numer wierzchołka trójkąta (rośnie o 1 przy tworzeniu vertice)
    [SerializeField]private GameObject player;
    [SerializeField]private float treshold = 0.5f; //jak duży ma byc dystans pomiędzy starą lokacją a nową żeby stwierdzić że gracz się rusza
    private bool isGenerating; //zmienna przechowująca informacje czy ma generować scieżkę czy nie

    //przed funkcją Start
    private void Awake()
    {
        isGenerating = true;
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        headTransforms = new Transform[4];
        lastPoints = new Vector3[4];

        //wyszukanie a następnie markerów w dzieciach obiektu
        Transform mp = player.transform.Find("PlayerHead").transform.Find("MeshPoints");
        headTransforms[(int)d.UL] = mp.Find("MeshPointUL").transform;
        headTransforms[(int)d.UR] = mp.Find("MeshPointUR").transform;
        headTransforms[(int)d.BL] = mp.Find("MeshPointBL").transform;
        headTransforms[(int)d.BR] = mp.Find("MeshPointBR").transform;


        //dodanie markerów do mesha żeby wygenerować początek ścieżki
        vertices = new List<Vector3>();
        vertices.Add(headTransforms[(int)d.UL].position);
        vertices.Add(headTransforms[(int)d.UR].position);
        vertices.Add(headTransforms[(int)d.BL].position);
        vertices.Add(headTransforms[(int)d.UR].position);
        vertices.Add(headTransforms[(int)d.BR].position);
        vertices.Add(headTransforms[(int)d.BL].position);

        //numerowanie wierzchołków
        triangles = new List<int>() { 0, 1, 2, 3, 4, 5 };
        currTriangleNo = 5;

        SetLastPoints();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        GenerateMesh();
        InvokeRepeating("GenerateTrail", 0.5f, 0.3f);
    }

    //funkcja dodająca nowe trójkaty do tablic verices i triangles
    void GenerateTrail()
    {
        if (CheckIfMoved() && isGenerating)
        {
            //lewa ścianka
            {
                //górny trójkąt lewa ścianka
                vertices.Add(headTransforms[(int)d.UL].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.UL]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.BL]);
                triangles.Add(currTriangleNo += 1);


                //dolny trójkąt lewa ścianka
                vertices.Add(headTransforms[(int)d.UL].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.BL]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.BL].position);
                triangles.Add(currTriangleNo += 1);
            }

            //prawa ścianka
            {
                //górny trójkąt prawa ścianka
                vertices.Add(lastPoints[(int)d.BR]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.UR]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.UR].position);
                triangles.Add(currTriangleNo += 1);

                //dolny trójkąt prawa ścianka
                vertices.Add(headTransforms[(int)d.BR].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.BR]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.UR].position);
                triangles.Add(currTriangleNo += 1);
            }

            //górna ścianka
            {
                //lewy trójkąt górna ścianka
                vertices.Add(headTransforms[(int)d.UL].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.UR].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.UL]);
                triangles.Add(currTriangleNo += 1);


                //prawy trójkąt górna ścianka
                vertices.Add(headTransforms[(int)d.UR].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.UR]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.UL]);
                triangles.Add(currTriangleNo += 1);
            }

            //dolna ścianka
            {
                //lewy trójkąt dolna ścianka
                vertices.Add(lastPoints[(int)d.BL]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.BR].position);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.BL].position);
                triangles.Add(currTriangleNo += 1);


                //prawy trójkąt dolna ścianka
                vertices.Add(lastPoints[(int)d.BL]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(lastPoints[(int)d.BR]);
                triangles.Add(currTriangleNo += 1);
                vertices.Add(headTransforms[(int)d.BR].position);
                triangles.Add(currTriangleNo += 1);
            }


            GenerateMesh();
        }
        else
            Debug.Log("Player didn't moved, ignoring drawing mesh");
        SetLastPoints();
    }

    //funkcja czyszcząca starego mesha i generująca nowego mesha na podstawie aktualnych tablic
    void GenerateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    //funkcja przypisująca aktualne wartości do tablicy lastPoints
    private void SetLastPoints()
    {
        lastPoints[(int)d.UL] = headTransforms[(int)d.UL].position;
        lastPoints[(int)d.UR] = headTransforms[(int)d.UR].position;
        lastPoints[(int)d.BL] = headTransforms[(int)d.BL].position;
        lastPoints[(int)d.BR] = headTransforms[(int)d.BR].position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //funkcja sprawdzająca czy gracz poruszył się od ostatniego wywołania funkcji GenerateTrail
    private bool CheckIfMoved()
    {
        
        if (Mathf.Abs(Vector3.Distance(headTransforms[0].position, lastPoints[0])) <= treshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[1].position, lastPoints[1])) <= treshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[2].position, lastPoints[2])) <= treshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[3].position, lastPoints[3])) <= treshold)
            return false;
        else
            return true;
    }

    //funkcje zatrzymujące i wznawiające generowanie ścieżki
    public void StopGenerating()
    {
        isGenerating = false;
        CancelInvoke("GenerateTrail");
    }
    public void StartGenerating()
    {
        isGenerating = true;
        InvokeRepeating("GenerateTrail", 0f, 0.3f);
    }
}
