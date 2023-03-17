using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_Event : MonoBehaviour
{
    [SerializeField]
    private RotateCam _rotateCam;
    [SerializeField]
    private GameObject outPanel;
    [SerializeField]
    private GameObject inputPanel;
    [SerializeField]
    private GameObject FixedViewPanel;
    [SerializeField]
    private GameObject FileInputFiedPanel;
    [SerializeField]
    private Button button_out;
    [SerializeField]
    private Button button_in;
    [SerializeField]
    private Button button_exit;
    [SerializeField]
    private Button button_view_select;
    [SerializeField]
    private Button button_close_program;
    [SerializeField]
    private Button button_load_program;

    private void OnEnable()
    {
        button_out.onClick.AddListener(OutShowHide);
        button_in.onClick.AddListener(InputShowHide);
        button_exit.onClick.AddListener(Exit);
        button_view_select.onClick.AddListener(ViewSelectShow);
        button_close_program.onClick.AddListener(CloseProgram);
        button_load_program.onClick.AddListener(InputFieldShow);
    }
    private void OnDisable()
    {
        button_out.onClick.RemoveListener(OutShowHide);
        button_in.onClick.RemoveListener(InputShowHide);
        button_exit.onClick.RemoveListener(Exit);
        button_view_select.onClick.RemoveListener(ViewSelectShow);
        button_close_program.onClick.RemoveListener(CloseProgram);
        button_load_program.onClick.RemoveListener(InputFieldShow);
    }
    private void OutShowHide()
    {
        ShowAndHide(outPanel);
    }
    private void InputShowHide()
    {
        ShowAndHide(inputPanel);
    }
    private void Exit()
    {
        Application.Quit();
    }
    private void ViewSelectShow() 
    {
        ShowAndHide(FixedViewPanel);
    }
    private void InputFieldShow() 
    {
        ShowAndHide(FileInputFiedPanel);
    }
    private void CloseProgram() 
    {
        VM_Runtime.Instance.close_progrom();
    }
    private void ShowAndHide(GameObject go) 
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
        }
    }
}
