using UnityEngine;

public class EnergyBar : StatsBar_HUD
{
    [SerializeField] UnityEngine.UI.Text titleText;
    [SerializeField] Color overdriveColor;

    Color defaultColor;

    void Awake()
    {
        defaultColor = titleText.color;
    }

    void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void PlayerOverdriveOn()
    {
        titleText.color = overdriveColor;
        percentText.color = overdriveColor;
        fillImageFront.color = overdriveColor;
    }

    void PlayerOverdriveOff()
    {
        titleText.color = defaultColor;
        percentText.color = defaultColor;
        fillImageFront.color = defaultColor;
    }
}