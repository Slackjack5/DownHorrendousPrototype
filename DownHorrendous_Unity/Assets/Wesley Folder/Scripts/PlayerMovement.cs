using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction { Left, Right };

    private readonly float debugRayLength = 2f;

    private float timeMoving = 0f;
    private bool isMoving;

    private Rigidbody rb;
    private Vector3 eulerAngleVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (InputManager.GizmosAreDisplayed)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * debugRayLength;
            Debug.DrawRay(transform.position, forward, Color.red);
        }
    }

    public void Rotate(Direction direction, float speed)
    {
        switch (direction)
        {
            case Direction.Left:
                eulerAngleVelocity = Vector3.up * -speed;
                break;
            case Direction.Right:
                eulerAngleVelocity = Vector3.up * speed;
                break;
        }
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    public void MoveForward(float maxSpeed)
    {
        isMoving = true;
        if (timeMoving < InputManager.Slipperiness)
        {
            timeMoving += Time.fixedDeltaTime;
        }
        rb.velocity = Vector3.Slerp(Vector3.zero, transform.forward * maxSpeed, timeMoving / InputManager.Slipperiness);
    }

    public IEnumerator Decelerate(float maxSpeed)
    {
        isMoving = false;
        while (true)
        {
            if (!isMoving)
            {
                if (timeMoving > 0f)
                {
                    timeMoving -= Time.fixedDeltaTime;
                    rb.velocity = Vector3.Slerp(Vector3.zero, transform.forward * maxSpeed, timeMoving / InputManager.Slipperiness);
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    timeMoving = 0f;
                    rb.velocity = Vector3.zero;
                    yield break;
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
