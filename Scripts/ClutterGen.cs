using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutterGen : MonoBehaviour
{
    public int clutterCount = 100;
    public int x = 500;
    public int z = 500;
    public float minHeight = 30f;
    public float maxHeight = 30f;

    public GameObject[] landPrefabs;
    public GameObject[] waterPrefabs;
    public GameObject[] nodes;
    public bool[] n;

    void Start()
    {
        float height = (float) GameObject.FindGameObjectWithTag("Water").GetComponent<WaterLevel>().waterHeight;
        nodes = new GameObject[clutterCount];
        n = new bool[clutterCount];

        for(int i = 0; i < clutterCount; i++) {
            Vector3 pos = new Vector3(Random.Range(-x+10, x-10), 1000, Random.Range(-z+10, z-10));
            RaycastHit hit;

            if (Physics.Raycast(pos, Vector3.down, out hit))
            {
                if(hit.point.y > minHeight && hit.point.y < maxHeight) {
                    GameObject obj = Instantiate(landPrefabs[Random.Range(0, landPrefabs.Length)], hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
                    if(height > hit.point.y) {
                        Destroy(obj);
                        n[i] = false;
                        obj = Instantiate(waterPrefabs[Random.Range(0, waterPrefabs.Length)], hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
                    }else {
                        n[i] = true;
                    }
                    nodes[i] = obj;
                }
            }
        }
    }

    void Update() {
        float height = (float) GameObject.FindGameObjectWithTag("Water").GetComponent<WaterLevel>().waterHeight;
        for(int i = 0; i < clutterCount; i++) {
            if(nodes[i] && height > nodes[i].transform.position.y && n[i]) {
                GameObject obj = Instantiate(waterPrefabs[Random.Range(0, waterPrefabs.Length)], nodes[i].transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
                Destroy(nodes[i]);
                nodes[i] = obj;
                n[i] = false;
            }
        }
    }
}
