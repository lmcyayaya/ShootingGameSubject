using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class CameraShaker : MonoBehaviour
    {
        private static CameraShaker instance = null;
        public static CameraShaker Instance
        {
            get {return instance;}
        }
        public float power = 0.2f;
        public float duration = 0.05f;
        public float slowDownAmount = 1.5f;
        public Transform cam;
        private Vector3 oriPos;
        private Vector3 orginPos;
        private float initiaDuration = 0.2f;
        void Start()
        {
            instance = this;
            cam = Camera.main.transform;
            oriPos = cam.transform.localPosition;

        }
        public IEnumerator CameraShakeOneShot(float p,float dur,float sDAmount)
        {
            orginPos = cam.transform.localPosition;
            while(dur > 0)
            {
                cam.localPosition = orginPos +Random.insideUnitSphere * p;
                dur -= Time.deltaTime * sDAmount;
                yield return null;
            }
            cam.transform.localPosition = orginPos;
            StartCoroutine(ResetCamera());
        }
        public void CameraKeepShake(float p)
        {
            orginPos = cam.transform.localPosition;
            cam.localPosition = orginPos +Random.insideUnitSphere * p;
        }
        public IEnumerator ResetCamera()
        {
            yield return new WaitForSeconds(Time.deltaTime);
            cam.transform.localPosition = oriPos;
        }
        public IEnumerator CameraShakeTest()
        {
            initiaDuration = duration;
            orginPos = cam.transform.localPosition;
            while(duration > 0)
            {
                cam.localPosition = orginPos +Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
                yield return null;
            }
            duration = initiaDuration;
            cam.transform.localPosition = orginPos;
        }
    }


