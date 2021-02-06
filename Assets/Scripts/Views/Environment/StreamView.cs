using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class StreamView : MonoBehaviour
    {
        public readonly float[] startingOpacity = { 0.1f, 0.3f };
        public readonly float[] startingScale = { 0.1f, 0.3f };
        
        public float speed { get; private set; } = 1f; 
        public float speedModifier { get; } = 0.75f;

        private Animator _animator;
        private SpriteRenderer _renderer;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.SetFloat("Offset", Random.value);

            _renderer = GetComponent<SpriteRenderer>();
            name = "Stream";
        }

        void Start()
        {
            float scale = Random.Range(startingScale[0], startingScale[1]);
            transform.localScale = Vector3.one * scale;
            _renderer.color = new Color(1f, 1f, 1f, Random.Range(startingOpacity[0], startingOpacity[1]));

            speed += speedModifier - scale;
            transform.position = new Vector3(Random.value * 10f - 5f, Random.value * 50f + 10f, 0);
        }

        void Update()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}
