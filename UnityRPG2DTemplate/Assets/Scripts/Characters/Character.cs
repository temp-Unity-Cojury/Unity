using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public string characterName;
    public int hp;
    public int attack;
}

public class Character : MonoBehaviour
{
    public CharacterStats stats;

    public void TakeDamage(int amount)
    {
        stats.hp -= amount;
        if (stats.hp <= 0)
        {
            Debug.Log(stats.characterName + " has died.");
        }
    }
}