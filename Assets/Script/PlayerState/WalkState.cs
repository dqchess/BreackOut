using UnityEngine;
using System.Collections;

public class WalkState : PlayerState
{
    public WalkState(GameObject obj, PlayerStateManager state)
        : base(obj, state)
    {
        _stateID = StateID.eStateID_Object_Walk;
    }

    public override void ProcessTransition()
    {
        if (m_NavAgent.velocity.magnitude <= 0.1f)             //判断寻路速度是否极接近零
        {
            m_StateManager.SetTransition(Transition.eTransiton_Object_Idle);  //若是，跃迁到站立状态
        }

        /*if (Input.GetMouseButtonDown(0))                      //若此时鼠标点击，则主角跟随移动且跃迁到跑步状态
        {
            RaycastHit hit;
            LayerMask mask = 1 << 9;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                m_NavAgent.SetDestination(hit.point);
                m_StateManager.SetTransition(Transition.eTransiton_Object_Walk);
            }
        }*/

		/*if (0 != MonsterManager.Instance.GetMonsterCount())
		{
			m_StateManager.SetTransition(Transition.eTransiton_Object_Attack1);
		}*/
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }
    public override void OnEnter()
    {
        //Debug.Log("walk");
        m_Player.PlayerAnimation("Walk");
        m_NavAgent.speed = PlayerData.SpeedWalk;

        m_AudioSource.clip = m_Player.m_WalkAudioClip;
        m_AudioSource.loop = true;
        m_AudioSource.volume = 1.0f;
        m_AudioSource.Play();
    }
    public override void OnExit()
    {

    }
}
