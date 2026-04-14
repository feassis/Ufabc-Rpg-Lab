using UnityEngine;

public class BoomerangSkill : Skill
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float speed = 2f;

    
    private float angle;


    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        Vector3 desiredPos = player.position + new Vector3(x, y, 0);

        transform.position = desiredPos;
    }
}
