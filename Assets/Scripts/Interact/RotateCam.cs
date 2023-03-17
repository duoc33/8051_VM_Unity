using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class RotateCam : MonoBehaviour
{
    public bool OnRotate = true;
    private Transform CamMoveTarget;
    private Transform ThisTarget;
    private float wheelSpeed = 2;
    public float camDisMultiple = 2;//摄像机默认距离倍数
    public Vector2 FovLimit;
    //private float disOffset;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject go = new GameObject("CamMoveTarget");
        CamMoveTarget = go.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ThisTarget != null&&OnRotate)
        {
            camerarotate();
            camerazoom();
        }
    }

    /// <summary>
    /// 自适应视角
    /// </summary>
    /// <param name="v3">需要观看的点</param>
    /// <param name="visualAngle">视角三维方向</param>
    public void SetFocues(Transform v3, Vector3 visualAngle)
    {
        float dis = BestSize(v3);
        ThisTarget = v3;
        CamMoveTarget.position = v3.position + new Vector3(dis * camDisMultiple * visualAngle.x, dis * camDisMultiple * 0.6f * visualAngle.y, dis * camDisMultiple * visualAngle.z);
        CamMoveTarget.LookAt(v3.position);
        this.transform.DORotate(CamMoveTarget.eulerAngles, 1);
        this.transform.DOMove(CamMoveTarget.position, 1);
    }

    /// <summary>
    /// 指定相机位置和角度
    /// </summary>
    /// <param name="camV3">相机的位置和角度</param>
    /// <param name="camTaget">观察点位置</param>
    public void SetCamPos(Transform camV3, Transform camTaget)
    {
        ThisTarget = camTaget;
        this.transform.DORotate(camV3.eulerAngles, 1);
        this.transform.DOMove(camV3.position, 1);
        this.transform.GetComponent<Camera>().DOFieldOfView(60, 1);
    }

    //摄像机围绕目标旋转操作
    private void camerarotate()
    {
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.Translate(Vector3.left * (mouse_x * 15f) * Time.deltaTime);
            transform.Translate(Vector3.up * (mouse_y * 15f) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.RotateAround(ThisTarget.transform.position, Vector3.up, mouse_x * 5);
            transform.RotateAround(ThisTarget.transform.position, transform.right, mouse_y * 5);
        }
    }

    //摄像机滚轮缩放
    private void camerazoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.GetComponent<Camera>().fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - 5, 30, 90);
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.GetComponent<Camera>().fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + 5, 30, 90);
        }
    }

    /// <summary>
    /// 根据传入物体的模型size计算出与摄像机的合适距离
    /// </summary>
    /// <param name="tran"></param>
    /// <returns></returns>
    public float BestSize(Transform tran)
    {
        float Sx = tran.GetComponent<MeshFilter>().mesh.bounds.size.x * Mathf.Abs(tran.localScale.x);
        float Sy = tran.GetComponent<MeshFilter>().mesh.bounds.size.y * Mathf.Abs(tran.localScale.y);
        float Sz = tran.GetComponent<MeshFilter>().mesh.bounds.size.z * Mathf.Abs(tran.localScale.z);

        float Bb = Sx;
        if (Sy > Bb) { Bb = Sy; }
        if (Sz > Bb) { Bb = Sz; }
        wheelSpeed = 2 + Bb * 0.12f;
        return Bb;
    }
}
