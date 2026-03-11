using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    UpgradeData data;

    public void Setup(UpgradeData upgrade)
    {
        data = upgrade;

        icon.sprite = upgrade.icon;
        title.text = upgrade.upgradeName;
        description.text = upgrade.description;
    }

    public void OnClick()
    {
        GameEvents.OnUpgradeSelected?.Invoke(data);
    }
}