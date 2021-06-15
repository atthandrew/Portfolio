using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerManager
{
	// PLAYER INFORMATION
	public Color m_PlayerColor;
	public Transform m_SpawnPoint;

	[HideInInspector] public int m_PlayerNumber;
	[HideInInspector] public string m_ColoredPlayerText;
	[HideInInspector] public GameObject m_Instance;
	[HideInInspector] public int m_Wins;

	[HideInInspector] public ArcherMovement m_Movement;
	private ArcherShooting m_Shooting;

	public void Setup()
	{
		// establish component connections
		m_Movement = m_Instance.GetComponent<ArcherMovement>();
		m_Shooting = m_Instance.GetComponent<ArcherShooting>();

		// set player numbers
		m_Movement.playerNumber = m_PlayerNumber;
		m_Shooting.playerNumber = m_PlayerNumber;
		m_Shooting.playerColor = m_PlayerColor;

		// setup player ui label
		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

		// change indicator color
		m_Instance.GetComponentInChildren<SpriteRenderer>().color = m_PlayerColor;
	}

	// disables player movement and shooting
	public void DisablePlayer()
	{
		m_Instance.GetComponent<PlayerInput>().enabled = false;
	}

	// enables player movement and shooting
	public void EnablePlayer()
	{
		m_Movement.enabled = true;
		m_Shooting.enabled = true;
	}

	// Move players back to spawn points
	public void Reset()
	{
		m_Movement.RespawnReset();
		m_Shooting.RespawnReset();

		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;
	}
}
