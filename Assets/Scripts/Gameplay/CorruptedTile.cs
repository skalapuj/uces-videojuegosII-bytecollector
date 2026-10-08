using System.Collections;
using UnityEngine;
namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CorruptedTile : MonoBehaviour
    {
        [SerializeField] private float warningTime = 1.0f;
        [SerializeField] private float activeTime = 4.0f;
        private void Start()
        {
            StartCoroutine(Lifecycle());
        }
        private IEnumerator Lifecycle()
        {
            Collider2D col = GetComponent<Collider2D>();
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color c = sr.color;
            col.enabled = false; // fase de aviso: no daña
            sr.color = new Color(c.r, c.g, c.b, 0.3f);
            yield return new WaitForSeconds(warningTime);
            col.enabled = true; // fase activa: daña
            sr.color = new Color(c.r, c.g, c.b, 1f);
            yield return new WaitForSeconds(activeTime);
            Destroy(gameObject);
        }
    }
}