using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FingerAnimation : MonoBehaviour
{
    public GameObject go;
    private GameObject cam;
    public RectTransform SlideFingerTransform, CamSlideFingerTransform;
    public Ease ease;
    public float duration, range;

    void Start()
    {
        StartCoroutine(introCoroutine());

        cam = GameObject.Find("Camera360");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.DestroyFingerAnim)
        {
            

            if (cam != null)
            {
                cam.SetActive(false);
            }
            else
            {
                go.SetActive(false);
            }
            
        }
    }

    void HowToChangeCamAnimation()
    {
        CamSlideFingerTransform
            .DOAnchorPos(new Vector3(100, -44, 0), duration)

            .SetEase(ease)
            .SetLoops(-1,LoopType.Yoyo);
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
        

        if (cam != null)
        {
            cam.SetActive(true);
            HowToChangeCamAnimation();
        }
        else
        {
            go.SetActive(true);
        }

        HowToPlayAnimation();
        
    }
}
