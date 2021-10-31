using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //void Update()
    //{
    //    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    //}

    private void FixedUpdate()
    {
        Vector3 eulerAngleVelocity = Vector3.up * rotationSpeed;
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(deltaRotation * rb.rotation);
    }
}
