using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Mono.Data.Sqlite;
using System.Data;
using System;
using SQLite4Unity3d;
using UnityEngine.SceneManagement;

//[DLLImport("Mono.Data.dll")]

public class Overlay : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
#if UNITY_STANDALONE
        Screen.SetResolution(1080, 1920, true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonAlphaChange(string oriButtonName, string newButtonName, bool noNewButton = false)
    {
        var oriButton = GameObject.Find(oriButtonName);
        // decreasing alpha to 0% for original button
        var color = oriButton.GetComponent<Image>().color;
        color.a = 0f;
        oriButton.GetComponent<Image>().color = color;
        if (!noNewButton)
        {
            var newButton = GameObject.Find(newButtonName);
            // increasing alpha to 50% for new button
            color = newButton.GetComponent<Image>().color;
            color.a += 0.5f;
            newButton.GetComponent<Image>().color = color;
        }
    }

    public void MenuClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("menu");
        ButtonAlphaChange("Menu", "Store Finder");
        //var storeFinderButton = GameObject.Find("Store Finder");
        //var oriColor = storeFinderButton.GetComponent<Image>().color;
        //oriColor.a += 0.5f;
        //storeFinderButton.GetComponent<Image>().color = oriColor;
    }
    public void StoreFinderClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("store_finder");
        ButtonAlphaChange("Store Finder", "Use Current");
    }
    public void UseCurrentClicked()
    {
        var panel_go = GameObject.Find("Panel");
        panel_go.GetComponent<Image>().sprite = Resources.Load<Sprite>("use_current(search Edi)");
        ButtonAlphaChange("Use Current", "Shop");
    }
    public void ShopClicked()
    {

        SceneManager.LoadScene("Search");
    }

    void DisplayMap()
    {
        var mapPanelGo = GameObject.Find("MapPanel");
        mapPanelGo.GetComponent<CanvasGroup>().alpha = 1;
        mapPanelGo.GetComponent<CanvasGroup>().blocksRaycasts = true;



        var RedShelf = GameObject.Find("Red Shelf");
        var curGO = Instantiate(RedShelf, mapPanelGo.transform);
        //curGO.transform.position = 
        
    }

    void GenerateShelf(string color, (float, float) position)
    {

    }

    void ReadInMapData()
    {

    }

    List<string> tempSearchResult = new List<string>() { "ddd", "dwdwd"};

    public void Search()
    {
        var searchText_go = GameObject.Find("InputText");
        var searchText = searchText_go.GetComponent<Text>().text;
        Debug.Log("Searching for: " + searchText);
        
    }
}
