using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
	PlayerControls controls;

	private void Awake()
	{
		controls = new PlayerControls();
	}

	// Title screen functions
	public void PlayGame()
	{
		SceneManager.LoadScene("ReadyLevelSelect");
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	// Level selection functions
	public void TitleScreen() { SceneManager.LoadScene("TitleScreen"); }
	public void Wind1()	{ SceneManager.LoadScene("Wind1"); }
	public void Wind2()	{ SceneManager.LoadScene("Wind2"); }
	public void Wind3()	{ SceneManager.LoadScene("Wind3"); }

	// other items
	private void OnEnable() { controls.Enable(); }
	private void OnDisable() { controls.Disable(); }
}
