using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Transform items;
    public GameObject prevObject;
    public List<GameObject> bricks = new List<GameObject>();
    public float turnSpeed;
    public float speed;
    public float lerpValue;
    public LayerMask layer;

    private Animator animator;
    private Camera cam;
    private Vector3 newPos =  new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
        else
        {
            if (animator.GetBool("isRunning"))
            {
                animator.SetBool("isRunning", false);
            }
        }
    }

    private void Movement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;

        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
        {
            Vector3 hitVec = hit.point;
            hitVec.y = transform.position.y;
            
            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue), Time.deltaTime * speed);
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);
            
            if (!animator.GetBool("isRunning"))
            {
                animator.SetBool("isRunning", true);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.name);

        if (other.transform.parent.name == "Bricks")
        {
            other.transform.SetParent(transform.GetChild(0));
            //newPos = transform.GetChild(0).localPosition;
            //newPos.z = 0;
            //newPos.x = 0;
            
            Debug.Log(transform.GetChild(0).localPosition);

            other.transform.localRotation = new Quaternion(0, 0.7071068f, 0, 0.7071068f);

            other.transform.DOLocalMove(newPos, 0.2f);
            newPos.y += 0.2f;

            bricks.Add(other.gameObject);
        }
    }
}
