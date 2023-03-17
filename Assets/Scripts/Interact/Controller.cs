using UnityEngine;
public class Controller : MonoBehaviour
{
    [SerializeField]
    private RotateCam rotateCam;
    [SerializeField]
    private Transform[] camPos;
    [SerializeField]
    private Transform[] camTagetPos;
    // Start is called before the first frame update
    void Start()
    {
        rotateCam.SetCamPos(camPos[0], camTagetPos[0]);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rotateCam.SetCamPos(camPos[0], camTagetPos[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rotateCam.SetCamPos(camPos[1], camTagetPos[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            rotateCam.SetCamPos(camPos[2], camTagetPos[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            rotateCam.SetCamPos(camPos[3], camTagetPos[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            rotateCam.SetCamPos(camPos[4], camTagetPos[4]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            rotateCam.SetCamPos(camPos[5], camTagetPos[5]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            rotateCam.SetCamPos(camPos[6], camTagetPos[6]);
        }
    }
    public void InitCam() 
    {
        rotateCam.SetCamPos(camPos[3], camTagetPos[3]);
    }
}
