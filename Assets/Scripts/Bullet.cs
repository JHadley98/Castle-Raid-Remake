using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 5;
    public string myTeam;

    private void OnTriggerEnter(Collider collision) 
    { 
        if(collision.gameObject.CompareTag(myTeam))
        {
            //Do nothing, ignore collisions with self or own team
        }
        else
        {
            //Apply damage if hitting red or blue team player
            if (collision.gameObject.CompareTag("RedTeam") || collision.gameObject.CompareTag("BlueTeam"))
            {
                collision.gameObject.GetComponent<AIController>().TakeDamage(damage);
            }
            //Destroy bullet if hitting anything (except self or own team from above)
            if (!collision.gameObject.CompareTag(myTeam))
            {
                Destroy(gameObject);
            }
        }
    }
}
