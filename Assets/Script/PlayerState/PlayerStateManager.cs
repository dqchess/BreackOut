using UnityEngine;
using System.Collections;

public class PlayerStateManager : MonoBehaviour
{

    private StateSystem m_StateSystem;      //创建一个主角状态跃迁的管理员
    void Awake()                            //只执行一次，初始化主角管理员
    {
        InitStateSystem();
    }
    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    public void Update()                   //不断的调用当前状态的跃迁过程
    {
        m_StateSystem.CurrentState.ProcessTransition();
    }

    public void FixedUpdate()              //不断的调用当前状态的跃迁过程
    {
        m_StateSystem.CurrentState.FixedUpdate();
    }
    public void InitStateSystem()         //初始化管理员
    {
        IdleState idle = new IdleState(this.gameObject, this);
        idle.AddTransition(Transition.eTransiton_Object_Walk, StateID.eStateID_Object_Walk);

        WalkState walk = new WalkState(this.gameObject, this);
        walk.AddTransition(Transition.eTransiton_Object_Idle, StateID.eStateID_Object_Idle);

        m_StateSystem = new StateSystem();
        m_StateSystem.AddState(idle);
        m_StateSystem.AddState(walk);
    }

        // walk.AddTransition(Transition.eTransiton_Object_Run, StateID.eStateID_Object_Run);
        // idle.AddTransition(Transition.eTransiton_Object_Attack1, StateID.eStateID_Object_Attack1);
        // idle.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);
        // idle.AddTransition(Transition.eTransiton_Object_Hurt, StateID.eStateID_Object_Hurt);
        // walk.AddTransition(Transition.eTransiton_Object_Attack1, StateID.eStateID_Object_Attack1);
        //walk.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);
        // walk.AddTransition(Transition.eTransiton_Object_Hurt, StateID.eStateID_Object_Hurt);

        //RunState run = new RunState(this.gameObject, this);
        // run.AddTransition(Transition.eTransiton_Object_Idle, StateID.eStateID_Object_Idle);
        // run.AddTransition(Transition.eTransiton_Object_Walk, StateID.eStateID_Object_Walk);
        //run.AddTransition(Transition.eTransiton_Object_Attack1, StateID.eStateID_Object_Attack1);
        //run.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);
        //run.AddTransition(Transition.eTransiton_Object_Hurt, StateID.eStateID_Object_Hurt);

        /* AttackState attack = new AttackState(this.gameObject, this);
         attack.AddTransition(Transition.eTransiton_Object_Walk, StateID.eStateID_Object_Walk);
         attack.AddTransition(Transition.eTransiton_Object_Run, StateID.eStateID_Object_Run);
         attack.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);
         //attack1.AddTransition (Transition.eTransiton_Object_Hurt, StateID.eStateID_Object_Hurt);

         RushState rush = new RushState(this.gameObject, this);
         rush.AddTransition(Transition.eTransiton_Object_Run, StateID.eStateID_Object_Run);
         rush.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);
         rush.AddTransition(Transition.eTransiton_Object_Idle, StateID.eStateID_Object_Idle);

         HurtState hurt = new HurtState(this.gameObject, this);
         hurt.AddTransition(Transition.eTransiton_Object_Idle, StateID.eStateID_Object_Idle);
         hurt.AddTransition(Transition.eTransiton_Object_Dead, StateID.eStateID_Object_Dead);
         hurt.AddTransition(Transition.eTransiton_Object_Rush, StateID.eStateID_Object_Rush);*/

        //DeadState dead = new DeadState(this.gameObject, this);

       // m_StateSystem = new StateSystem();
       // m_StateSystem.AddState(idle);
       // m_StateSystem.AddState(walk);
       // m_StateSystem.AddState(run);
       // m_StateSystem.AddState(attack);
       // m_StateSystem.AddState(rush);
        //m_StateSystem.AddState(hurt);
       // m_StateSystem.AddState(dead);
  
    public void SetTransition(Transition t)            //跃迁到指定状态
    {
        m_StateSystem.PerformTransition(t);
    }

    public StateID GetCurrentStateID()
    {
        return m_StateSystem.CurrentStateID;
    }

    public StateID GetLastStateID()
    {
        return m_StateSystem.LastStateID;
    }

    public State GetState(StateID id)
    {
        return m_StateSystem.GetState(id);
    }
    private void Init()
    {
        this.GetComponent<Animation>().Play("Idle");
    }
}
