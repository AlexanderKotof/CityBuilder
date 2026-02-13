using System;
using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.WindowSystem.Window;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.Views.Implementation.Windows
{
    public class StartWindowView : WindowViewBase<StartWindowModel>
    {
        [SerializeField]
        private GameObject _registrationPopup;

        [SerializeField]
        private GameObject _enterGamePopup;
        
        [SerializeField]
        private TextMeshProUGUI _nicknameText;
        [SerializeField]
        private Button _enterGameButton;
        
        [SerializeField]
        private Button _registerButton;
        
        [SerializeField]
        private TMP_InputField _nicknameInputField;
        
        public override void Initialize(StartWindowModel model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);

            Subscribe(_nicknameInputField.onValueChanged.AsObservable(), OnNicknameChanged);
            
            //TODO: compare memory consumption
            Subscribe((Model, _nicknameInputField), _registerButton.OnClickAsObservable(),
                static (state, _) => state.Model.RegistrationSubmit.Execute(state._nicknameInputField.text));
            Subscribe(Model, _enterGameButton.OnClickAsObservable(), 
                static (m, v) => m.EnterGamePressed.Execute(v));
            
            Subscribe(Model.PlayerNickname, nickname => _nicknameText.text = $"Nickname {nickname}");
            Subscribe(Model.ShowRegistration, active => _registrationPopup.SetActive(active));
            Subscribe(Model.ShowEnteringGame, active => _enterGamePopup.SetActive(active));
        }

        private void OnNicknameChanged(string value)
        {
            _registerButton.interactable = IsValidNickname(value);
        }

        private static bool IsValidNickname(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Length > 3;
        }
    }
}