using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
	public GameObject p1;
	public Transform p1Spawn;
	private int p1Score;
	
	public GameObject p2;
	public Transform p2Spawn;
	private int p2Score;

	public GameObject roundOverUI;
	public GameObject pauseUI;
	public EventSystem es;

	public int pointsToWin = 3;
	public float respawnDelay = 2f;

	public List<GameObject> firstSelection;
	PlayerControls controls;
	private bool isPaused;

	private void Awake()
	{
		controls = new PlayerControls();

		controls.UI.Pause.started += OnPause;
	}

	// Start is called before the first frame update
	void Start()
	{
		pauseUI.SetActive(false);
		isPaused = false;
	}

	// PAUSE FUNCTIONS

	public void OnPause(InputAction.CallbackContext ctx)
	{
		if (isPaused)
			UnPause();
		else
			Pause();
	}

	public void Pause()
	{
		es.firstSelectedGameObject = firstSelection[0];
		isPaused = true;
		pauseUI.SetActive(true);
		Time.timeScale = 0f;
	}

	public void UnPause()
	{
		isPaused = false;
		pauseUI.SetActive(false);
		Time.timeScale = 1f;
	}

	public void QuitToTitle()
	{
		SceneManager.LoadScene(0);
	}

	public void GoToLevelSelect()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("ReadyLevelSelect");
	}

	// THE REST OF THE FUNCTIONS

	public void RoundOver(Object go)
	{
		// Award win to the correct player
		if ((go.name.Equals("Player1")))
			p2Score++;
		else
			p1Score++;

		// show win message

		if (p1Score >= pointsToWin || p2Score >= pointsToWin)
		{
			// end game
			GameOver();
		}
		else
		{
			// Reset positions
			//Invoke("Respawn", respawnDelay);
			Respawn();
		}
	}

	void Respawn()
	{
		Debug.Log("Respawning");
		
		// destroy all arrows
		GameObject[] arrowArray = GameObject.FindGameObjectsWithTag("Arrow");
		foreach (GameObject arr in arrowArray)
			Destroy(arr);

		p1.transform.position = p1Spawn.position;
		p2.transform.position = p2Spawn.position;
		
		Time.timeScale = 1f;
	}

	public void PlayAgain()
	{
		p1Score = 0;
		p2Score = 0;
		roundOverUI.SetActive(false);
		Respawn();
	}

	void GameOver()
	{
		Debug.Log("GameOver");
		es.firstSelectedGameObject = firstSelection[1];
		roundOverUI.SetActive(true);
		Time.timeScale = 0f;
	}

	// other items
	private void OnEnable() { controls.Enable(); }
	private void OnDisable() { controls.Disable(); }
}
