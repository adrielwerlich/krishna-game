using System.Collections;
using UnityEngine;

public class CustomBomb2 : MonoBehaviour
{

    public float duration = 5f;
    public float radius = 5f;
    public float explosionDuration = .2f;
    public GameObject explosionModel;

    private float explosionTimer;
    private bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = duration;
        explosionModel.transform.localScale = Vector3.one * radius;
        explosionModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f && exploded == false) {
            exploded = true;
            Collider[] hitObjects = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider collider in hitObjects)
            {
                //UnityEngine.Debug.Log(collider.name + " was hit!");
                var enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Hit(enemy.Type, "bomb", multiplier: 2);
                }
            }

            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode() {
        explosionModel.SetActive(true);

        yield return new WaitForSeconds(explosionDuration);

        Destroy(gameObject);
    }
}
