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

    

    //중력 적용
    public float gravity = -20;
    float velocityY;        //낙하속도(벨로시티는 방향과 힘을 들고 있다)

    float jumpPower = 10.0f;    //점프 파워
    int jumpCount = 0;  //점프 카운트 ==> 2단점프까지만.

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

        //controller.Move(dir * speed * Time.deltaTime);
        //땅 뚫지 못하고, 충돌처리는 되었으나 하늘은 아직 날수있음 => 중력값 조정 필요
        
        //캐릭터 점프
        //점프 버튼을 누르면 수직 속도에 점프파워를 넣는다
        //땅에 닿으면 velocityY값을 0으로 초기화
       //if(controller.isGrounded)   //땅에 닿았는지 확인   //점프 씹힌다.
       //{     
       //}

        //CollisionFlags.Above;(위쪽) 
        //CollisionFlags.Below;(아래) 
        //CollisionFlags.Sides;(옆)
        if (controller.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            //중력 적용하기
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }

        //2단점프까지만 가능
        if (jumpCount < 2)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = jumpPower;
                jumpCount++;
            }
        }

        //중력 적용하면서 이동
        controller.Move(dir * speed * Time.deltaTime);
    }







}
