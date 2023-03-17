using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{

    static readonly string path = "Prefabs/UI/Panel/MainPanel";
    public MainPanel() : base(new UIType(path)) { }
    public override void OnEnter()
    {
        base.OnEnter();
        UITool.GetOrAddComponetInChildren<Button>("EXIT").onClick.AddListener(
            () =>
            {
                Application.Quit();
            });
        UITool.GetOrAddComponetInChildren<Button>("CloseProgram").onClick.AddListener(
            () =>
            {
                VM_Runtime.Instance.close_progrom();
            });
        UITool.GetOrAddComponetInChildren<Button>("LoadProgram").onClick.AddListener(
          () =>
          {
              //Push(new InputFieldPanel());
          });
        UITool.GetOrAddComponetInChildren<Button>("OutClose").onClick.AddListener(
         () =>
         {

         });
    }
}
