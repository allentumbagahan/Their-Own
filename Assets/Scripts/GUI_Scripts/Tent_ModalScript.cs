using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tent_ModalScript : MonoBehaviour
{
    VisualElement root;
    private void OnEnable() {
        
    }
    private void InitializeComponents()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }
    public void Close()
    {
        root.style.display = DisplayStyle.None;
    }
    public void Show()
    {
        Debug.Log("allen");
        root.style.display = DisplayStyle.Flex;
    }

}
