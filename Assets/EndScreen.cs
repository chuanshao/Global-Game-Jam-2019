using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {

    public void Play_Again()
	{
		SceneManager.LoadScene(2);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
