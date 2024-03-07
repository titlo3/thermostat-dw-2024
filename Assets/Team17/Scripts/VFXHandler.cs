using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Team17
{
    public class VFXHandler : MonoBehaviour
    {
        [Header("Heat Levels: (Not adjustable at runtime)")]
        [SerializeField][Range(0f, 1f)] float heatLevel;
        [SerializeField][Range(0f, 1f)] float heatLevelCurrent;

        [Header("Object Refs: ")]
        [SerializeField] Thermostat thermostat;
        [SerializeField] ParticleSystem snow;
        [SerializeField] ParticleSystem heat;
        [SerializeField] VolumeProfile volume;

        [Header("Color Correction:")]
        [SerializeField] Color heatColor;
        [SerializeField] Color coldColor;

        [Header("Tuners: ")]
        [SerializeField][Range(0, 100)] float maxHeatEmission;
        [SerializeField][Range(0, 100)] float maxColdEmission;

        // Start is called before the first frame update
        void Start()
        {
            heatLevelCurrent = heatLevel;
        }

        // Update is called once per frame
        void Update()
        {
            //Set heat level
            heatLevel = thermostat.getHeat();

            //Color Correction
            ColorAdjustments colorAdjustments;
            volume.TryGet<ColorAdjustments>(out colorAdjustments);
            float colorR = Mathf.Lerp(coldColor.r, heatColor.r, heatLevel);
            float colorG = Mathf.Lerp(coldColor.g, heatColor.g, heatLevel);
            float colorB = Mathf.Lerp(coldColor.b, heatColor.b, heatLevel);

            colorAdjustments.colorFilter.value = new Color(colorR, colorG, colorB);

            //Snow Effects
            ParticleSystem.EmissionModule snowEmission = snow.emission;
            if (heatLevel < 0.5f)
                snowEmission.rateOverTime = Mathf.SmoothStep(maxColdEmission, 0, heatLevel);
            else
                snowEmission.rateOverTime = 0;
            //Heat effects
            ParticleSystem.EmissionModule heatEmission = heat.emission;
            if (heatLevel > 0.5f)
                heatEmission.rateOverTime = Mathf.SmoothStep(0, maxHeatEmission, heatLevel);
            else
                heatEmission.rateOverTime = 0;


        }
    }
}
