using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float m_playerHp = 10;
    [SerializeField] float m_speed = 10;

    [SerializeField] uint m_playerFlag = 0;

    float m_undefeatableTime = 0.0f;
    [SerializeField] float m_maxUndefeatableTime = 3.0f;

    enum flagName
    {
        damage = 0x001,
        invincible = 0x002,
        ded = 0x004,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        InputController();
        InvincivleTime();
        IsDamage();
        IsDed();
    }

    //public void DamagePlayer(float damage)
    //{
    //    if ((m_playerFlag & (uint)flagName.damage) > 0)
    //    {
    //        m_playerHp -= damage;
    //        m_playerFlag &= ~(uint)flagName.damage;
    //    }
    //}

    void IsDed()
    {
        if (m_playerHp <= 0)
        {
            m_playerFlag |= (uint)flagName.ded;
        }
    }

    /// <summary>
    /// ゲームパッド入力処理
    /// </summary>
    void InputController()
    {
        float rStickH = Input.GetAxis("PlayerMoveHorizontal");
        float rStickV = Input.GetAxis("PlayerMoveVertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        if (Mathf.Abs(rStickH) + Mathf.Abs(rStickV) > 0.1f)
        {
            Vector3 moveVec = new Vector3();
            Vector3 addVec = new Vector3();

            moveVec = (forward * rStickV + right * rStickH).normalized;
            addVec = moveVec * m_speed * Time.deltaTime;
            transform.position += addVec;
        }
    }

    /// <summary>
    /// キーボード入力処理
    /// </summary>
    void InputKey()
    {
        Vector3 forward = transform.forward.normalized;
        Vector3 right = transform.right.normalized;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += forward * m_speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= forward * m_speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += right * m_speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= right * m_speed * Time.deltaTime;
        }
    }

    void IsDamage()
    {
        if ((m_playerFlag & (uint)flagName.damage) > 0 && (m_playerFlag & (uint)flagName.invincible) > 0)
        {
            m_playerHp--;
            m_playerFlag &= ~(uint)flagName.damage;
        }
    }

    void InvincivleTime()
    {
        if ((m_playerFlag & (uint)flagName.invincible) > 0)
        {
            if (m_undefeatableTime < m_maxUndefeatableTime)
            {
                m_undefeatableTime += Time.deltaTime;
            }
            else
            {
                m_undefeatableTime = 0.0f;
                m_playerFlag &= ~(uint)flagName.invincible;
            }
        }
    }

    void Flasing()
    {
        //Color a = this.gameObject.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            m_playerFlag |= (uint)flagName.damage;
            m_playerFlag |= (uint)flagName.invincible; 
        }
    }
}
