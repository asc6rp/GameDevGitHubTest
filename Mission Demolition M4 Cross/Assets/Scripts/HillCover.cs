using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillCover : MonoBehaviour
{
    [Header("Inscribed")]
    public Sprite[] hillSprites;           
    public int numHills = 20;              
    public Vector3 minPos = new Vector3(-30, 0, 15);    
    public Vector3 maxPos = new Vector3(300, 0, 20);   
    [Tooltip("For scaleRange, x is the min value and y is the max value.")]
    public Vector2 scaleRange = new Vector2(1, 3);    
    public Transform groundTransform;                 

    [Header("Parallax Settings")]
    public float parallaxFactor = 0.05f;   

    private Transform camTransform;        
    private Vector3 previousCamPos;        


    void Start()
    {

        camTransform = Camera.main?.transform;


        if (camTransform == null)
        {
            Debug.LogWarning("Main Camera not found. Make sure there is a camera in the scene tagged as 'MainCamera'.");
        }

        previousCamPos = camTransform != null ? camTransform.position : Vector3.zero;

        Transform parentTrans = this.transform;
        GameObject hillGO;
        Transform hillTrans;
        SpriteRenderer sRend;
        float scaleMult;


        float hillYPos = groundTransform != null ? groundTransform.position.y : minPos.y;

        for (int i = 0; i < numHills; i++)
        {
            
            hillGO = new GameObject("Hill" + i);
            hillTrans = hillGO.transform;
            sRend = hillGO.AddComponent<SpriteRenderer>();

       
            int spriteNum = Random.Range(0, hillSprites.Length);
            sRend.sprite = hillSprites[spriteNum];

   
            hillTrans.position = RandomPos(hillYPos);
            hillTrans.SetParent(parentTrans, true);

   
            scaleMult = Random.Range(scaleRange.x, scaleRange.y);
            hillTrans.localScale = Vector3.one * scaleMult;

          
            sRend.sortingOrder = -2;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camTransform == null) return;  

        Vector3 camDelta = camTransform.position - previousCamPos;


        foreach (Transform hill in transform)
        {
            float zFactor = 1 - Mathf.Abs(hill.position.z) * parallaxFactor;
            hill.position += new Vector3(camDelta.x * zFactor, 0, 0);  
        }

        
        previousCamPos = camTransform.position;
    }

    
    Vector3 RandomPos(float groundY)
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(minPos.x, maxPos.x);
        pos.y = groundY;  
        pos.z = Random.Range(minPos.z, maxPos.z);  
        return pos;
    }
}
