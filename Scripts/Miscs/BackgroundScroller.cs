using System.Collections;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;

    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    IEnumerator Start()
    {
        while (true)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }
    }
}