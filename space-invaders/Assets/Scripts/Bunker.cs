using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    public Projectile missilePrefab;
    public float timeToWait;
    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.timeToWait, this.timeToWait);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void ResetBunker()
    {
        gameObject.SetActive(true);
    }
    private void MissileAttack()
    {
        if (Random.value < (0.07f) && this.gameObject.activeInHierarchy)
        {
            Instantiate(this.missilePrefab, this.transform.position, Quaternion.identity);
 
        }
    }
    
}
