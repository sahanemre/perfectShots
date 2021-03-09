using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 cineMacOffset;
    Vector3 startPos, endPos, direction;
    public float maxForce, forceModifier, force;
    public LayerMask rayLayer;
    public Vector3 ballStartPosition;
    LineRenderer lr;
    TrailRenderer tr;
    Rigidbody rb;

    public GameObject MainCam, ActionCam,introCam;

    public bool isSlideAnimAct;
    public bool canShoot, isThatBall = false;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private Transform virtualCameraTransform;
    private Vector3 startPosCam,startPosEuler;
    private float inputAxisValue,currentAxisValue;

    public float jumpToUpF;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();

        Debug.Log("startPos: " + startPosCam + " startEuler: " + startPosEuler);
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

        //Debug.Log(cinemachineVirtualCamera.transform.eulerAngles);
        rotateTheCamera();
    }

    private void rotateTheCamera()
    {
        if (!isThatBall)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        break;
                    case TouchPhase.Moved:
                        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue += Input.GetAxis("Mouse X");
                        currentAxisValue = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue;
                        break;
                    case TouchPhase.Stationary:
                        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;
                        break;
                    case TouchPhase.Ended:
                        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;
                        break;
                    default:
                        break;
                }

            }
        }
    }

    private void OnMouseDown()
    {
        if (canShoot)
        {
            isThatBall = true;
            GameManager.Instance.isTimeStart = true;
            isSlideAnimAct = false;
            startPos = ClickedPoint();
            lr.enabled = true;
            GameManager.Instance.lrBool(true);
        }
        

        
    }

    public void OnMouseDrag()
    {     
        endPos = ClickedPoint();
        endPos.y = lr.transform.position.y - 0.3f;

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            break;
        //        case TouchPhase.Moved:
        //            if (inputAxisValue >= 4f || inputAxisValue <= -4f)
        //            {
        //                inputAxisValue += Input.GetAxis("Mouse X");
        //                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue += Input.GetAxis("Mouse X");
        //                currentAxisValue = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue;
        //            }
        //            else if (inputAxisValue < 4f || inputAxisValue > 4f)
        //            {
        //                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;
        //                Debug.Log(" < 3: " + inputAxisValue);
        //            }
        //            break;
        //        case TouchPhase.Ended:
        //            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //inputAxisValue += Input.GetAxis("Mouse X");

        //if (inputAxisValue >= 4f || inputAxisValue <= -4f)
        //{
        //    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue += Input.GetAxis("Mouse X");
        //    currentAxisValue = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue;
        //    Debug.Log("> 3: " + inputAxisValue);
        //}
        //else if (inputAxisValue < 4f || inputAxisValue > 4f)
        //{
        //    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisValue = 0;
        //    Debug.Log(" < 3: " + inputAxisValue);
        //}
                        
        

        // lr.SetPosition(1, new Vector3(-endPos.x,transform.position.y - 1,(transform.position.z + force + 3.5f))); 
        lr.SetPosition(1, endPos);
        
        startPos = transform.position;
        force = Mathf.Clamp(Vector3.Distance(startPos, endPos) * forceModifier, 0, maxForce);
        endPos = startPos + ((endPos - startPos).normalized * force);
        
    }

    private void OnMouseUp()
    {
        

        if (canShoot)
        {
            lr.enabled = false;

            GameManager.Instance.lrBool(false);
            GameManager.Instance.restartButtonAnim(true);

            direction = startPos - endPos;
            direction.y = direction.y + force / 1.8f;
            rb.AddForce(direction * force / 2, ForceMode.Impulse);
            Debug.Log("dr: " + direction + " force: " + force);
            GameManager.Instance.playThrowClip();
            //GameManager.Instance.CameraAngle(false, true);
            isThatBall = false;
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

    public void AfterRestartCollider()
    {
        transform.position = ballStartPosition;
        canShoot = true;
        GameManager.Instance.restartButtonAnim(false);
        //rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;

        //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value = 0;

        Debug.Log("startPos: " + startPosCam + " startEuler: " + startPosEuler);
        //GameManager.Instance.CameraAngle(true, false);
        MainCam.SetActive(true);
        ActionCam.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RestartCollider"))
        {
            AfterRestartCollider();
        }

        if (other.gameObject.CompareTag("GoNextLevelColllider"))
        {
            canShoot = true;
            //Handheld.Vibrate();
            GameManager.Instance.isTimeStart = false;
            GameManager.Instance.isLevelFinished = true;
            GameManager.Instance.restartButtonAnim(false);
            StartCoroutine(collisionCoroutine());
            
            //Cam
            //MainCam.SetActive(true);
            //ActionCam.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            GameManager.Instance.playBasketBallBounceClip();
            //Handheld.Vibrate();
        }

        if (collision.gameObject.CompareTag("BouncePlatform"))
        {
            GameManager.Instance.playBasketBallBounceClip();
            GetComponent<SphereCollider>().material.bounceCombine = PhysicMaterialCombine.Maximum;
            GetComponent<SphereCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            GetComponent<SphereCollider>().material.dynamicFriction = 0;
            GetComponent<SphereCollider>().material.staticFriction = 0;
        }

        if (collision.gameObject.CompareTag("jumpToUp"))
        {
            GameManager.Instance.playBasketBallBounceClip();
            rb.AddForce(new Vector3(0,15,0),ForceMode.Impulse);
        }

        //if (collision.gameObject.CompareTag("jumpToLeft"))
        //{
        //    rb.AddForce(new Vector3(-15, 0, 0), ForceMode.Impulse);
        //}

        //if (collision.gameObject.CompareTag("jumpToRight"))
        //{
        //    rb.AddForce(new Vector3(+15, 0, 0), ForceMode.Impulse);
        //}
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
        
        yield return new WaitForSeconds(0.5f);
        //GameManager.Instance.GoNextLevel();
        GameManager.Instance.nextButtonAnim();

        //yield return new WaitForSeconds(0.5f);

        //GameManager.Instance.confetiStop();
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

