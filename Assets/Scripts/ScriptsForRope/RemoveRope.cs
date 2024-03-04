using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveRope : Singleton<RemoveRope> {
    public Rope rope;

	public float timeDel;
	public float speed;
	float _speed;
	public float limitDis;
	public int limitDel;
	public float timerCheck;
	public bool hasEnded;
	float disz, dis;
	Vector3 _dir, _dirz;
	Transform firstSegment;
	Transform secondSegment;
	Transform secondSegment_Nguoc;
	Transform endSegment;
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] GameObject uIComplete;
	int test = 1, test_nguoc = 2;

	public int count;
	// Use this for initialization
	void OnEnable () {

		timerCheck = 3f;
		limitDel = 10;
		test = 1;
		hasEnded = false;
		if (_speed == 0)
			_speed = speed;
		else
			speed = _speed;
		count = rope.transform.childCount;
		if (!rope) 
		{
			rope = GetComponent<Rope> ();
			
		}
		if (rope)
		{
			if (rope.transform.childCount > 0)
			{
				firstSegment = rope.transform.GetChild(0);
				firstSegment.GetComponent<Rigidbody2D>().isKinematic = true;

				endSegment = rope.transform.GetChild(rope.transform.childCount - 1);
				endSegment.GetComponent<Rigidbody2D>().isKinematic = true;
			}
		}

	}

	// Update is called once per frame
	void Update() {

		if (timerCheck <= 0f || uIComplete.activeInHierarchy)
		{
			hasEnded = true;
			return;
		}

        if (rope.transform.childCount - test - 1 <= limitDel)
        {
            speed = 0.05f;
			limitDel = 2;
			lineRenderer.positionCount = 2;
		}
        //ChecknRemove();

        firstSegment = rope.transform.GetChild(0);
		/*secondSegment = rope.transform.GetChild(1);
        secondSegment_Nguoc = rope.transform.GetChild(rope.transform.childCount - 2);*/
		secondSegment = rope.transform.GetChild(test);
		secondSegment_Nguoc = rope.transform.GetChild(rope.transform.childCount - test - 1);


		disz = Vector3.Distance(secondSegment_Nguoc.position, firstSegment.position);

		timerCheck -= Time.deltaTime;

		if (timerCheck <= 1f)
        {
			SoundManager.Instance.StopRopePullingSound();
		}
		if(timerCheck <= 0.1f)
        {
			speed = 0.8f;
        }
        dis = Vector3.Distance(secondSegment.position, firstSegment.position);
        
        if (disz <= limitDis && dis <= limitDis && rope.transform.childCount - test - 1 > limitDel)
        {
			rope.transform.GetChild(test).gameObject.SetActive(false);

			//Destroy(rope.transform.GetChild(1).gameObject);
            speed -= (speed - 1) / (count - 1 - limitDel);
			rope.transform.GetChild(rope.transform.childCount - test - 1).gameObject.SetActive(false);
			//Destroy(rope.transform.GetChild(rope.transform.childCount - 2).gameObject);
            speed -= (speed - 1) / (count - 1 - limitDel);
            timerCheck = 2f;
			test += 1;
        }
        else
        {
            if (rope.transform.childCount <= limitDel) 
                speed = 0.05f;  
            if (dis >= limitDis)
            { 
                _dir = (firstSegment.position - secondSegment.position).normalized;
				secondSegment.GetComponent<Rigidbody2D>().velocity = _dir * speed;
			}
            if (disz >= limitDis)
            {
                _dirz = (firstSegment.position - secondSegment_Nguoc.position).normalized;
				secondSegment_Nguoc.GetComponent<Rigidbody2D>().velocity = _dirz * speed; 
			
            }
        }
    }

	public float GetTimeLeft()
    {
		return timerCheck;
    }

	public bool CheckifGameHasEnd()
    {
		if (uIComplete.activeInHierarchy) 
			return true;
		return false;
    } 

    private void OnDisable()
    {
		timerCheck = 0f;
		lineRenderer.positionCount = 0;//reset lai line renderer de? ko bi net ve~ o? man` truoc hien len
    }
}
