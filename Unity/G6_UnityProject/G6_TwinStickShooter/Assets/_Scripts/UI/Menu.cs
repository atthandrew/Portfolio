using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	// title screen
	public void PlayGame() { SceneManager.LoadScene("ReadyLevelSelect"); }
	public void QuitGame() { Application.Quit(); }

	// level select
	public void TitleScreen() { SceneManager.LoadScene("TitleScreen"); }
	public void Wind1() { SceneManager.LoadScene("Wind1"); }
	public void Wind2() { SceneManager.LoadScene("Wind2"); }
	public void Wind3() { SceneManager.LoadScene("Wind3"); }
	public void Wind4() { SceneManager.LoadScene("Wind4"); }

	// in-game menus
	public void PlayAgain() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
	public void QuitToTitle() { SceneManager.LoadScene("TitleScreen"); }
	public void GoToLevelSelect() 
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("ReadyLevelSelect"); 
	}
}
