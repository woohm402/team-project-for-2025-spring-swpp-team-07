using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public KartController kartController;
    public BGMController bgmController;

    [SerializeField]
    private GameObject upgradeUI;
    private List<GameObject> upgradeUIItems;

    private List<Upgrade> selectedUpgrades;

    private void Start() {
        upgradeUIItems = new List<GameObject>();
        selectedUpgrades = new List<Upgrade>();

        for (int i = 0; i < 3; i++)
            upgradeUIItems.Add(upgradeUI.transform.GetChild(i).gameObject);
    }

    public void PickUpgrade(int checkpoint) {
        if (checkpoint == 5) return;

        upgradeUI.SetActive(true);
        Time.timeScale = 0;
        List<Upgrade> upgrades = new List<Upgrade>(); // should be length 3

        switch (checkpoint) {
            case 1:
                Upgrade u101 = new Upgrade101(kartController);
                Upgrade u102 = new Upgrade102();
                Upgrade u103 = new Upgrade103();
                Upgrade u104 = new Upgrade104();
                Upgrade u105 = new Upgrade105(kartController);
                upgrades.Add(u101);
                upgrades.Add(u102);
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u103, u104, u105 }).pick(1));
                break;
            case 2:
                Upgrade u201 = new Upgrade201(kartController);
                Upgrade u202 = new Upgrade202();
                Upgrade u203 = new Upgrade203();
                Upgrade u204 = new Upgrade204();
                Upgrade u205 = new Upgrade205();
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u201, u202 }).pick(1));
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u203, u204, u205 }).pick(2));
                break;
            case 3:
                Upgrade u301 = new Upgrade301();
                Upgrade u302 = new Upgrade302();
                Upgrade u303 = new Upgrade303();
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u301, u302, u303 }).pick(3));
                break;
            case 4:
                Upgrade u401 = new Upgrade401();
                Upgrade u402 = new Upgrade402(bgmController);
                Upgrade u403 = new Upgrade403();
                Upgrade u404 = new Upgrade404();
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u401, u402, u403, u404 }).pick(3));
                break;
        }

        foreach (var upgrade in upgrades) {
            int index = upgrades.IndexOf(upgrade);
            upgradeUIItems[index].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = upgrade.GetTitle();
            upgradeUIItems[index].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = upgrade.GetDescription();
            upgradeUIItems[index].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => PickComplete(upgrade));
            upgradeUIItems[index].SetActive(true);
        }
    }

    private void PickComplete(Upgrade upgrade)
    {
        upgradeUI.SetActive(false);
        upgrade.OnPick();
        Time.timeScale = 1;
        selectedUpgrades.Add(upgrade);

        foreach (var item in upgradeUIItems)
            item.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
