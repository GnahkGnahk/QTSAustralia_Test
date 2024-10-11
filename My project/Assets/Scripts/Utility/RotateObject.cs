
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeedX = 30f;
    public float rotationSpeedY = 45f;
    public float rotationSpeedZ = 60f;

    private Coroutine rotationCoroutine;

    private void Start()
    {
        transform.position += new Vector3(0, 1, 0);
    }

    private void OnEnable()
    {
        StartRotation();
    }
    private void OnDisable()
    {
        StopRotation();
    }

    public void StartRotation()
    {
        if (rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(Rotate());
        }
    }

    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
            yield return null;
        }
    }
}
