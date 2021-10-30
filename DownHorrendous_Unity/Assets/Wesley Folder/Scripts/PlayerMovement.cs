using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction { Left, Right };

    private readonly float debugRayLength = 2f;

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

    public void MoveForward(float forceMagnitude)
    {
        rb.AddForce(CalculateTrajectory() * forceMagnitude, ForceMode.Force);
    }

    public IEnumerator Slip()
    {
        rb.AddForce(transform.up * CollisionManager.SlipHeight, ForceMode.Impulse);
        rb.AddTorque(transform.right * -CollisionManager.SlipTorqueMagnitude, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => IsGrounded());
        shouldBeUpright = true;
        player.canInput = true;
        yield break;
    }

    private Vector3 CalculateTrajectory()
    {
        Vector3 bottomRayStartPosition = new Vector3(transform.position.x, transform.position.y - capsuleCollider.bounds.extents.y, transform.position.z);
        Vector3 topRayStartPosition = new Vector3(transform.position.x, transform.position.y - capsuleCollider.bounds.extents.y + InputManager.StepOffset, transform.position.z);
        Ray bottomRay = new Ray(bottomRayStartPosition, transform.forward);
        Ray topRay = new Ray(topRayStartPosition, transform.forward);
        if (Physics.Raycast(bottomRay, out RaycastHit bottomHit, 0.01f) && Physics.Raycast(topRay, out RaycastHit topHit, Mathf.Infinity))
        {
            if (topHit.distance > bottomHit.distance)
            {
                float slopeAngle = Mathf.Atan2(topHit.point.y - bottomHit.point.y, topHit.distance - bottomHit.distance);
                if (slopeAngle <= InputManager.SlopeOffset)
                {
                    Quaternion rotation = Quaternion.AngleAxis(-slopeAngle, transform.right);
                    rb.useGravity = false;
                    return rotation * transform.forward;
                }
                else
                {
                    rb.useGravity = true;
                    return transform.forward;
                }
            }
            else
            {
                rb.useGravity = true;
                return transform.forward;
            }
        }
        else
        {
            rb.useGravity = true;
            return transform.forward;
        }
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

    public void UseGravity()
    {
        rb.useGravity = true;
    }
}
