using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsoTactics
{
    //Generic CharacterShake script
    public class Reactions : MonoBehaviour
    {
        private Reactions _instance;

        private Vector3 _originalPos;
        private float _timeAtCurrentFrame;
        private float _timeAtLastFrame;
        private float _fakeDelta;

        void Awake()
        {
            _instance = this;
        }

        void Update()
        {
            // Calculate a fake delta time, so we can Shake while game is paused.
            _timeAtCurrentFrame = Time.realtimeSinceStartup;
            _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
            _timeAtLastFrame = _timeAtCurrentFrame;
        }

        public void Shake(float duration, float amount)
        {
            _instance._originalPos = _instance.gameObject.transform.localPosition;
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.CShake(duration, amount));
        }

        public void Evade(float duration, float amount)
        {
            _instance._originalPos = _instance.gameObject.transform.localPosition;
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.CEvade(duration, amount));
        }

        private IEnumerator CShake(float duration, float amount)
        {

            while (duration > 0)
            {
                transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

                duration -= _fakeDelta;

                yield return null;
            }

            transform.localPosition = _originalPos;
        }
        
        private IEnumerator CEvade(float duration, float amount)
        {

            while (duration > 0)
            {
                var pos = _originalPos.x - 0.45f;
                transform.localPosition = new Vector3(pos, _originalPos.y, _originalPos.z);

                duration -= _fakeDelta;

                yield return null;
            }

            transform.localPosition = _originalPos;
        }
    }
}
