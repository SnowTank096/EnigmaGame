using UnityEngine.InputSystem;
using UnityEngine;

interface IInteractable {
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }   
            }
        }
    }
}
