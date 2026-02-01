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
        private readonly Queue<PlayerAction> _playerActions = new Queue<PlayerAction>();
        
        private PlayerAction _currentPlayerAction;
        private bool _isInProcess;

        //TODO: provide better algorithm
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