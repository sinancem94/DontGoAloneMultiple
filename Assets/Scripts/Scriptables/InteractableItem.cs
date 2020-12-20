using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New InteractableItem", menuName = "Board/InteractableItem", order = 2)]
public class InteractableItem : ScriptableObject, IInteractable
{
    public string ExplenationText;
    
    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
