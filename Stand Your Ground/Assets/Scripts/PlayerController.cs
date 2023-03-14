using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [Header("Player Movement and Gravity")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 4.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] public float distance;
    private float topDistance;
    private Vector3 startposition;
    [SerializeField] private TMP_Text distanceText;

    [Header("Game Settings")]
    [SerializeField] private float minSpawnDistance = 15f;
    [SerializeField] private float maxSpawnDistance = 50f;
    private float spawnDistanceMultiplier;
    private float spawnDistance;
    private int killCount = 0;
    [SerializeField] private TMP_Text killCountText;
    
    [Header("Gun Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;
    
    [Header("Health Settings")]
    [SerializeField] public HealthBar healthBar;
    [SerializeField] private TMP_Text livesText;
    public float health = 100f;
    public float maxHealth = 100f;
    private float lives = 3f;

    [Header("Ammo Settings")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private int ammo = 30;

    [Header("Player Animation")]
    private Animator animator;

    [Header("Canvas")]
    public GameObject gameOver;
    [SerializeField] public GameObject hipCrosshair;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    public EventManager eventManager;
    private SpawnSystem spawnSystem;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction takeDamageAction;
    private InputAction sprintAction;
    private InputAction ammoAction;
    private InputAction healthAction;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        spawnSystem = GameObject.Find("Spawn System").GetComponent<SpawnSystem>();
        
        // Get Input actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        takeDamageAction = playerInput.actions["Take Damage"];
        sprintAction = playerInput.actions["Sprint"];
        ammoAction = playerInput.actions["Ammo"];
        healthAction = playerInput.actions["Health"];

        // Double check canvas
        gameOver.SetActive(false);
        hipCrosshair.SetActive(true);
    }

    private void Start() {
        startposition.z = transform.position.z;
        topDistance = 0f;
        distance = 0f;

        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    }

    private void OnEnable(){
        shootAction.performed += _ => ShootGun();
        eventManager.enemyDiedEvent.AddListener(EnemyKilled);
    }

    private void OnDisable(){
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun(){
        if (ammo <= 0) return;

        ammo--;

        RaycastHit hit;

        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        Bullet bulletController = bullet.GetComponent<Bullet>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity)){
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else{
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = true;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Take Damage. For Testing purposes will set this to input
        if (takeDamageAction.triggered){
            TakeDamage(10f);
        }


        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;

        if (sprintAction.ReadValue<float>() > 0){
            controller.Move(move * Time.deltaTime * sprintSpeed);
        }
        else{
            controller.Move(move * Time.deltaTime * playerSpeed);
        }

        if (ammoAction.triggered){
            spawnSystem.SpawnAmmo(1);
        }

        if (healthAction.triggered){
            spawnSystem.SpawnPowerup(0);
        }

        // Player distance
        distance = transform.position.z - startposition.z;
        if (distance > topDistance){
            topDistance = distance;
        }

        if (distance >= spawnDistance){
            eventManager.RaiseDistanceReachedEvent(distance);
            spawnDistance = spawnDistance + Random.Range(minSpawnDistance, maxSpawnDistance);
        }

        // Update animator
        animator.SetFloat("Speed", move.magnitude);

        // Update text
        ammoText.text = ammo.ToString();
        livesText.text = lives.ToString();
        distanceText.text = topDistance.ToString("#");
        killCountText.text = killCount.ToString();

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);

    }
    public void AddAmmo(int amount){
        ammo += amount;
    }

    public void TakeDamage(float damage){
        
        health -= damage;
        healthBar.UpdateHealthBar(health);
        if (health <= 0 && lives > 0){
            lives --;
            health = maxHealth;
            if (lives <= 0){
                health = 0f;
                GameOver();
            }
        }
    }

    public void EnemyKilled(){
        killCount++;
        }

    public void AddHealth(float amount){
        health += amount;
        Debug.Log("Health added. Health is now " + health);
        // Update health bar
        healthBar.UpdateHealthBar(health);
    }

    void GameOver(){
        gameOver.SetActive(true);
        hipCrosshair.SetActive(false);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject.name + "is the object that hit me");
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            TakeDamage(enemy.enemyData.attack * Time.deltaTime);
        }
    }

}
