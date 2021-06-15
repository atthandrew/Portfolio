using UnityEngine;
using UnityEngine.UI;

public class MatchOverMenu : MonoBehaviour
{
	public GameObject matchOverUI;
	public Button playAgain;

	public void MatchOver()
	{
		matchOverUI.SetActive(true);
		matchOverUI.GetComponentInParent<PauseMenu>().enabled = false;
		playAgain.Select();
	}
}
