using UnityEngine;
using System.Collections;

public abstract class PlayerState : State
{
    public GameObject m_Model;
    public Animation m_Animation;
    public UnityEngine.AI.NavMeshAgent m_NavAgent;
  //  public AudioSource m_AudioSource;
    public PlayerStateManager m_StateManager;
    public Player m_Player;

    public AudioSource m_AudioSource;

    public PlayerState(GameObject obj, PlayerStateManager state)
    {
        m_Model = obj;
        m_Animation = m_Model.GetComponent<Animation>();
        m_NavAgent = m_Model.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_AudioSource = m_Model.GetComponent<AudioSource>();
        m_StateManager = state;
        m_Player = m_Model.GetComponent<Player>();

        m_AudioSource = m_Model.GetComponent<AudioSource>();
    }
}
