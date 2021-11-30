public class BossHealthBar : StatsBar_HUD
{
    protected override void SetPercentText()
    {
        // percentText.text = string.Format("{0:N2}", targetFillAmount * 100f) + "%";
        percentText.text = (targetFillAmount * 100f).ToString("f2") + "%";
    }
}