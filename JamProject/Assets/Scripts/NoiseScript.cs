using UnityEngine;

public class NoiseScript : MonoBehaviour
{
    public float scaleFactor = 0.7f; //ratio of collider size to volume
    public float noiseMultiplier = 1.2f; //how much two sounds will compound eachother
    public float minSize = 0.75f;
    public float radiusScale = 0.8f;
    public float noiseStrength = 0f;

    public GameObject soundMeter;

    void Start()
    {
        updateSize();
    }

    void Update()
    {
        updateSize();

    }

    void updateSize()
    {
        float wantRadius = (float)Mathf.Pow(noiseStrength * scaleFactor, radiusScale);
        if (wantRadius > minSize) { 
            transform.gameObject.GetComponent<CircleCollider2D>().radius = wantRadius;
        } 
        else
        {
            transform.gameObject.GetComponent<CircleCollider2D>().radius = minSize;
        }
        float meterHeight = noiseStrength / 16f;
        soundMeter.transform.localScale = new Vector3(soundMeter.transform.localScale.x, meterHeight, soundMeter.transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Noise"))
        //{
        //    if (collision.gameObject.GetComponent<NoiseScript>().noiseStrength >= 1.5f) noiseStrength *= noiseMultiplier;
        //    updateSize();
        //}
        //else 
        if (collision.CompareTag("Guard"))
        {
            collision.gameObject.GetComponent<AI_Controller>().handleNoise(transform.position, noiseStrength);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Noise"))
        //{
        //    noiseStrength /= noiseMultiplier;
        //    updateSize();
        //}

    }

    public void addNoiseStrength(float strengthToAdd)
    {
        noiseStrength += strengthToAdd;
        updateSize();
    }
}
