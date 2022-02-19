using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGenerator : MonoBehaviour
{
    public GameObject step;
    public GameObject player;

    private MeshRenderer meshRenderer;
    private Collider collider;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = player.GetComponent<MeshRenderer>();
        collider = player.GetComponent<BoxCollider>();
        playerController = player.GetComponent<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
