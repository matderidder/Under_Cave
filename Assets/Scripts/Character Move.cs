using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float Speed;
    public float turnSpeed;
    public float jumpSpeed;
    public float fallSpeed;
    public bool isGrounded;
    public Rigidbody rigid;
    public float raycastDown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //lock mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //movement with arrows or wasd, get axis and move ridged body
        float horiMove = Input.GetAxis("Horizontal");
        float vertMove = Input.GetAxis("Vertical");
        Vector3 move = new Vector3 (horiMove, 0 ,vertMove);
        move.Normalize();
        transform.Translate (move*Speed*Time.deltaTime);

        //turn camera with player mouse
        float yTurn = Input.GetAxis ("Mouse X") * turnSpeed;
        transform.eulerAngles = (new Vector3 (0,transform.eulerAngles.y + yTurn,0));

        //Ground check
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDown))
            isGrounded = true;
        else
            isGrounded = false;

        //Jumping if on ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
        //
    }
}
