using UnityEngine;

namespace Game.Characters
{
    [System.Serializable]
    public class Stats
    {
        public int hp = 100;
        public int atk = 10;
        public int def = 5;
        public int spd = 10;
    }

    public class Character : MonoBehaviour
    {
        public string characterId;
        public string displayName = "New Character";
        public Stats stats = new Stats();
    }
}
