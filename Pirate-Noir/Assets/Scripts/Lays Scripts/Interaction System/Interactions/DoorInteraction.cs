using UnityEngine;

// Implement the interface after MonoBehaviour
public class DoorInteraction : MonoBehaviour, IInteractable
{

    public bool CanInteract()
    {
        // Logic to determine if the door can be interacted with
        throw new
    }

    public bool Interact(Interactor interactor)
    {
        // Logic to handle the interaction with the door
        Debug.Log("Door has been interacted with!");
        return true; // Placeholder, replace with actual logic
    }
}
