using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]Rigidbody2D rg2d;
    [SerializeField]LineRenderer lineRenderer;
    HingeJoint2D hingeJoint2D;
    [SerializeField]CapsuleCollider2D capsuleCollider2D;

    public Transform targetT;
    public float speed;
    bool isCheck = true;
    RemoveRope removeRope;
    private void OnEnable()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();
        removeRope = RemoveRope.Instance;
            //if (transform.GetSiblingIndex() == 0 || transform.GetSiblingIndex() == transform.parent.childCount - 1)
            //    return; 
            //Vector2 dir = (Vector2.zero - (Vector2)transform.position).normalized;
            //GetComponent<Rigidbody2D>().AddForce(dir * speed);
        Invoke("nonCheck", 1f);
    }
    // Update is called once per frame
    void Update()
    {   
        if(removeRope.hasEnded)
        {
            capsuleCollider2D.enabled = false;
            rg2d.bodyType = RigidbodyType2D.Static;
            return;
        }
        if (targetT == null)
        {
            lineRenderer.positionCount = 0;
            if (hingeJoint2D != null)
            {
                if (hingeJoint2D.connectedBody != null)
                    targetT = hingeJoint2D.connectedBody.transform;

            }

            //Debug.Log("kc::" + Vector2.Distance(targetT.position, transform.position) + "   "+gameObject.name);
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetT.position);
        }

        Vector3 vt = rg2d.velocity;
        if (vt.x > 2 || vt.y > 2)
        {

            rg2d.velocity = vt / 2;

        }
    }

    private void nonCheck()
    {
        isCheck = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isCheck)
            return;
        /*if (collision.gameObject.CompareTag("True_Obj"))
        {
            return;
        }*/
        if (!collision.gameObject.CompareTag("Pole"))
        {
            collision.transform.position += Vector3.up * speed * Time.deltaTime*0.1f ;
            //isCheck = false;
        }
    } 
    /// <summary>
    /// OnCollisionExit is called when this collider/rigidbody has
    /// stopped touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Pole"))
        {
            //Debug.Log("zzz OnCollisionExit2D ::"+other.gameObject.name);
            
            isCheck = false;
        }
    } 

}
