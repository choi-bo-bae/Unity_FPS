using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    //플레이어 이동
    private float h;
    private float v;
    
    public float speed = 5.0f;//이동하는 속도

    private CharacterController controller; //캐릭터 컨트롤러


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        //대각선 이동 속도를 상하좌우 속도와 동일하게 만들기
        //게임에 따라 일부러 대각선은 빠르게 이동하도록 하는 경우도 있음
        //이럴 때는 벡터의 정규화(노말라이즈)를 하면 안 된다.

        //카메라가 보는 방향으로 이동해야 한다.
        dir = Camera.main.transform.TransformDirection(dir);
        //TransformDirection => 메인카메라가 바라보는 방향으로 플레이어도 함께 맞춰줌

        //transform.Translate(dir * speed * Time.deltaTime);
        //문제점 : 하늘, 땅 충돌처리 안 됨

        controller.Move(dir * speed * Time.deltaTime);
        //땅 뚤히 못하고, 충돌처리는 되었으나 하늘은 아직 날수있음 => 중력값 조정 필요
    }
}
