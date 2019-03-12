using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{

    public GameObject m_Target = null;
    public List<GameObject> m_TargetList = new List<GameObject>();

    static private TargetManager _instance = null;
    static public TargetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TargetManager();
            }
            return _instance;
        }
    }
}
