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
    private bool shouldBeUpright = true;

    private Vector3 eulerAngleVelocity;

    private Player player;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        player = GetComponent<Player>();
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

    private void FixedUpdate()
    {
        if (shouldBeUpright)
        {
            StayUpright();
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

    //public void MoveForward(float maxSpeed) OLD Version of Movement
    //{
    //    isMoving = true;
    //    if (timeMoving < InputManager.Slipperiness)
    //    {
    //        timeMoving += Time.fixedDeltaTime;
    //    }
    //    //rb.AddForce(transform.forward * (maxSpeed / 50f), ForceMode.VelocityChange);
    //    Vector3 deltaPosition = transform.forward * maxSpeed;
    //    //if (Physics.CheckCapsule(new Vector3(capsuleCollider.bounds.max.x, capsuleCollider.bounds.max.y - 0.5f, capsuleCollider.bounds.max.z) , new Vector3(capsuleCollider.bounds.min.x, capsuleCollider.bounds.min.y + 0.4f, capsuleCollider.bounds.min.z), 1f))
    //    //{
    //    //    //deltaPosition.y = Physics.gravity.y / Time.fixedDeltaTime;
    //    //}
    //    if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.01f))
    //    {
    //        if (transform.position.y > startingY + 0.01f)
    //        {
    //            if (!Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.5f))
    //            {
    //                canGoUp = false;
    //                deltaPosition.y = Physics.gravity.y / 2f;
    //            }else if (canGoUp)
    //            {
    //                deltaPosition.y = -Physics.gravity.y / 2f;
    //            }
    //            else
    //            {
    //                deltaPosition.y = Physics.gravity.y / 2f;
    //            }
    //        }
    //        else
    //        {
    //            deltaPosition.y = Physics.gravity.y / 2f;
    //        }
    //    }
    //    else
    //    {
    //        startingY = transform.position.y;
    //        canGoUp = true;
    //    }
    //    rb.velocity = Vector3.Slerp(Vector3.zero, deltaPosition, timeMoving / InputManager.Slipperiness);
    //}

    public void MoveForward(float forceMagnitude)
    {
        rb.AddForce(transform.forward * forceMagnitude, ForceMode.Force);
    }

    public IEnumerator Slip()
    {
        //float originalAngularDrag = rb.angularDrag;
        //rb.angularDrag = 0f;
        //shouldBeUpright = false;
        rb.AddForce(transform.up * CollisionManager.SlipHeight, ForceMode.Impulse);
        rb.AddTorque(transform.right * -CollisionManager.SlipTorqueMagnitude, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => IsGrounded());
        //rb.angularDrag = originalAngularDrag;
        shouldBeUpright = true;
        player.canInput = true;
        yield break;
    }

    private bool IsGrounded()
    {
        Vector3 sphereCenter = new Vector3(transform.position.x, transform.position.y - (capsuleCollider.bounds.extents.y + 0.1f), transform.position.z);
        if (!Physics.CheckSphere(sphereCenter, capsuleCollider.radius, CollisionManager.GroundMask))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void StayUpright() //based on this: https://answers.unity.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
    {
        Vector3 predictedUp = Quaternion.AngleAxis(rb.angularVelocity.magnitude * Mathf.Rad2Deg * PlayerPhysics.StabilityFactor / PlayerPhysics.StabilizeSpeed, rb.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        rb.AddTorque(torqueVector * PlayerPhysics.StabilizeSpeed * PlayerPhysics.StabilizeSpeed);
    }
}
