using UnityEngine;

public class VaseTrigger : MonoBehaviour {

	public bool In = false;

	public GameObject[] Vase_Stages;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			In = true;
		}
	}

	void Update()
	{
		if (In)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				//Make Noise
				Vase_Stages[0].SetActive(false);
				Vase_Stages[1].SetActive(true);
			}
		}
	}
}
