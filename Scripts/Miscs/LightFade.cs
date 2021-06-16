using System.Collections;
using UnityEngine;

class LightFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] bool delay = false;
    [SerializeField] float delayTime = 0f;
    [SerializeField] float startIntensity = 30f;
    [SerializeField] float finalIntensity = 0f;

    new Light light;

    float t;

    WaitForSeconds waitDelayTime;

    void Awake()
    {
        light = GetComponent<Light>();

        waitDelayTime = new WaitForSeconds(delayTime);
    }

    void OnEnable()
    {
        StartCoroutine(nameof(LightCoroutine));
    }

    IEnumerator LightCoroutine()
    {
        if (delay)
        {
            yield return waitDelayTime;
        }

        light.intensity = startIntensity;
        light.enabled = true;
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            light.intensity = Mathf.Lerp(startIntensity, finalIntensity, t);

            yield return null;
        }

        light.enabled = false;
    }
}