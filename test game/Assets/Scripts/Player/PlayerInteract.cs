using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField]
    private float interactionDistance = 3f;
    [SerializeField]
    private LayerMask interactionMask;
    private PlayerUI uiManager;
    private InputManager uiInput;
    void Start()
    {
        playerCamera = GetComponent<PlayerLook>().cam;
        uiManager = GetComponent<PlayerUI>();
        uiInput = GetComponent<InputManager>();
    }
    void Update()
    {
        uiManager.UpdateText(string.Empty);
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin,ray.direction * interactionDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactionMask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable otherinteractable = hitInfo.collider.GetComponent<Interactable>();
                uiManager.UpdateText(otherinteractable.promptMessage);
                if(uiInput.onFoot.Interact.triggered)
                {
                    otherinteractable.BaseInteract();
                }
            }
        }
    }
}
