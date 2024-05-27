using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    float[] heights = { 4, 3, 2, 1, 0, -1, -2, -3, -4 };
    public List<float> currentHeights;
    int maxObjects = 8; // must be at least 1 less than total elements in heights[]
    [SerializeField] int currentObjectCount = 0;
    [SerializeField] Bullet objectToSpawn;
    public List<Bullet> spawnedObjects;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = new List<Bullet>();
        currentHeights = new List<float>();
        currentHeights.AddRange(heights);
        StartCoroutine(SpawnObjects());
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Bullet bullet in spawnedObjects)
        {
            if (bullet.transform.position.x > 8f && bullet.direction == 1)
            {
                bullet.direction = -1;
                bullet.transform.position = new Vector2(bullet.transform.position.x + 6f * bullet.direction * Time.deltaTime,
                    GetRandomY(bullet.transform.position.y));
            }
            else if (bullet.transform.position.x < -8f && bullet.direction == -1)
            {
                bullet.direction = 1;
                bullet.transform.position = new Vector2(bullet.transform.position.x + 6f * bullet.direction * Time.deltaTime,
                    GetRandomY(bullet.transform.position.y));
            }
            else
            {
                bullet.transform.position = new Vector2(bullet.transform.position.x + 6f * bullet.direction * Time.deltaTime, bullet.transform.position.y);
            }
        }
    }

    IEnumerator SpawnObjects()
    {
        while (currentObjectCount < maxObjects)
        {
            float waitTime = Random.Range(2, 6);
            yield return new WaitForSeconds(waitTime);

            Bullet newObject = Instantiate(objectToSpawn, GetRandomPosition(), Quaternion.identity);
            newObject.direction = newObject.transform.position.x < 8f ? 1 : -1;
            spawnedObjects.Add(newObject);
            currentObjectCount++;
        }
    }

    float GetRandomY(float? existingY)
    {
        int roll = Random.Range(0, currentHeights.Count);
        float y = currentHeights[roll];
        currentHeights.RemoveAt(roll);

        if(existingY != null)
            currentHeights.Insert(roll, existingY.Value);

        return y;
    }

    Vector2 GetRandomPosition()
    {
        // Customize this function to generate the position where you want to spawn the objects
        bool chooseNegative = Random.Range(0, 2) == 0; // Randomly choose between 0 and 1
        float x = chooseNegative ? -9f : 9f;
        float y = GetRandomY(null);
        return new Vector2(x, y);
    }
}
