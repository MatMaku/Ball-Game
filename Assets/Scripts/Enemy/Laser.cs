using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    public float chargeDuration = 1f;
    public float activeDuration = 3f;
    public float transitionSpeed = 3f;

    public Vector3 warningScale = new Vector3(0.1f, 0.5f, 1f);
    public Vector3 activeScale = new Vector3(0.3f, 2.3f, 1f);

    private Collider2D laserCollider;

    private void Awake()
    {
        laserCollider = GetComponent<Collider2D>();
        StartCoroutine(LaserSequence());
    }

    private IEnumerator LaserSequence()
    {
        // Aparece y carga visualmente
        yield return StartCoroutine(TransitionScale(warningScale));
        laserCollider.enabled = false;

        // Espera en modo carga
        yield return new WaitForSeconds(chargeDuration);

        // Activa el rayo
        yield return StartCoroutine(TransitionScale(activeScale));
        laserCollider.enabled = true;

        // Mantiene el rayo activo
        yield return new WaitForSeconds(activeDuration);

        // Se desactiva y vuelve a la escala original
        laserCollider.enabled = false;
        yield return StartCoroutine(TransitionScale(warningScale));

        // Destruye el láser
        Destroy(gameObject);
    }

    private IEnumerator TransitionScale(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }
}
