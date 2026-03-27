using UnityEngine;
using System.Collections;

public class SawTrap : MonoBehaviour
{
    public float liftheight = 2f; // The height the saw will lift
    public float upspeed= 2f; // The speed at which the saw will lift
    public float downspeed = 5f; // The speed at which the saw will lower
    public float waittime = 1f; // The time the saw will wait at the top before lowering

    private Vector3 startposition; // The starting position of the saw
    private Vector3 targetposition; // The target position of the saw

    public LayerMask whatIsPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startposition = transform.position;
        targetposition = startposition + Vector3.up * liftheight;   
        StartCoroutine(Saw());
    }
    void Attack()
    {
        Collider2D collinfo = Physics2D.OverlapCircle(transform.position, 0.5f, whatIsPlayer);

        if (collinfo)
        {
            if (collinfo.gameObject.GetComponent<Player>() != null)
            {
                collinfo.gameObject.GetComponent<Player>().TakeDamage(5); // Log message for debugging purposes
            }
        }
    }
    IEnumerator Saw()
    {
        while (true)
        {
            // Move the saw up to the target position
            while (transform.position != targetposition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetposition, upspeed * Time.deltaTime);
                yield return null;
            }
            // Wait at the top for the specified time
            yield return new WaitForSeconds(waittime);
            // Move the saw down to the starting position
            while (transform.position != startposition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startposition, downspeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(waittime);
        }
    }
}
