using UnityEngine;

public class NoiseScript : MonoBehaviour
{
    public float scaleFactor; //ratio of collider size to volume
    public float noiseMultiplier; //how much two sounds will compound eachother
    public float minSize = 0.75f;
    public float noiseStrength = 0f;

    void Start()
    {
        updateSize();
    }

    void Update()
    {

    }

    void updateSize()
    {
        if (noiseStrength * scaleFactor < minSize) { 
            transform.gameObject.GetComponent<CircleCollider2D>().radius = noiseStrength * scaleFactor;
        } 
        else
        {
            transform.gameObject.GetComponent<CircleCollider2D>().radius = minSize;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sound"))
        {
            noiseStrength *= noiseMultiplier;
            updateSize();
        }
        else if (collision.CompareTag("Guard"))
        {
            collision.gameObject.GetComponent<AI_Controller>().handleNoise(transform.position, noiseStrength);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sound"))
        {
            noiseStrength /= noiseMultiplier;
            updateSize();
        }

    }

    public void addNoiseStrength(float strengthToAdd)
    {
        noiseStrength += strengthToAdd;
    }
}
