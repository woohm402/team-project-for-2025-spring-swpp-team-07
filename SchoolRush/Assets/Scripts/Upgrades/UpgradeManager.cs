using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public KartController kartController;
    public Transform CharacterModels;
    public BGMController bgmController;

    [SerializeField]
    private GameObject upgradeUI;
    private List<GameObject> upgradeUIItems;

    private List<Upgrade> selectedUpgrades;
    private List<Transform> colleagues;

    private void Start() {
        upgradeUIItems = new List<GameObject>();
        selectedUpgrades = new List<Upgrade>();
        colleagues = new List<Transform>();

        for (int i = 0; i < 3; i++)
            upgradeUIItems.Add(upgradeUI.transform.GetChild(i).gameObject);

        for (int i = 0; i < CharacterModels.childCount; i++)
        {
            colleagues.Add(CharacterModels.GetChild(i));
        }

        foreach (var t in colleagues)
            t.gameObject.SetActive(false);
    }

    public void SetColleagueActive(int index, bool on)
    {
        if (index < 0 || index >= colleagues.Count) return;
        colleagues[index].gameObject.SetActive(on);
    }

    public void PickUpgrade(int checkpoint) {
        upgradeUI.SetActive(true);
        Time.timeScale = 0;
        List<Upgrade> upgrades = new List<Upgrade>(); // should be length 3

        switch (checkpoint) {
            case 1:
                Upgrade u101 = new Upgrade101(kartController);
                Upgrade u102 = new Upgrade102(kartController);
                Upgrade u103 = new Upgrade103();
                Upgrade u104 = new Upgrade104();
                Upgrade u105 = new Upgrade105(kartController);
                upgrades.Add(u101);
                upgrades.Add(u102);
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u103, u104, u105 }).pick(1));
                SetColleagueActive(0, true);
                break;
            case 2:
                Upgrade u201 = new Upgrade201(kartController);
                Upgrade u202 = new Upgrade202(kartController);
                Upgrade u203 = new Upgrade203(kartController);
                Upgrade u204 = new Upgrade204(kartController);
                Upgrade u205 = new Upgrade205();
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u201, u202 }).pick(1));
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u203, u204, u205 }).pick(2));
                SetColleagueActive(1, true);
                break;
            case 3:
                Upgrade u301 = new Upgrade301(kartController);
                Upgrade u302 = new Upgrade302(kartController);
                Upgrade u303 = new Upgrade303(kartController);
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u301, u302, u303 }).pick(3));
                SetColleagueActive(2, true);
                break;
            case 4:
                Upgrade u401 = new Upgrade401(kartController);
                Upgrade u402 = new Upgrade402(bgmController);
                Upgrade u403 = new Upgrade403();
                Upgrade u404 = new Upgrade404();
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u401, u402, u403, u404 }).pick(3));
                SetColleagueActive(3, true);
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
