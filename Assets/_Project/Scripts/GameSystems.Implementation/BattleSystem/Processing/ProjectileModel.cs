using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Processing
{
    public class ProjectileModel
    {
        public ProjectileModel(IBattleUnit shooter, IBattleUnit target)
        {
            Shooter = shooter;
            Target = target;
        }

        public IBattleUnit Shooter { get; set; }
        public IBattleUnit Target { get; set; }
    }
}