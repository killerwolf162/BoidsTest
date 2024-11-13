using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomLocation : MonoBehaviour
{
    private void Awake()
    {
        this.transform.position = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
    }
}
