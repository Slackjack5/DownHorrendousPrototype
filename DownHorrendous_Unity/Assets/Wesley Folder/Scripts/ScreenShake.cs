using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 5f)] private float shakeDuration;
    private float shakeTime;
    [SerializeField] [Range(0.01f, 10f)] private float shakeSpeed;
    [SerializeField] [Range(0.01f, 3f)] private float shakeMagnitude;

    private Camera thisCamera;
    private Vector3 cameraDefaultPosition;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
        cameraDefaultPosition = thisCamera.transform.position;
    }

    public IEnumerator Shake()
    {
        shakeTime = 0f;
        yield return null;
        while (shakeTime <= shakeDuration)
        {
            Vector3 cameraDefaultPositionLocal = transform.InverseTransformPoint(cameraDefaultPosition);
            float cameraPositionX = cameraDefaultPositionLocal.x + (shakeDuration - shakeTime) / shakeDuration * shakeMagnitude * Mathf.Sin(shakeTime * shakeSpeed);
            Vector3 deltaCameraPosition = transform.TransformPoint(new Vector3(cameraPositionX, 0f, 0f));
            thisCamera.transform.position = deltaCameraPosition;
            //float cameraPositionX = cameraDefaultPosition.x + (shakeDuration - shakeTime) / shakeDuration * shakeMagnitude * Mathf.Sin(shakeTime * shakeSpeed);
            //thisCamera.transform.position = new Vector3(cameraPositionX, cameraDefaultPosition.y, cameraDefaultPosition.z);
            shakeTime += Time.deltaTime;
            if (shakeTime >= shakeDuration)
            {
                thisCamera.transform.position = cameraDefaultPosition;
                yield break;
            }
            yield return null;
        }
    }
}
