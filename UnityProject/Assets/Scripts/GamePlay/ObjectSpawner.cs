using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> SpawnPrefabs = new List<GameObject>();

    [SerializeField] private List<Rigidbody> AllFallingObjects = new List<Rigidbody>();

    [SerializeField] private float MaxFallSpeed;

    public static ObjectSpawner instance;

    public static ObjectSpawner Instance
    {
        get { return instance; }
    }


    private void Awake()
    {
        if (instance != null && instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        StartCoroutine(spawnInterval(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
       /* foreach (Rigidbody item in AllFallingObjects)
        {
            item.linearVelocity = Vector3.ClampMagnitude(item.linearVelocity, MaxFallSpeed);
            Debug.Log(item.linearVelocity);
        }*/
    }

    public void AddObjectToList(Rigidbody rb)
    {
        AllFallingObjects.Add(rb);
    }

    public void RemoveObjectFromList(Rigidbody rb)
    {
        AllFallingObjects.Remove(rb);
    }

    public void SpawnObject()
    {
        int indexToSpawn = Random.Range(0, SpawnPrefabs.Count);
        int scale = Random.Range(0, SpawnPrefabs.Count);

        GameObject go = Instantiate(SpawnPrefabs[indexToSpawn], RandomPositionSpawn(), RandomRotation());
        go.name = SpawnPrefabs[indexToSpawn].name+"Clone";
        ObjectSpawner.instance.AddObjectToList(go.GetComponent<Rigidbody>());
        go.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));

    }

    public Vector3 RandomPositionSpawn()
    {
        return new Vector3(Random.Range(-5,5), 10, 0);
    }

    public Quaternion RandomRotation()
    {
        Vector3 rot = new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));

        return Quaternion.Euler(rot);

    }

    IEnumerator spawnInterval(float t)
    {
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(t);
        }
    }
}
