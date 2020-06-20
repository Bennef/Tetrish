using UnityEngine;

namespace Scripts.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        public bool GetLeftButtonDown()
        {
            return Input.GetButtonDown("Move Left");
        }

        public bool GetRightButtonDown()
        {
            return Input.GetButtonDown("Move Right");
        }

        public bool GetRotateButtonDown()
        {
            return Input.GetButtonDown("Rotate");
        }

        public bool GetDownButtonDown()
        {
            return Input.GetButton("Down");
        }

        public bool GetAnyKeyDown()
        {
            return Input.anyKeyDown;
        }
    }
}