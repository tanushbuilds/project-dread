using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea] public string npcText;   
    public string[] playerResponses; 
    public DialogueNode[] nextNodes;      
}
