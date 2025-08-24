using System;
using UnityEngine;
using System.Linq; 
using Game.Characters;
 public class CharacterFromJsonLoader : MonoBehaviour
{
    [Header("적용 대상 Character")]
    public Character target;

    [Header("찾을 캐릭터 ID (비우면 target.characterId 사용)")]
    public string targetId = ""; // ★ 추가

    [Header("Resources/CharacterData/basehero.json (확장자 제외)")]
    public string resourceFile = "CharacterData/hero"; // ★ 경로 수정 (현재 hero.json 기준)

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("[Loader] target이 비어 있어요.");
            return;
        }

        // 1) JSON 텍스트 로드
        var text = Resources.Load<TextAsset>(resourceFile);
        if (text == null)
        {
            Debug.LogError($"[Loader] 파일 없음: Resources/{resourceFile}.json");
            return;
        }

        // 2) 배열 JSON 파싱
        var all = JsonArrayHelper.FromJson<CharacterData>(text.text);
        if (all == null || all.Length == 0)
        {
            Debug.LogError("[Loader] JSON 파싱 실패 또는 비어 있음 (배열 루트인지 확인!)");
            return;
        }

        // 3) 빈 객체 제거 (네 hero.json 두 번째 {} 방지)
        var candidates = all.Where(c => !string.IsNullOrEmpty(c.id)).ToArray();
        if (candidates.Length == 0)
        {
            Debug.LogError("[Loader] 유효한 id를 가진 캐릭터가 없습니다.");
            return;
        }

        // 4) 찾을 id 결정: targetId 우선, 없으면 target.characterId
        string id = string.IsNullOrWhiteSpace(targetId) ? target.characterId : targetId;
        if (string.IsNullOrWhiteSpace(id))
        {
            Debug.LogError("[Loader] 검색할 id가 비었습니다. targetId나 target.characterId를 설정하세요.");
            return;
        }

        // 5) id 매칭
        var found = candidates.FirstOrDefault(c => c.id == id);
        if (found == null)
        {
            Debug.LogError($"[Loader] id '{id}' 를 찾지 못했습니다. 사용 가능: {string.Join(", ", candidates.Select(c => c.id))}");
            return;
        }

        // 6) 적용
        CharacterMapper.ApplyToComponent(found, target);
        Debug.Log($"[Loader] Loaded: {target.displayName} ({target.characterId})  " +
                $"Lv{target.stats.level} HP{target.stats.hp} MP{target.stats.mp}");
    }
}