public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        player.PickUpMissile();
        base.PickUp();
    }
}