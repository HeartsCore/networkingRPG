public class EnemyStats : UnitStats 
{
    public override void OnStartServer()
    {
        CurHealth = _maxHealth;
    }
}
