namespace GameSystems.Implementation.BattleSystem
{
    public static class BattleRules
    {
        public const float MoveMagnitudeThreshold = 0.1f;
        
        public static float GetAttacksPerSecond(this IBattleUnit unit)
        {
            return 1 / unit.Config.AttackSpeed;
        }

        public static float GetRealMoveSpeed(this IBattleUnit unit)
        {
            return unit.Config.MoveSpeed;
        }
        
        public static float GetAttackRange(this IBattleUnit unit)
        {
            return unit.Config.AttackRange;
        }
        
        public static float GetAttackRangeSqr(this IBattleUnit unit)
        {
            return unit.GetAttackRange() * unit.GetAttackRange();
        }
    }
}