using System;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Projectiles
{
    public static class DamageCalculator
    {
        public static float GetDamage(IBattleUnit attacker, IBattleUnit target)
        {
            return Math.Max(1, attacker.Config.Damage - target.Config.Defense);
        }
    }
}