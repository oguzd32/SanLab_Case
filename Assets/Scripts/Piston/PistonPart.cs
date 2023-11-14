using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PistonPart : MonoBehaviour
{
    public Piston.Part type;
    public GameObject ghostPrefab;   

    public event Action<PistonPart> partAssembled;
    public event Action<PistonPart> partDissambled;

    // private variables
    private GameObject ghostInstance;
    private AssemblyPoint assemblyPoint;

    // cached component(s)
    private DragController dragController;

    private void Start()
    {       
        dragController = GetComponent<DragController>();

        if (dragController == null)
        {
            dragController = gameObject.AddComponent<DragController>();
        } 

        dragController.mouseUp += AssemblyThePart;
    }

    private void OnDestroy()
    {
        if (dragController != null) 
        {
            dragController.mouseUp -= AssemblyThePart;
        }
    }

    private void AssemblyThePart()
    {
        if (assemblyPoint == null) return;        

        StartCoroutine(MoveAssemblyPoint(assemblyPoint.transform.position));


        if (ghostInstance != null) 
        {
            Destroy(ghostInstance);
        }

        partAssembled?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!dragController.isDragging) return;

        if (!other.gameObject.TryGetComponent(out AssemblyPoint point)) return;

        if (point.pistonPart == type)
        {          
            assemblyPoint = point;

            ghostInstance = Instantiate(ghostPrefab);

            ghostInstance.transform.position = assemblyPoint.transform.position;
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (!dragController.isDragging) return;

        if (!other.gameObject.TryGetComponent(out AssemblyPoint point)) return;

        if (point.pistonPart == type)
        {
            assemblyPoint = null;

            Destroy(ghostInstance);
        }
        
    }

    /// <summary>
    /// Move the part to the assembly point
    /// </summary>
    /// <param name="targetPoint">assembly point</param>
    /// <returns></returns>
    private IEnumerator MoveAssemblyPoint(Vector3 targetPoint)
    {
        float timer = 0.5f;

        while (timer > 0f) 
        {
            timer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * 5);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = targetPoint;
    }
}
