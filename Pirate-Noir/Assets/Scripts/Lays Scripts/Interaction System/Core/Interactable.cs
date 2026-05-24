public interface Interactable
{
    // Defining functions
     public bool CanInteract();
     //Takes Interactor as a parameter. Allows the script to access the Interactor script
    public bool Interact(Interactor interactor);

}
