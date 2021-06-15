// GENERATED AUTOMATICALLY FROM 'Assets/Test/Test_Scripts/TestControls_Brackeys.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class TestControls_Brackeys : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public TestControls_Brackeys()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestControls_Brackeys"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""42a1aef1-e4d5-451e-ab6c-b8281902668a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b5c90955-0901-4b99-af37-770abbfbdd22"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""c0c7992e-3ab8-4c24-aa11-6bca222b3aa6"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": ""StickDeadzone(max=0.98)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""214b15b8-6fbd-49e5-989f-fb5f45a7ecbc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""c6fb46d0-b716-45ab-bdd6-079fd22d450a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""AxisDeadzone(max=0.95)"",
                    ""interactions"": ""Press(pressPoint=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6a82930b-57e0-47b7-9c20-de839fe2db11"",
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
                    ""id"": ""1d42ad30-afef-47b6-9d88-a8efbc1e78e4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(max=0.98)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae987a31-2c4d-4659-827d-e0a5c1abdc12"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c61be6c4-4a7f-438b-8791-7bea0c338ec2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone(max=0.95)"",
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
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Move = m_Test.FindAction("Move", throwIfNotFound: true);
        m_Test_Look = m_Test.FindAction("Look", throwIfNotFound: true);
        m_Test_Jump = m_Test.FindAction("Jump", throwIfNotFound: true);
        m_Test_Fire = m_Test.FindAction("Fire", throwIfNotFound: true);
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

    // Test
    private readonly InputActionMap m_Test;
    private ITestActions m_TestActionsCallbackInterface;
    private readonly InputAction m_Test_Move;
    private readonly InputAction m_Test_Look;
    private readonly InputAction m_Test_Jump;
    private readonly InputAction m_Test_Fire;
    public struct TestActions
    {
        private TestControls_Brackeys m_Wrapper;
        public TestActions(TestControls_Brackeys wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Test_Move;
        public InputAction @Look => m_Wrapper.m_Test_Look;
        public InputAction @Jump => m_Wrapper.m_Test_Jump;
        public InputAction @Fire => m_Wrapper.m_Test_Fire;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void SetCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_TestActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_TestActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnLook;
                Jump.started -= m_Wrapper.m_TestActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnJump;
                Fire.started -= m_Wrapper.m_TestActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_TestActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
            }
        }
    }
    public TestActions @Test => new TestActions(this);
    public interface ITestActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
