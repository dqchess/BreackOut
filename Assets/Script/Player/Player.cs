using UnityEngine;
using System.Collections;
using System;
#region 寻路组件
/* 
public class Player : MonoBehaviour
{

    NavMeshAgent m_NavAgent;
    Animation m_anim;
    // Use this for initialization
    void Start()
    {
        m_NavAgent = this.GetComponent<NavMeshAgent>();
        m_anim = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask mask = 1 << 8;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                this.transform.LookAt(hit.point);
                m_NavAgent.SetDestination(hit.point);
                m_anim.Play("run");
            }

        }
    }
}
*/
#endregion
public class Player : MonoBehaviour
{
    public static Player MainPlayer = null;
    private UnityEngine.AI.NavMeshAgent m_NavAgent;
    private Animation m_anim;
    public float m_speed = 5f;
    //private bool isDoubleClick = false;
    private PlayerStateManager m_StateManager;
   // public GameObject m_AttackTarget;

    //public AudioClip m_RunAudioClip;
    public AudioClip m_WalkAudioClip;
    //public AudioClip m_AttackAudioClip02;
    // AudioClip m_HurtAudio;
    //public AudioClip m_DeadAudio;
    //public AudioClip m_RushAudio;

    void Awake()
    {
        MainPlayer = this;
    }
    // Use this for initialization
    void Start()
    {
        m_NavAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_StateManager = this.GetComponent<PlayerStateManager>();
        m_anim = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) //前
        //{
        //    this.transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow)) //后
        //{
        //    this.transform.Translate(Vector3.forward * -m_speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
        //{
        //    this.transform.Translate(Vector3.right * -m_speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
        //{
        //    this.transform.Translate(Vector3.right * m_speed * Time.deltaTime);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            print("MouseDown1");
            //1. 获取鼠标点击位置
            //创建射线;从摄像机发射一条经过鼠标当前位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //发射射线
            RaycastHit hitInfo = new RaycastHit();
            LayerMask mask = 1 << 8;
            if (Physics.Raycast(ray, out hitInfo, mask))
            {
                print("MouseDown2");
                //获取碰撞点的位置
                m_NavAgent.SetDestination(hitInfo.point);
            }
        }
    }
    /*if (Input.GetKey(KeyCode.W))
    {
        if (m_StateManager.GetCurrentStateID() != StateID.eStateID_Object_Dead)
        {
            if (isDoubleClick)
            {

                RaycastHit hit;
                LayerMask mask = 1 << 9;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    this.transform.LookAt(hit.point);
                    Debug.Log(hit.point);
                }
                m_StateManager.SetTransition(Transition.eTransiton_Object_Rush);
                isDoubleClick = false;

            }
            else
            {
                isDoubleClick = true;
                StartCoroutine(CheckDoubleClick(0.2f));
            }

            if (m_StateManager.GetCurrentStateID() != StateID.eStateID_Object_Rush &&
               m_StateManager.GetCurrentStateID() != StateID.eStateID_Object_Walk)
            {
                if (m_StateManager.GetCurrentStateID() != StateID.eStateID_Object_Dead)
                {
                    RaycastHit hit;
                    LayerMask mask = 1 << 9;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                    {
                        this.transform.LookAt(hit.point);
                        m_NavAgent.SetDestination(hit.point);
                        Debug.Log(hit.point);
                    }
                }
            }
        }
    }


}*/
   
    public void PlayerAnimation(string clip)
     {
         if (m_anim != null)
             m_anim.CrossFade(clip);
     }

    /* *   private IEnumerator CheckDoubleClick(float time)
     {
         yield return new WaitForSeconds(time);
         isDoubleClick = false;
     }


     void OnTriggerEnter(Collider collider)
     {
         if (collider.name == "Claw Left" || collider.name == "Claw Right")
         {
             //Debug.Log ("attack happen");
             if (m_StateManager.GetCurrentStateID() != StateID.eStateID_Object_Hurt)
             {
                 m_AttackTarget = collider.gameObject;
                 //Debug.Log("to hurt");
                 m_StateManager.SetTransition(Transition.eTransiton_Object_Hurt);
                 PlayerData.HP -= 10;
                 if (PlayerData.HP <= 0)
                 {
                     m_StateManager.SetTransition(Transition.eTransiton_Object_Dead);
                 }
             }
         }

     }*/
}