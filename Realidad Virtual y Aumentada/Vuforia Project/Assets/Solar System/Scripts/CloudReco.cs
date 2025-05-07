using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;
using UnityEngine.Networking;
using System.IO;

public class CloudReco : MonoBehaviour
{

    public class ObjectTargetInfo
    {
        public string name;
        public string points;
        public string url;

        public static ObjectTargetInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ObjectTargetInfo>(jsonString);
        }
    }

    CloudRecoBehaviour mCloudRecoBehaviour;
    string mTargetMetadata = "";
    ObjectTargetInfo datosTarget;
    bool mIsScanning = false;
    public TextMeshProUGUI PlanetText;
    public ImageTargetBehaviour ImageTargetTemplate;
    public PointsManager pointsManager;


    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        datosTarget = ObjectTargetInfo.CreateFromJSON(cloudRecoSearchResult.MetaData);

        pointsManager.CheckAnswer(datosTarget.name);



        mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);

        StartCoroutine(GetAssetBundle(datosTarget.url));

        mCloudRecoBehaviour.enabled = false;
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }

    }


    IEnumerator GetAssetBundle(string link)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(link);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            string[] allAssetNames = bundle.GetAllAssetNames();
            string gameObjectName = Path.GetFileNameWithoutExtension(allAssetNames[0]).ToString();
            GameObject objectFound = bundle.LoadAsset(gameObjectName) as GameObject;
            Instantiate(objectFound, ImageTargetTemplate.transform.position, ImageTargetTemplate.transform.rotation);

        }
    }

    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + datosTarget.name);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                // Reset Behaviour
                mCloudRecoBehaviour.enabled = true;
                mTargetMetadata = "";
            }
        }
    }
}
