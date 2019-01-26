using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPointer : MonoBehaviour {

	public bool OnFirstOption = true;

    void Update()
	{
		if(Input.GetKeyDown(KeyCode.S)&&transform.position.y >= -3.83f)
		{
			transform.position = new Vector2(transform.position.x, transform.position.y - 1.5f);
			OnFirstOption = false;
			FindObjectOfType<AudioManager>().Play("Keyboard");
		}
		else if(Input.GetKeyDown(KeyCode.W)&&transform.position.y <= -2.35f)
		{
			transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f);
			OnFirstOption = true;
			FindObjectOfType<AudioManager>().Play("Keyboard");
		}

		if (OnFirstOption)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				print("Load Next Scene");
				SceneManager.LoadScene(1);
			}
		}
		else if(!OnFirstOption)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Quit();
				print("Quitting Game");
			}
		}
	}

	void Quit()
	{
		Application.Quit();
	}
}
