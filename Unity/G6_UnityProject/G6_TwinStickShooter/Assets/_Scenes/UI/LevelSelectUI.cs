using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
	public void PlayWind1()
	{
		SceneManager.LoadScene("Wind1");
	}

	public void PlayWind2()
	{
		SceneManager.LoadScene("Wind2");
	}

	public void PlayWind3()
	{
		SceneManager.LoadScene("Wind3");
	}
}
