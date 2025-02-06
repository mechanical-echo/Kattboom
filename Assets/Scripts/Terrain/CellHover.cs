using UnityEngine;

public class CellHover : MonoBehaviour
{
    private Camera camera;
    private Renderer blockRenderer;
    
    public bool isHovered = false;
    
    public Color hoverColor;
    public Color pathRangeColor;
    
    private bool isInRange
    {
        get { return gameObject.CompareTag("AvailablePath"); }
    }

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        blockRenderer  = gameObject.GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                AddHoverEffect();
            }
            else
            {
                RemoveHoverEffect();
            }
        }
        else
        {
            RemoveHoverEffect();
        }
    }

    private void AddHoverEffect()
    {
        if (!isHovered && isInRange)
        {
            blockRenderer.material.color = hoverColor;
            gameObject.transform.position =
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            isHovered = true;
        }
    }

    private void RemoveHoverEffect()
    {
        if (isHovered && isInRange)
        {
            blockRenderer.material.color = pathRangeColor;
            gameObject.transform.position =
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.1f, gameObject.transform.position.z);
            isHovered = false;
        }
    }
}