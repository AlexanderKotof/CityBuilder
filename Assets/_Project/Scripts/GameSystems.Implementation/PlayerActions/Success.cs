using System;

namespace CityBuilder.GameSystems.Implementation
{
    public record Success(Action RewindHandle = null) : IResult
    {
        public Action RewindHandle { get; } = RewindHandle;
    }
}