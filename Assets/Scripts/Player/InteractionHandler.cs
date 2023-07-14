using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[RequireComponent(typeof(Camera))]
public class InteractionHandler : MonoBehaviour
{

    [SerializeField] float interactionRadius = 5f;
    Camera cam;
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
            List<GameObject> InteractiblesToActivate = new List<GameObject>();
            foreach (Collider coll in Physics.OverlapSphere(transform.position, interactionRadius))
            {
                if (coll.gameObject.GetComponentInChildren<IInteractible>() != null && IsVisibleOnScreen(coll.gameObject))
                {
                    InteractiblesToActivate.Add(coll.gameObject);
                }
            }
            foreach(GameObject obj in InteractiblesToActivate)
            {
                obj.GetComponentInChildren<IInteractible>().Interact();
            }
        }
    }
    private bool IsVisibleOnScreen(GameObject objectToCheck)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(objectToCheck.transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < UnityEngine.Screen.width && screenPos.y > 0f && screenPos.y < UnityEngine.Screen.height;
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
