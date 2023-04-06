using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Uart_Function : MonoBehaviour
{
    [SerializeField]
    private GameObject OutputPanel;
    [SerializeField]
    private GameObject InputPanel;
    [SerializeField]
    private InputField InputText;
    private byte outChar;
    private Queue<byte> MonoToVm = new Queue<byte>();
    private byte InputChar;
    private void Start() 
    {
        InputText.onValueChanged.AddListener(GetInputChar);
    }
    private void OnDestroy() 
    {
        InputText.onValueChanged.RemoveListener(GetInputChar);
    }
    public void InputChar_exe(Queue<byte> InputTextChar)
    {
        if (MonoToVm.Count > 0) 
        {
            if(!InputPanel.activeSelf) InputPanel.SetActive(true);
            InputTextChar.Enqueue(MonoToVm.Dequeue());
            VM_8051_Mono.Instance.isInputChar = true;
        }
    }
    private void GetInputChar(string charData) 
    {
        if(charData.Length==0)return;
        InputChar = (byte)charData[charData.Length - 1];
        MonoToVm.Enqueue(InputChar);
    }
    public void Output_exe(Queue<byte> ReceiveChar) 
    {
        StartCoroutine(OutputChar(ReceiveChar));
    }
    private IEnumerator OutputChar(Queue<byte> ReceiveChar) 
    {
        while(VM_Runtime.Instance.isRunning)
        {
            if (ReceiveChar.Count > 0)
            {
                if(!OutputPanel.activeSelf) OutputPanel.SetActive(true);
                outChar = ReceiveChar.Dequeue();
                OutputPanel.GetComponentInChildren<Text>().text += (char)outChar;
            }
            yield return null;
        }
    }
    public void close() 
    {
        InputPanel.GetComponentInChildren<Text>().text="";
        InputPanel.SetActive(false);
        OutputPanel.GetComponentInChildren<Text>().text ="";
        OutputPanel.SetActive(false);
        MonoToVm?.Clear();
    }
}
