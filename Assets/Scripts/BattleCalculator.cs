public static class BattleCalculator
{
    public static void  CalculateAttack(Unit attackUnit, Unit targetUnit) {
        // 적당히 계산식.
        int damage = targetUnit.isInGuradPosition ? (attackUnit.attack / 2) : attackUnit.attack;
        targetUnit.GetDamage(damage);
    }
}