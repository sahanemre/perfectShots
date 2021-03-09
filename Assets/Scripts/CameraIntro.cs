using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraIntro : MonoBehaviour
{
    private Transform transform1;
    public float duration;
    public Ease animEase;
    void Start()
    {
        transform1 = gameObject.GetComponent<Transform>();

        transform1
            .DOMoveX(3f, duration, false)
            .SetEase(animEase);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
