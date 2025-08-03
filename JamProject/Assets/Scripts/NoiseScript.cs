using UnityEngine;

public class NoiseScript : MonoBehaviour
{
    public float scaleFactor; //ratio of collider size to volume
    public float noiseMultiplier; //how much two sounds will compound eachother

    public float noiseStrength = 0;

    void Start()
    {
        updateSize();
    }

    void Update()
    {

    }

    void updateSize()
    {
        transform.gameObject.GetComponent<CircleCollider2D>().radius = noiseStrength * scaleFactor;
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
