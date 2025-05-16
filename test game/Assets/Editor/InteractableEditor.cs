using UnityEditor;

[CustomEditor(typeof(Interactable), true)]

public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable PlayerInteractable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            PlayerInteractable.promptMessage = EditorGUILayout.TextField("Prompt Message", PlayerInteractable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);
            if (PlayerInteractable.GetComponent<InteractionEvent>() == null)
            {
                PlayerInteractable.useEvents = true;
                PlayerInteractable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (PlayerInteractable.useEvents)
            {
                if (PlayerInteractable.GetComponent<InteractionEvent>() == null)
                    PlayerInteractable.gameObject.AddComponent<InteractionEvent>();
            }
            else
            {
                if (PlayerInteractable.GetComponent<InteractionEvent>() != null)
                    DestroyImmediate(PlayerInteractable.GetComponent<InteractionEvent>());
            }
        }
    }
}
