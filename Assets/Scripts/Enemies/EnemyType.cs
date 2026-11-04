using UnityEngine;


[CreateAssetMenu(menuName = "MageRage/Enemy Type")]
public class EnemyType : ScriptableObject
{
    public string id = "grunt";
    public float speed = 2f;  
    public int maxHealth = 3;
    public int contactDamage = 1; 
    public float scale = 0.6f;
    public Color color = Color.red;
    public int xp = 1; 
}