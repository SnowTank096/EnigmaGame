using UnityEngine;


public class RopeVisualizer : MonoBehaviour 
{
    public Transform[] ropeSegments;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("BlackPlainMaterial");
        lineRenderer.numCornerVertices = 4;
        lineRenderer.numCapVertices = 4;
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.startWidth = 0.075f;
        lineRenderer.endWidth = 0.075f;
    }


    void LateUpdate()
    {
        if (ropeSegments == null || ropeSegments.Length == 0) return;

        if (lineRenderer.positionCount != ropeSegments.Length)
        {
            lineRenderer.positionCount = ropeSegments.Length;
        }

        for (int i = 0; i < ropeSegments.Length; i++)
        {
            if (ropeSegments[i] != null)
            {
                Vector3 finalVisualPosition = ropeSegments[i].position;

                if (i == 0)
                {
                    Vector3 localOffset = new Vector3(0f, 0.07f, -0.05f); 
                    finalVisualPosition += ropeSegments[i].TransformDirection(localOffset);
                }
                else if (i == 5)
                {
                    Vector3 localOffset = new Vector3(0f, 0.07f, -0.05f); 
                    finalVisualPosition += ropeSegments[i].TransformDirection(localOffset);
                }

                lineRenderer.SetPosition(i, finalVisualPosition);
            }
        }
    }

}

