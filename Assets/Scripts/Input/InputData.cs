// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/InputData.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Scripts.Input
{
    public class @InputData : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputData()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputData"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""23082180-fc82-4aa8-bcb0-61b34818b9cd"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""5c1ddf29-23bd-4512-a828-31bce0195cd2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d119bcb8-5525-4c93-a000-62bcbfce7ce6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryContact"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8047196f-4322-4aea-ac90-266c5f18147d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gyro"",
                    ""type"": ""Value"",
                    ""id"": ""994eefc1-eadb-4044-b102-b93427f897c0"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""585e0952-1d64-4ab1-a958-1bc902a18ede"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f549758-1061-4c09-b206-b29caaa5c54c"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35cc858f-0281-4a61-820c-d6e17a11a267"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7be9cb1f-6c4d-40e0-9ee1-634c170ea10c"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d481e95c-53a8-4c15-87cd-f763b337a132"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""PrimaryContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c937e673-c26a-4a60-981b-f9f99da29efb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""PrimaryContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d7050d2-b8fc-4fde-b429-551a68133f30"",
                    ""path"": ""<Gyroscope>/angularVelocity"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Gyro"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Android"",
            ""bindingGroup"": ""Android"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gyroscope>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Click = m_Player.FindAction("Click", throwIfNotFound: true);
            m_Player_Position = m_Player.FindAction("Position", throwIfNotFound: true);
            m_Player_PrimaryContact = m_Player.FindAction("PrimaryContact", throwIfNotFound: true);
            m_Player_Gyro = m_Player.FindAction("Gyro", throwIfNotFound: true);
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
        private readonly InputAction m_Player_Click;
        private readonly InputAction m_Player_Position;
        private readonly InputAction m_Player_PrimaryContact;
        private readonly InputAction m_Player_Gyro;
        public struct PlayerActions
        {
            private @InputData m_Wrapper;
            public PlayerActions(@InputData wrapper) { m_Wrapper = wrapper; }
            public InputAction @Click => m_Wrapper.m_Player_Click;
            public InputAction @Position => m_Wrapper.m_Player_Position;
            public InputAction @PrimaryContact => m_Wrapper.m_Player_PrimaryContact;
            public InputAction @Gyro => m_Wrapper.m_Player_Gyro;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Click.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                    @Click.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                    @Click.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                    @Position.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPosition;
                    @Position.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPosition;
                    @Position.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPosition;
                    @PrimaryContact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryContact;
                    @PrimaryContact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryContact;
                    @PrimaryContact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryContact;
                    @Gyro.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGyro;
                    @Gyro.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGyro;
                    @Gyro.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGyro;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Click.started += instance.OnClick;
                    @Click.performed += instance.OnClick;
                    @Click.canceled += instance.OnClick;
                    @Position.started += instance.OnPosition;
                    @Position.performed += instance.OnPosition;
                    @Position.canceled += instance.OnPosition;
                    @PrimaryContact.started += instance.OnPrimaryContact;
                    @PrimaryContact.performed += instance.OnPrimaryContact;
                    @PrimaryContact.canceled += instance.OnPrimaryContact;
                    @Gyro.started += instance.OnGyro;
                    @Gyro.performed += instance.OnGyro;
                    @Gyro.canceled += instance.OnGyro;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_AndroidSchemeIndex = -1;
        public InputControlScheme AndroidScheme
        {
            get
            {
                if (m_AndroidSchemeIndex == -1) m_AndroidSchemeIndex = asset.FindControlSchemeIndex("Android");
                return asset.controlSchemes[m_AndroidSchemeIndex];
            }
        }
        private int m_PCSchemeIndex = -1;
        public InputControlScheme PCScheme
        {
            get
            {
                if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
                return asset.controlSchemes[m_PCSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnClick(InputAction.CallbackContext context);
            void OnPosition(InputAction.CallbackContext context);
            void OnPrimaryContact(InputAction.CallbackContext context);
            void OnGyro(InputAction.CallbackContext context);
        }
    }
}
