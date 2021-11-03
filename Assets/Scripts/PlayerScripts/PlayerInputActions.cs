// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""3b7b3188-b958-4dac-9b0b-d4f27e2acead"",
            ""actions"": [
                {
                    ""name"": ""Pause/Previous UI"",
                    ""type"": ""Button"",
                    ""id"": ""6c728e1e-0cab-42a9-89a0-774d3af89e93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d986a886-a3cd-47b9-8776-734edfb47cc3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrickRotation"",
                    ""type"": ""Value"",
                    ""id"": ""421cf89d-ea64-42d3-9662-56e0e4402e37"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EnableTrick"",
                    ""type"": ""Button"",
                    ""id"": ""1d25ce20-cbb1-42f9-98a4-5982c07f14d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EnableBoost"",
                    ""type"": ""Button"",
                    ""id"": ""bbfee99d-0f61-4784-a31e-171853682c22"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RecordReplay"",
                    ""type"": ""Button"",
                    ""id"": ""4e806ffd-40f3-4db3-a51a-ca32dddc3eb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Temp"",
                    ""type"": ""Button"",
                    ""id"": ""33434220-d173-4688-87a2-2870924cfb61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Leaderboard"",
                    ""type"": ""Button"",
                    ""id"": ""289d387f-3983-4b57-abb1-7998dc20f74b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6b496fc7-1167-43d1-b0f1-075792894000"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause/Previous UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b4e5b10b-9435-49f7-b9dd-dbb56a53ed4b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d348dd04-4070-4e52-b586-aad4d82da163"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b2b9f5f4-fb32-453c-9591-482e1f599d60"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c069c6a9-e6fc-4a68-936a-305350525036"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""88901b54-75ad-4c4f-94d8-c5a616357649"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8a63ee46-a0c2-4c4d-9d6e-e56051a62895"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EnableTrick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""39d49cd7-3596-4cc0-873e-a8b0680a084a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TrickRotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""44470654-5048-40ab-82c2-f6d4e9af4537"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TrickRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""92cd7f7a-ad12-4a07-8f71-ed39e6b8aea9"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TrickRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fdfddc9f-c7fd-4c74-9869-6335eaa65cac"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TrickRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1b9fe63f-abbf-46ce-b8be-c648f0111db9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TrickRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c05e4331-4938-4a67-8eec-657cb41d8369"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EnableBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15553537-9eaf-4fe6-8d74-630c868edacd"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RecordReplay"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0dd5ef7-a848-4ca4-9859-21f855b0b816"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Temp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""9362abc8-e0d7-4154-a73e-84171af7fdfb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Leaderboard"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""fb622f56-6b79-4ea1-ba2f-d2f75812138e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Leaderboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""460d64c9-bb30-40e9-b698-622942ef520e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Leaderboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""8f3b0358-d8e8-4bb5-90ce-63bd384f09ca"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_PausePreviousUI = m_Player.FindAction("Pause/Previous UI", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_TrickRotation = m_Player.FindAction("TrickRotation", throwIfNotFound: true);
        m_Player_EnableTrick = m_Player.FindAction("EnableTrick", throwIfNotFound: true);
        m_Player_EnableBoost = m_Player.FindAction("EnableBoost", throwIfNotFound: true);
        m_Player_RecordReplay = m_Player.FindAction("RecordReplay", throwIfNotFound: true);
        m_Player_Temp = m_Player.FindAction("Temp", throwIfNotFound: true);
        m_Player_Leaderboard = m_Player.FindAction("Leaderboard", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_PausePreviousUI;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_TrickRotation;
    private readonly InputAction m_Player_EnableTrick;
    private readonly InputAction m_Player_EnableBoost;
    private readonly InputAction m_Player_RecordReplay;
    private readonly InputAction m_Player_Temp;
    private readonly InputAction m_Player_Leaderboard;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PausePreviousUI => m_Wrapper.m_Player_PausePreviousUI;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @TrickRotation => m_Wrapper.m_Player_TrickRotation;
        public InputAction @EnableTrick => m_Wrapper.m_Player_EnableTrick;
        public InputAction @EnableBoost => m_Wrapper.m_Player_EnableBoost;
        public InputAction @RecordReplay => m_Wrapper.m_Player_RecordReplay;
        public InputAction @Temp => m_Wrapper.m_Player_Temp;
        public InputAction @Leaderboard => m_Wrapper.m_Player_Leaderboard;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @PausePreviousUI.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPausePreviousUI;
                @PausePreviousUI.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPausePreviousUI;
                @PausePreviousUI.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPausePreviousUI;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @TrickRotation.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrickRotation;
                @TrickRotation.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrickRotation;
                @TrickRotation.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrickRotation;
                @EnableTrick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableTrick;
                @EnableTrick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableTrick;
                @EnableTrick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableTrick;
                @EnableBoost.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableBoost;
                @EnableBoost.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableBoost;
                @EnableBoost.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnableBoost;
                @RecordReplay.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecordReplay;
                @RecordReplay.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecordReplay;
                @RecordReplay.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecordReplay;
                @Temp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTemp;
                @Temp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTemp;
                @Temp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTemp;
                @Leaderboard.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeaderboard;
                @Leaderboard.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeaderboard;
                @Leaderboard.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeaderboard;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PausePreviousUI.started += instance.OnPausePreviousUI;
                @PausePreviousUI.performed += instance.OnPausePreviousUI;
                @PausePreviousUI.canceled += instance.OnPausePreviousUI;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @TrickRotation.started += instance.OnTrickRotation;
                @TrickRotation.performed += instance.OnTrickRotation;
                @TrickRotation.canceled += instance.OnTrickRotation;
                @EnableTrick.started += instance.OnEnableTrick;
                @EnableTrick.performed += instance.OnEnableTrick;
                @EnableTrick.canceled += instance.OnEnableTrick;
                @EnableBoost.started += instance.OnEnableBoost;
                @EnableBoost.performed += instance.OnEnableBoost;
                @EnableBoost.canceled += instance.OnEnableBoost;
                @RecordReplay.started += instance.OnRecordReplay;
                @RecordReplay.performed += instance.OnRecordReplay;
                @RecordReplay.canceled += instance.OnRecordReplay;
                @Temp.started += instance.OnTemp;
                @Temp.performed += instance.OnTemp;
                @Temp.canceled += instance.OnTemp;
                @Leaderboard.started += instance.OnLeaderboard;
                @Leaderboard.performed += instance.OnLeaderboard;
                @Leaderboard.canceled += instance.OnLeaderboard;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    public struct MenuActions
    {
        private @PlayerInputActions m_Wrapper;
        public MenuActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnPausePreviousUI(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnTrickRotation(InputAction.CallbackContext context);
        void OnEnableTrick(InputAction.CallbackContext context);
        void OnEnableBoost(InputAction.CallbackContext context);
        void OnRecordReplay(InputAction.CallbackContext context);
        void OnTemp(InputAction.CallbackContext context);
        void OnLeaderboard(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
    }
}
