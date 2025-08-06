using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    public string[] characterPool = { "Knight", "Archer", "Mage" };

    public void RollGacha()
    {
        int index = Random.Range(0, characterPool.Length);
        Debug.Log("You got: " + characterPool[index]);
    }
}