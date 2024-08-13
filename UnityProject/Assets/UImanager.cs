using UnityEngine;
using TMPro;
public class UImanager : MonoBehaviour
{
    public static UImanager instance;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (instance != null && instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }
    public void UpdateText(int number)
    {
        text.text = "Food Fragments:" + number.ToString();
    }


}
