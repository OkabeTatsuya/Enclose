using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : EnemyBace
{
    GameObject m_playerObj = null;
    GameObject m_gameDirectorObj = null;

    GameDirector m_gameDirectorScript = null;

    Transform m_playerTransform = null;

    float m_delta = 0;
    [SerializeField] float m_followTime = 0;
    [SerializeField] float m_coolTime = 0;

    bool m_isCoolTime = false;
    [SerializeField]  bool m_hitPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        m_playerObj = GameObject.Find("Player");
        m_gameDirectorObj = GameObject.Find("GameDirector");
        m_gameDirectorScript = m_gameDirectorObj.GetComponent<GameDirector>();

        m_playerTransform = m_playerObj.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        IsHitPlayer();
        FollowController();
    }

    void IsDes()
    {
        if (m_hp <= 0)
        {
            
        }
    }

    void FollowController()
    {
        m_delta += Time.deltaTime;

        if (m_isCoolTime)
        {
            if (m_coolTime >= m_delta)
            {

            }
            else
            {
                m_delta = 0;
                m_isCoolTime = false;
            }
        }
        else
        {
            FollowPlayer();
        }

    }

    void FollowPlayer()
    {
        Vector3 playerVec = m_playerTransform.position;
        Vector3 thisVec = this.transform.position;

        if (m_followTime >= m_delta)
        {
            Vector3 addVec = playerVec - thisVec;

            thisVec = addVec * m_speed * Time.deltaTime;
            transform.position += thisVec;
        }
        else
        {
            m_delta = 0;
            m_isCoolTime = true;
        }
    }

    void IsHitPlayer()
    {
        if (m_hitPlayer)
        {
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_hitPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_hitPlayer = false;
        }
    }
}
