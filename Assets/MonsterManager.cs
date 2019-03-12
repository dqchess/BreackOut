using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager
{

    private static MonsterManager m_instance = null;
    public static MonsterManager Instance
    {
        //int Count = MonsterID;
        get
        {
            if (m_instance == null)
                m_instance = new MonsterManager();
            return m_instance;
        }
    }



    public static int MonsterID = 0;
    //private static int MonsterID = 0;
    private Dictionary<int, Monster> m_monsterDic = new Dictionary<int, Monster>();

    public void CreateMonster(Vector3 pos)
    {
        Object perfeb = Resources.Load("Toon Barbarian-Blue");
        if (perfeb != null)
        {
            GameObject monsterObj = GameObject.Instantiate(perfeb) as GameObject;
            if (monsterObj != null)
            {
                monsterObj.transform.position = pos;
                Monster monster = monsterObj.GetComponent<Monster>();
                if (monster != null)
                {
                    ++MonsterID;
                    m_monsterDic.Add(MonsterID, monster);
                    monster.Init(MonsterID);
                }
            }
        }
    }


    public int count
    {
        //int Count = MonsterID;
        get { return m_monsterDic.Count; }
    }

    public int GetMonsterCount()
    {
        int count = 0;
        if (Player.MainPlayer != null)
        {
            Player player = Player.MainPlayer;
            List<GameObject> monsters = TargetManager.Instance.m_TargetList;
            monsters.Clear();
            GameObject target = null;

            float minAngle = PlayerData.AttackAngle2;
            float minDistance = PlayerData.AttackDistance1;
            foreach (Monster obj in m_monsterDic.Values)
            {
                float distance = Vector3.Distance(obj.transform.position, player.transform.position);
                float angle = Vector3.Dot(player.transform.forward, (obj.transform.position - player.transform.position));
                if (distance < PlayerData.AttackDistance1)
                {
                    angle = Mathf.Acos(angle) / Mathf.PI * 180.0f;
                    if (angle < PlayerData.AttackAngle1)
                    {
                        count++;
                        monsters.Add(obj.gameObject);
                        if (angle < minAngle)
                        {
                            minAngle = angle;
                            target = obj.gameObject;
                        }
                    }

                    if (distance < PlayerData.AttackDistance2)
                    {
                        if (angle < PlayerData.AttackAngle2)
                        {
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                if (target == null)
                                {
                                    target = obj.gameObject;
                                }
                            }
                            if (!monsters.Contains(obj.gameObject))
                            {
                                count++;
                                monsters.Add(obj.gameObject);
                            }
                        }
                    }
                    //monsters.Add (obj.gameObject);
                }
            }
            TargetManager.Instance.m_Target = target;

        }
        return count;
    }

    public void DestroyMonster(Monster monster)
    {
        if (TargetManager.Instance.m_Target != null)
        {
            Monster targetMonster = TargetManager.Instance.m_Target.GetComponent<Monster>();
            if (monster == targetMonster)
            {
                TargetManager.Instance.m_Target = null;
            }
            if (TargetManager.Instance.m_TargetList.Contains(monster.gameObject))
            {
                TargetManager.Instance.m_TargetList.Remove(monster.gameObject);
            }
        }
        m_monsterDic.Remove(monster.MonsterData.id);
        GameObject.Destroy(monster.gameObject);
    }

}