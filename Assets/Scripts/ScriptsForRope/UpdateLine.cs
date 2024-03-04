using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLine : Singleton<UpdateLine>
{
    public Gradient lineColor;
    public float limitDis;
    LineRenderer line;
    [SerializeField] GameObject uiComplete;
    public List<Vector2> allUpdatedPosition;
    [HideInInspector] public List<Vector3> listPos;
    List<int> indexGameObjectStillActive;
    Camera cam;
    public RectTransform gobject_parent;
    [SerializeField]RemoveRope removeRope;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (line == null)
            line = this.GetComponent<LineRenderer>();
        cam = Camera.main;
        line.colorGradient = lineColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (removeRope.limitDel <= 5) return;
        //Get poss;
        //neu' ko khoi? tao. trong update thi` no se~ add diem lien tuc cho den' vo han.
        listPos = new List<Vector3>(); 
        allUpdatedPosition = new List<Vector2>();
        indexGameObjectStillActive = new List<int>();

        //them segment van~ dang bat. cua day 
        for (int i = 0; i< transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                indexGameObjectStillActive.Add(i);
            }
        }

        //check xem cac' segment cua? day
        for (int i = 0; i < indexGameObjectStillActive.Count; i++)
        {
            if (i > 1)
            {
                float dis = Vector3.Distance(transform.GetChild(indexGameObjectStillActive[i - 1]).position, transform.GetChild(indexGameObjectStillActive[i]).position);
                if (dis >= limitDis)
                {
                    //Debug.Log("myDis::" + dis);
                    listPos.Add(transform.GetChild(indexGameObjectStillActive[i]).position);
                    PosToAddIntoPolygon2D(i);

                }
            }
            else
            {
                listPos.Add(transform.GetChild(indexGameObjectStillActive[i]).position);
                PosToAddIntoPolygon2D(i);
            }
            line.positionCount = listPos.Count;
            line.SetPositions(listPos.ToArray());
            //line.colorGradient = lineColor;
            //GetComponent<Rope>().myLine.gameObject.SetActive(false);
            if (GetComponent<Rope>().myLine != null)
                Destroy(GetComponent<Rope>().myLine.gameObject);
        }
    }

    private void PosToAddIntoPolygon2D(int i)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, transform.GetChild(indexGameObjectStillActive[i]).position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gobject_parent, screenPos, cam, out localPos);
        allUpdatedPosition.Add(localPos);
    }

    private void OnDisable()
    {
        allUpdatedPosition.Clear();
    }

    public float GetTimeLeft()
    {
        return removeRope.GetTimeLeft();
    }

    public bool CheckIfGameHasEnd()
    {
        return removeRope.CheckifGameHasEnd();
    }

    public bool CheckIfPlayerHasDrawn()
    {
        if(allUpdatedPosition.Count>1)
        {
            return true;
        }
        return false;
    }
}
