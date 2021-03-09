using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    private Transform ball;
    LineRenderer lr;
    void Start()
    {

        ball = GameObject.Find("Basketball").transform;
        lr = GetComponent<LineRenderer>();

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector3(0,48.5f,6));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
