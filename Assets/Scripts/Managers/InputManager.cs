using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using cakeslice;

public enum InputState {CameraView, TacticalView};
public class InputManager : SingletonMB<InputManager>
{
    public InputState CurrentInputState;
    [Header("Variables")]
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Transform tacticalViewTransform;
    
    public GameObject selectedSanta;

    [Header("Input Parameters")]
    public float MovementSensitivity;
    public float RotationSensitivity;
    public LayerMask SantaLayer;
    public LayerMask ClickableLayer;
    public LayerMask NavMeshLayer;

    // ****************************** Private Variables *********************************** //
    //Cam Variables
    private float CamXAxis;
    private float CamZAxis;
    private float CamYaw;
    private float CamPitch;
    //Raycast variables
    private Ray ray;
    private RaycastHit hit;

    //Private caching variables
    private GameObject selectedGiftHouse;
    private Vector3 LastCamPosition;
    private Quaternion LastCamRotation;

    //private input variables
    private bool isShiftSelected = false;

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
                cameraObject.transform.eulerAngles = new Vector3(CamPitch * RotationSensitivity,CamYaw * RotationSensitivity, 0);
            }
            
        }
    }

    public void TacticalView()
    {
        if(CurrentInputState == InputState.TacticalView)
        {
            //Select Deselect the Santa
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 10000, SantaLayer))
                    {
                        selectSanta();
                    }
                    else
                    {
                        DeselectSanta();
                    }
                    if (Physics.Raycast(ray, out hit, 10000, ClickableLayer))
                    {
                        SelectGiftHouse();
                    }
                    else
                    {
                        DeselectGiftHouse();
                    }
                }
            }


            //Select Santa's Actions
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (selectedSanta != null)
                    {
                        SantaActionWithShift();
                    }
                }
            }

            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (selectedSanta != null)
                {
                    //begin the santa movment accordin its waypoints
                    selectedSanta.GetComponent<Santa>().ClickShiftFinished();
                }
            }

            else 
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //Check if there is santa selected
                    if (selectedSanta != null)
                    {
                        SantaActionWithoutShift();
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
                    //Caching the last transform of the camera in the movment view
                    LastCamPosition = cameraObject.transform.position;
                    LastCamRotation = cameraObject.transform.rotation;
                    //Zoom out to the tactical mode view
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(cameraObject, tacticalViewTransform.position,tacticalViewTransform.rotation, 1));
                    break;
                case InputState.TacticalView:
                    CurrentInputState = InputState.CameraView;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(cameraObject, LastCamPosition,LastCamRotation, 1));
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator MoveTo(GameObject obj, Vector3 toPosition,Quaternion toRotation,float period)
    {
        //Move the object to the destination positon and orientation 
        float timer = 0f;

        //Get the first position and rotation
        Vector3 firstPosition = obj.transform.position;
        Quaternion firstRotation = obj.transform.rotation;

        while (timer / period < 1)
        {
            obj.transform.position = Vector3.Lerp(firstPosition, toPosition, timer / period);
            obj.transform.rotation = Quaternion.Lerp(firstRotation, toRotation, timer / period);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = toPosition;
        obj.transform.rotation = toRotation;
    }

    private void selectSanta()
    {
        if (selectedSanta != null)
        {
            //Dishighlight the santa if there is previous santa selected before the new one
            selectedSanta.GetComponent<Outline>().enabled = false;
        }
        selectedSanta = hit.collider.gameObject;

        //Highlight santa
        selectedSanta.GetComponent<Outline>().enabled = true;

        //clear and Enable santa scroll view
        SantaMainController.Instance.ClearSantaScrollView();
        SantaMainController.Instance.InitSantaScrollView(selectedSanta.GetComponent<Santa>().collectedGifts);
        print("select");
    }

    private void DeselectSanta()
    {
        //Dishighlight santa
        if (selectedSanta != null)
        {
            selectedSanta.GetComponent<Outline>().enabled = false;
        }

        selectedSanta = null;

        //Disable santa scroll view
        SantaMainController.Instance.ClearSantaScrollView();
        print("deselect");
    }

    private void SelectGiftHouse()
    {
        if (selectedGiftHouse != null)
        {
            //Dishighlight the gift or the house if there is previous object selected before the new one
            if (selectedGiftHouse.tag == "House")
            {
                selectedGiftHouse.GetComponent<Outline>().color = 2;
            }
            else
            {
                selectedGiftHouse.GetComponent<Outline>().enabled = false;
            }
        }
        selectedGiftHouse = hit.collider.gameObject;

        //Dishighlight the previous associations
        SantaGameManager.Instance.DeselectGiftsAndHousesAssociations();

        //Disable the santa scroll view
        SantaMainController.Instance.ClearSantaScrollView();

        //Highlight gift or house and get the correspondant gift or house
        if (selectedGiftHouse.tag == "House")
        {
            selectedGiftHouse.GetComponent<Outline>().color = 1;
            //Highlight the associated gifts
            SantaGameManager.Instance.selectHouse(selectedGiftHouse.GetComponent<House>().id);
        }
        else
        {
            //If the object selected is a gift enable its outline
            selectedGiftHouse.GetComponent<Outline>().enabled = true;
            //Highlight the associated house
            SantaGameManager.Instance.selectGift(selectedGiftHouse.GetComponent<Gift>().houseId);
        }
        print("select");
    }

    private void DeselectGiftHouse()
    {
        //Dishighlight gift or house
        if (selectedGiftHouse != null)
        {
            if (selectedGiftHouse.tag == "House")
            {
                selectedGiftHouse.GetComponent<Outline>().color = 2;
            }
            else
            {
                selectedGiftHouse.GetComponent<Outline>().enabled = false;
            }
        }
        //Dishighlight the associations
        SantaGameManager.Instance.DeselectGiftsAndHousesAssociations();
        selectedGiftHouse = null;

        //Disable santa scroll view
        //SantaMainController.Instance.ClearSantaScrollView();

        print("deselect");
    }

    private void SantaActionWithoutShift()
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

            if (hitObject.tag == "House")
            {
                santaComponent.clickDestination(hitObject.GetComponent<House>());
                hitObject.GetComponent<House>().SelectHouse(santaComponent.id);
            }
        }
    }

    private void SantaActionWithShift()
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
                santaComponent.clickDestinationWithShift(hitObject.GetComponent<Gift>());
                hitObject.GetComponent<Gift>().SelectGift(santaComponent.id);
            }

            if (hitObject.tag == "House")
            {
                santaComponent.clickDestinationWithShift(hitObject.GetComponent<House>());
                hitObject.GetComponent<House>().SelectHouse(santaComponent.id);
            }

        }
        else if (Physics.Raycast(ray, out hit, 10000, NavMeshLayer))
        {
            //Caching the hit object
            GameObject hitObject = hit.collider.gameObject;

            //Caching the santa component
            Santa santaComponent = selectedSanta.GetComponent<Santa>();

            //Instantiate a waypoint
            GameObject waypoint = SantaGameManager.Instance.InstantiateWaypoint(hit.point, santaComponent.id);

            santaComponent.clickDestinationWithShift(waypoint.GetComponent<Waypoint>());
        }
    }
}
