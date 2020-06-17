using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject m_playerObj;

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_playerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //MoveCamera();
        InputController();
    }

    void MoveCamera()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.RotateAround(m_playerObj.transform.position, Vector3.up, -5f);
        }
        else if (Input.GetKey(KeyCode.RightShift))
        {
            transform.RotateAround(m_playerObj.transform.position, Vector3.up, 5f);
        }
    }

    void InputController()
    {
        Vector3 playerPos = m_playerObj.transform.position;

        float vertical = Input.GetAxis("CameraVertical");
        float horizonal = Input.GetAxis("CameraHorizontal");

        if (Mathf.Abs(horizonal) + Mathf.Abs(vertical) > 0.1f)
        {
            Vector3 cameraVec = playerPos - this.transform.position;

            float inputAngle = Mathf.Atan2(horizonal, vertical) * Mathf.Rad2Deg;
            float cameraAngle = Mathf.Atan2(cameraVec.x, cameraVec.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, inputAngle + cameraAngle, 0);

            Transform playerTrans = m_playerObj.transform;
            playerTrans.rotation = Quaternion.Slerp(playerTrans.rotation, targetRotation, Time.deltaTime * rotSpeed);
        }
    }
}
