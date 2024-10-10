using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ObstaclePoolItemB : PoolItem, IObsticelFeature {
    [SerializeField] NavMeshAgent agent;

    bool isChasingPlayer = true;
    MeshRenderer meshRenderer = null;

    internal override Type GetItemType()
    {
        return typeof(ObstaclePoolItemB);
    }

    public void Setup()
    {
        if (!meshRenderer)
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        isChasingPlayer = true;
        Utility.ChangeColorMateMesh(Color.grey, meshRenderer);
    }

    public void UpdateDestination(Vector3 position)
    {
        if (gameObject.activeInHierarchy && isChasingPlayer)
        {
            agent.destination = position;
            float dist = Vector3.Distance(position, transform.position);
            if (dist < 1.1f)
            {
                //  Turn red
                Utility.ChangeColorMateMesh(Color.red, meshRenderer);
            }
        }
    }

    public void OnTouchPlayer(Collision collision)
    {
        Debug.Log("OnCollisionEnter Enemy B!");
        StartCoroutine(IE_OnTouchPlayer(collision));
    }

    IEnumerator IE_OnTouchPlayer(Collision collision)
    {
        isChasingPlayer = false;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        Vector3 direction = collision.transform.position - transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);

        yield return new WaitForSeconds(3);
        PoolSystem.ReturnItem(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Helper.TAG_PLAYER)
        {
            OnTouchPlayer(collision);
        }
    }
}
