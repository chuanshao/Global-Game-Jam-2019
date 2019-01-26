using UnityEngine;

public class PlayerClothing : MonoBehaviour {

	public GameObject InHand;
	public GameObject Dropped;

	public Transform DropPosition;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Dropped.SetActive(true);
			Dropped.transform.position = DropPosition.position;
			InHand.SetActive(false);
		}
	}
}
