using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class LightPulse : MonoBehaviour
{
    [FormerlySerializedAs("_effect")] public AnimationCurve effect;
    private Light2D _lightSource;
    // Start is called before the first frame update
    void Start()
    {
        _lightSource = gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _lightSource.falloffIntensity = effect.Evaluate(Time.time)/10;
    }
}
