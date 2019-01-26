using UnityEngine;

public class ClothingDrop : MonoBehaviour {

	BoxCollider2D collider;
	Rigidbody2D rb;

	void Start()
	{
		collider = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Ground")
		{
			collider.enabled = false;
			rb.simulated = false;
		}
	}
}
