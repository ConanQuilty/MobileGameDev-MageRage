using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletHud : MonoBehaviour
{
    [SerializeField] TMP_Text coinsLabel;
    [SerializeField] TMP_Text upgradeLabel;
    [SerializeField] TMP_Text bonusLabel;
    [SerializeField] Button upgradeButton;
    Wallet _wallet;

    void OnEnable()
    {
        _wallet = Wallet.Instance;
        _wallet.Changed += Refresh;
        Refresh();
    }

    void OnDisable() { if (_wallet != null) _wallet.Changed -= Refresh; }

    void Refresh()
    {
        coinsLabel.SetText("{0:0} coins", (float)_wallet.Coins);
        if (upgradeLabel) upgradeLabel.SetText("Arcane Power {0:0} > {1:0}  ({2:0} coins)",(float)_wallet.Level, 
                                                (float)(_wallet.Level + 1), (float)_wallet.NextCost);
        if (bonusLabel) bonusLabel.SetText("+{0:0}% spell damage", (float)(_wallet.Level * 15));
        if (upgradeButton) upgradeButton.interactable = _wallet.Coins >= _wallet.NextCost;
    }

    public void OnUpgrade() => _wallet.TryUpgrade(); 
}