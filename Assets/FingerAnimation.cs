using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FingerAnimation : MonoBehaviour
{
    public GameObject go;
    public RectTransform SlideFingerTransform;
    public Ease ease;
    public float duration, range;

    void Start()
    {
        StartCoroutine(introCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.DestroyFingerAnim)
        {
            go.SetActive(false);
        }
    }
    void HowToPlayAnimation()
    {
        SlideFingerTransform
            .DOAnchorPos(new Vector3(32, -382, 0), duration)

            .SetEase(ease)
            .SetLoops(-1, LoopType.Restart);
    }

    IEnumerator introCoroutine()
    {


        yield return new WaitForSeconds(3f);
        go.SetActive(true);
        HowToPlayAnimation();

    }
}
