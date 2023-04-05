using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingPrototype.Gameplay.Input
{
    public static class CustomInput
    {
        private static PlayerInput _inputs;
        private static bool inited = false;

        public static void Init()
        {
            if (inited)
            {
                Debug.LogWarning("Input Already Initialized!");
                return;
            }

            inited = true;
            _inputs = new PlayerInput();
            _inputs.Enable();
        }

        public static void SetInputBindings(string bindings) 
        {
            if (!inited) Init();
            _inputs.asset.RemoveAllBindingOverrides();
            _inputs.asset.LoadBindingOverridesFromJson(bindings);
        }

        public static void ChangeInputState(bool state) 
        {
            if (state) _inputs.Enable();
            else _inputs.Disable();
        }

        public static PlayerInput Input
        {
            get
            {
                if (!inited)
                {
                    Init();
                }

                if (_inputs == null)
                {
                    Debug.LogWarning("_inputs initialized but its null!");
                }

                return _inputs;
            }
        }
    }
}