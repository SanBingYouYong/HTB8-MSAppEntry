using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public List<string> shelfIDList;  // A01
    public List<string> shelfPartitionList;  // 12

    public string clickedResultProductName = "";

    public Canvas canvas;

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

        //StartCoroutine(loadStreamingAsset("databaseItems.json"));
    }

    // Update is called once per frame
    void Update()
    {
        //if (clickedResultProductName != "")
        //{
        //    var DetailPanel = GameObject.Find("DetailPanel");
        //    Debug.Log("Current Result is: " + clickedResultProductName);
        //    //DetailPanel.transform.position = new Vector3(3.046387f, -315f, 0f);
        //    //var pos = DetailPanel.GetComponent<RectTransform>().position;
        //    //pos.x = -1.5f;
        //    //DetailPanel.GetComponent<RectTransform>().position = pos;
        //}
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
            Highlight.transform.localPosition = new Vector3(69.1f, 0f, 94.7f);
            var oneVertiPart = -138.3f / float.Parse(partition);
            var vertiPos = Highlight.transform.localPosition;
            vertiPos.x += oneVertiPart * float.Parse(horiIndex);
            Highlight.transform.localPosition = vertiPos;
            return;
        }


        Highlight.transform.localPosition = new Vector3(-73.1f, 0f, 94.7f);


        //// to zero
        //var pos = new Vector3(0f, 0f, 0f);
        ////Debug.Log(pos);
        //pos.x -= parentShelf.transform.position.x;
        //Highlight.transform.localPosition = pos;
        Debug.Log("Pos.X is: " + parentShelf.transform.position.x);

        Debug.Log("Canvas Scaler " + canvas.scaleFactor);
        Debug.Log(canvas.scaleFactor * parentShelf.GetComponent<RectTransform>().rect.size);

        //Debug.Log("Size Delta is: " + parentShelf.GetComponent<RectTransform>().GetWorldCorners());
        

        // add portions according to hori index / shelf partition: 
        
        Debug.Log("Hori is " + horiIndex);
        Debug.Log("Parti is " + partition);

        //pos.x += Math.Abs(parentShelf.transform.position.x) * 2 / float.Parse(partition) * float.Parse(horiIndex);  // 80 * 2 / 12 * 3

        //pos = Highlight.transform.position;
        //pos.x += Math.Abs(parentShelf.transform.position.x) * 2 / float.Parse(partition) * float.Parse(horiIndex);  // 80 * 2 / 12 * 3
        //Highlight.transform.position = vg;
        // 1120 per hori bar
        // 144.7 per hori bar in local
        // -138.3 per verti bar in local

        //pos = Highlight.transform.localPosition;
        //pos.x = 1120 / float.Parse(partition) * float.Parse(horiIndex);
        //Highlight.transform.position = pos;
        var onePartition = 144.7f / float.Parse(partition);
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
        //string jsonString = UnityWebRequestJsonString("databaseItems.json");
        StartCoroutine(loadStreamingAsset("databaseItems.json"));
        //Debug.Log(databaseItemsReadIn);
        //var testText = GameObject.Find("TestText");
        //testText.GetComponent<Text>().text = databaseItemsReadIn;

        //JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(databaseItemsReadIn) as JObject;
        //Array allItems = jObject.ToString().ToArray();
        //JArray a = JArray.Parse(databaseItemsReadIn);

        List<Item> allItems = JsonConvert.DeserializeObject<List<Item>>(databaseItemsReadIn);

        //Array allItems = jObject.Properties;
        foreach (Item item in allItems)
        {
            productNameList.Add((string)item.name);
            productShelfIDList.Add((string)item.shelfID);
            productHoriIndexList.Add((string)item.horiIndex);
            productDescriptionList.Add((string)item.description);

            //productNameList.Add((string)item.GetValue("name"));
            //productShelfIDList.Add((string)item.GetValue("ShelfID"));
            //productHoriIndexList.Add((string)item.GetValue("HoriIndex"));
            //productDescriptionList.Add((string)item.GetValue("description"));

        }
        StartCoroutine(loadStreamingAsset("databaseShelves.json"));

        
        //jsonString = null;

        //string newJsonString = UnityWebRequestJsonString("databaseShelves.json");
        //Debug.Log(newJsonString);

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
        //productNameList.Clear();
        //productShelfIDList.Clear();
        //productHoriIndexList.Clear();
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
        //Debug.Log(this.GetComponentInChildren<Text>().text);
        var buttonName = EventSystem.current.currentSelectedGameObject.name + "Name";
        //Debug.Log(buttonName);
        Debug.Log("clicked: " + GameObject.Find(buttonName).GetComponent<Text>().text);
    }

    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
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
        
        
        //yield return result;
    }


    /// <summary>
    /// 通过UnityWebRequest获取本地StreamingAssets文件夹中的Json文件
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    public string UnityWebRequestJsonString(string fileName)
    {
        string url;

        #region 分平台判断 StreamingAssets 路径
        //如果在编译器 或者 单机中
#if UNITY_EDITOR || UNITY_STANDALONE

        url = "file://" + Application.dataPath + "/StreamingAssets/" + fileName;
        //否则如果在Iphone下
#elif UNITY_IPHONE

            url = "file://" + Application.dataPath + "/Raw/"+ fileName;
            //否则如果在android下
#elif UNITY_ANDROID
            //url = "jar:file://" + Application.dataPath + "!/assets/"+ fileName;
            url = "jar:file://" + Application.dataPath + "/Raw/"+ fileName;
#endif
        #endregion
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();//读取数据
        //while (true)
        //{
        //    if (request.downloadHandler.isDone)//是否读取完数据
        //    {
        //        return request.downloadHandler.text;
        //    }
        //}
        while (!request.isDone)
        {
            if (request.isNetworkError || request.isHttpError)
            {
                break;
            }
        }
        return request.downloadHandler.text;
    }

    public void StoreLoginClicked()
    {
        SceneManager.LoadScene("StoreLogin");
    }

}
