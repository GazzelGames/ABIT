using UnityEngine;

public class NPCDialogueScript : MonoBehaviour {

    [SerializeField]
    private NormalDialogue nd;

    public NormalDialogue GetDialogue()
    {
        return nd;
    }

}
