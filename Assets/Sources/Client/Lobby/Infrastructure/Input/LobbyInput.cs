//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Sources/Client/Lobby/Infrastructure/Input/LobbyInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Client.Lobby.Infrastructure.Input
{
    public partial class @LobbyInput : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @LobbyInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""LobbyInput"",
    ""maps"": [
        {
            ""name"": ""MainMenu"",
            ""id"": ""6b5bb5ae-9ace-46f8-842d-7be5cdc4c523"",
            ""actions"": [
                {
                    ""name"": ""OpenGuildFiles"",
                    ""type"": ""Button"",
                    ""id"": ""edf5e4af-09ef-4260-90cf-64f87c013f66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9dd71844-234d-4678-8339-198eae5580fa"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenGuildFiles"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GuildFiles"",
            ""id"": ""176c6870-23f3-4ea4-b3c7-77ced005f829"",
            ""actions"": [
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""09bf299d-a571-42cd-9c16-87d56406dc34"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Home"",
                    ""type"": ""Button"",
                    ""id"": ""74778824-c67b-4d61-b0e6-2e671b10fc2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5a92403f-95f6-48af-9e66-f0c3562139db"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5be5bf90-344f-4544-afee-9890115610c5"",
                    ""path"": ""<Keyboard>/home"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Home"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // MainMenu
            m_MainMenu = asset.FindActionMap("MainMenu", throwIfNotFound: true);
            m_MainMenu_OpenGuildFiles = m_MainMenu.FindAction("OpenGuildFiles", throwIfNotFound: true);
            // GuildFiles
            m_GuildFiles = asset.FindActionMap("GuildFiles", throwIfNotFound: true);
            m_GuildFiles_Back = m_GuildFiles.FindAction("Back", throwIfNotFound: true);
            m_GuildFiles_Home = m_GuildFiles.FindAction("Home", throwIfNotFound: true);
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
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // MainMenu
        private readonly InputActionMap m_MainMenu;
        private IMainMenuActions m_MainMenuActionsCallbackInterface;
        private readonly InputAction m_MainMenu_OpenGuildFiles;
        public struct MainMenuActions
        {
            private @LobbyInput m_Wrapper;
            public MainMenuActions(@LobbyInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @OpenGuildFiles => m_Wrapper.m_MainMenu_OpenGuildFiles;
            public InputActionMap Get() { return m_Wrapper.m_MainMenu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MainMenuActions set) { return set.Get(); }
            public void SetCallbacks(IMainMenuActions instance)
            {
                if (m_Wrapper.m_MainMenuActionsCallbackInterface != null)
                {
                    @OpenGuildFiles.started -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenGuildFiles;
                    @OpenGuildFiles.performed -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenGuildFiles;
                    @OpenGuildFiles.canceled -= m_Wrapper.m_MainMenuActionsCallbackInterface.OnOpenGuildFiles;
                }
                m_Wrapper.m_MainMenuActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @OpenGuildFiles.started += instance.OnOpenGuildFiles;
                    @OpenGuildFiles.performed += instance.OnOpenGuildFiles;
                    @OpenGuildFiles.canceled += instance.OnOpenGuildFiles;
                }
            }
        }
        public MainMenuActions @MainMenu => new MainMenuActions(this);

        // GuildFiles
        private readonly InputActionMap m_GuildFiles;
        private IGuildFilesActions m_GuildFilesActionsCallbackInterface;
        private readonly InputAction m_GuildFiles_Back;
        private readonly InputAction m_GuildFiles_Home;
        public struct GuildFilesActions
        {
            private @LobbyInput m_Wrapper;
            public GuildFilesActions(@LobbyInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Back => m_Wrapper.m_GuildFiles_Back;
            public InputAction @Home => m_Wrapper.m_GuildFiles_Home;
            public InputActionMap Get() { return m_Wrapper.m_GuildFiles; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GuildFilesActions set) { return set.Get(); }
            public void SetCallbacks(IGuildFilesActions instance)
            {
                if (m_Wrapper.m_GuildFilesActionsCallbackInterface != null)
                {
                    @Back.started -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnBack;
                    @Back.performed -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnBack;
                    @Back.canceled -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnBack;
                    @Home.started -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnHome;
                    @Home.performed -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnHome;
                    @Home.canceled -= m_Wrapper.m_GuildFilesActionsCallbackInterface.OnHome;
                }
                m_Wrapper.m_GuildFilesActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Back.started += instance.OnBack;
                    @Back.performed += instance.OnBack;
                    @Back.canceled += instance.OnBack;
                    @Home.started += instance.OnHome;
                    @Home.performed += instance.OnHome;
                    @Home.canceled += instance.OnHome;
                }
            }
        }
        public GuildFilesActions @GuildFiles => new GuildFilesActions(this);
        public interface IMainMenuActions
        {
            void OnOpenGuildFiles(InputAction.CallbackContext context);
        }
        public interface IGuildFilesActions
        {
            void OnBack(InputAction.CallbackContext context);
            void OnHome(InputAction.CallbackContext context);
        }
    }
}
