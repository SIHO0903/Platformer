using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float moveSpeed = 0.1f;
    private Material material;

    private float distance;
    private Vector3 cameraStartPosition;
    void Awake()
    {
        material = GetComponent<Renderer>().material;

        cameraStartPosition = transform.parent.transform.position;

    }

    void Update()
    {
        distance = transform.parent.transform.position.x - cameraStartPosition.x;
        material.SetTextureOffset("_MainTex", new Vector2(distance,0) * moveSpeed);
    }
}
