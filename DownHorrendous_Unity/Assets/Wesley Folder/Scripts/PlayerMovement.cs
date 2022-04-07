using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private LoverValuesScriptableObject _loverValues;

    public enum Direction { Left, Right };

    public bool walkParticleExists;
    public GameObject WalkParticles { get; private set; }

    private readonly float debugRayLength = 2f;

    private bool shouldBeUpright = true;

    private Vector3 eulerAngleVelocity;

    public IEnumerator meetEyesCoroutine;

    private Lover _lover;
    private Lover _otherLover;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    void Start()
    {
        meetEyesCoroutine = MeetEyes();
        _lover = GetComponent<Lover>();
        _loverValues = _lover.LoverValues;
        Lover[] playersArray = FindObjectsOfType<Lover>();
        foreach (Lover playerIndex in playersArray)
        {
            if (playerIndex.gameObject != gameObject)
            {
                _otherLover = playerIndex;
            }
        }
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        Debug.Log(_loverValues.SlopeOffset);
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
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    public void MoveForward(float forceMagnitude)
    {
        _rigidbody.AddForce(CalculateTrajectory() * forceMagnitude, ForceMode.Force);
        if (!walkParticleExists)
        {
            WalkParticles = Instantiate(ParticleManager.WalkParticles, transform);
            walkParticleExists = true;
        }
    }

    public IEnumerator Slip()
    {
        _rigidbody.AddForce(transform.up * CollisionManager.SlipHeight, ForceMode.Impulse);
        _rigidbody.AddTorque(transform.right * -CollisionManager.SlipTorqueMagnitude, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => IsGrounded());
        shouldBeUpright = true;
        _lover.canInput = true;
        yield break;
    }

    public IEnumerator OnFire()
    {
        float timeOnFire = 0f;
        int randomStartDirection = Random.Range(0, 2);
        Direction randomDirection;
        if (randomStartDirection == 0)
        {
            randomDirection = Direction.Left;
        }
        else
        {
            randomDirection = Direction.Right;
        }
        float randomTurnDuration = Random.Range(0.25f, 1f);
        float timeTurning = 0f;
        yield return new WaitForFixedUpdate();
        while (timeOnFire <= InputManager.FireDuration)
        {
            if (timeTurning <= randomTurnDuration)
            {
                Rotate(randomDirection, _loverValues.RotationSpeed / 4f);
                timeTurning += Time.fixedDeltaTime;
                if (timeTurning >= randomTurnDuration)
                {
                    if (randomDirection == Direction.Left)
                    {
                        randomDirection = Direction.Right;
                    }
                    else
                    {
                        randomDirection = Direction.Left;
                    }
                    randomTurnDuration = Random.Range(0.25f, 1f);
                    timeTurning = 0f;
                }
            }
            timeOnFire += Time.fixedDeltaTime;
            if (timeOnFire >= InputManager.FireDuration)
            {
                _lover.isOnFire = false;
                foreach (Renderer renderer in _lover.playerRenderers)
                {
                    renderer.material.color = _lover.baseColor;
                }
                Destroy(_lover.PlayerCollisions.FireParticles);
                _lover.PlayerCollisions.fireParticleExists = false;
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator MeetEyes()
    {
        //Quaternion facingOtherLover = Quaternion.FromToRotation(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(otherPlayer.transform.position.x, 0f, otherPlayer.transform.position.z));
        Vector3 directionToOtherLover = (new Vector3(_otherLover.transform.position.x, transform.position.y, _otherLover.transform.position.z) - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToOtherLover);
        while (true)
        {
            //rb.MoveRotation(facingOtherLover);
            _rigidbody.MoveRotation(lookRotation);
            yield break;
        }
    }

    private Vector3 CalculateTrajectory()
    {
        Vector3 bottomRayStartPosition = new Vector3(transform.position.x, transform.position.y - (_capsuleCollider.height / 2), transform.position.z);
        Vector3 topRayStartPosition = new Vector3(transform.position.x, transform.position.y - (_capsuleCollider.height / 2) + 0.01f, transform.position.z);
        Ray bottomRay = new Ray(bottomRayStartPosition, transform.forward);
        Ray topRay = new Ray(topRayStartPosition, transform.forward);
        if (Physics.Raycast(bottomRay, out RaycastHit bottomHit, 2f) && Physics.Raycast(topRay, out RaycastHit topHit, Mathf.Infinity))
        {
            Debug.Log(bottomHit.collider.gameObject.layer);
            if (topHit.distance > bottomHit.distance)
            {
                float slopeAngle = Mathf.Atan2(topHit.point.y - bottomHit.point.y, topHit.distance - bottomHit.distance);
                if (slopeAngle > 0 && slopeAngle <= _loverValues.SlopeOffset)
                {
                    Quaternion rotation = Quaternion.AngleAxis(-slopeAngle, transform.right);
                    _rigidbody.useGravity = false;
                    return rotation * transform.forward;
                }
                else
                {
                    _rigidbody.useGravity = true;
                    return Vector3.zero;
                }
            }
            else
            {
                _rigidbody.useGravity = true;
                return transform.forward;
            }
        }
        else
        {
            _rigidbody.useGravity = true;
            return transform.forward;
        }
    }

    private bool IsGrounded()
    {
        Vector3 sphereCenter = new Vector3(transform.position.x, transform.position.y - (_capsuleCollider.bounds.extents.y + 0.1f), transform.position.z);
        if (!Physics.CheckSphere(sphereCenter, _capsuleCollider.radius, CollisionManager.GroundMask))
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
        Vector3 predictedUp = Quaternion.AngleAxis(_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * PlayerPhysics.StabilityFactor / PlayerPhysics.StabilizeSpeed, _rigidbody.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        _rigidbody.AddTorque(PlayerPhysics.StabilizeSpeed * PlayerPhysics.StabilizeSpeed * torqueVector);
    }

    public void UseGravity()
    {
        _rigidbody.useGravity = true;
    }
}
