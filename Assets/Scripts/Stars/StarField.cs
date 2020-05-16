using UnityEngine;
using UnityEngine.Assertions;

public class StarField : MonoBehaviour
{
    public int MaxStars = 100;
    public float StarSize = 0.1f;
    public float StarSizeRange = 0.5f;
    public float FieldWidth = 80f;
    public float FieldHeight = 250f;
    public bool Colorize = false;
    public float ParallaxFactor = 0f;

    float xOffset;
    float yOffset;

    ParticleSystem Particles;
    ParticleSystem.Particle[] Stars;
    
    public float theScrollSpeed = 0.025f;

    Transform stars;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        stars = gameObject.transform;

        Stars = new ParticleSystem.Particle[MaxStars];
        Particles = GetComponent<ParticleSystem>();

        Assert.IsNotNull(Particles, "Particle system missing from object!");

        xOffset = FieldWidth * 0.5f;  // Offset the coordinates to distribute the spread
        yOffset = FieldHeight * 0.5f; // around the object's center

        for (int i = 0; i < MaxStars; i++)
        {
            float randSize = Random.Range(StarSizeRange, StarSizeRange + 1f); // Randomize star size within parameters
            float scaledColor = (true == Colorize) ? randSize - StarSizeRange : 1f; // If coloration is desired, color based on size

            Stars[i].position = GetRandomInRectangle(FieldWidth, FieldHeight) + transform.position;
            Stars[i].startSize = StarSize * randSize;
            Stars[i].startColor = new Color(1f, scaledColor, scaledColor, 1f);
        }
        Particles.SetParticles(Stars, Stars.Length); // Write data to the particle system
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        stars.position = new Vector3(stars.position.x, stars.position.y - theScrollSpeed, stars.position.z);
        for (int i = 0; i < MaxStars; i++)
        {
            Vector3 pos = Stars[i].position + transform.position;

            if (pos.x < (stars.position.x - xOffset))
            {
                pos.x += FieldWidth;
            }
            else if (pos.x > (stars.position.x + xOffset))
            {
                pos.x -= FieldWidth;
            }

            if (pos.y < (stars.position.y - yOffset))
            {
                pos.y += FieldHeight;
            }
            else if (pos.y > (stars.position.y + yOffset))
            {
                pos.y -= FieldHeight;
            }

            Stars[i].position = pos - transform.position;
        }
        Particles.SetParticles(Stars, Stars.Length);

        Vector3 newPos = stars.position * ParallaxFactor; // Calculate the position of the object
        transform.position = newPos;
    }

    /// <summary>
    /// Get a random value within a certain rectangle area.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Vector3 GetRandomInRectangle(float width, float height)
    {
        float x = Random.Range(0, width);
        float y = Random.Range(0, height);
        return new Vector3(x - xOffset, y - yOffset, 0);
    }
}