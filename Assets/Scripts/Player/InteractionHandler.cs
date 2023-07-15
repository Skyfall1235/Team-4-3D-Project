using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InteractionHandler : MonoBehaviour
{
    Camera cam;

    [SerializeField] private List<GameObject> interactableObjectsInRange= new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (!interactableObjectsInRange.Contains(other.gameObject) && other.gameObject.GetComponentInParent<IInteractable>() != null)
        {
            interactableObjectsInRange.Add(other.gameObject);
            if(other.gameObject.GetComponentInParent<Outline>() != null)
            {
                other.gameObject.GetComponentInParent<Outline>().enabled = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (interactableObjectsInRange.Contains(other.gameObject))
        {
            interactableObjectsInRange.Remove(other.gameObject);
            if (other.gameObject.GetComponentInParent<Outline>() != null)
            {
                other.gameObject.GetComponentInParent<Outline>().enabled = false;
            }
        }
    }
    private void Start()
    {
        if(GetComponent<Camera>() != null)
        {
            cam = GetComponent<Camera>();
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            foreach(GameObject obj in interactableObjectsInRange)
            {
                obj.GetComponentInParent<IInteractable>().Interact();
            }
        }
    }
    private bool IsVisibleOnScreen(GameObject objectToCheck)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(objectToCheck.transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
        if (onScreen && objectToCheck.GetComponent<Renderer>() != null && objectToCheck.GetComponent<Renderer>().isVisible)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
