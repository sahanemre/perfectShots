using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Vector3 startPos, endPos, direction;
    public float maxForce, forceModifier, force;
    public LayerMask rayLayer;
    Vector3 ballStartPosition;
    LineRenderer lr;
    TrailRenderer tr;
    Rigidbody rb;

    public GameObject MainCam, ActionCam,introCam;

    public bool isSlideAnimAct;
    public bool canShoot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        //tr = GetComponent<TrailRenderer>();

        lr.enabled = false;
        ballStartPosition = transform.position;

        canShoot = true;
        //ArrowFingerAnimation
        isSlideAnimAct = true;
        //CameraAngle
        StartCoroutine(levelIntro());

    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
    }

    private void OnMouseDown()
    {
        if (canShoot)
        {
            GameManager.Instance.isTimeStart = true;
            isSlideAnimAct = false;
            startPos = ClickedPoint();
            lr.enabled = true;
        }
        

        
    }

    public void OnMouseDrag()
    {
        endPos = ClickedPoint();
        endPos.y = lr.transform.position.y - 0.3f;

        // lr.SetPosition(1, new Vector3(-endPos.x,transform.position.y - 1,(transform.position.z + force + 3.5f))); 
        lr.SetPosition(1, endPos);
        
        startPos = transform.position;
        force = Mathf.Clamp(Vector3.Distance(startPos, endPos) * forceModifier, 0, maxForce);
        endPos = startPos + ((endPos - startPos).normalized * force);
        Debug.Log(endPos);
    }

    private void OnMouseUp()
    {
        

        if (canShoot)
        {
            lr.enabled = false;

            direction = startPos - endPos;
            direction.y = direction.y + force / 1.8f;
            rb.AddForce(direction * force / 2, ForceMode.Impulse);
            Debug.Log("dr: " + direction + " force: " + force);
            GameManager.Instance.playThrowClip();
            //GameManager.Instance.CameraAngle(false, true);

            //cam
            MainCam.SetActive(false);
            ActionCam.SetActive(true);

        }

        canShoot = false;


       
    }
    Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            position = hit.point;
        }
        return position;
    }


    #region CollisionAndTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RestartCollider"))
        {
            transform.position = ballStartPosition;
            canShoot = true;
            //rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints = RigidbodyConstraints.None;


            //GameManager.Instance.CameraAngle(true, false);
            MainCam.SetActive(true);
            ActionCam.SetActive(false);
        }

        if (other.gameObject.CompareTag("GoNextLevelColllider"))
        {
            canShoot = true;
            GameManager.Instance.isTimeStart = false;
            GameManager.Instance.isLevelFinished = true;
            StartCoroutine(collisionCoroutine());
            


            //Cam
            MainCam.SetActive(true);
            ActionCam.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            GameManager.Instance.playBasketBallBounceClip();
            
        }

        if (collision.gameObject.CompareTag("BouncePlatform"))
        {
            GameManager.Instance.playBasketBallBounceClip();
            GetComponent<SphereCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum;
            GetComponent<SphereCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            GetComponent<SphereCollider>().material.dynamicFriction = 0;
            GetComponent<SphereCollider>().material.staticFriction = 0;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        GetComponent<SphereCollider>().material.bounceCombine = PhysicMaterialCombine.Average;
        GetComponent<SphereCollider>().material.frictionCombine = PhysicMaterialCombine.Average;
        GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
        GetComponent<SphereCollider>().material.staticFriction = 0.6f;
    }

    IEnumerator collisionCoroutine()
    {
        GameManager.Instance.playBasketClip();

        yield return new WaitForSeconds(0.4f);

        rb.constraints = RigidbodyConstraints.FreezeAll;
        GameManager.Instance.playWinClip();

        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.confetiStop();
        yield return new WaitForSeconds(0.5f);
        //GameManager.Instance.GoNextLevel();
        GameManager.Instance.nextButtonAnim();
    }

    #endregion


    IEnumerator levelIntro()
    {
        introCam.SetActive(true);
        MainCam.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        introCam.SetActive(false);
        MainCam.SetActive(true);

    }
}
