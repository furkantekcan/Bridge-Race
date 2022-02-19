using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public GameObject step;
    public float turnSpeed;
    public float speed;
    public float lerpValue;
    public LayerMask layer;

    private Animator animator;
    private Camera cam;
    private Vector3 newPos;

    private List<GameObject> bricks = new List<GameObject>();
    private GameObject lastItem;

    private float gravity = 9.8f;
    //
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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

            //transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue), Time.deltaTime * speed);
            controller.Move(Vector3.Normalize(hitVec - transform.position)*speed*Time.deltaTime - transform.up * gravity * Time.deltaTime);
            
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);
            
            if (!animator.GetBool("isRunning"))
            {
                animator.SetBool("isRunning", true);
            }

        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.transform.name);
        if (hit.gameObject.transform.parent.name == "Bricks")
        {
            if (bricks.Count == 0)
            {
                newPos = new Vector3(0, 0, 0);
            }
            hit.transform.SetParent(transform.GetChild(0));
            Debug.Log(transform.GetChild(0).localPosition);

            hit.transform.localRotation = new Quaternion(0, 0.7071068f, 0, 0.7071068f);

            hit.transform.DOLocalMove(newPos, 0.2f);
            newPos.y += 0.2f;

            bricks.Add(hit.gameObject);
        }

        else if (hit.transform.GetChild(0).transform.name == "Collider" && hit.transform.GetComponent<MeshRenderer>().enabled == false)
        {
            if (bricks.Count > 0)
            {
                hit.transform.GetComponent<MeshRenderer>().enabled = true;
                hit.transform.GetChild(0).gameObject.SetActive(false);
                Destroy(bricks[bricks.Count -1]);
                bricks.RemoveAt(bricks.Count-1);
                
            }
            
        }
        else if (hit.transform.name == "Finnish")
        {

        }

        else
        {
            return;
        }
    }

    
}
