using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] camPos;
    [SerializeField]
    private Transform[] camTarget;
    [SerializeField]
    private RotateCam _rotateCam;
    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i;
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                _rotateCam.SetCamPos(camPos[index], camTarget[index]);
            }
            );
        }
    }
}
