using UnityEngine;
using System.Collections;

public class MovementRotationController : MonoBehaviour {

	// Used to turn the turntable

    public GameObject rotateObject;
    public GameObject liftObject;
   
	public int rotateSpeed = 45; //Degrees per second

    public float liftSpeed = 1f;
    public float liftObjectMaxLiftOffset = 3f;

    private float liftObjectMinY;

    private Vector3 targetHeight;
    private Vector3 currentHeight;

	private Vector3 targetAngle;
	private Vector3 currentAngle;

    void Start()
    {
        liftObjectMinY = liftObject.transform.position.y;
        currentHeight = liftObject.transform.position;
        targetHeight = liftObject.transform.position;
    }

	void Update () {
        currentAngle = Vector3.MoveTowards(currentAngle, targetAngle, rotateSpeed * Time.deltaTime);
        currentHeight = Vector3.MoveTowards(currentHeight, targetHeight, liftSpeed * Time.deltaTime);
        rotateObject.transform.eulerAngles = new Vector3(rotateObject.transform.eulerAngles.x, currentAngle.y, rotateObject.transform.eulerAngles.z);
        liftObject.transform.position = new Vector3(liftObject.transform.position.x, currentHeight.y, liftObject.transform.position.z);

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
    {;
        targetAngle = new Vector3(0, rotateObject.transform.eulerAngles.y + 45f, 0);
    }
	public void MoveRight ()
	{
        targetAngle = new Vector3(0, rotateObject.transform.eulerAngles.y - 45f, 0);
    }

    public void MoveUp()
    {
        targetHeight = new Vector3(0, liftObjectMinY + liftObjectMaxLiftOffset, 0);
    }

    public void MoveDown()
    {
        targetHeight = new Vector3(0, liftObjectMinY, 0);
    }
}
