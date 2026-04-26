using UnityEngine;

public class GnarBoomerangSkill : Skill
{
    [SerializeField] private GnarBoomerang gnarBoomerangPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private float speed;
    [SerializeField] private float goingTime;
    [SerializeField] private float backingTime;

    private float timer = 0;

    private void Update()
    {
        if(timer <= 0)
        {
            var randDir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);

            var gnarBoom = Instantiate<GnarBoomerang>(gnarBoomerangPrefab);

            gnarBoom.transform.position = transform.position;

            gnarBoom.Setup(randDir.normalized, player, speed, goingTime, backingTime, player.gameObject.GetComponent<PlayerCombat>().GetDamage());

            timer = cooldown;
        }

        timer -= Time.deltaTime;
    }
}