using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    Text waveText;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }
    
    void OnEnable()
    {
        waveText.text = "- WAVE " + EnemyManager.Instance.WaveNumber + " -";
    }
}