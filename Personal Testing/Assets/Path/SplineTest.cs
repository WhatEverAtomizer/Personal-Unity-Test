using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineTest : MonoBehaviour
{
    public KnotLinkCollection knotLinkCollection;
    public BezierKnot bexierKnot;
    public SplineKnotIndex splineKnotIndex;
    public SplineContainer splineContainer;
    // Start is called before the first frame update
    void Start()
    {
        knotLinkCollection = splineContainer.KnotLinkCollection;
        //Debug.LogWarning(splineContainer.KnotLinkCollection);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
