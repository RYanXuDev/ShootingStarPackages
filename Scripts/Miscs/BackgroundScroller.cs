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
        while (GameManager.GameState != GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }
    }
}