using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    public GameObject ballController;
    LineRenderer lr;
    

    public float power;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        //ballController = FindObjectOfType<BallController>();
        //ballController = GetComponent<BallController>();
        //power = ballController.force;
        lr.sortingOrder = 1;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.SetPosition(0,new Vector3(ballController.transform.position.x+1.5f, ballController.transform.position.y, ballController.transform.position.z));
       
    }

    // Update is called once per frame
    void Update()
    {
        //power = ballController.force;
        Debug.Log(GameManager.Instance.power);

        //lr.SetPosition(1, transform.position*GameManager.Instance.power);
        lr.SetPosition(1, new Vector3(1.5f, ballController.transform.position.y + GameManager.Instance.power, ballController.transform.position.z));
    }
}
