using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TouchMarkerFullScreen : MonoBehaviour
{
    bool onFlick;

    LineRenderer lineRenderer;
    [SerializeField] Material lineMaterial;

    [SerializeField] GameObject startMarker;      //カメラの子に配置
    [SerializeField] GameObject currentMarker;

    Touch touch;
    int touchID;

    void Start()
    {
        startMarker.SetActive(false);
        currentMarker.SetActive(false);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;


        onFlick = false;
    }

    void Update()
    {
        int touchCount = Input.touchCount;

        //タッチしている場所（指）が0より多ければ（タッチしていたら）
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
                    startMarker.SetActive(true);
                    currentMarker.SetActive(true);

                    Vector3 startPosSc = touch.position;
                    startPosSc.z = 10.0f;
                    Vector3 startMarkerPos = Camera.main.ScreenToWorldPoint(startPosSc);
                    startMarker.transform.position = startMarkerPos;
                    currentMarker.transform.position = startMarkerPos;

                    touchID = touch.fingerId;
                    onFlick = true;
                }
                
            }
            //フリック中はfingerIDが同じときcurrentMarkerの位置を更新しstartMarkerとラインで結ぶ
            else
            {
                if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && touch.fingerId == touchID)   //=GetMouseButton
                {
                    Vector3 currentPosSc = touch.position;
                    currentPosSc.z = 10.0f;
                    Vector3 currentMarkerPos = Camera.main.ScreenToWorldPoint(currentPosSc);
                    currentMarker.transform.position = currentMarkerPos;

                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, startMarker.transform.position);
                    lineRenderer.SetPosition(1, currentMarker.transform.position);

                    lineRenderer.material = lineMaterial;
                    lineRenderer.startWidth = 0.2f;
                    lineRenderer.endWidth = 0.6f;

                }

                //指を離したときマーカー、ラインを消し、フリック中状態を解除
                if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID)    //GetMouseButtonUp
                {
                    startMarker.SetActive(false);
                    currentMarker.SetActive(false);

                    lineRenderer.enabled = false;
                    onFlick = false;
                }
            }

        }
        //タッチしていないときはマーカー、ライン、フリック中状態をオフに
        else
        {
            startMarker.SetActive(false);
            currentMarker.SetActive(false);

            lineRenderer.enabled = false;
            onFlick = false;
        }
    }
}