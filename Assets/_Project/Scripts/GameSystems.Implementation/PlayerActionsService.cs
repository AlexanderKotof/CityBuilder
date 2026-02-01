using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation
{
    public interface IResult
    {
        
    }

    public record Success(Action RewindHandle = null) : IResult
    {
        public Action RewindHandle { get; } = RewindHandle;
    }

    public record Fail : IResult;

    public record PlayerAction(Func<UniTask<IResult>> Action, Func<bool> ValidateAction = null, string NameOf = null)
    {
        public string NameOf { get; } = NameOf;
        public Func<UniTask<IResult>> Action { get; } = Action;
        public Func<bool> ValidateAction { get; } = ValidateAction;
    }
    
    public record PlayerActionHandle()
    {
        public UniTask<IResult> Task { get; }
    }
    
    public class PlayerActionsService
    {
        private readonly Queue<PlayerAction> _playerActions = new Queue<PlayerAction>();
        
        private PlayerAction _currentPlayerAction;
        private bool _isInProcess;

        public async UniTask<IResult> QueueAction(PlayerAction action)
        {
            if (_currentPlayerAction != null)
            {
                await UniTask.WaitWhile(_isInProcess, static value => value);
                
                return await ExecuteAction(action);
            }
            
            return await ExecuteAction(action);
        }

        private async UniTask<IResult> ExecuteAction(PlayerAction action)
        {
            Debug.Log("Start executing action: " + (action.NameOf ?? "Empty"));
            _isInProcess = true;
            _currentPlayerAction = action;
            var result = await _currentPlayerAction.Action();
            _currentPlayerAction = null;
            _isInProcess = false;
            Debug.Log("Complete executing action: " + (action.NameOf ?? "Empty"));
            return result;
        }
    }
}