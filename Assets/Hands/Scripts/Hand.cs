using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Hands.Scripts
{
    public class Hand : MonoBehaviour
    {
        private static readonly int TRIGGER_HASH = Animator.StringToHash("Trigger");
        private static readonly int GRIP_HASH = Animator.StringToHash("Grip");
        
        public InputDeviceCharacteristics controllerCharacteristics;    
        private InputDevice targetDevice;
        public Animator handAnimator;
        
        private void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            List<InputDevice> devices = new List<InputDevice>();

            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
        }

        private void UpdateHandAnimation()
        {
            if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat(TRIGGER_HASH, triggerValue);
            }
            else
            {
                handAnimator.SetFloat(TRIGGER_HASH, 0f);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                handAnimator.SetFloat(GRIP_HASH, gripValue);
            }
            else
            {
                handAnimator.SetFloat(GRIP_HASH, 0);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(!targetDevice.isValid)
            {
                TryInitialize();
            }
            else
            {
                UpdateHandAnimation();
            }
        }
    }
}
