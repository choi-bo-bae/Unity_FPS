using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //카메라는 플레이어의 따라 다녀야 한다.
    //플레이어한테 바로 카메라를 붙여서 이동해도 상관은 없으나
    //게임에 따라 드라마틱한 연출(혹은 시점 전환)이 불가능하므로
    //타겟을 따라다니게 하면서 1인칭으로 혹은 3인칭으로 변경이 쉽다
    //또한 순간이동이 아닌 슈팅게임에서 꼬리가 따라다니는 것 같은 효과도 연출이 가능.
    //지금은 눈 역할을 하기 때문에 순간이동 하듯이 처리.

    public Transform target;

    public float followSpeed = 5.0f;

    //private bool follow = false;

    public Transform target1st;
    public Transform target3rd;

    private bool isFPS = true;

    

    // Update is called once per frame
    void Update()
    {
        //카메라의 위치를 강제로 타겟 위치에 고정
        //transform.position = target.transform.position;

        //FollowTarget();

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    follow = true;
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    follow = false;   
        //}

        //1 to 3, 3 to 1인칭
        ChangeView();
    }

    void ChangeView()
    {
        if(Input.GetKeyDown("1"))
        {
            isFPS = true;

        }
        if(Input.GetKeyDown("3"))
        {
            isFPS = false;
        }

        if(isFPS)
        {
            transform.position = target1st.transform.position;
        }
        else
        {
            transform.position = target3rd.transform.position;
        }
    }

    //타겟 따라다니기
    //private void FollowTarget()
    //{

    //    if (follow == true)    //1인칭
    //    {
    //        transform.position = target.transform.position;         
    //    }


    //    if (follow == false)   //3인칭
    //    {
    //        transform.position = target.transform.position - new Vector3(0, -3, 10);
    //        //방향 = 타겟 - 자신
    //        Vector3 dir = target.transform.position - transform.position;
    //        dir.Normalize();
    //    }



    //    //방향 = 타겟 - 자신
    //    //Vector3 dir = target.transform.position - transform.position;
    //    //dir.Normalize();

    //    //transform.Translate(dir * followSpeed * Time.deltaTime);

    //    ////문제가 있음 -> 정확한 위치로 가지 못해, 계속 위치를 맞추려는 시도를 함
    //    //if (Vector3.Distance(transform.position, target.transform.position) < 1.0f)
    //    //{
    //    //    transform.position = target.transform.position;
    //    //}

    //}


}
