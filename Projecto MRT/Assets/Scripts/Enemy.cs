using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class enemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public EnemyDataSO enemyData;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    public Vector3 rayCastOffset;
    public string deathScene;

    public GameObject hideText, stopHideText;

    void Start()
    {
        enemyData.walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        enemyData.aiDistance = Vector3.Distance(player.position, this.transform.position);
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, enemyData.sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemyData.walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                enemyData.chasing = true;
            }
        }
        if (enemyData.chasing == true)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = enemyData.chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("sprint");

        }
        if (enemyData.walking == true)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = enemyData.walkSpeed;
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk");
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                aiAnim.ResetTrigger("sprint");
                aiAnim.ResetTrigger("walk");
                aiAnim.SetTrigger("idle");
                ai.speed = 0;
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                enemyData.walking = false;
            }
        }
    }
    public void stopChase()
    {
        enemyData.walking = true;
        enemyData.chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    IEnumerator stayIdle()
    {
        enemyData.idleTime = Random.Range(enemyData.minIdleTime, enemyData.maxIdleTime);
        yield return new WaitForSeconds(enemyData.idleTime);
        enemyData.walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    IEnumerator chaseRoutine()
    {
        enemyData.chaseTime = Random.Range(enemyData.minChaseTime, enemyData.maxChaseTime);
        yield return new WaitForSeconds(enemyData.chaseTime);
        stopChase();
    }

}