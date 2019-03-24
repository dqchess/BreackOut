using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEntrance1 : MonoBehaviour
{
    /// <summary>
    /// 显示弹窗的UI
    /// </summary>
    public GameObject UI;

    /// <summary>
    /// 小游戏描述文本
    /// </summary>
    public string UITextContent;

    public string SceneName;

    public Text UIText;

    public Button ApplyButton;

    public Button CancelButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            UI.gameObject.SetActive(true);
            UIText.text = UITextContent;
            ApplyButton.onClick.RemoveAllListeners();
            ApplyButton.onClick.AddListener(Click);
            CancelButton.onClick.RemoveAllListeners();
            CancelButton.onClick.AddListener(Cancel);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UI.gameObject.SetActive(false);
    }

    private void Click()
    {
        SceneManager.LoadScene(SceneName);
    }

    private void Cancel()
    {
        UI.SetActive(false);
    }
}
