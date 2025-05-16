using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//let camera follow target
namespace Cainos.CustomizablePixelCharacter
{
    public class CameraFollow : MonoBehaviour
    {
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        public GameObject player;

        private void Start()
        {
            if (player == null) return;

            offset = transform.position - player.transform.position;
        }

        private void Update()
        {
            if (player == null) return;

            targetPos = player.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
