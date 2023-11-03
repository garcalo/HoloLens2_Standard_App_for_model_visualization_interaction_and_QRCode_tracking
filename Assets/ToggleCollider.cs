using UnityEngine;

public class ToggleCollider : MonoBehaviour
{
    private bool isColliderActive = false;
    private BoxCollider boxCollider;

    private void Start()
    {
        // Get the BoxCollider component attached to the game object
        boxCollider = GetComponent<BoxCollider>();

    }

    public void ToggleColliderState()
    {
        Debug.Log("toggling collider state");
        // Toggle the state of the box collider
        isColliderActive = !isColliderActive;
        boxCollider.enabled = isColliderActive;
    }
}
