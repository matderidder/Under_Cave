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
    public Camera main;
    public GameObject target;
    public bool isblock;
    public bool isattack;
    public GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        //lock mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        main = Camera.main;
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
        float xTurn = Input.GetAxis("Mouse Y") * turnSpeed;
        Mathf.Clamp(xTurn, -15f, 15f);
        target.transform.localEulerAngles = (new Vector3(target.transform.eulerAngles.x + -xTurn, 0, 0));
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

        //if click do attack, no anitmation right now just "Swing"
        if(Input.GetMouseButtonDown(0))
        {
            //swing code go here want actual model and ani for this as would like to see where it swings can call reference to anoth code block
        }

        //if held second mouse button block placeholder will put code in another block/script with actual weapon and code that
        if(Input.GetMouseButton(1))
        {
            isblock = true;
            //placeholder blocking ani
            weapon.transform.eulerAngles = new Vector3(weapon.transform.eulerAngles.x, 270f, weapon.transform.eulerAngles.z);
        }
        if(Input.GetMouseButtonUp(1))
        {
            isblock = false;
            weapon.transform.eulerAngles = new Vector3(weapon.transform.eulerAngles.x, 0f, weapon.transform.eulerAngles.z);
        }
        
    }

}
