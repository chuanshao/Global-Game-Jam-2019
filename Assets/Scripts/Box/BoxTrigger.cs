using UnityEngine;

public class BoxTrigger : MonoBehaviour {

	public bool In = false;
	public bool PickedUp = false;

	PlayerBox box;

	private SpriteRenderer rend;

	void Start()
	{
		rend = GetComponent<SpriteRenderer>();
	}

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			In = true;
			box = other.gameObject.GetComponent<PlayerBox>();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			In = false;
		}
	}

	void Update()
	{
		if(In)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				PickedUp = !PickedUp;

				if (!PickedUp)
				{
					box.PutOn();
					rend.enabled = false;
					In = true;
				}
				else
				{
					box.TakeOff();
					rend.enabled = true;
					transform.position = box.BoxPosition.position;
				}
			}
		}
	}
}
