using System;
using UnityEngine;
using System.Linq;   // ★ 배열에서 id 찾으려면 필요

namespace Game.Characters
{
    // ----- 런타임 컴포넌트 -----
    [Serializable]
    public class Stats
    {
        public int level;
        public int hp;
        public int mp;
        public int atk;
        public int def;
        public int spd;
        public int exp;
    }

    public class Character : MonoBehaviour
    {
        public string characterId;
        public string displayName = "New Character";
        public Stats stats = new Stats();
    }

    // ----- JSON 데이터 모델 -----
    [Serializable]
    public class BasestatsData
    {
        public int level;
        public int hp;
        public int mp;
        public int atk;
        public int def;
        public int spd;
        public int exp;
    }

    [Serializable]
    public class SkillData
    {
        public string id;
        public float power;
        public string effect; // 없으면 null
    }

    [Serializable]
    public class EquipmentData
    {
        public string id;
    }

    [Serializable]
    public class CharacterData
    {
        public string id;
        public string name;
        public string rarity;
        public BasestatsData basestats;
        public SkillData[] skills;
        public EquipmentData[] equipment;
    }

    // ----- 매퍼 -----
    public static class CharacterMapper
    {
        public static void ApplyToComponent(CharacterData src, Character dst)
        {
            if (src == null || dst == null) return;

            dst.characterId = src.id;
            dst.displayName = src.name;

            if (src.basestats != null && dst.stats != null)
            {
                dst.stats.level = src.basestats.level;
                dst.stats.hp = src.basestats.hp;
                dst.stats.mp = src.basestats.mp;
                dst.stats.atk = src.basestats.atk;
                dst.stats.def = src.basestats.def;
                dst.stats.spd = src.basestats.spd;
                dst.stats.exp = src.basestats.exp;
            }
        }
    }

    // ----- 단일 JSON 로더 (파일 1개 = 캐릭터 1개) -----
   public static class JsonArrayHelper
    {
        [System.Serializable]
        private class Wrapper<T> { public T[] array; }

        public static T[] FromJson<T>(string json)
        {
            string wrapped = "{ \"array\": " + json + " }";
            var wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(wrapped);
            return wrapper?.array;
        }

        // 🔥 여기 추가!
        public static string ToJson<T>(T[] array, bool prettyPrint = false)
        {
            var wrapper = new Wrapper<T> { array = array };
            string wrapped = UnityEngine.JsonUtility.ToJson(wrapper, prettyPrint); 
            // {"array":[...]} → 배열 부분만 추출
            int s = wrapped.IndexOf('[');
            int e = wrapped.LastIndexOf(']');
            return wrapped.Substring(s, e - s + 1); 
        }
    }
    
}
