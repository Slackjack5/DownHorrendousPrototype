using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction { Left, Right };

    private readonly float debugRayLength = 2f;

    private float timeMoving = 0f;
    private bool isMoving;
    private float distanceToGround;
    private float startingY;
    private bool canGoUp = true;

    private Vector3 eulerAngleVelocity;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        distanceToGround = capsuleCollider.bounds.extents.y;
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
        //rb.AddForce(transform.forward * (maxSpeed / 50f), ForceMode.VelocityChange);
        Vector3 deltaPosition = transform.forward * maxSpeed;
        //if (Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.max.y - 0.5f, capsuleCollider.bounds.max.z) , new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.min.y + 0.4f, capsuleCollider.bounds.min.z), 1f))
        //{
        //    //deltaPosition.y = Physics.gravity.y / Time.fixedDeltaTime;
        //}
        if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.01f))
        {
            if (transform.position.y > startingY + 0.01f)
            {
                if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.5f))
                {
                    canGoUp = false;
                    deltaPosition.y = Physics.gravity.y / 2f;
                }else if (canGoUp)
                {
                    deltaPosition.y = -Physics.gravity.y / 2f;
                }
                else
                {
                    deltaPosition.y = Physics.gravity.y / 2f;
                }
            }
            else
            {
                deltaPosition.y = Physics.gravity.y / 2f;
            }
        }
        else
        {
            startingY = transform.position.y;
            canGoUp = true;
        }
        rb.velocity = Vector3.Slerp(Vector3.zero, deltaPosition, timeMoving / InputManager.Slipperiness);
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
                    Vector3 deltaPosition = transform.forward * maxSpeed;
                    if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f))
                    {
                        if (transform.position.y > startingY + 0.1f)
                        {
                            if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.5f))
                            {
                                canGoUp = false;
                                deltaPosition.y = Physics.gravity.y / 2f;
                            }
                            else if (canGoUp)
                            {
                                deltaPosition.y = -Physics.gravity.y / 2f;
                            }
                            else
                            {
                                deltaPosition.y = Physics.gravity.y / 2f;
                            }
                        }
                        else
                        {
                            deltaPosition.y = Physics.gravity.y / 2f;
                        }
                    }
                    else
                    {
                        startingY = transform.position.y;
                        canGoUp = true;
                    }
                    rb.velocity = Vector3.Slerp(Vector3.zero, deltaPosition, timeMoving / InputManager.Slipperiness);
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
