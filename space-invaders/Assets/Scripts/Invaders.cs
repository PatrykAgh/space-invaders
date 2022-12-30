using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public AnimationCurve speed;
    public Projectile missilePrefab;
    public float missileAttackRate = 1.0f;
    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public int amountAlived => this.totalInvaders - this.amountKilled;
    public float percentKilled => (float)this.amountKilled/(float)this.totalInvaders;
    private Vector3 _direction = Vector2.right;
    private void Awake()
    {
        for(int row = 0; row<rows;row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width/2, -height/2);
            Vector2 rowPosition = new Vector2(centering.x, centering.y + row*2.0f);
            for (int column = 0; column<columns; column++)
            {
                
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector2 position = rowPosition;
                position.x += column * 2.0f;
                invader.transform.localPosition = position;
            }
            
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }
    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if(_direction == Vector3.right && invader.position.x>= (rightEdge.x-1.0f))
            {
                AdvanceRow();
            }else if(_direction == Vector3.left && invader.position.x <= (leftEdge.x+1.0f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0f / (float)this.amountAlived))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        amountKilled++;
        if (this.amountKilled >= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
