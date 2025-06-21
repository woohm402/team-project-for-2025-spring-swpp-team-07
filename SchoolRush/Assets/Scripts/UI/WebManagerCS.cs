using UnityEngine;
using UnityEngine.UI;

public class WebManagerCS : MonoBehaviour {
    public Button button;

    public void Start() {
      button.onClick.AddListener(OnClick);
    }

    public void OnClick() {
        Debug.Log("Opening web page: ");
        Application.OpenURL("https://schoolrush.vercel.app");
    }
}
