using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    //vị trí của cameraMain
    private Transform cameraMain;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Play playerInput;

    //lấy child của Player
    private Transform child; 

    public void Awake()
    {
        playerInput = new Play();
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        //lấy vị trí transform của Camera
        //khi truy cập thuộc tính, trả về kết quả hợp lệ đầu tiên từ bộ nhớ cache của nó.
        cameraMain = Camera.main.transform;
        //lấy Capsule của Player
        child = transform.GetChild(0).transform;
    }

    void Update()
    {

        //check xem có trên chạm đất không
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // trả về giá trị di chuyển của W & S và A & D( kết quả từ -1 đến 1)
        Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        //di chuyển camera theo player
        Vector3 move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(movementInput != Vector2.zero)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(child.localEulerAngles.x, cameraMain.localEulerAngles.y, child.localEulerAngles.z));
            child.rotation = Quaternion.Lerp(child.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
