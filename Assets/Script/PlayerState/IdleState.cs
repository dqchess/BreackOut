using UnityEngine;
using System.Collections;

public class IdleState : PlayerState //站立状态，继承了主角状态基类
{

    public IdleState(GameObject obj, PlayerStateManager state) //构造函数，继承子类的两个变量
        : base(obj, state)
    {
        _stateID = StateID.eStateID_Object_Idle;               //初始化状态ID为站立的ID
    }

    public override void ProcessTransition()                   //跃迁过程函数
    {
        //   Debug.Log(m_NavAgent.velocity.magnitude);
        if (m_NavAgent.velocity.magnitude > 0)                 //若寻路速度大于零
        {
            m_StateManager.SetTransition(Transition.eTransiton_Object_Run); //调用跃迁函数（参数为跑）
        }
    }
    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }
    public override void OnEnter()                           //刚进入状态时主角执行动作
    {
        if (null != m_Animation)
        {
            m_Player.PlayerAnimation("Idle");
        }
        //m_NavAgent.Stop ();
    }

    public override void OnExit()
    {
        m_NavAgent.Resume();                                //重新开始寻路
    }
}
