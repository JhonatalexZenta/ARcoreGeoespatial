using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


using UnityEngine.XR.ARFoundation;

using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;


//using UnityEngine.XR.ARSubsystems;




public class VPSManager : MonoBehaviour
{
    [SerializeField] private AREarthManager earthManager;

    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EartPosition EartPosition;
    
    }


    [Serializable]
    public struct EartPosition
    {
        public Double Latitude;
         public Double Longitude;
          public Double Altitud;
    
    }

    
    [SerializeField] private   ARAnchorManager  aRAnchorManager;
    [SerializeField] private   List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();



    // Start is called before the first frame update
    void Start()
    {
        VerifyGeospatialSupport();
    }


    private void VerifyGeospatialSupport(){
        
        var result = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);

        switch (result)
        {
            case FeatureSupported.Supported:
                Debug.Log("Ready to use Vps");
                PlaceObjects();
            break;



            case FeatureSupported.Unknown:
                Debug.Log("Ready to use Vps");
                Invoke("VerifyGeospatialSupport", 5.0f);
            break;



            case FeatureSupported.Unsupported:
                Debug.Log("VPS unsupported");
            break;

       
        }



    }


    private void PlaceObjects(){
    
        if(earthManager.EarthTrackingState==TrackingState.Tracking)
        {

            var geospatialPose = earthManager.CameraGeospatialPose;

            foreach(var obj in  geospatialObjects)
            {
        
            var earthPosition = obj.EartPosition;
            var objAnchor = ARAnchorManagerExtensions.AddAnchor(aRAnchorManager,earthPosition.Latitude, earthPosition.Longitude,earthPosition.Altitud,Quaternion.identity);
            Instantiate(obj.ObjectPrefab,objAnchor.transform);

            }
    
        }else if(earthManager.EarthTrackingState==TrackingState.None){
    
        Invoke ("PlaceObjects",5.0f);
    
        }

    }
   
}
