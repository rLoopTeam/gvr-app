///Author: Michael House 
///Date: 1/27/2016

using UnityEngine;
using System.Collections;

public class MovementRotationController : MonoBehaviour {

	// Used to turn the turntable

    public GameObject rotateObject;
    public GameObject[] liftObjects;

    int currentLiftIndex = 0;
    int targetLiftIndex = 0;
   
	public float rotateSpeed = 1; //Seconds per 45 degrees

    public float liftSpeed = 1f;
    public float liftObjectMaxLiftOffset = 3f;

    private float[] liftObjectMinY;
    private Vector3[] currentHeight;
    private Vector3[] targetHeight;
    private Coroutine[] liftCorroutines;

    private Quaternion startRotation;
    private Quaternion endRotation;
    float timeRotating;
    

    void Start()
    {
        liftObjectMinY = new float[liftObjects.Length];
        currentHeight = new Vector3[liftObjects.Length];
        targetHeight = new Vector3[liftObjects.Length];
        liftCorroutines = new Coroutine[liftObjects.Length];
        startRotation = endRotation = rotateObject.transform.rotation;

        for (int i = 0; i < liftObjects.Length; i++)
        {
            liftObjectMinY[i] = liftObjects[i].transform.position.y;
            currentHeight[i] = liftObjects[i].transform.position;
            targetHeight[i] = liftObjects[i].transform.position;
        }
    }

    IEnumerator LiftLowerObject(int objectIndex)
    {
        do
        {
            currentHeight[objectIndex] = Vector3.MoveTowards(currentHeight[objectIndex], targetHeight[objectIndex], liftSpeed * Time.deltaTime);
            liftObjects[objectIndex].transform.position = new Vector3(liftObjects[objectIndex].transform.position.x, currentHeight[objectIndex].y, liftObjects[objectIndex].transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        while (currentHeight[objectIndex].y != targetHeight[objectIndex].y);
    }

	void Update () {
        if (timeRotating < rotateSpeed)
        {
            timeRotating += Time.deltaTime;
            rotateObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeRotating / rotateSpeed);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveUp();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveDown();
    }

    public void MoveLeft ()
    {
        startRotation = rotateObject.transform.rotation;
        endRotation = Quaternion.AngleAxis(45, rotateObject.transform.up) * startRotation;
        timeRotating = 0;
    }
	public void MoveRight ()
	{
        startRotation = rotateObject.transform.rotation;
        endRotation = Quaternion.AngleAxis(-45, rotateObject.transform.up) * startRotation;
        timeRotating = 0;
    }

    public void MoveUp()
    {
        if (currentLiftIndex >= liftObjects.Length)
            return;

        targetHeight[currentLiftIndex] = new Vector3(0, liftObjectMinY[currentLiftIndex] + liftObjectMaxLiftOffset, 0);
        if (liftCorroutines[currentLiftIndex] != null)
            StopCoroutine(liftCorroutines[currentLiftIndex]);
        StartCoroutine(LiftLowerObject(currentLiftIndex));
        currentLiftIndex++;
    }

    public void MoveDown()
    {
        if (currentLiftIndex <= 0)
            return;

        currentLiftIndex--;

        targetHeight[currentLiftIndex] = new Vector3(0, liftObjectMinY[currentLiftIndex], 0);
        if (liftCorroutines[currentLiftIndex] != null)
            StopCoroutine(liftCorroutines[currentLiftIndex]);
        StartCoroutine(LiftLowerObject(currentLiftIndex));
    }
}
