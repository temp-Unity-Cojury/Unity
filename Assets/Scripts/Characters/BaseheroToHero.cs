using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Game.Characters;   // CharacterData, JsonArrayHelper

public class BaseHeroToHero : MonoBehaviour
{
    [Header("원본 JSON (Resources 경로, 확장자 X)")]
    public string sourceFile = "CharacterData/basehero";    // 예: Assets/Resources/CharacterData/basehero.json

    [Header("출력 JSON (Assets 하위 경로)")]
    public string outputSubPath = "Resources/CharacterData/hero.json"; // 최종: Assets/Resources/CharacterData/hero.json

    [Header("추출할 캐릭터 ID")]
    public string targetId = "hero_001";

    // ▶ 플레이 시작 시 자동으로 1회 생성(비어 있거나 없으면)
    private void Start()
    {
        Debug.Log("[Extractor] Start() 호출됨");
        var fullPath = Path.Combine(Application.dataPath, outputSubPath);
        if (!File.Exists(fullPath) || new FileInfo(fullPath).Length == 0)
        {
            Debug.Log("[Extractor] hero.json이 없거나 비어 있어 자동 생성 시도");
            Extract();   // 아래 메서드 호출
        }
        else
        {
            Debug.Log($"[Extractor] 이미 존재: {fullPath} (size={new FileInfo(fullPath).Length} bytes)");
        }
    }

    [ContextMenu("Extract hero_001 → hero.json")]
    public void Extract()
    {
        try
        {
            // 1) 원본 읽기 (Resources, 확장자 X)
            var src = Resources.Load<TextAsset>(sourceFile);
            if (src == null)
            {
                Debug.LogError($"[Extractor] 원본 없음: Resources/{sourceFile}.json");
                return;
            }

            // 2) 파싱 (네 모델이 basestats → 혹시 baseStats면 교정)
            string raw = src.text.Replace("\"baseStats\"", "\"basestats\"");
            var all = JsonArrayHelper.FromJson<CharacterData>(raw);
            if (all == null || all.Length == 0)
            {
                Debug.LogError("[Extractor] 파싱 실패/데이터 없음 (루트가 배열인지, 키 스펠링 확인)");
                return;
            }

            var found = all.FirstOrDefault(c => c != null && c.id == targetId);
            if (found == null)
            {
                Debug.LogError($"[Extractor] id '{targetId}' 없음");
                return;
            }

            // 3) 배열 루트로 직렬화
            string jsonArray = JsonArrayHelper.ToJson(new[] { found }, prettyPrint: true);
            Debug.Log($"[Extractor] 직렬화 완료 (len={jsonArray.Length})");

            // 4) 절대경로로 저장
            string fullPath = Path.Combine(Application.dataPath, outputSubPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            // 경로/권한 확인 겸 예비 쓰기 → 최종 쓰기
            File.WriteAllText(fullPath, "[]", new UTF8Encoding(false));
            File.WriteAllText(fullPath, jsonArray, new UTF8Encoding(false));

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.Log($"[Extractor] DONE → {fullPath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Extractor] 예외 발생: {ex.GetType().Name}\n{ex.Message}");
        }
    }
}
