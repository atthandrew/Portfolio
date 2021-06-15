// GENERATED AUTOMATICALLY FROM 'Assets/_Scripts/Player/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""d3f7c1da-3f33-41d8-a77b-a4c3a544e3ad"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""003121ea-0373-4869-8ac1-6df38ccdb39f"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""8d58a106-63d9-494e-a5db-f65faa0d9f30"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""a87dc691-ad20-40bd-94ba-e079f7e364f5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DodgeRoll"",
                    ""type"": ""Button"",
                    ""id"": ""0a9f35d4-771a-4a0c-b3d8-d5af11f64248"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PullArrow"",
                    ""type"": ""Button"",
                    ""id"": ""995c3629-a30f-4b09-b7fa-64821bf429d6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d552ef19-a4c5-4f2a-b457-0bae72120ac9"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83adc26e-7651-4619-bf1a-1ad6e2541464"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43efffe3-d2bd-4081-86c2-228a0384bdd0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DodgeRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7420c1a-5dee-4e5a-924f-bf7722669410"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""292e66fd-6007-464d-8b3b-f628df0612c6"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PullArrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""6f36149a-bcd6-4195-a611-1b9ab7a12f9b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""6cbda331-3b72-4cc8-b3b1-a22fa546c51e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""bd769f4b-bd6c-47cf-987d-98413686fa14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e9b75867-c4b8-452b-9647-6d8b9f9328cf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""3997ddfd-9c99-4a76-93fe-43b9728f55a7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6d34bace-4386-4278-845e-95a8b9a994ae"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abd73d6d-ca98-4555-abac-3ae480816de0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d775e05-f2de-46b3-a867-0655f3ff5e86"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b815a2b-bd48-4074-9856-558364c2cdd1"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""445d891f-d678-49d7-9088-30c96ba5d9d0"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RSAim_RTShoot"",
            ""id"": ""06ca8862-3fb7-4994-b3de-c1d7ac81d038"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c670dce0-aef5-4848-b407-e8d2dc62ca33"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""27e56ea8-b664-4271-b683-c42da0a274ec"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""74d2faed-dde2-46b8-8493-5537ca3ce650"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0b86b35-22b9-4b7d-8d58-ead90d46b016"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b79c39eb-45ed-4400-bc3c-a4b573ac0725"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bef2fef3-b6c9-4352-b68a-5d3405832b0c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RSAimShoot"",
            ""id"": ""c97f9add-a2fd-4864-894b-91f7c4faac07"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""bcc74a0b-87f8-46ee-b6b8-d1ddbddf6c52"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""5da3066e-9105-465f-b9fa-22f4ab3fced4"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Value"",
                    ""id"": ""c18dea9a-6d6f-4732-9847-c8b072210b4c"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""56eb9404-008b-406d-b214-f721081a2ee9"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fe24cf1-8fe7-43a4-911b-7f283afeac9a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5d09bb5-e552-4893-a49a-251477bd22d3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.8,max=1)"",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_Fire = m_Gameplay.FindAction("Fire", throwIfNotFound: true);
        m_Gameplay_DodgeRoll = m_Gameplay.FindAction("DodgeRoll", throwIfNotFound: true);
        m_Gameplay_PullArrow = m_Gameplay.FindAction("PullArrow", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
        m_UI_Select = m_UI.FindAction("Select", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        // RSAim_RTShoot
        m_RSAim_RTShoot = asset.FindActionMap("RSAim_RTShoot", throwIfNotFound: true);
        m_RSAim_RTShoot_Move = m_RSAim_RTShoot.FindAction("Move", throwIfNotFound: true);
        m_RSAim_RTShoot_Look = m_RSAim_RTShoot.FindAction("Look", throwIfNotFound: true);
        m_RSAim_RTShoot_Fire = m_RSAim_RTShoot.FindAction("Fire", throwIfNotFound: true);
        // RSAimShoot
        m_RSAimShoot = asset.FindActionMap("RSAimShoot", throwIfNotFound: true);
        m_RSAimShoot_Move = m_RSAimShoot.FindAction("Move", throwIfNotFound: true);
        m_RSAimShoot_Look = m_RSAimShoot.FindAction("Look", throwIfNotFound: true);
        m_RSAimShoot_Fire = m_RSAimShoot.FindAction("Fire", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_Fire;
    private readonly InputAction m_Gameplay_DodgeRoll;
    private readonly InputAction m_Gameplay_PullArrow;
    public struct GameplayActions
    {
        private PlayerControls m_Wrapper;
        public GameplayActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @Fire => m_Wrapper.m_Gameplay_Fire;
        public InputAction @DodgeRoll => m_Wrapper.m_Gameplay_DodgeRoll;
        public InputAction @PullArrow => m_Wrapper.m_Gameplay_PullArrow;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                Fire.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                DodgeRoll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodgeRoll;
                DodgeRoll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodgeRoll;
                DodgeRoll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodgeRoll;
                PullArrow.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPullArrow;
                PullArrow.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPullArrow;
                PullArrow.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPullArrow;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
                DodgeRoll.started += instance.OnDodgeRoll;
                DodgeRoll.performed += instance.OnDodgeRoll;
                DodgeRoll.canceled += instance.OnDodgeRoll;
                PullArrow.started += instance.OnPullArrow;
                PullArrow.performed += instance.OnPullArrow;
                PullArrow.canceled += instance.OnPullArrow;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Move;
    private readonly InputAction m_UI_Select;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Cancel;
    public struct UIActions
    {
        private PlayerControls m_Wrapper;
        public UIActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_UI_Move;
        public InputAction @Select => m_Wrapper.m_UI_Select;
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                Select.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                Select.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                Select.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Select.started += instance.OnSelect;
                Select.performed += instance.OnSelect;
                Select.canceled += instance.OnSelect;
                Pause.started += instance.OnPause;
                Pause.performed += instance.OnPause;
                Pause.canceled += instance.OnPause;
                Cancel.started += instance.OnCancel;
                Cancel.performed += instance.OnCancel;
                Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // RSAim_RTShoot
    private readonly InputActionMap m_RSAim_RTShoot;
    private IRSAim_RTShootActions m_RSAim_RTShootActionsCallbackInterface;
    private readonly InputAction m_RSAim_RTShoot_Move;
    private readonly InputAction m_RSAim_RTShoot_Look;
    private readonly InputAction m_RSAim_RTShoot_Fire;
    public struct RSAim_RTShootActions
    {
        private PlayerControls m_Wrapper;
        public RSAim_RTShootActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_RSAim_RTShoot_Move;
        public InputAction @Look => m_Wrapper.m_RSAim_RTShoot_Look;
        public InputAction @Fire => m_Wrapper.m_RSAim_RTShoot_Fire;
        public InputActionMap Get() { return m_Wrapper.m_RSAim_RTShoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RSAim_RTShootActions set) { return set.Get(); }
        public void SetCallbacks(IRSAim_RTShootActions instance)
        {
            if (m_Wrapper.m_RSAim_RTShootActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnLook;
                Fire.started -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_RSAim_RTShootActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_RSAim_RTShootActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
            }
        }
    }
    public RSAim_RTShootActions @RSAim_RTShoot => new RSAim_RTShootActions(this);

    // RSAimShoot
    private readonly InputActionMap m_RSAimShoot;
    private IRSAimShootActions m_RSAimShootActionsCallbackInterface;
    private readonly InputAction m_RSAimShoot_Move;
    private readonly InputAction m_RSAimShoot_Look;
    private readonly InputAction m_RSAimShoot_Fire;
    public struct RSAimShootActions
    {
        private PlayerControls m_Wrapper;
        public RSAimShootActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_RSAimShoot_Move;
        public InputAction @Look => m_Wrapper.m_RSAimShoot_Look;
        public InputAction @Fire => m_Wrapper.m_RSAimShoot_Fire;
        public InputActionMap Get() { return m_Wrapper.m_RSAimShoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RSAimShootActions set) { return set.Get(); }
        public void SetCallbacks(IRSAimShootActions instance)
        {
            if (m_Wrapper.m_RSAimShootActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnLook;
                Fire.started -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_RSAimShootActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_RSAimShootActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
            }
        }
    }
    public RSAimShootActions @RSAimShoot => new RSAimShootActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnDodgeRoll(InputAction.CallbackContext context);
        void OnPullArrow(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
    public interface IRSAim_RTShootActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
    public interface IRSAimShootActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
