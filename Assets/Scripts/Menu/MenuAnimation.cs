using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	Animator anim;
	public bool Times_Up = false;

    void Start()
    {
		anim = GetComponent<Animator>();
		StartCoroutine(Play_Animation());
		FindObjectOfType<AudioManager>().Play("Background 1");
    }

	IEnumerator Play_Animation()
	{
		int Rand_Time = Random.Range(0, 11);
		yield return new WaitForSeconds(Rand_Time);
		Times_Up = true;
		anim.SetBool("Timeup", Times_Up);
		FindObjectOfType<AudioManager>().Play("Laser");
		yield return new WaitForSeconds(1f);
		Times_Up = false;
		anim.SetBool("Timeup", Times_Up);
		StartCoroutine(Play_Animation());
	}
}
