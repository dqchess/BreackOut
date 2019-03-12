using UnityEngine;
using System.Collections;

public class MonsterData
{
    public int id;
    public float fireRange;
    public float intonateTime;
    public float runSpeed;
    public int maxHp;
    public int curHp;
    public MonsterData()
    {
        id = 0;
        fireRange = 2.0f;
        intonateTime = 1.5f;
        runSpeed = 3.0f;
        maxHp = 100;
        curHp = 100;
    }
}
public class Monster : MonoBehaviour
{
    private MonsterData m_mosterData = new MonsterData();
    public MonsterData MonsterData
    {
        get { return m_mosterData; }
        set { m_mosterData = value; }
    }
    void Awake()
    {
        UnityEngine.AI.NavMeshHit closestHit;
        if (UnityEngine.AI.NavMesh.SamplePosition(this.transform.position, out closestHit, 500, 1))
        {
            this.transform.position = closestHit.position;
            UnityEngine.AI.NavMeshAgent agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = m_mosterData.runSpeed;
            }
        }
    }
    public void Init(int id)
    {
        m_mosterData.id = id;
    }

    public void OnHurt(int hp)
    {
        m_mosterData.curHp -= hp;
    }
}
