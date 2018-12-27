using UnityEngine;

namespace CustomController
{
    [SerializeField]
    public class CustomInput
    {
        //I believe what is happening is these bools are being set to false the moment the 
        //class is being created in another mono behavior. and they are assigned only once;
        //#if UNITY_EDITOR
        //movement


        //Auto implemented KeyCode property 
        //make this the master that is used for default settings

        public bool inputLeft = Input.GetKey(KeyCode.A);
        public bool inputRight = Input.GetKey(KeyCode.D);
        public bool inputUp = Input.GetKey(KeyCode.W);
        public bool inputDown = Input.GetKey(KeyCode.S);

        //interaction
        public bool equipedItem1 = Input.GetKey(KeyCode.Keypad1);
        public bool equipedItem2 = Input.GetKey(KeyCode.Keypad2);
        public bool equipedItem3 = Input.GetKey(KeyCode.Keypad3);
        public bool proceedTalking = Input.GetKey(KeyCode.C);
        public bool swingSword = Input.GetKey(KeyCode.Space);

        //menu
        public bool pauseGame = Input.GetKey(KeyCode.Escape);
        public bool map = Input.GetKey(KeyCode.Tab);
        //#endif

    }
}

