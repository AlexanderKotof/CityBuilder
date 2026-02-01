using Cysharp.Threading.Tasks;

namespace CityBuilder.GameSystems.Implementation
{
    public record PlayerActionHandle()
    {
        public UniTask<IResult> Task { get; }
    }
}