using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Vector3 offset;
	public GameObject player;
    public float RotSpeed = 150.0F;
    public float minY = -30.0f;
    public float maxY = 30.0f;
    float RotUpDown;
    Vector3 euler;

    // Use this for initialization
    void Start () {
		offset = transform.position - player.transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
       
    }
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = euler;
        transform.position = player.transform.position + offset;
        RotUpDown = -Input.GetAxis("Mouse Y") * RotSpeed * Time.deltaTime;
        
        // Doing movements
        euler.x += RotUpDown;

        if (euler.x >= maxY)
        {
            euler.x = maxY;
        }
        if (euler.x <= minY)
        {
            euler.x = minY;
        }
            
        /*Vector3 euler = transform.localEulerAngles;
        euler += new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * 15f;
        print(euler.x);     
        
        
        transform.localEulerAngles = euler;
        */

        //transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * 5f);
        //transform.rotation = new Quaternion(transform.rotation.x, player.transform.rotation.y, transform.rotation.z, player.transform.rotation.w);
        //transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z);
        //transform.rotation = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y"), Vector3.forward);


    }
}
