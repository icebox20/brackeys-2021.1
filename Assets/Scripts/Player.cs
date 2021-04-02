using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {

    [SerializeField]
    bool firstPlayer, secondPlayer;
    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 7;
    [SerializeField]
    float dashPower = 5, lookRotationSpeed = 0.2f, dashDelayTime = .4f, maxVelocityMagnitude = 50;
    float turnSmoothVelocity;
    [SerializeField]
    ParticleSystem dashParticle;
    [SerializeField]
    Transform characterChild;
    
    bool isWalking, canDash = true;
    
    Vector3 input;

    Rigidbody rb;

    [Header("Animations")]
    [SerializeField]
    Animator anim;
    [SerializeField]
    string dashanimname;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        if (GameManager.isPlaying) { 
            Move();
            Rotate();
            if (hitSpaceButton() && canDash) Dash();
        }
    }

    void Dash() {
        if (rb.velocity.magnitude < maxVelocityMagnitude) { 
            anim.Play(dashanimname);
            dashParticle.Play();
            rb.AddForce(characterChild.forward * dashPower, ForceMode.Impulse);
            canDash = false;
            Invoke("EnableDashing", dashDelayTime);
        }
    }
    void EnableDashing() => canDash = true;
    void Move() {
        transform.position += (new Vector3(HorizontalMovement(), 0, VerticalMovement()));
    }
    void Rotate() {
        if (firstPlayer) {
            if (Input.GetKey(KeyCode.A)) input.x = -1;
            else if (Input.GetKey(KeyCode.D)) input.x = 1;
            else input.x = 0;
            if (Input.GetKey(KeyCode.S)) input.y = -1;
            else if (Input.GetKey(KeyCode.W)) input.y = 1;
            else input.y = 0;
        }
        if (secondPlayer) {
            if (Input.GetKey(KeyCode.LeftArrow)) input.x = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) input.x = 1;
            else input.x = 0;
            if (Input.GetKey(KeyCode.DownArrow)) input.y = -1;
            else if (Input.GetKey(KeyCode.UpArrow)) input.y = 1;
            else input.y = 0;
        }

        Vector3 direction = new Vector3(input.y, 0, -input.x).normalized;
        if (input.magnitude >= .1f) {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
            float targetAngle =  Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, lookRotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        } else if (isWalking) { isWalking = false; anim.SetBool("isWalking", isWalking); }
    }

    float VerticalMovement() {
        if (firstPlayer) { 
            if (Input.GetKey(KeyCode.S)) return -1 * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.W)) return 1 * moveSpeed * Time.deltaTime;
        }
        if (secondPlayer) { 
            if (Input.GetKey(KeyCode.DownArrow)) return -1 * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow)) return 1 * moveSpeed * Time.deltaTime;
        }
        return 0;
    }
    float HorizontalMovement() {
        if (firstPlayer) { 
            if (Input.GetKey(KeyCode.A)) return -1 * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) return 1 * moveSpeed * Time.deltaTime;
        }
        if (secondPlayer) {
            if (Input.GetKey(KeyCode.LeftArrow)) return -1 * moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow)) return 1 * moveSpeed * Time.deltaTime;
        }
        return 0;
    }
    bool hitSpaceButton() {
        if (firstPlayer && Input.GetKeyDown(KeyCode.Space)) return true;
        if (secondPlayer && Input.GetKeyDown(KeyCode.RightShift)) return true;
        return false;
    }
     
    void OnTriggerEnter(Collider col) {
        canDash = true;
    }

    public bool GetSecondPlayer()
    {
        return secondPlayer;
    }
}
