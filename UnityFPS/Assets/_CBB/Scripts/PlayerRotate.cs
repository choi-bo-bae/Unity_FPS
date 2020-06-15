using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{

    //카메라를 마우스 움직임에 따라 회전처리
    public float speed = 150;   //초당 150도를 움직이겠다는 것 => 회전 속도(Time.deltaTime을 통해 1초에 150도 회전)

    //회전 각도를 직접 제어하자
    float angleX;


   
    // Update is called once per frame
    void Update()
    {
        //플레이어 회전처리
        Rotate();
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");

        angleX += h * speed * Time.deltaTime;
       
        transform.eulerAngles = new Vector3(0, angleX, 0);
        
    }


  
}
