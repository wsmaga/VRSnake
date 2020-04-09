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
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceTreshold = 0.5f; //jak duży ma byc dystans pomiędzy starą lokacją a nową żeby stwierdzić że gracz się rusza
    [SerializeField] private float timeToNextUpdate = 0.3f;
    [SerializeField] private uint updatesBeforeGap = 15;
    [SerializeField] private uint updatesAfterGap = 3;
    private bool isGenerating; //zmienna przechowująca informacje czy ma generować scieżkę czy nie
    private Coroutine trailGenerationCoroutine;

    //przed funkcją Start
    private void Awake()
    {
        isGenerating = true;
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        headTransforms = new Transform[4];
        lastPoints = new Vector3[4];
        currTriangleNo = -1;
        //wyszukanie a następnie przypisanie markerów mesha w dzieciach obiektu
        Transform mp = player.transform.Find("PlayerHead").transform.Find("MeshPoints");
        headTransforms[(int)d.UL] = mp.Find("MeshPointUL").transform;
        headTransforms[(int)d.UR] = mp.Find("MeshPointUR").transform;
        headTransforms[(int)d.BL] = mp.Find("MeshPointBL").transform;
        headTransforms[(int)d.BR] = mp.Find("MeshPointBR").transform;


        vertices = new List<Vector3>();
        triangles = new List<int>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        trailGenerationCoroutine=StartCoroutine(TrailGenerationCoroutine());

    }

    //funkcja dodająca nowe trójkaty do tablic verices i triangles
    void GenerateTrailPart()
    {
        if (isGenerating)
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
            UpdateMesh();
            SetLastPoints();
        }
        
    }

    //funkcja czyszcząca starego mesha i generująca nowego mesha na podstawie aktualnych tablic
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
      
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
    
    //funkcja tworząca początkową ściankę ogona
    private void GenerateFirstPlane()
    {
      
        vertices.Add(headTransforms[(int)d.UL].position);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(headTransforms[(int)d.UR].position);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(headTransforms[(int)d.BL].position);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(headTransforms[(int)d.UR].position);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(headTransforms[(int)d.BR].position);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(headTransforms[(int)d.BL].position);
        triangles.Add(currTriangleNo += 1);
        SetLastPoints();
        UpdateMesh();
    }

    //funkcja tworząca końcową ściankę ogona
    private void GenerateLastPlane()
    {
        vertices.Add(lastPoints[(int)d.BL]);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(lastPoints[(int)d.UR]);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(lastPoints[(int)d.UL]);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(lastPoints[(int)d.BL]);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(lastPoints[(int)d.BR]);
        triangles.Add(currTriangleNo += 1);
        vertices.Add(lastPoints[(int)d.UR]);
        triangles.Add(currTriangleNo += 1);
        UpdateMesh();
    }
    //korutyna generująca ogon gracza
    IEnumerator TrailGenerationCoroutine()
    {
        while(isGenerating)
        {
            Debug.Log("COROUTINE:Generate first plane");
            GenerateFirstPlane();
            yield return new WaitForSeconds(timeToNextUpdate);
            for (int i = 0; i < updatesBeforeGap; i++)
            {
                GenerateTrailPart();
                yield return new WaitForSeconds(timeToNextUpdate);
            }
            Debug.Log("COROUTINE:Generate last plane");
            GenerateLastPlane();
            yield return new WaitForSeconds(timeToNextUpdate*updatesAfterGap);
        }
    }

    //funkcje zatrzymujące i wznawiające generowanie ścieżki
    public void StopGenerating()
    {
      
        StopCoroutine(trailGenerationCoroutine);
        GenerateLastPlane();
        isGenerating = false;
    }
    public void StartGenerating()
    {
        GenerateFirstPlane();
        trailGenerationCoroutine=StartCoroutine(TrailGenerationCoroutine());
        isGenerating = true;
    }
    public bool IsGenerating() { return isGenerating; }


    //_DEPRECATED funkcja sprawdzająca czy gracz poruszył się od ostatniego wywołania funkcji GenerateTrailPart
    //przestałem używać bo robi problemy jak gra przylaguje a nie optymalizuje prawie nic
    private bool CheckIfMoved()
    {

        if (Mathf.Abs(Vector3.Distance(headTransforms[0].position, lastPoints[0])) <= distanceTreshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[1].position, lastPoints[1])) <= distanceTreshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[2].position, lastPoints[2])) <= distanceTreshold &&
            Mathf.Abs(Vector3.Distance(headTransforms[3].position, lastPoints[3])) <= distanceTreshold)
            return false;
        else
            return true;
    }
}
