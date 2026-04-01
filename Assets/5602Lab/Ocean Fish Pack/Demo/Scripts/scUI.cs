using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[System.Serializable]
public class ListRow
{
    public GameObject[] ListPrefabs; // Array of prefabs for a single row
}

public class scUI : MonoBehaviour
{

    public new Camera camera; // The camera that will move
    public float speed = 5f; // Camera movement speed
    public float minY = 0f; // Minimum Y-axis value
    public float maxY = 10f; // Maximum Y-axis value

    private bool moveUp = false; // Whether to move upward
    private bool moveDown = false; // Whether to move downward

    public GameObject spawnEff;
    public GameObject[] prefabs; // Prefabs to be spawned
    public Transform spawnPoint; // Location where the prefab will be spawned
    public float offsetPerInstance = 1.0f; // Distance between each instance


    private int instanceCount = 0;


    public RectTransform ListPage;
    public Transform ListPosition;

    public ListRow[] ListGroup; // Displayed as a 2D array in the Inspector
    public GameObject[] startHideObject;



    // Start is called before the first frame update
    void Start()
    {
        // Set a fixed screen resolution (1280x720)
        Screen.SetResolution(1280, Mathf.RoundToInt(1280 / (16f / 9f)), false);
        ListGroupSpawn(0);

        foreach (GameObject obj in startHideObject)
        {
            obj.SetActive(false);
        }

    }



    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = camera.transform.position;


        if (moveUp && currentPosition.y < maxY)
        {
            // Move the camera upward along the Y-axis
            camera.transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (moveDown && currentPosition.y > minY)
        {
            // Move the camera downward along the Y-axis
            camera.transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    // Called when the upward movement button is pressed
    public void OnMoveUpButtonDown()
    {
        moveUp = true;
    }

    // Called when the upward movement button is released
    public void OnMoveUpButtonUp()
    {
        moveUp = false;
    }

    // Called when the downward movement button is pressed
    public void OnMoveDownButtonDown()
    {
        moveDown = true;
    }

    // Called when the downward movement button is released
    public void OnMoveDownButtonUp()
    {
        moveDown = false;
    }


    public void SpawnPrefab()
    {
        if (prefabs != null && spawnPoint != null)
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            float randomX = Random.Range(-5, 5);
            float randomY = Random.Range(-4, 4);

            float startRotationZ = 0;

            Vector3 randomPosition = new Vector3(randomX, camera.transform.position.y + randomY, 0);

            // Spawn the prefab

            if(randomX > 1)
            {
                startRotationZ = 180;

            }

            Quaternion spawnRotation = Quaternion.Euler(90, 0, startRotationZ); // Apply individual rotation values


            //GameObject newObject = Instantiate(prefabs[randomIndex], spawnPoint.position + Vector3.right * offsetPerInstance * instanceCount, spawnRotation);
            GameObject newObject = Instantiate(prefabs[randomIndex], randomPosition, spawnRotation, spawnPoint);
            GameObject newObject2 = Instantiate(spawnEff, randomPosition, spawnRotation, spawnPoint);

            // Create unique material
            Renderer renderer = newObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(renderer.material); // Instantiate the material
            }

            instanceCount++; // Increase instance count
        }

    }

    public void ListGroupSpawn(int groupNum)
    {


    
    foreach (Transform child in ListPosition.transform)
    {
        Destroy(child.gameObject);
    }


    float gapX = 12f / (ListGroup[groupNum].ListPrefabs.Length-1);

    ListPage.anchoredPosition = new Vector2( -(30*((ListGroup.Length - 1)))/2 + 30*groupNum, 38);

    for (int i = 0; i < ListGroup[groupNum].ListPrefabs.Length; i++)
    {


        GameObject newObject = Instantiate(ListGroup[groupNum].ListPrefabs[i], new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0), ListPosition);
        newObject.transform.localPosition = new Vector3(-6 + gapX*i, 0, 0);
        newObject.transform.localScale = newObject.transform.localScale * 1.5f;
        newObject.GetComponent<scFishMove>().enabled = false;


    }


}



}
