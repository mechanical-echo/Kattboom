using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*--------------------------------------------
 *           Movement with arrow buttons
 ---------------------------------------------*/
public class PlayerMovement : MonoBehaviour
{
    public GameObject Player;
    public float stepSize = 5f;
    public Animator animator;
    public bool stepDone = true;
    public GameObject playerCanvas;
    public GameObject cam;
    public float direction = 0f;
    float normalizedDirection;
    private void Update()
    {
        Player.GetComponent<Transform>().rotation = Quaternion.Lerp(Player.transform.rotation, Quaternion.Euler(0f, direction, 0f), Time.deltaTime * 2f);
        normalizedDirection = (direction % 360 + 360) % 360;
    }
    void Start()
    {
        playerCanvas.transform.rotation = Quaternion.LookRotation(playerCanvas.transform.position - cam.transform.position);
        playerCanvas.SetActive(false);
    }
    public void Forward()
    {
        if (stepDone == false) return;
        stepDone = false;
        Step();
    }
    public void Backward()
    {
        direction += 180f;
    }
    public void Left()
    {
        direction += -90f;
    }
    public void Right()
    {
        direction += 90f;
    }

    private void Step()
    {
        playerCanvas.SetActive(true);
        Vector3 previousPosition = Player.GetComponent<Transform>().position;
        float newXPos = previousPosition.x, newZPos = previousPosition.z, newYPos = previousPosition.y;
        Debug.Log("LOG | Direction = " + direction + " | PlayerMovement.cs 51");

        //forwards
        if (normalizedDirection == 0)
        {
            newXPos = previousPosition.x;
            newZPos = previousPosition.z + stepSize;
        }
        else
        if (normalizedDirection == 90)
        {
            newXPos = previousPosition.x + stepSize;
            newZPos = previousPosition.z;
        }
        else
        if (normalizedDirection == 180)
        {
            newXPos = previousPosition.x;
            newZPos = previousPosition.z - stepSize;
        }
        else
        if (normalizedDirection == 270)
        {
            newXPos = previousPosition.x - stepSize;
            newZPos = previousPosition.z;
        }

        Player.GetComponent<Transform>().position = new Vector3(newXPos, newYPos, newZPos);
        playerCanvas.transform.rotation = Quaternion.LookRotation(playerCanvas.transform.position - cam.transform.position);
        animator.SetTrigger("Step");
        StartCoroutine(AfterStep());
    }
    IEnumerator AfterStep()
    {
        yield return new WaitForSeconds(1.7f);
        stepDone = true;
        playerCanvas.SetActive(false);
    }
}
