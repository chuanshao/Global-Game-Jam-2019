using UnityEngine;

public class PlayerBox : MonoBehaviour {

	public GameObject Box;
	public Transform BoxPosition;
	public Transform Box_Origin;

	public bool Hidden = false;

    public void PutOn()
	{
		Box.SetActive(true);
		Hidden = false;
	}

	public void TakeOff()
	{
		Box.SetActive(false);	
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Hidden = !Hidden;

			if (Hidden)
			{
				Rigidbody2D rb = Box.GetComponent<Rigidbody2D>();
				rb.simulated = true;
			}
			else
			{
				Box.transform.position = Box_Origin.position;
				Rigidbody2D rb = Box.GetComponent<Rigidbody2D>();
				rb.simulated = false;
			}
		}
	}
}
