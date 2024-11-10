using TMPro;
using UnityEngine;

namespace WinterUniverse
{
    public class FactionSlotUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Setup(FactionRelationship fr)
        {
            _text.text = $"{fr.Faction.DisplayName}: {fr.State}";
        }
    }
}