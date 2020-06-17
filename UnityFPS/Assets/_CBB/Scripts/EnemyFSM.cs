using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    

    #region "몬스터 공통으로 필요한 변수들 6개"
    private CharacterController cc;     //캐릭터 컨트롤러
    EnemyState state;                   //몬스터 상태변수 
    public GameObject spawnPoint;       //스폰 포인트
    public GameObject target;           //플레이어
    public int hp = 100;             //체력
    public float gravity = -20;
    #endregion

    #region "Idle상태에 필요한 변수들 1개"
    public float searchGround = 20.0f;  //플레이어 탐색 범위
    #endregion

    #region "Move상태에 필요한 변수들 1개"
    public float speed = 2.0f;          //이동 속도
    #endregion

    #region "Attack상태에 필요한 변수들 3개"
    private float curTime = 0.0f;
    private float maxTime = 1.0f;      //1초에 한번 공격
    private float attackGround = 3.0f; //공격 범위
    #endregion

    #region "Return상태에 필요한 변수들 0개"
    #endregion

    #region "Damaged상태에 필요한 변수들 0개"
    #endregion

    #region "Die상태에 필요한 변수들 0개"
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        state = EnemyState.Idle;
    }



    // Update is called once per frame
    void Update()
    {
       
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

        if (hp <= 0)    //죽음
        {
            StartCoroutine(Die());
        }
    }

    public void HitDamage(int value)
    {
        hp -= value;
        print("HP : " + hp);
        StartCoroutine(Damaged());
    }

    private void Idle()
    {
       //1. 플레이어와 일정범위가 되면 이동 상태로 변경
       //플레이어 찾기 GameObject.Find("Player")
       
       //일정거리 20미터 (Distance, magnitude, sqrMagnitude)

       if(Vector3.Distance(target.transform.position, transform.position) < searchGround)
       {
            print("추격한다!");
            state = EnemyState.Move;
       }
       else
       {
            print("대기한다!");
            state = EnemyState.Idle;
       }
       //상태변경
       //상태전환 출력
    }

    private void Move()
    {
        //1. 플레이어와 일정범위가 되면 공격 상태로 변경
        if (Vector3.Distance(target.transform.position, transform.position) < searchGround 
            && Vector3.Distance(target.transform.position, transform.position) > attackGround)
        {
            print("추격한다!");

            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            transform.forward = dir;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
        }
        if (Vector3.Distance(target.transform.position, transform.position) < attackGround)
        {
            print("공격한다!");
            state = EnemyState.Attack;
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchGround)
        {
            print("돌아가자!");
            state = EnemyState.Return;
        }
        //플레이어를 추격하더라도 처음위치에서 너무 벗어나지 않도록 한다.
        //플레이어처럼 캐릭터 컨트롤러 이용하기
        //추격범위는 마음대로
        //상태변경
        //상태전환 출력
    }

    private void Attack()
    {

        //1. 플레이어와 일정한 시간 간격으로 공격

        //2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)

        if(Vector3.Distance(target.transform.position, transform.position) > attackGround)
        {
            print("재추격한다!");
            state = EnemyState.Move;

        }
        else
        {
            curTime += Time.deltaTime;
            if (curTime > maxTime)
            {
                print("공격한다!");
                curTime = 0.0f;
            }
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchGround)
        {
            print("돌아가자!");
            state = EnemyState.Return;
        }
        //3. 공격 범위 마음대로
        //상태변경
        //상태전환 출력 - print
    }


    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
       
        if(Vector3.Distance(spawnPoint.transform.position, target.transform.position) < searchGround
            && Vector3.Distance(spawnPoint.transform.position, transform.position) < searchGround)
        {
            state = EnemyState.Move;
        }
        else if(Vector3.Distance(spawnPoint.transform.position, transform.position) < 0.1f)
        {
            print("스폰 포인트에 도착!");
            transform.position = spawnPoint.transform.position;
            state = EnemyState.Idle;
        }
        else
        {
            print("돌아가자!");

            Vector3 dir = spawnPoint.transform.position - transform.position;
            dir.Normalize();
            transform.forward = dir;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }

        //처음위치에서 일정 범위
        //상태변경
        //상태전환 - 트렌지션

    }

    IEnumerator Damaged()
    {
       
        //코루틴 사용
        //1 몬스터 체력이 1 이상인경우

       
        if (Vector3.Distance(target.transform.position, transform.position) < attackGround)
        {
            print("공격한다!");
            state = EnemyState.Attack;
        }
        if (Vector3.Distance(target.transform.position, transform.position) < searchGround)
        {
            print("추격한다!");
            state = EnemyState.Move;
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchGround)
        {
            print("돌아가자!");
            state = EnemyState.Return;
        }
       
         print("공격받았다!");
       

        //2 다시 이전 상태로 변경
        //상태 변경
        //상태 전환 출력

        yield return 0;
    }

    IEnumerator Die()
    {
      
        //코루틴 사용
        //체력이 0이하인 경우

        print("죽었다!");
        state = EnemyState.Die;
        Destroy(gameObject);


        //몬스터 오브젝트 삭제
        yield return 0;
    }

   

}
