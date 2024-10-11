using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCover : MonoBehaviour
{
    [Header("Inscribed")]
    public Sprite[] cloudSprites;
    public int numClouds = 40;
    public Vector3 minPos = new Vector3(-30,-5,-5);
    public Vector3 maxPos = new Vector3(300, 40, 5);
    [Tooltip("For scaleRange, x is the min values and y is the max value.")]
    public Vector2 scaleRange = new Vector2(1, 4);

    [Header("Parrallax Settings")]
    public float parallaxFactor = 0.1f;

    private Transform camTransform;
    private Vector3 previousCamPos;
    
    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        previousCamPos = camTransform.position;

        Transform parentTrans = this.transform;
        GameObject cloudGO;
        Transform cloudTrans;
        SpriteRenderer sRend;
        float scaleMult;
        for(int i = 0; i < numClouds; i++)
        {
            //Create a new GameObject from scratch and get its transform
            cloudGO = new GameObject();
            cloudTrans = cloudGO.transform;
            sRend = cloudGO.AddComponent<SpriteRenderer>();

            int spriteNum = Random.Range(0, cloudSprites.Length);
            sRend.sprite = cloudSprites[spriteNum];

            cloudTrans.position = RandomPos();
            cloudTrans.SetParent(parentTrans, true);

            scaleMult = Random.Range(scaleRange.x, scaleRange.y);
            cloudTrans.localScale = Vector3.one * scaleMult;

            sRend.sortingOrder = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camDelta = camTransform.position - previousCamPos;

        foreach(Transform cloud in transform)
        {
            float zFactor = 1 - Mathf.Abs(cloud.position.z) * parallaxFactor;
            cloud.position += camDelta * zFactor;
        }

        previousCamPos = camTransform.position;
    }

    Vector3 RandomPos()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(minPos.x, maxPos.x);
        pos.y = Random.Range(minPos.y, maxPos.y);
        pos.z = Random.Range(minPos.z, maxPos.z);
        return pos;
    }
}
