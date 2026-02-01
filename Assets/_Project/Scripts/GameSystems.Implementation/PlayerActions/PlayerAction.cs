using System;
using Cysharp.Threading.Tasks;

namespace CityBuilder.GameSystems.Implementation
{
    public record PlayerAction(Func<UniTask<IResult>> Action, Func<bool> ValidateAction = null, string NameOf = null)
    {
        public string NameOf { get; } = NameOf;
        public Func<UniTask<IResult>> Action { get; } = Action;
        public Func<bool> ValidateAction { get; } = ValidateAction;
    }
}