using UnityEngine;

public class NoiseScript : MonoBehaviour
{
    public float scaleFactor; //ratio of collider size to volume
    public float noiseMultiplier; //how much two sounds will compound eachother

    public float noiseStrength = 0;

    void Start()
    {

    }

    void Update()
    {
        
    }

    void updateSize()
    {
        transform.localScale = new Vector3(noiseStrength * scaleFactor, transform.localScale.y, transform.localScale.z);
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
