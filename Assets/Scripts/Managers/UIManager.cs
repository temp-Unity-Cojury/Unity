using UnityEngine;

namespace Game.Managers
{
    public class UIManager : MonoBehaviour
    {
        public void ShowPanel(GameObject panel) => panel.SetActive(true);
        public void HidePanel(GameObject panel) => panel.SetActive(false);
        public void TogglePanel(GameObject panel) => panel.SetActive(!panel.activeSelf);
    }
}
