using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

	public int view;
	public Vector3 One;
	public Vector3 Two;
	public Rigidbody rb;
	public float Speed = 1000;

	void Update () {

//		if(Input.GetKeyDown(KeyCode.Alpha1)){
//
//			view = 0;
//			transform.position = One;
//			transform.rotation = Quaternion.Euler (90, 0, 0);
//		}

		if (Input.GetKey (KeyCode.W) && rb.velocity.magnitude < 10){

			if (Input.GetKey(KeyCode.LeftShift)){

				rb.AddForce (transform.forward *Time.deltaTime/Time.timeScale * Speed * 10);
			}else if (rb.velocity.magnitude < 7){

				rb.AddForce (transform.forward *Time.deltaTime/Time.timeScale * Speed * 5);
			}
		}

		if (Input.GetKey (KeyCode.S) && rb.velocity.magnitude < 5){

			rb.AddForce (transform.forward *Time.deltaTime/Time.timeScale  * -Speed * 4);
		}

		if (Input.GetKey (KeyCode.A) && rb.velocity.magnitude < 5){

			rb.AddForce (transform.right *Time.deltaTime/Time.timeScale * -Speed * 4);
		}

		if (Input.GetKey (KeyCode.D) && rb.velocity.magnitude < 5){

			rb.AddForce (transform.right * Time.deltaTime/Time.timeScale  * Speed * 4);
		}

		if (Input.GetKey(KeyCode.Mouse1)){

			transform.Rotate (-Input.GetAxis ("Mouse Y"), Input.GetAxis ("Mouse X"), 0);

			Cursor.lockState = CursorLockMode.Locked;

			float z = transform.eulerAngles.z;
			transform.Rotate(0, 0, -z);
		}

		if (Input.GetKeyUp(KeyCode.Mouse1)){

			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void ViewOne(bool V1){

		view = 0;
		transform.position = One;
		transform.rotation = Quaternion.Euler (90, 0, 0);
	}
}
