using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //폭탄의 역할
    //예전 총알은 생성하면 스스로 날아가지만,
    //폭탄은 플레이어가 던질 때까지 대기해야한다.
    //폭탄이 다른 오브젝트와 충돌 시 터져야 한다.
    //터진다는 것 : 다른 오브젝트 삭제, 폭탄 삭제

    public GameObject fxfactory;    //폭탄 임펙트 프리팹
   

    //충돌처리
    private void OnCollisionEnter(Collision collision)
    {


        //폭발 이펙트 보여주고

        GameObject fx = Instantiate(fxfactory);
        fx.transform.position = transform.position;
        //혹시나 이펙트가 사라지지않는 경우
        Destroy(fx, 2.0f);  //2초 후에 폭발이펙트 제거

        //다른 오브젝트 파괴
        //자신 파괴(자신은 반드시 마지막에 사라져야 한다)
        Destroy(gameObject);

    }



}
