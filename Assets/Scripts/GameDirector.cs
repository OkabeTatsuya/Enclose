using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    GameObject m_playerObj;
    GameObject m_enemies;
    [SerializeField]List<GameObject> m_lineObj;

    List<LineRenderer> m_lineRenderer = new List<LineRenderer>();
    List<EnemyScript> m_enemyScript = new List<EnemyScript>();
    Player m_playerScript;

    public int m_maxPropObjNum = 3;
    public List<GameObject> m_truePropObj = new List<GameObject>();

    public float m_triTime = 0.0f;
    public float m_visivleTriTime = 3.0f;

    uint gameFlag;

    public bool m_isCreateTri = false;
    public bool m_isResetLine = false;

    enum FlagName
    {

    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetingTruePropObj();
        SearchObject();
    }

    // Update is called once per frame
    void Update()
    {
        IsMaxPropObj();
        ReSetTruePropObj();
    }

    //Setter
    public void SetTruePropObj(GameObject obj)
    {
        for(int i = 0; i < m_maxPropObjNum; i++)
        {
            if (m_truePropObj[i] == null)
            {
                m_truePropObj[i] = obj;
                break;
            }
        }
    }

    public void DamagePlayer(float damage)
    {
        //m_playerScript.DamagePlayer(damage);
    }

    void SetingTruePropObj()
    {
        for (int i = 0; i < m_maxPropObjNum; i++)
        {
            m_truePropObj.Add(null);
        }
    }

    //Propの中身をnullにする
    void ReSetTruePropObj()
    {
        if (m_isCreateTri)
        {
            m_triTime += Time.deltaTime;

            if (m_triTime > m_visivleTriTime)
            {
                m_isResetLine = true;
            }
        }
        else {
            m_isResetLine = false;
        }

        if (m_isResetLine)
        {
            for (int i = 0; i < m_truePropObj.Count; i++)
            {
                m_truePropObj[i] = null;
            }

            for (int i = 0; i < m_lineObj.Count; i++)
            {
                m_lineObj[i].SetActive(false);
            }

            m_isCreateTri = false;
            m_triTime = 0.0f;
        }
    }

    //オブジェクトやスクリプトを取ってくる
    void SearchObject()
    {
        m_playerObj = GameObject.Find("Player");
        m_enemies = GameObject.Find("Enemies");
        //m_lineObj = GameObject.Find("AreaLine");

        m_playerScript = m_playerObj.GetComponent<Player>();
        for (int i = 0; i < m_lineObj.Count; i++)
        {
            m_lineRenderer.Add(m_lineObj[i].GetComponent<LineRenderer>());
        }

        foreach (Transform enemyObj in m_enemies.transform)
        {
            m_enemyScript.Add(enemyObj.gameObject.GetComponent<EnemyScript>());
        }
    }

    //ベクトルの引き算
    Vector2 SubVector(Vector3 vectorA,Vector3 vectorB)
    {
        Vector2 ret;
        ret.x = vectorA.x - vectorB.x;
        ret.y = vectorA.z - vectorB.z;

        return ret;
    }

    //ベクトルの外積
    Vector2 CrossProduct(Vector3 vl, Vector3 vr)
    {
        Vector3 ret;
        ret.x = vl.y * vr.z - vl.z * vr.y;
        ret.y = vl.z * vr.x - vl.x * vr.z;
        ret.z = vl.x * vr.y - vl.y * vr.x;

        return ret;
    }

    //ベクトルの内積
    float DotProduct(Vector3 vl, Vector3 vr)
    {
        return vl.x * vr.x + vl.y * vr.y + vl.z * vr.z;
    }

    void IsMaxPropObj()
    {
        CreateLineObj();

        if (m_truePropObj[2] != null)
        {
            Vector3[] truePropPos = new Vector3[3];
            for(int i = 0; i < m_truePropObj.Count; i++)
            {
                truePropPos[i] = m_truePropObj[i].transform.position;
            }

            foreach (Transform enemyObj in m_enemies.transform)
            {
                HitTriangle(truePropPos[0], truePropPos[1], truePropPos[2], enemyObj.gameObject);
            }
        }
    }

    //敵が三角形の中にいた時の処理
    void HitTriangle(Vector3 propA, Vector3 propB, Vector3 propC, GameObject enemy)
    {
        Vector3 enemyPos = enemy.transform.position;

        Vector2 AB = SubVector(propB, propA);
        Vector2 BP = SubVector(enemyPos, propB);

        Vector2 BC = SubVector(propC, propB);
        Vector2 CP = SubVector(enemyPos, propC);

        Vector2 CA = SubVector(propA, propC);
        Vector2 AP = SubVector(enemyPos, propA);

        float c1 = AB.x * BP.y - AB.y * BP.x;
        float c2 = BC.x * CP.y - BC.y * CP.x;
        float c3 = CA.x * AP.y - CA.y * AP.x;

        if ((c1 > 0 && c2 > 0 && c3 > 0) || (c1 < 0 && c2 < 0 && c3 < 0))
        {
            Destroy(enemy);
        }
        else
        {

        }
    }

    void CreateLineObj()
    {
        if (m_truePropObj[1] != null)
        {
            m_lineObj[0].SetActive(true);
            m_lineRenderer[0].SetPosition(0, m_truePropObj[0].transform.position);
            m_lineRenderer[0].SetPosition(1, m_truePropObj[1].transform.position);
            m_lineRenderer[0].startWidth = 0.5f;
            m_lineRenderer[0].endWidth = 0.5f;
        }
        else
        {
            //m_lineObj[0].SetActive(false);
        }

        if (m_truePropObj[2] != null)
        {
            m_lineObj[1].SetActive(true);
            m_lineRenderer[1].SetPosition(0, m_truePropObj[1].transform.position);
            m_lineRenderer[1].SetPosition(1, m_truePropObj[2].transform.position);
            m_lineRenderer[1].startWidth = 0.5f;
            m_lineRenderer[1].endWidth = 0.5f;

            m_lineObj[2].SetActive(true);
            m_lineRenderer[2].SetPosition(0, m_truePropObj[2].transform.position);
            m_lineRenderer[2].SetPosition(1, m_truePropObj[0].transform.position);
            m_lineRenderer[2].startWidth = 0.5f;
            m_lineRenderer[2].endWidth = 0.5f;

            m_isCreateTri = true;
        }
    }
}
