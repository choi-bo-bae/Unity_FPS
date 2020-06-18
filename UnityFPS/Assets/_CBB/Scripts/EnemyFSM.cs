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
    GameObject target;           //플레이어
    public int hp = 100;             //체력
    #endregion

    #region "Idle상태에 필요한 변수들 1개"
    public float searchRange = 20.0f;  //플레이어 탐색 범위
    #endregion

    #region "Move상태에 필요한 변수들 1개"
    public float speed = 2.0f;          //이동 속도
    #endregion

    #region "Attack상태에 필요한 변수들 3개"
    public float attack = 5.0f;     //공격력
    private float curTime = 0.0f;   
    public float maxTime = 1.0f;      //1초에 한번 공격
    public float attackRange = 2.0f; //공격 범위
    #endregion

    #region "Return상태에 필요한 변수들 0개"
    public float moveRange = 30.0f; //최대로 갈 수 있는 범위
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
        target = GameObject.Find("Player");
        //spawnPoint = GameObject.Find("SpawnPoint");
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
                //Damaged();
                break;
            case EnemyState.Die:
               // Die();
                break;
        }
    }

    //플레이어에서 충돌감지를 해서 데미지를 플레이어에서 줄 수 있도록 public으로 선언
    public void HitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지를 중첩으로 주지 않는다.
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        if (hp > 0)
        {
            //체력 감소
            hp -= value;
            print("HP : " + hp);
            StartCoroutine(Damaged());
        }
        else
        {
            //죽음
            StartCoroutine(Die());
        }

    }

    private void Idle()
    {
       //1. 플레이어와 일정범위가 되면 이동 상태로 변경
       //플레이어 찾기 GameObject.Find("Player")
       
       //일정거리 20미터 (Distance, magnitude, sqrMagnitude)

       if(Vector3.Distance(target.transform.position, transform.position) < searchRange)
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
        if (Vector3.Distance(target.transform.position, transform.position) < searchRange 
            && Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            print("추격한다!");

            //이동 방향 - 벡터의 뺄셈
            Vector3 dir = (target.transform.position - transform.position).normalized;
            //dir.Normalize();

            //transform.forward = dir;    //방법1
            //transform.forward = Vector3.Lerp(transform.forward, dir, 5.0f * Time.deltaTime);    //방법2 -> 선형보간(Lerp)를 이용한 부드러운 회전
            //-> 쿼터니언 값이 아니기 때문에, 완벽히 일직선에 있으면 어느 방향으로 회전해야 하는지 몰라서 백덤블링을 하게 된다.
            //최종적으로 자연스러운 회전처리를 하려면 쿼터니언 값을 이용해야한다.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5.0f * Time.deltaTime);  //방법3 -> 쿼터니언을 이용한 러프

            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //cc.Move(dir * speed * Time.deltaTime); ->플레이어가 점프 할 경우, 하늘로 따라옴
            cc.SimpleMove(dir * speed); //중력값  대신 simpleMove를 사용
            //simpleMove -> 최소한의 물리가 적용, 중력 문제 해소. 내부적으로 시간처리를 하기 때문에 Time.deltaTime을 사용하지 않는다.
        }
        if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
        {
            print("공격한다!");
            state = EnemyState.Attack;
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchRange)
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

        if(Vector3.Distance(target.transform.position, transform.position) > attackRange)
        {
            print("재추격한다!");
            state = EnemyState.Move;
        }
        else
        {
            //일정시간마다 공격하기
            curTime += Time.deltaTime;

            if (curTime > maxTime)
            {
                print("공격한다!");
                //플레이어의 컴포넌트를 가져와서 데미지 주기
                //player.GetComponent<PlayerMove>().hitDamage(10);
                //타이머 초기화
                curTime = 0.0f;
            }
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchRange)
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
        
        if(Vector3.Distance(spawnPoint.transform.position, target.transform.position) < moveRange
            && Vector3.Distance(spawnPoint.transform.position, transform.position) < moveRange)
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
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5.0f * Time.deltaTime);
            cc.SimpleMove(dir * speed);
        }

        //처음위치에서 일정 범위
        //상태변경
        //상태전환 - 트렌지션

    }

    IEnumerator Damaged()
    {
       
        //코루틴 사용
        //1 몬스터 체력이 1 이상인경우

       
        if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
        {
            print("공격한다!");
            state = EnemyState.Attack;
        }
        if (Vector3.Distance(target.transform.position, transform.position) < searchRange)
        {
            print("추격한다!");
            state = EnemyState.Move;
        }
        if (Vector3.Distance(spawnPoint.transform.position, transform.position) > searchRange)
        {
            print("돌아가자!");
            state = EnemyState.Return;
        }
       
         print("공격받았다!");
       

        //2 다시 이전 상태로 변경
        //상태 변경
        //상태 전환 출력

        //yield return 0;
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator Die()
    {
      
        //코루틴 사용
        //체력이 0이하인 경우

        print("죽었다!");
        state = EnemyState.Die;
       

        //혹시나 돌고 있는 코루틴 꺼주기
        StopAllCoroutines();

        cc.enabled = false; //혹시나. 해서 캐릭터 컨트롤러 끄기
        
        yield return new WaitForSeconds(2.0f);

        //몬스터 오브젝트 삭제
        Destroy(gameObject);

        //yield return 0;
    }
    

    private void OnDrawGizmos()
    {
        //공격 가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어를 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        //최대로 갈 수 있는 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint.transform.position, moveRange);
    }

}
