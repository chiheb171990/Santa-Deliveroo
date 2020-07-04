using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InputState {CameraView, TacticalView};
public class InputManager : SingletonMB<InputManager>
{
    public InputState CurrentInputState;
    [Header("Variables")]
    [SerializeField] private GameObject cameraObject;

    [Header("Input Parameters")]
    public float MovementSensitivity;
    public float RotationSensitivity;
    public LayerMask SantaLayer;
    public LayerMask ClickableLayer;

    //Private Variables
    private float CamXAxis;
    private float CamZAxis;
    private float CamYaw;
    private float CamPitch;
    private Ray ray;
    private RaycastHit hit;
    private GameObject selectedSanta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeViewMode();
        MoveCamera();
        TacticalView();
    }

    public void MoveCamera()
    {
        if(CurrentInputState == InputState.CameraView)
        {
            //Get the keyboard axis valus
            CamXAxis = Input.GetAxis("Horizontal");
            CamZAxis = Input.GetAxis("Vertical");

            //Move the camera according to its forward and right vector
            cameraObject.transform.Translate(Vector3.right * CamXAxis * MovementSensitivity);
            cameraObject.transform.Translate(Vector3.forward * CamZAxis * MovementSensitivity);

            //Rotate Camera when the user click on the mouse
            if (Input.GetMouseButton(0))
            {
                //Get the Mouse Axis values
                CamYaw += Input.GetAxis("Mouse X");
                CamPitch -= Input.GetAxis("Mouse Y");

                //Rotate Camera according to mouse movement
                cameraObject.transform.eulerAngles = new Vector3(CamPitch * RotationSensitivity, CamYaw * RotationSensitivity, 0);
            }
            
        }
    }

    public void TacticalView()
    {
        if(CurrentInputState == InputState.TacticalView)
        {
            //Select Deselect the Santa
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray,out hit, 10000, SantaLayer))
                {
                    selectedSanta = hit.collider.gameObject;
                    print("select");
                }
                else
                {
                    selectedSanta = null;
                    print("deselect");
                }
            }

            //Select Santa's Actions
            if (Input.GetMouseButtonDown(1))
            {
                //Check if there is santa selected
                if (selectedSanta != null)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 10000, ClickableLayer))
                    {
                        //Caching the hit object
                        GameObject hitObject = hit.collider.gameObject;

                        //Caching the santa component
                        Santa santaComponent = selectedSanta.GetComponent<Santa>();

                        if (hitObject.tag == "Gift")
                        {
                            santaComponent.clickDestination(hitObject.GetComponent<Gift>());
                            hitObject.GetComponent<Gift>().SelectGift(santaComponent.id);
                        }

                        if (hitObject.tag == "Space")
                        {
                            santaComponent.clickDestination(hitObject.GetComponent<Gift>());
                        }

                        if(hitObject.tag == "House")
                        {
                            santaComponent.clickDestination(hitObject.GetComponent<House>());
                        }
                    }
                }
            }
        }
    }

    public void ChangeViewMode()
    {
        // Change the current view state when clicking on a space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (CurrentInputState)
            {
                case InputState.CameraView:
                    CurrentInputState = InputState.TacticalView;
                    break;
                case InputState.TacticalView:
                    CurrentInputState = InputState.CameraView;
                    break;
                default:
                    break;
            }
        }
    }
}
