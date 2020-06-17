using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    GameObject m_gameDirectorObj;
    [SerializeField]GameObject m_particleObj;

    GameDirector m_gameDirectorScript;

    [SerializeField] bool m_hitFlag;
    //uint m_hitFlag;

    enum flagName {
        hitFlag = 0x001,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_gameDirectorObj = GameObject.Find("GameDirector");
        m_gameDirectorScript = m_gameDirectorObj.GetComponent<GameDirector>();
        foreach(Transform particle in this.gameObject.transform)
        {
            m_particleObj = particle.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DebugUpdata();
        FalseProp();
    }

    //Debugの文字表示
    void DebugUpdata()
    {
        //if((m_hitFlag & 0x001) > 0)
        //{
        //    Debug.Log("1");
        //}

        //if ((m_hitFlag & 0x002) > 0)
        //{
        //    Debug.Log("2");
        //}
    }

    void FalseProp()
    {
        if (m_gameDirectorScript.m_isResetLine)
        {
            m_hitFlag = false;
            m_particleObj.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !m_hitFlag)
        {
            m_hitFlag = true;
            m_particleObj.SetActive(true);
            m_gameDirectorScript.SetTruePropObj(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !m_hitFlag)
        {
            m_hitFlag = true;
            m_particleObj.SetActive(true);
            m_gameDirectorScript.SetTruePropObj(this.gameObject);
        }
    }
}
