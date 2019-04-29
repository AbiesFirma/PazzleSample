using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRayTest : MonoBehaviour
{
    Rigidbody ballRb;

    bool onFlick;
    bool onCatch;

    Touch touch;
    int touchID;

    Vector3 moveDir;
    [SerializeField] float movePower = 2.0f;
    [SerializeField] float stopForce = 1.0f;

    float touchTime;
    [SerializeField] float longTapTime = 2.0f;

    GameObject hitObj;
    public GameObject targetEnemy;

    Vector3 hitObjStartPos;
    [SerializeField] GameObject catchMarker;
    [SerializeField] GameObject targetEnemyMarker;

    void Start()
    {
        onFlick = false;
        onCatch = false;

        touchTime = 0.0f;

        if (catchMarker == null)
        {
            catchMarker = GameObject.Find("CatchMarker");
        }
        if (targetEnemyMarker == null)
        {
            targetEnemyMarker = GameObject.Find("TargetEnemyMarker");
        }

        catchMarker.SetActive(false);
        targetEnemyMarker.SetActive(false);

    }

    void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount > 0)
        {
            //触れているすべての指を判定
            foreach (Touch t in Input.touches)
            {
                //フリック中か否か
                if (!onFlick)
                {
                    touch = t;
                }
                else
                {
                    //フリック中はfingerIDが同じ（フリックを始めた指である）場合に更新
                    if (t.fingerId == touch.fingerId)
                    {
                        touch = t;
                    }
                    else
                    {
                        //Debug.Log("otherFinger");
                    }
                }

            }


            //フリック中でない場合フリック開始処理
            if (!onFlick)
            {
                if (touch.phase == TouchPhase.Began)    //=GetMouseButtonDown
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position/*Input.mousePosition*/);
                    RaycastHit hit;
                    int layerMask = (1 << 10 | 1 << 11);

                    if (Physics.Raycast(ray, out hit, layerMask))
                    {
                        if (hit.collider.gameObject.tag == "Red" || hit.collider.gameObject.tag == "Blue" || hit.collider.gameObject.tag == "Green" ||
                                   hit.collider.gameObject.tag == "Yellow" || hit.collider.gameObject.tag == "Black")
                        {
                            hitObj = hit.collider.gameObject;
                            ballRb = hitObj.GetComponent<Rigidbody>();
                            hitObjStartPos = hitObj.transform.position;

                            onCatch = true;
                            catchMarker.SetActive(true);
                            catchMarker.transform.position = hitObj.transform.position;

                            
                        }
                        else if (hit.collider.gameObject.tag == "Enemy")
                        {
                            targetEnemy = hit.collider.gameObject;
                        }
                    }

                    touchID = touch.fingerId;
                    onFlick = true;

                }

            }
            //フリック中はfingerIDが同じときcurrentMarkerの位置を更新しstartMarkerとラインで結ぶ
            else
            {
                //ボールのヒット中
                if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && touch.fingerId == touchID)   //=GetMouseButton
                {
                    Vector3 currentPosSc = touch.position;
                    currentPosSc.z = 10.0f;

                    if (onCatch)
                    {
                        if (hitObj == null)
                        {
                            onCatch = false;
                            touchTime = 0.0f;

                            catchMarker.SetActive(false);
                        }
                        else
                        {

                            Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentPosSc);

                            currentPos.z = hitObj.transform.position.z;

                            catchMarker.transform.position = hitObj.transform.position;

                            moveDir = currentPos - hitObjStartPos;


                            //長押し(Stationary=その場でタッチしてる、動いていない or Move＝動いているが移動量が1.0以下の場合)
                            if (touch.phase == TouchPhase.Stationary || (touch.phase == TouchPhase.Moved && moveDir.magnitude <= 1.0f))
                            {
                                touchTime += Time.deltaTime;

                                if (touchTime > longTapTime)
                                {
                                    hitObj.SendMessage("LongTapped");
                                }
                            }
                            else
                            {
                                touchTime = 0.0f;
                            }
                        }
                        
                    }
                    

                }


                //指を離したときマーカー、ラインを消し、フリック中状態を解除
                if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID)    //GetMouseButtonUp
                {
                    onFlick = false;
                    onCatch = false;
                    touchTime = 0.0f;

                    catchMarker.SetActive(false);

                }
            }
        }
        //タッチしていないときはマーカー、ライン、フリック中状態をオフに
        else
        {
            onFlick = false;
            onCatch = false;
            hitObj = null;

            touchTime = 0.0f;

            moveDir = Vector3.zero;
        }


        if (targetEnemy != null)
        {
            targetEnemyMarker.SetActive(true);
            targetEnemyMarker.transform.position = targetEnemy.transform.position;
        }
        else
        {
            targetEnemyMarker.SetActive(false);
        }

    }


    void FixedUpdate()
    {
        if (touch.phase == TouchPhase.Moved && onCatch && hitObj != null)
        {
            ballRb.AddForce((moveDir * movePower) - (ballRb.velocity * stopForce), ForceMode.Acceleration);
            ballRb.AddForce(-Physics.gravity, ForceMode.Acceleration);
        }
    }
}
  
