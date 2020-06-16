using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //public GameObject playerBoom;

   // Ray ray;

    private float rayDistance = 10.0f;

    public GameObject bulletImpactFactory;  //총알 임팩트 팩토리
    
    public GameObject bombFactory;  //폭탄 프리팹
    public float throwPower = 20.0f;//던지는 파워

    public GameObject firePoint;


    // Start is called before the first frame update
    void Start()
    {
       // ray = new Ray();
    }

    // Update is called once per frame
    void Update()
    {
       
        Fire();
      
    }

 

    private void Fire()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        //1인칭게임의 경우, 카메라의 포지션을 가져다 쓴다.
        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.rotation);

        //ray.origin = Camera.main.transform.position;
        //ray.direction = Camera.main.transform.forward;

        //레이캐스트로 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo))
            {
                //print("충돌 오브젝트 : " + hitInfo.collider.name);
                //Debug.DrawRay(ray.origin, hitInfo.transform.position - transform.position, Color.red, 3.0f);

                //충돌지점에 충돌 이펙트 튀겨주기
                //총알 파편 이펙트 생성
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //부딫힌 지점은 hitInfo.point안에 있음
                bulletImpact.transform.position = hitInfo.point;
                //파편 이펙트
                //파편이 부딫힌 지점이 향하는 방향으로 튀도록 한다
                bulletImpact.transform.forward = hitInfo.normal;


                //레이어마스크 사용 충돌처리(최적화)
                //유니티 내부적으로 속도향상을 위해 비트연산 처리가 된다
                //총 32비트를 사용하기 때문에 레이어도 32개까지 존재

                //int layer = gameObject.layer;
                //layer = 1 << 8 | 1 << 9 | 1 << 12;

                //0000 0000 0000 0001 => 0000 0001 0000 0000
                //여러 if문을 레이어 하나로 처리 가능하다.
                //비트연산자는 최적화에 도움이 된다.
                // if(Physics.Raycast(ray, out hitInfo, layer))         레이어에 속한 애들과 충돌
                // if(Physics.Raycast(ray, out hitInfo, ~layer))        레이어에 속한 애들만 빼고 충돌


            }
            //else
            //{
            //    Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 3.0f);
            //}

        }



        //수류탄 투척
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePoint.transform.position;
            //폭탄은 플레이어가 던지기 때문에
            //폭탄의 리지드바디를 이용해 던지면 된다.
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //전방으로 힘을 가한다
           // rb.AddForce(Camera.main.transform.forward * throwPower);


            //ForceMode.Acceleration =>연속적인 힘을 가한다(질량의 영향 없음)
            //ForceMode.Force => 연속적인 힘을 가한다(질량의 영향을 받음)
            //ForceMode.Impulse => 순간적인 힘을 가한다(질량의 영향을 받음)
            //ForceMode.VelocityChange => 순간적인 힘을 가한다(질량 영향 없음)

            //45도 각도로 발사
            //각도를 주려면 어떻게 해야 하나 => 벡터의 덧셈
            Vector3 dir = Camera.main.transform.forward + Camera.main.transform.up;
            dir.Normalize();
            rb.AddForce(dir * throwPower, ForceMode.Impulse);
        }


    }



}
