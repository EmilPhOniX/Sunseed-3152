using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class seedGrowingController : MonoBehaviour
{
    SeedController seedController;
    private GameObject childObject;
    private int currentStep;

    private void WateredSeeds() => StartCoroutine(Growth(3, 1, new Vector3(0, 0, 0), 0, () => seedController.OnGrowing()));
    private void GrowSeeds() => StartCoroutine(Growth(5, 1, new Vector3(0.15f, 0.3f, 0.15f), 0, () => seedController.OnReady()));
    private void GrowVegetables() => StartCoroutine(Growth(10, 1, new Vector3(0.03f, 0.03f, 0.03f), 0.05f, () => Explode()));

    private void Start()
    {
        seedController = GetComponentInParent<SeedController>();

        if (transform.childCount > 0)
        {
            childObject = transform.GetChild(0).gameObject;

            if (childObject.CompareTag("SeedsDots"))
            {
                GrowSeeds();
            }

            if (childObject.CompareTag("Vegetables"))
            {
                GrowVegetables();
            }
        }
        else
        {
            Debug.Log("No child object found for growth. Check for watered");
            if (this.CompareTag("Watered"))
            {
                Debug.Log("This item is watered");
                WateredSeeds();
            }
        }
    }

    private IEnumerator Growth(int stepDuration, int stepWaiting, Vector3 growingStep, float elevatorLevel, Action onOver)
    {
        currentStep = 0;

        // Boucle tant que le nombre d'étapes actuelles est inférieur à la durée totale
        while (currentStep < stepDuration)
        {
            // Attend le nombre de secondes spécifié avant la prochaine étape
            yield return new WaitForSeconds(stepWaiting);

            if (childObject != null)
            {
                childObject.transform.localScale += growingStep;
                childObject.transform.position += new Vector3(0, elevatorLevel, 0);

                if (childObject.CompareTag("Vegetables"))
                {
                    var renderers = childObject.GetComponentsInChildren<Renderer>();

                    foreach (var rend in renderers)
                    {
                        rend.material.color = Color.Lerp(rend.material.color, Color.red, 0.1f);
                    }
                }
            }
            currentStep++;
        }
        onOver.Invoke();
    }

    private void Explode()
    {
        ParticleSystem ps = childObject.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();

            var renderer = childObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
        seedController.Destroy();
        Destroy(gameObject);
    }
}
