using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileInput : MonoBehaviour
{
    [SerializeField]
    private GameObject fileNoneTip;
    private void OnEnable()
    {
        this.transform.GetChild(0).GetComponent<InputField>().onEndEdit.AddListener(LoadProgrom);
    }
    private void OnDisable()
    {
        this.transform.GetChild(0).GetComponent<InputField>().onEndEdit.RemoveListener(LoadProgrom);
    }
    private void LoadProgrom(string path) 
    {
        path = this.transform.GetChild(0).GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(path)) return;
        if (!File.Exists(path)) 
        {
            fileNoneTip.SetActive(true);
            return;
        }
        VM_Runtime.Instance.load_progrom(path);
        this.transform.gameObject.SetActive(false);
    }
}
