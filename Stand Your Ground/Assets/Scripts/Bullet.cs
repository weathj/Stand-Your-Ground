using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private ParticleSystem bulletHitParticle;

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable(){
        Destroy(gameObject, timeToDestroy);
    }

    void Update(){
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (!hit && Vector3.Distance(transform.position, target) < .01f){
                Destroy(gameObject);
            }
    }

    private void OnCollisionEnter(Collision other){
        ContactPoint contact = other.GetContact(0);
        // GameObject.Instantiate(bulletDecal, contact.point, Quaternion.LookRotation(contact.normal));
        // bulletHitParticle.transform.position = contact.point;
        // bulletHitParticle.Play();
        Destroy(gameObject);

        if (other.gameObject.tag == "Enemy"){
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.enemyData.health -= 10f;
            Debug.Log("Enemy health: " + enemy.enemyData.health);
            if (enemy.enemyData.health <= 0f){
                enemy.Die();
            }
        }
    }
}
