using UnityEngine;

public class AvailablePath : MonoBehaviour
{
    private Color originalColor;
    private CellHover cellHoverScript;

    private void Start()
    {
        originalColor = gameObject.GetComponentInChildren<Renderer>().material.color;
        cellHoverScript = gameObject.GetComponent<CellHover>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!cellHoverScript.isHovered)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = cellHoverScript.pathRangeColor;
        }
        gameObject.tag = "AvailablePath";
    }

    private void OnTriggerExit(Collider col)
    {
        if (!cellHoverScript.isHovered)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = originalColor;
        }
        gameObject.tag = "Block";
    }
}