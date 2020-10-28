using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSizer : MonoBehaviour
{
    public float minSize = 0.8f;
    public float maxSize = 1.2f;
    void Start()
    {
        float rnd = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(transform.localScale.x * rnd, transform.localScale.y * rnd, transform.localScale.z * rnd);
    }
}
