using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    private List<Vector3> m_bornPos = new List<Vector3>();
    // Use this for initialization

    void Awake()
    {
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            m_bornPos.Add(this.transform.GetChild(i).position);
        }
    }

    void Start()
    {
        for (int i = 0; i < 5; ++i)
        {
            BornMonster();
        }
        this.InvokeRepeating("BornMonster", 5.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BornMonster()
    {
        if (MonsterManager.Instance.count < 12)
        {
            int random = UnityEngine.Random.Range(0, m_bornPos.Count);
            MonsterManager.Instance.CreateMonster(m_bornPos[random]);
        }
    }

}
