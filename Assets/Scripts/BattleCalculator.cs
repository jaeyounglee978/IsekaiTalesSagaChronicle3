public static class BattleCalculator
{
    public static void CalculateAttack(Unit attackUnit, Unit targetUnit)
    {
        // 적당히 계산식.
        int damage = targetUnit.isInGuardPosition ? (attackUnit.attack / 2) : attackUnit.attack;
        targetUnit.GetDamage(damage);
    }
}