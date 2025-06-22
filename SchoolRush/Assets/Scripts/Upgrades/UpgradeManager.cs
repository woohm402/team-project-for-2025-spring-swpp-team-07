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
    public PassengerController passengerController;
    public TrafficController trafficController;
    public AggressiveCarSpawner aggressiveCarSpawner;

    [SerializeField]
    private GameObject upgradeUI;
    private List<GameObject> upgradeUIItems;

    private List<Upgrade> selectedUpgrades;
    private List<Transform> colleagues;

    private AudioManager am;

    private void Start() {
        upgradeUIItems = new List<GameObject>();
        selectedUpgrades = new List<Upgrade>();
        colleagues = new List<Transform>();
        am = AudioManager.Instance;

        for (int i = 1; i <= 3; i++)
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
        am.PlayOneShot(am.checkpointAudio);

        upgradeUI.SetActive(true);
        Time.timeScale = 0;
        List<Upgrade> upgrades = new(); // should be of length 3

        switch (checkpoint) {
            case 1:
                Upgrade u101 = new Upgrade101(kartController);
                Upgrade u102 = new Upgrade102(kartController);
                Upgrade u103 = new Upgrade103(passengerController);
                Upgrade u104 = new Upgrade104(trafficController);
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
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u201, u202 }).pick(1));
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u203, u204 }).pick(2));
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
                Upgrade u404 = new Upgrade404(trafficController, aggressiveCarSpawner);
                upgrades.AddRange(new RandomPicker<Upgrade>(new List<Upgrade> { u401, u402, u403, u404 }).pick(3));
                SetColleagueActive(3, true);
                break;
        }

        foreach (var upgrade in upgrades) {
            int index = upgrades.IndexOf(upgrade);

            upgradeUIItems[index].transform.GetChild(0).Find("Image").GetComponent<Image>().sprite = upgrade.GetAugment();

            Button button = upgradeUIItems[index].transform.Find("Button").gameObject.GetComponent<Button>();
            button.GetComponent<Image>().sprite = upgradeUIItems[index].transform.Find("PlaceHolder").GetComponent<Image>().sprite;
            button.onClick.AddListener(() => PickComplete(upgrade));
            button.onClick.AddListener(() => am.PlayOneShot(am.upgradeAudio));

            upgradeUIItems[index].SetActive(true);
        }
    }

    private void PickComplete(Upgrade upgrade)
    {
        upgradeUI.SetActive(false);
        upgrade.OnPick();
        kartController.GetPlayerData().InsertUpgrade(upgrade);
        Time.timeScale = 1;
        selectedUpgrades.Add(upgrade);

        foreach (var item in upgradeUIItems)
            item.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
