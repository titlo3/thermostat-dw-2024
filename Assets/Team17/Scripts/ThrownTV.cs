using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team17
{
    public class ThrownTV : MonoBehaviour
    {
        [SerializeField] GameObject startpoint;
        [SerializeField] GameObject endpoint;
        [SerializeField] GameObject thermostatSlider;

        [Header("Animation Settings: ")]
        [SerializeField] float animationTime;
        float animationTimeC;

        [SerializeField][Range(0, 6)] float verticalAnimationScaler;
        [SerializeField] AnimationCurve verticalAnimation;

        [SerializeField] AnimationCurve horizontalAnimation;

        [SerializeField] float rotationSpeed;

        [SerializeField] Vector2 sizeRange;




        // Start is called before the first frame update
        void Start()
        {
            transform.position = startpoint.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (animationTimeC < animationTime)
            {
                animationTimeC += Time.deltaTime;
                transform.position = Vector3.Lerp(startpoint.transform.position, endpoint.transform.position, animationTimeC / animationTime);
                transform.position += new Vector3(0, verticalAnimation.Evaluate(animationTimeC) * verticalAnimationScaler, 0);
                transform.Rotate(0, 0, rotationSpeed);
                transform.localScale = Vector2.one * Mathf.Lerp(sizeRange.x, sizeRange.y, animationTimeC / animationTime);
                thermostatSlider.SetActive(false);
            }
            else
            {
                GetComponent<Animator>().enabled = false;
            }

        }
    }
}
