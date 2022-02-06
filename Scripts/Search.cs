using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Search : MonoBehaviour
{
    public List<string> productNameList;  // Sock, product name
    public List<string> productShelfIDList;  // A01, asile ID
    public List<string> productHoriIndexList;  // 0 or 1
    public List<string> productDescriptionList;  // xxxxx

    public List<string> shelfIDList;  // A01, A02
    public List<string> shelfPartitionList;  // 12, 6, 8

    public string clickedResultProductName = "";

    //public Canvas canvas;

    public List<int> ListOfIndexes;

    public string databaseItemsReadIn;
    public string databaseShelvesReadIn;


    // Start is called before the first frame update
    void Start()
    {
        productNameList = new List<string>();
        productShelfIDList = new List<string>();
        productHoriIndexList = new List<string>();
        productDescriptionList = new List<string>();

        shelfIDList = new List<string>();
        shelfPartitionList = new List<string>();

        SearchItemReadIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDetail()
    {
        var SearchResultScrollView = GameObject.Find("Scroll View Search Result");
        SearchResultScrollView.GetComponent<CanvasGroup>().alpha = 0;
        SearchResultScrollView.GetComponent<CanvasGroup>().blocksRaycasts = false;

        var DetailPanel = GameObject.Find("DetailPanel");
        Debug.Log("Current Result is: " + clickedResultProductName);
        DetailPanel.GetComponent<CanvasGroup>().alpha = 1;
        DetailPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

        var ProductName = GameObject.Find("ProductName");
        ProductName.GetComponent<Text>().text = clickedResultProductName;

        var DescriptionText = GameObject.Find("DescriptionText");
        DescriptionText.GetComponent<Text>().text = productDescriptionList[productNameList.IndexOf(clickedResultProductName)];

        var ShelfIDActual = GameObject.Find("ShelfIDActual");
        ShelfIDActual.GetComponent<Text>().text = productShelfIDList[productNameList.IndexOf(clickedResultProductName)];

        // highlight: 
        var Highlight = GameObject.Find("Highlight");
        Highlight.GetComponent<CanvasGroup>().alpha = 1;

        var parentShelf = GameObject.Find(ShelfIDActual.GetComponent<Text>().text);
        
        Highlight.transform.SetParent(parentShelf.transform, false);
        Highlight.transform.localScale = new Vector3(0.1f, 1f, 1f);

        var horiIndex = productHoriIndexList[productNameList.IndexOf(clickedResultProductName)];
        var partition = shelfPartitionList[shelfIDList.IndexOf(productShelfIDList[productNameList.IndexOf(clickedResultProductName)])];
        // partition: index of shelf ID <- shelfID <- index of Product Name

        if (ShelfIDActual.GetComponent<Text>().text.Contains('C'))  // vertical shelf, temp and ugly
        {
            Highlight.transform.localPosition = new Vector3(69.1f, 0f, 94.7f);  // measured in Editor
            var oneVertiPart = -138.3f / float.Parse(partition);  // measured in Editor
            var vertiPos = Highlight.transform.localPosition;
            vertiPos.x += oneVertiPart * float.Parse(horiIndex);
            Highlight.transform.localPosition = vertiPos;
            return;
        }

        Highlight.transform.localPosition = new Vector3(-73.1f, 0f, 94.7f);  // measured in Editor
        var onePartition = 144.7f / float.Parse(partition);  // measured in Editor
        var pos = Highlight.transform.localPosition;
        pos.x += onePartition * float.Parse(horiIndex);
        Highlight.transform.localPosition = pos;
    }

    public void BackToSearchResult()
    {
        Debug.Log("Back to Search Result clicked");
        var SearchResultScrollView = GameObject.Find("Scroll View Search Result");
        SearchResultScrollView.GetComponent<CanvasGroup>().alpha = 1;
        SearchResultScrollView.GetComponent<CanvasGroup>().blocksRaycasts = true;

        var DetailPanel = GameObject.Find("DetailPanel");
        DetailPanel.GetComponent<CanvasGroup>().alpha = 0;
        DetailPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void SearchItemReadIn()
    {
        StartCoroutine(loadStreamingAsset("databaseItems.json"));
        // Item defined in StaffInterface; 
        List<Item> allItems = JsonConvert.DeserializeObject<List<Item>>(databaseItemsReadIn);
        
        foreach (Item item in allItems)
        {
            productNameList.Add((string)item.name);
            productShelfIDList.Add((string)item.shelfID);
            productHoriIndexList.Add((string)item.horiIndex);
            productDescriptionList.Add((string)item.description);
        }

        StartCoroutine(loadStreamingAsset("databaseShelves.json"));
        // don't need to modify the shelf, at least for now, so used a different approach; 
        // don't want to define a shelf class... 
        JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(databaseShelvesReadIn) as JObject;
        Array allShelves = jObject.GetValue("shelves").ToArray();
        foreach (JObject shelf in allShelves)
        {
            shelfIDList.Add((string)shelf.GetValue("id"));
            shelfPartitionList.Add((string)shelf.GetValue("ShelfPartition"));
        }
    }

    void SearchItem(string keyWord)
    {
        ListOfIndexes = new List<int>();
        var Content = GameObject.Find("Content");
        
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string name in productNameList)
        {
            if ((name.ToUpper()).Contains((keyWord.ToUpper())))
            {
                ListOfIndexes.Add(productNameList.IndexOf(name));
            }
        }
    }

    public void SearchClicked()
    {
        var searchText_go = GameObject.Find("InputText");
        var searchText = searchText_go.GetComponent<Text>().text;
        Debug.Log("Searching for: " + searchText);
        SearchItem(searchText);
        DisplaySearchResult();
    }

    void DisplaySearchResult()
    {
        var resultButton = Resources.Load("ResultButton");
        var Content = GameObject.Find("Content");
        foreach (int i in ListOfIndexes)
        {
            var curResButton = Instantiate(resultButton) as GameObject;
            curResButton.transform.SetParent(Content.transform);
            curResButton.transform.localScale = new Vector3(1f, 1f, 1f);
            curResButton.name = i.ToString() + "th Result";
            curResButton.transform.GetChild(0).name = curResButton.name + "Name";
            curResButton.transform.GetChild(0).GetComponent<Text>().text = productNameList[i];
        }
    }

    public void SearchResultClicked()
    {
        Debug.Log("Result Clicked");
        var buttonName = EventSystem.current.currentSelectedGameObject.name + "Name";
        Debug.Log("clicked: " + GameObject.Find(buttonName).GetComponent<Text>().text);
    }

    // NOTE: WWW was not used, but in the last minute we were trying everything to make json IO possible on Android; 
    // it's working, can switch to UnityWebRequestJsonString any time, but too lazy

    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);  // :(
            yield return www;
            result = www.text;
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
        }

        Debug.Log("Loaded file: " + result);
        
        if (fileName == "databaseItems.json")
        {
            databaseItemsReadIn = result;
        }
        else
        {
            databaseShelvesReadIn = result;
        }
    }

    public void StoreLoginClicked()
    {
        SceneManager.LoadScene("StoreLogin");
    }

}
