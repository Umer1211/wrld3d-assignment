using System.Collections;
using UnityEngine;
using Wrld;
using Wrld.Common.Maths;
using Wrld.Space;
using Wrld.Transport;
public class transport : MonoBehaviour
{
    [SerializeField]
    private GameObject boxPrefab = null;

    private readonly LatLongAltitude m_inputCoords = LatLongAltitude.FromDegrees(37.784468, -122.401268, 10.0);
    private float m_inputHeadingDegreesA = 225.0f;
    private float m_inputHeadingDegreesB = 300.0f;
    private bool m_isHeadingA;
    private TransportPositioner m_transportPositioner;
    GameObject m_sphereInput;
    GameObject m_sphereOutput;
    GameObject m_directionIndicatorInput;
    GameObject m_directionIndicatorOutput;

    private void OnEnable()
    {
        CreateVisualizationObjects();

        var options = new TransportPositionerOptionsBuilder()
            .SetInputCoordinates(m_inputCoords.GetLatitude(), m_inputCoords.GetLongitude())
            .SetInputHeading(GetCurrentInputHeading())
            .Build();

        m_transportPositioner = Api.Instance.TransportApi.CreatePositioner(options);
        m_transportPositioner.OnPointOnGraphChanged += OnPointOnGraphChanged;

        StartCoroutine(ToggleInputHeading());
    }

    private void OnDisable()
    {
        GameObject.Destroy(m_sphereInput);
        GameObject.Destroy(m_sphereOutput);
        GameObject.Destroy(m_directionIndicatorInput);
        GameObject.Destroy(m_directionIndicatorOutput);

        m_transportPositioner.OnPointOnGraphChanged -= OnPointOnGraphChanged;
        m_transportPositioner.Discard();
    }

    IEnumerator ToggleInputHeading()
    {
        while (enabled)
        {
            m_isHeadingA = !m_isHeadingA;
            //m_directionIndicatorInput.transform.eulerAngles = new Vector3(0.0f, GetCurrentInputHeading(), 0.0f);

            m_transportPositioner.SetInputHeading(GetCurrentInputHeading());
            yield return new WaitForSeconds(5);
        }
    }

    private float GetCurrentInputHeading()
    {
        return m_isHeadingA ? m_inputHeadingDegreesA : m_inputHeadingDegreesB;
    }

    private void OnPointOnGraphChanged()
    {
        if (m_transportPositioner.IsMatched())
        {
            var pointOnGraph = m_transportPositioner.GetPointOnGraph();

            var outputLLA = LatLongAltitude.FromECEF(pointOnGraph.PointOnWay);
            const double verticalOffset = 1.0;
            outputLLA.SetAltitude(outputLLA.GetAltitude() + verticalOffset);
            var outputPosition = Api.Instance.SpacesApi.GeographicToWorldPoint(outputLLA);

           
        }
    }

    private void CreateVisualizationObjects()
    {
        var inputPosition = Api.Instance.SpacesApi.GeographicToWorldPoint(m_inputCoords);
        m_sphereInput = CreateSphere(Color.red, 2.0f);
        
        m_sphereInput.transform.localPosition = inputPosition;

       
    }


    private GameObject CreateSphere(Color color, float radius)
    {
        
        var sphere = Instantiate(boxPrefab) as GameObject; 
        sphere.transform.localScale = Vector3.one * radius;
        sphere.transform.parent = this.transform;
        return sphere;
    }

}

