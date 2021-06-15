using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// PUBLIC FIELDS
	public int m_RoundsToWin = 3;
	public float m_StartDelay = 3f;
	public float m_EndDelay = 3f;

	// REFERENCES
	public DynamicCamera m_DynamicCamera;
	public Text m_MessageText;
	public MatchOverMenu matchOverUI;
	public GameObject m_PlayerPrefab;
	public PlayerManager[] m_Players;

	// PRIVATE FIELDS
	private int m_RoundNumber = 0;
	private WaitForSeconds m_StartWait;
	private WaitForSeconds m_EndWait;
	private PlayerManager m_RoundWinner;
	private PlayerManager m_GameWinner;

	// AWAKE, START

    void Start()
    {
		m_StartWait = new WaitForSeconds(m_StartDelay);
		m_EndWait = new WaitForSeconds(m_EndDelay);

		SpawnPlayers();
		SetupCamera();

		StartCoroutine(GameLoop());
    }

	// GAME LOOP FUNCTIONS
	 private void SpawnPlayers()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			// instantiate each player, set player number, establish necessary connections
			m_Players[i].m_Instance = Instantiate(m_PlayerPrefab, m_Players[i].m_SpawnPoint.position, m_Players[i].m_SpawnPoint.rotation) as GameObject;
			m_Players[i].m_PlayerNumber = i + 1;
			m_Players[i].Setup();
		}
	}

	private void SetupCamera()
	{
		Transform[] targets = new Transform[m_Players.Length];

		for (int i = 0; i < m_Players.Length; i++)
		{
			targets[i] = m_Players[i].m_Instance.transform;
		}

		m_DynamicCamera.m_Targets = targets;
	}

	private IEnumerator GameLoop()
	{
		yield return StartCoroutine(RoundStart());

		yield return StartCoroutine(RoundPlay());

		yield return StartCoroutine(RoundEnd());

		if (m_GameWinner != null)
		{
			DisablePlayerControls();
			matchOverUI.MatchOver();
		}
		else
		{
			ResetPlayers();
			StartCoroutine(GameLoop());
		}
	}

	private IEnumerator RoundStart()
	{
		// Destroy any lingering arrows
		GameObject[] arrowArray = GameObject.FindGameObjectsWithTag("Arrow");
		foreach (GameObject arr in arrowArray)
			Destroy(arr);

		// start new round
		//ResetPlayers();
		
		m_DynamicCamera.SetStartPositionAndSize();

		m_RoundNumber++;
		m_MessageText.text = "ROUND " + m_RoundNumber;

		yield return m_StartWait;
	}

	private IEnumerator RoundPlay()
	{
		m_MessageText.text = "";

		while (!OnePlayerLeft())
		{
			yield return null;
		}
	}

	private IEnumerator RoundEnd()
	{
		m_RoundWinner = null;

		m_RoundWinner = GetRoundWinner();

		if (m_RoundWinner != null)
			m_RoundWinner.m_Wins++;

		m_GameWinner = GetGameWinner();

		string message = EndMessage();
		m_MessageText.text = message;

		yield return m_EndWait;
	}

	private bool OnePlayerLeft()
	{
		int playersRemaining = 0;

		for (int i = 0; i < m_Players.Length; i++)
		{
			if (!m_Players[i].m_Movement.isDead)
				playersRemaining++;
		}

		return playersRemaining <= 1;
	}

	private PlayerManager GetRoundWinner()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			if (!m_Players[i].m_Movement.isDead)
				return m_Players[i];
		}

		return null;
	}

	private PlayerManager GetGameWinner()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			if (m_Players[i].m_Wins == m_RoundsToWin)
				return m_Players[i];
		}

		return null;
	}

	private string EndMessage()
	{
		string message = "DRAW!";

		if (m_RoundWinner != null)
			message = m_RoundWinner.m_ColoredPlayerText + " WINS!";

		message += "\n\n";

		for (int i = 0; i < m_Players.Length; i++)
		{
			message += m_Players[i].m_ColoredPlayerText + ": " + m_Players[i].m_Wins;
			message += "\n";
		}


		if (m_GameWinner != null)
			message = m_GameWinner.m_ColoredPlayerText + " IS THE CHAMPION!";

		return message;
	}

	void ResetPlayers()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			m_Players[i].Reset();
		}
	}

	void EnablePlayerControls()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			m_Players[i].EnablePlayer();
		}
	}

	void DisablePlayerControls()
	{
		for (int i = 0; i < m_Players.Length; i++)
		{
			m_Players[i].DisablePlayer();
		}
	}
}
