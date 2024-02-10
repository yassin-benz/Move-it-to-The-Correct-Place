/*TO-DO :
 * Make a Better/Faster Random Spawn on the SpawnTimer() Method
 * Destroy the spawned Face if the posCounter == 0 and the time run out
 * Destroy the spawned Face if the Face goes on top of another FACE
 * Display the Next Side Pos
 * spawnTimer goes faste by the time
 * replace the old Input by the New Input and Integrate the Mobile Touches
 * Reward the Player each time completed a face with diffrente levels
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] left_top_Faces = new GameObject[4];
    [SerializeField] private GameObject[] left_bottom_Faces = new GameObject[4];
    [SerializeField] private GameObject[] right_top_Faces = new GameObject[4];
    [SerializeField] private GameObject[] right_bottom_Faces = new GameObject[4];
    [SerializeField] private Transform[] spawnPos = new Transform[4];
    [SerializeField] private float spawnTimer = 2f; // responsible of the time between each spawn

    private GameObject spawnedObject = null;
    private List<GameObject> spawnedObject_up = new List<GameObject>(); //storing the object that are on the top square
    private List<GameObject> spawnedObject_down = new List<GameObject>(); //storing the object that are on the down square
    private List<GameObject> spawnedObject_right = new List<GameObject>(); //storing the object that are on the right square
    private List<GameObject> spawnedObject_left = new List<GameObject>(); //storing the object that are on the left square

    //storing the Target position of each face
    private readonly Vector2 upPos = new Vector2(0, 2.5f); 
    private readonly Vector2 downPos = new Vector2(0, -2.5f);
    private readonly Vector2 rightPos = new Vector2(2.5f, 0);
    private readonly Vector2 leftPos = new Vector2(-2.5f, 0);

    //storing the number of spawned object per side {cuz can't be over 4 sides per face in the game}
    private int left_topCount = 0;
    private int left_downCount = 0; 
    private int right_topCount = 0; 
    private int right_downCount = 0;

    private int posCounter = 0; //responsible for Locking the player Input if he perform a Move

    private void Start()
    {
        //Start Spawning after a given amount of time
        StartCoroutine(SpawnTimer(spawnTimer));
    }

    private void Update()
    {
        //Move the spawned face by player input
        if(spawnedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveUnite(upPos, spawnedObject_up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveUnite(downPos, spawnedObject_down);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveUnite(rightPos, spawnedObject_right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveUnite(leftPos, spawnedObject_left);
            }
        }
    }
    // Summary:
    //     Move the spawned face to the target direction
    //     and add it to the list that is responsible for that direction to store it
    // Parameters:
    //   direction : target position
    //   spawnedObjectList : responsible of a specific position 
    private void MoveUnite(Vector2 direction,List<GameObject> spawnedObjectList)
    {
        if (posCounter == 0)
        {
            spawnedObject.transform.position += (Vector3)direction;
            spawnedObjectList.Add(spawnedObject);
            posCounter = 1;
        }
        //clear the square if is full after moving the face
        ClearArea();
    }
    // Summary:
    //     call the SpawnFace method for a random Face side ,foreach given time
    //     and reset the posCounter to unlock Unite Movement
    // Parameters:
    //   timeForNextSpawn : time of the next spawn 
    private IEnumerator SpawnTimer(float timeForNextSpawn)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(timeForNextSpawn);
            int rndFace = UnityEngine.Random.Range(0,4);
            SpawnFace(rndFace);
            posCounter = 0;
        }
    }
    // Summary:
    //     perform the spawn action for a given face index
    // Parameters:
    //   face : face index
    private void SpawnFace(int face)
    {
        if (face < 0 || face>3) return;

        if (face==0)
        {
            SpawnLeftTop();
        }
        if (face == 1)
        {
            SpawnLeftBottom();
        }
        if (face == 2)
        {
            SpawnRightTop();
        }
        if (face == 3)
        {
            SpawnRightBottom();
        }
    }
    // Summary:
    //     spawn the Left Top Side of a face randomly
    //     and increase the left_topCounter,
    //     and return if it goes beyond 4
    private void SpawnLeftTop()
    {
        if (left_topCount >= 4) return;
        int rndFace = UnityEngine.Random.Range(0, 4);
        spawnedObject = Instantiate(left_top_Faces[rndFace], spawnPos[0].position,Quaternion.identity);
        left_topCount++;
    }
    // Summary:
    //     spawn the Left Bottom Side of a face randomly
    //     and increase the left_BottomCounter,
    //     and return if it goes beyond 4
    private void SpawnLeftBottom()
    {
        if(left_downCount>= 4) return;
        int rndFace = UnityEngine.Random.Range(0, 4);

        spawnedObject = Instantiate(left_bottom_Faces[rndFace], spawnPos[1].position, Quaternion.identity);
        left_downCount++;
    }
    // Summary:
    //     spawn the Right Top Side of a face randomly
    //     and increase the Right_TopCounter,
    //     and return if it goes beyond 4
    private void SpawnRightTop()
    {
        if (right_topCount >= 4) return;
        int rndFace = UnityEngine.Random.Range(0, 4);

        spawnedObject = Instantiate(right_top_Faces[rndFace], spawnPos[2].position, Quaternion.identity);
        right_topCount++;
    }
    // Summary:
    //     spawn the Right Bottom Side of a face randomly
    //     and increase the Right_BottomCounter,
    //     and return if it goes beyond 4
    private void SpawnRightBottom()
    {
        if(right_downCount>= 4) return;
        int rndFace = UnityEngine.Random.Range(0, 4);

        spawnedObject = Instantiate(right_bottom_Faces[rndFace], spawnPos[3].position, Quaternion.identity);
        right_downCount++;
    }
    // Summary:
    //     Clear the square area that has a complete face 
    private void ClearArea()
    {
        if (spawnedObject_up.Count >= 4)
        {
            InitializeList(spawnedObject_up);
        }
        if (spawnedObject_down.Count >= 4)
        {
            InitializeList(spawnedObject_down);
        }
        if (spawnedObject_right.Count >= 4)
        {
            InitializeList(spawnedObject_right);
        }
        if (spawnedObject_left.Count >= 4)
        {
            InitializeList(spawnedObject_left);
        }
    }
    // Summary:
    //     Destroy the object that are on that area AKA listOfFaceSides
    //     and reset the list to the empty state
    //     and decrise the side counter by 1 for each side
    // Parameters:
    //   listOfFaceSides : a list that contain the all the sides of a face for a specific square area
    private void InitializeList(List<GameObject> listOfFaceSides)
    {
        int _length = listOfFaceSides.Count;

        for (int i = 0; i < _length; i++)
        {
            //TO-DO : player gain points

            Destroy(listOfFaceSides[i].gameObject);
        }
        listOfFaceSides.Clear();
        listOfFaceSides.Capacity = 0;

        left_downCount -= 1;
        right_downCount -= 1;
        left_topCount -= 1;
        right_topCount -= 1;
    }
}
