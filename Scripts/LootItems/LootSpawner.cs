using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;

    public void Spawn(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);
        }
    }
}