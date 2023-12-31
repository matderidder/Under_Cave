using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterOptions : MonoBehaviour
{

    public float Speed;
    public float turnSpeed;
    public float jumpSpeed;
    public float fallSpeed;
    public bool isGrounded;
    public Rigidbody rigid;
    public float raycastDown;
    public Camera main;
    public GameObject target;
    public bool isblock;
    public GameObject weapon;
    private Animator animator;

    //Attacking Vars
    public float atkDist, atkSpeed, atkDelay, atkDmg;
    public LayerMask atkLayer;
    bool atkActive;

    //Health System
    int currHealth;
    public int maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        //lock mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        main = Camera.main;
        animator = GetComponent<Animator>();
        raycastDown = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {

        //movement with arrows or wasd, get axis and move ridged body
        float horiMove = Input.GetAxis("Vertical");
        float vertMove = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3 (-horiMove, 0 ,vertMove);
        move.Normalize();
        transform.Translate (move*Speed*Time.deltaTime);
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Animator>().SetTrigger("Run");
        }
        else
        {
            this.GetComponent<Animator>().SetTrigger("Idle");
        }
        


        //turn camera with player mouse
        float yTurn = Input.GetAxis ("Mouse X") * turnSpeed;
        transform.eulerAngles = (new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + yTurn, transform.eulerAngles.z));
        float xTurn = Input.GetAxis("Mouse Y") * turnSpeed;
        Mathf.Clamp(xTurn, -15f, 15f);
        target.transform.localEulerAngles = (new Vector3(target.transform.eulerAngles.x + -xTurn, target.transform.localEulerAngles.y, target.transform.localEulerAngles.z));

        //Ground check
        if (Physics.Raycast(transform.position, -Vector3.up, raycastDown + 0.1f))
            isGrounded = true;
        else
            isGrounded = false;

        //Jumping if on ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            this.GetComponent<Animator>().SetTrigger("Jump");
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        //if click do attack, no anitmation right now just "Swing"
        if(Input.GetMouseButton(0))
        {
            //swing code go here want actual model and ani for this as would like to see where it swings can call reference to anoth code block
            Attack();
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

    private void Attack() {

        //Returns if the user cannot attack, or IS Attacking
        if (atkActive)
            return;
        
        atkActive = true;
        Invoke(nameof(ResetAttack), atkSpeed); //Allows the user to attack again once their attack speed is back
        Invoke(nameof(AttackRaycast), atkDelay); //Lets the hit go through a small delay after (mostly to line up with the animation)
    }

    private void ResetAttack() {
        atkActive = false;
    }

    private void AttackRaycast() {
    
        if (Physics.Raycast(main.transform.position, main.transform.forward, out RaycastHit hit, atkDist, atkLayer)) {
            //Hit animation, if any
            if (hit.transform.TryGetComponent<EnemyBehavior>(out EnemyBehavior T)) { //ERROR LINE
                T.TakeDamage(atkDmg); //TODO ON ENEMY
            }
        }
    }

    public void TakeDamage(float amount) {
        currHealth -= amount;

        if (currHealth <= 0f) {
            //Define your death.
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Maze");
        }
    }
}
