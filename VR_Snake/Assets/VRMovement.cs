using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRMovement : MonoBehaviour
{
    private const float vectorResize = 0.01f;
    public float mSpeed = 3.0f;
    public float gravity = 2.0f;
    private bool isMoving = true;
    private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform snakeHead;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //snakeHead.position = playerTransform.position;
            snakeHead.rotation = Quaternion.Euler(0, cameraTransform.localEulerAngles.y, 0);
            Vector3 vForward = snakeHead.transform.forward*vectorResize;
            //snakeHead.position = snakeHead.position + vForward/vectorResize;
            vForward.y = -gravity*vectorResize;
            characterController.Move(vForward*mSpeed);

        }

    }
}
