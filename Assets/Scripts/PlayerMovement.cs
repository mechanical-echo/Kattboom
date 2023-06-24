using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*--------------------------------------------
 *           Movement and rotation
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
    public bool rotationDone = true;
    public float newXPos, newYPos, newZPos;
    public Vector3 previousPos;
    public float transitionSpeed = 2f;
    public Vector3 cameraTargetPos;
    public float cameraDirection = -45f;
    private void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraTargetPos, Time.deltaTime * 1.5f);
        cam.transform.rotation = Quaternion.LookRotation(-(cam.transform.position - Player.transform.position));

        playerCanvas.transform.rotation = Quaternion.LookRotation(playerCanvas.transform.position - cam.transform.position);
        
        Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, Quaternion.Euler(0f, direction, 0f), Time.deltaTime * transitionSpeed);
        
        normalizedDirection = (direction % 360 + 360) % 360;
        
        if (Player.transform.rotation == Quaternion.Euler(0f, direction, 0f) && stepDone == true)
        {
            rotationDone = true;
           playerCanvas.SetActive(false);
        }
        else if(rotationDone==false) 
        {
            playerCanvas.SetActive(true);
        }
    }
    void Start()
    {
        
        playerCanvas.transform.rotation = Quaternion.LookRotation(playerCanvas.transform.position - cam.transform.position);
        playerCanvas.SetActive(false);
        previousPos = Player.GetComponent<Transform>().position;
    }
    public void Forward()
    {
        if (stepDone == false || rotationDone==false) return;
        stepDone = false;
        Step();
    }
    public void Left()
    {
        if (stepDone == false || rotationDone == false) return;
        rotationDone = false;
        direction += -90f;
        cameraDirection += -90f;
        normalizedDirection = (direction % 360 + 360) % 360;
        changeCameraPosition();
    }
    public void Right()
    {
        if (stepDone == false || rotationDone == false) return;
        rotationDone = false;
        direction += 90f;
        cameraDirection += 90f;
        normalizedDirection = (direction % 360 + 360) % 360;
        changeCameraPosition();
    }
    private void changeCameraPosition()
    {
        float x = Player.transform.position.x, z=Player.transform.position.z;
        Debug.Log("x = "+x+", "+z);
        Debug.Log("Direction = " + normalizedDirection);
        if (normalizedDirection == 0)
        {
            cameraTargetPos = new Vector3(x + 65, 65, z - 65);
           
        }
        else
        if (normalizedDirection == 90)
        {
            cameraTargetPos = new Vector3(x - 65, 65, z - 65);
        }
        else
        if (normalizedDirection == 180)
        {
            cameraTargetPos = new Vector3(x - 65, 65, z + 65);
        }
        else
        if (normalizedDirection == 270)
        {
            cameraTargetPos = new Vector3(x + 65, 65, z + 65);
        }
        Debug.Log(cameraTargetPos);
    }

    private void Step()
    {
        playerCanvas.SetActive(true);
        previousPos = Player.GetComponent<Transform>().position;
        newXPos = previousPos.x; 
        newZPos = previousPos.z; 
        newYPos = previousPos.y;
        float x=cam.transform.position.x, z=cam.transform.position.z;
        //forwards
        if (normalizedDirection == 0)
        {
            newXPos = previousPos.x;
            newZPos = previousPos.z + stepSize;
            z += stepSize;
        }
        else
        if (normalizedDirection == 90)
        {
            newXPos = previousPos.x + stepSize;
            newZPos = previousPos.z;
            x += stepSize;
        }
        else
        if (normalizedDirection == 180)
        {
            newXPos = previousPos.x;
            newZPos = previousPos.z - stepSize;
            z -=stepSize;
        }
        else
        if (normalizedDirection == 270)
        {
            newXPos = previousPos.x - stepSize;
            newZPos = previousPos.z;
            x -= stepSize;
        }
        Player.GetComponent<Transform>().position = new Vector3(newXPos, newYPos, newZPos);
        cameraTargetPos = new Vector3(x, 65, z);
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
