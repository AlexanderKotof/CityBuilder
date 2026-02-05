using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation
{
    /// <summary>
    /// All player gameplay actions should go through this system 
    /// TODO: This system also can send requests to backend in future 
    /// </summary>
    public class PlayerActionsService
    {
        private readonly List<PlayerAction> _playerActions = new List<PlayerAction>();
        
        private PlayerAction _currentPlayerAction;
        private bool _isInProcess;

        //TODO: provide better algorithm
        public async UniTask<IResult> EnqueueAction(PlayerAction action)
        {
            if (_currentPlayerAction != null)
            {
                Debug.Log($"[PlayerActionsService] Action queued {action.NameOf ?? "Empty"}...");
                _playerActions.Add(action);
                await UniTask.WaitWhile(_isInProcess, static value => value);
                var result = await ExecuteAction(action);
                _playerActions.Remove(action);
                return result;
            }
            
            return await ExecuteAction(action);
        }

        private async UniTask<IResult> ExecuteAction(PlayerAction action)
        {
            Debug.Log("[PlayerActionsService] Start executing action: " + (action.NameOf ?? "Empty"));
            _isInProcess = true;
            _currentPlayerAction = action;
            var result = await _currentPlayerAction.Action();
            _currentPlayerAction = null;
            _isInProcess = false;
            Debug.Log("[PlayerActionsService] Complete executing action: " + (action.NameOf ?? "Empty"));
            return result;
        }
    }
}