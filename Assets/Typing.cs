using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Typing : MonoBehaviour {

	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";

	void Start()
	{
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for(int i = 0; i < fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i);
			this.GetComponent<Text>().text = currentText;
			yield return new WaitForSeconds(delay);
			FindObjectOfType<AudioManager>().Play("Keyboard");
		}

		print("Done");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(2);
	}
}
