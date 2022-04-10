using System.Collections;
using System.Collections.Generic;
using Wrld;
using Wrld.Space;
using UnityEngine;

public class Building : MonoBehaviour
{

    [SerializeField]
    private GameObject boxPrefab = null;

    private LatLong cameraLocation = LatLong.FromDegrees(37.795641, -122.404173);
    private LatLong boxLocation1 = LatLong.FromDegrees(37.795159, -122.404336);
    private LatLong boxLocation2 = LatLong.FromDegrees(37.795173, -122.404229);
    
    
    private LatLong boxLocation3 = LatLong.FromDegrees(37.797143, -122.404559);
    private LatLong boxLocation4 = LatLong.FromDegrees(37.796123, -122.404029);
    private LatLong boxLocation5 = LatLong.FromDegrees(37.794103, -122.404929);
    private LatLong boxLocation6 = LatLong.FromDegrees(37.794203, -122.405229);
    private LatLong boxLocation7 = LatLong.FromDegrees(37.799303, -122.406229);



    private void OnEnable()
    {
        StartCoroutine(Creator());
    }
    
    IEnumerator Creator()
    {
        
       
            yield return new WaitForSeconds(0f);

            MakeBox(boxLocation1);
            MakeBox(boxLocation2);
            MakeBox(boxLocation3);
            MakeBox(boxLocation4);
            MakeBox(boxLocation5);
            MakeBox(boxLocation6);
            MakeBox(boxLocation7);
    }

    void MakeBox(LatLong latLong)
    {
        var ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(latLong);
        LatLongAltitude buildingIntersectionPoint;
        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        if (didIntersectBuilding)
        {
            var boxAnchor = Instantiate(boxPrefab) as GameObject;
            boxAnchor.transform.parent = this.transform;
            boxAnchor.GetComponent<GeographicTransform>().SetPosition(buildingIntersectionPoint.GetLatLong());

            var box = boxAnchor.transform.GetChild(0);
            box.localPosition = Vector3.up * (float)buildingIntersectionPoint.GetAltitude();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
