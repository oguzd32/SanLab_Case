using System;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public event Action mouseUp;

    public bool isDragging {  get; private set; }

    Vector3 mousePosition;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private Vector3 GetMousePosition => mainCamera.WorldToScreenPoint(transform.position);

    private void OnMouseDown()
    {
        if (!this.enabled) return;

        isDragging = true;

        mousePosition = Input.mousePosition - GetMousePosition;        
    }

    private void OnMouseUp()
    {
        if (!this.enabled) return;

        isDragging = false;

        mouseUp?.Invoke();
    }

    private void OnMouseDrag()
    {
        if(!this.enabled) return;

        isDragging = true;

        transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
}
