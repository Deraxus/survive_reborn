using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance;

    public TMP_Text timer;
    public TMP_Text coins;
    public TMP_Text hp;
    public TMP_Text maxhp;
    public TMP_Text itemItemMessage;
    public TMP_Text itemDescriptionMessage;
    public TMP_Text patrons;

    public GameObject playerObject;
    public GameObject timerObject;

    public float messageDuration = 3f;
    public float messageFadeDelay = 1f;

    public string currentItemMessage;
    public string currentItemDescription;

    private int publicCoins;
    private float publicTimer;

    private float seconds;
    private float seconds2;

    public Image patronsPanelSprite;
    public GameObject reloadBarSlider;

    [HideInInspector] public List<bool> ItemMessagesDeck = new List<bool>();
    public List<TMP_Text> ItemMessages = new List<TMP_Text>();

    private bool togl = false;
    private bool isShowingMessage = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < ItemMessages.Count; i++)
        {
            ItemMessagesDeck.Add(false);
        }

        timer.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveManager.Instance != null && WaveManager.Instance.enabled)
        {
            publicTimer = WaveManager.Instance.timer;
            float round_timer = Mathf.Round(publicTimer);
            if (((int)(round_timer) % 60) < 10)
            {
                timer.text = $"{(int)((round_timer) / 60)}:0{(int)(round_timer) % 60}"; // ����������� ������� �� �������� ������
            }
            else
            {
                timer.text = $"{(int)((round_timer) / 60)}:{(int)(round_timer) % 60}"; // ����������� ������� �� �������� ������
            }
        }
        coins.text = playerObject.GetComponent<Player>().coins.ToString();
        hp.text = $"{playerObject.GetComponent<Player>().HP}/{playerObject.GetComponent<Player>().MaxHP}";
        if (isShowingMessage == true)
        {
            ChangeTextSlowlyVoid(itemItemMessage, itemDescriptionMessage, currentItemMessage, currentItemDescription, messageFadeDelay, messageDuration);
        }
    }

    public IEnumerator ChangeTextSlowly(TMP_Text text, string message, float delay = 3) {
        text.color = Color32.Lerp(new Color32(0, 0, 0, 0), new Color32(255, 255, 255, 255), 3000000000);
        yield return new WaitForSeconds(delay);
    }

    public void StartNewMessage(string message, string description = null) {
        /*itemItemMessage.color = new Color(itemItemMessage.color.r, itemItemMessage.color.g, itemItemMessage.color.b, 0);
        itemDescriptionMessage.color = new Color(itemDescriptionMessage.color.r, itemDescriptionMessage.color.g, itemDescriptionMessage.color.b, 0);
        seconds = 0;
        seconds2 = 0;
        togl = false;*/
        TMP_Text localItemMessage = null ;
        for (int i = 0; i < ItemMessages.Count; i++)
        {
            if (ItemMessagesDeck[i] == false)
            {
                localItemMessage = ItemMessages[i];
                break;
            }
        }
        
        if (localItemMessage == null )
        {
            return;
        }

        localItemMessage.GetComponent<ItemMessageShowing>().StartShowing();
        //isShowingMessage = true;
        localItemMessage.text = message;
        //currentItemMessage = message;
        //currentItemDescription = description;
    }

    private void ChangeTextSlowlyVoid(TMP_Text itemItemText, TMP_Text itemDescriptionText = null, string message = "�� ��������� �������!", string description = "�� ����� �������!", float delay = 1, float duration = 3) {
        itemItemText.text = message;
        itemDescriptionText.text = description;
        if (isShowingMessage == false) {
            return;
        }
        if (((seconds / delay) <= 1) && (togl == false))
        {
            seconds += Time.deltaTime;
            itemItemMessage.color = Color.Lerp(new Color(itemItemMessage.color.r, itemItemMessage.color.g, itemItemMessage.color.b, 0), new Color(itemItemMessage.color.r, itemItemMessage.color.g, itemItemMessage.color.b, 1), seconds / delay);
            itemDescriptionMessage.color = Color.Lerp(new Color(itemDescriptionMessage.color.r, itemDescriptionMessage.color.g, itemDescriptionMessage.color.b, 0), new Color(1, 1, 1, 1), seconds / delay);
        }
        else {
            togl = true;
            seconds2 += Time.deltaTime;
            if (seconds2 >= duration) {
                seconds -= Time.deltaTime;
                itemItemMessage.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), seconds / delay);
                itemDescriptionMessage.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), seconds / delay);
            }
        }
        if (seconds2 >= (delay * 2 + duration)) {
            togl = false;
            seconds = 0;
            seconds2 = 0;
            itemItemText.text = string.Empty;
            itemDescriptionText.text = string.Empty;
            isShowingMessage = false;
        }

    }
}
