using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    Transform parentTransform;
    float distToAgent;
    public float fadeDistance;
    public SpriteRenderer spriteRenderer;

    public float angleToMouse; //in degrees

    void Start()
    {
        parentTransform = transform.parent;
        distToAgent = transform.position.magnitude;
    }

    void Update()
    {
        calculateAngleToMouse();

        //move and rotate the cursor
        transform.rotation = Quaternion.Euler(0f, 0f, angleToMouse * Mathf.Rad2Deg);
        transform.localPosition = new Vector3(-Mathf.Sin(angleToMouse), Mathf.Cos(angleToMouse), 0);

        //make the indicator more transparent if the mouse is too close
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 agentToMouse = (mousePos - parentTransform.position);
        float agentToMouseMag = new Vector2(agentToMouse.x, agentToMouse.y).magnitude;

        Color newColor = spriteRenderer.color;
        newColor.a = calculateAlpha(agentToMouseMag);
        spriteRenderer.color = newColor;
    }

    public void calculateAngleToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 agentToMouse = (mousePos - parentTransform.position);
        angleToMouse = Mathf.Atan2(-agentToMouse.x, agentToMouse.y);
    }

    public float calculateAlpha(float distAway)
    {
        float newAlpha = (2 * distAway / fadeDistance) - 1f; //if dist from agent to mouse is less than fade distance, alpha will decrease
        if (distAway > fadeDistance) newAlpha = 1f;
        if (distAway < fadeDistance / 2) newAlpha = 0f;
        return newAlpha;
    }
}
