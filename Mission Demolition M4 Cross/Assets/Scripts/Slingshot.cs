using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    [Header("LineRenderer")]
    private LineRenderer lineRenderer;
    public GameObject leftArm;
    public GameObject rightArm;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        leftArm = transform.Find("LeftArm").gameObject;
        rightArm = transform.Find("RightArm").gameObject;
    }
    void OnMouseEnter()
    {
        /*print("Slingshot:OnMouseEnter()");*/
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);
        /*print("Slingshot:OnMouseExit()");*/
    }

    void OnMouseDown()
    {
        //the player pressed the mouse over slingshot
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(projectilePrefab) as GameObject;
        //start it at the launchPoint
        projectile.transform.position = launchPos; 
        //set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;

        lineRenderer.enabled = true;
    }

    void Update()
    {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) 
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        lineRenderer.SetPosition(0, leftArm.transform.position);
        lineRenderer.SetPosition(1, projectile.transform.position);
        lineRenderer.SetPosition(2, rightArm.transform.position);

        if (Input.GetMouseButtonUp(0)) 
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            lineRenderer.enabled = false;
            MissionDemolition.SHOT_FIRED();
        }
    }
}
