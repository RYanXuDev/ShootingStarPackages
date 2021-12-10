using System.Collections;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] float speed = 360f;
    [SerializeField] Vector3 angle;

    void OnEnable()
    {
        StartCoroutine(RotateCoroutine());
    }

    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            transform.Rotate(angle * speed * Time.deltaTime);
            
            yield return null;
        }
    }
}