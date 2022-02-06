using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultButton : MonoBehaviour
{
    private string buttonName;

    public void ThisResultButtonClicked()
    {
        var buttonName = EventSystem.current.currentSelectedGameObject.name + "Name";
        buttonName = GameObject.Find(buttonName).GetComponent<Text>().text;
        
        var search_go = GameObject.Find("SearchOverlay");
        search_go.GetComponent<Search>().clickedResultProductName = buttonName;
        search_go.GetComponent<Search>().DisplayDetail();
    }

    void Update()
    {
        
    }
}
