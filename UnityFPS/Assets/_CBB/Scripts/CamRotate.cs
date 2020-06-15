using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //카메라를 마우스 움직임에 따라 회전처리
    public float speed = 150;   //초당 150도를 움직이겠다는 것 => 회전 속도(Time.deltaTime을 통해 1초에 150도 회전)

    //회전 각도를 직접 제어하자
    float angleX, angleY;
    

    

    // Update is called once per frame
    void Update()
    {
        //카메라 회전처리
        Rotate();
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        // Vector3 dir = new Vector3(h, v, 0);
        //이렇게 가면 안 됨. 각각의 축을 기반으로 회전해야 함

        // Vector3 dir = new Vector3(-v, h, 0);

        //1번
        //transform.Rotate(dir * speed * Time.deltaTime);
        //문제점
        //유니티 엔진에서 제공해주는 함수를 사용함에 있어서
        //Translate함수는 그래도 사용하는데 큰 불편함이 없지만
        //회전을 담당하는 Rotate함수를 사용하면
        //우리가 제어하기가 힘들다.
        //인스펙터 창의 로테이션 값은 오일러 각도로 표기되어 있으나, 
        //내부적으로는 쿼터니언(짐벌락 현상이 일어나지 않음)으로 작동하기 때문에 짐벌락 현상이 일어난다.
        //회전을 직접 제어 할 때는 Rotate함수는 사용하지 않고
        //트렌스폼의 오일러앵글을 사용하면 된다.

        //2번
        //transform.eulerAngles += dir * speed * Time.deltaTime;
        //문제점
        //카메라 문제(-90 ~ 90)도 까지밖에 안 움직임. 이후 이상한 값이 들어가버린다.
        //직접 회전각도를 제한해서 처리하면 된다.


        //3번
        //Vector3 angle = transform.eulerAngles;
        //angle += dir * speed * Time.deltaTime;
        //if (angle.x > 60) angle.x = 60;
        //if (angle.x < -60) angle.x = -60;
        //transform.eulerAngles = angle;
        //문제점
        //아래로 60도는 멀쩡히 돌아가나, 위로 올리다보면 60도로 다시 돌아가게 된다.
        //유니티 내부에서 -각도는 360도를 더해서 처리된다.
        //내가 만든 각도를 가지고 처리해야 한다.


        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -60, 60);
        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
        //캐릭터 선택 전 돌려볼 수 있음

    }




}
