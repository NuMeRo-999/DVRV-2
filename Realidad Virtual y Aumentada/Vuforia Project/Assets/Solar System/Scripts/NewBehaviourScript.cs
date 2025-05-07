// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.Networking;

// public class NewBehaviourScript : MonoBehaviour
// {

//    void Start()
//    {
//        TestAsset loadedJsonFile = Resources.Load<TestAsset>("TestAsset");
//        Debug.Log(loadedJsonFile.testString);
//        jugador = Metadata.CreateFromJSON(loadedJsonFile.text);
//        StartCoroutine(GetAssetBundle());
//    }

//    IEnumerator GetAssetBundle()
//    {
//        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://url/to/your/assetbundle");
//        yield return www.SendWebRequest();

//        if (www.result != UnityWebRequest.Result.Success)
//        {
//            Debug.Log(www.error);
//        }
//        else
//        {
//            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
//            string[] allAssetNames = bundle.GetAllAssetNames();
//            string gameObjectName = Path.GetFileNameWithoutExtension(allAssetNames[0]).ToString;
//            GameObject go = bundle.LoadAsset<GameObject>(gameObjectName);
//            Instantiate(go);
//        }
//    }

//    void Update()
//    {
        
//    }
// }
