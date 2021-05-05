using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;

    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}