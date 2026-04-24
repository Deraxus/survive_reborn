using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Mutagen : MonoBehaviour
{
    public GameObject Player;

    // Вес мутагена.
    // 1 - самый маленький
    // 2 - средний, на это число умножаются коэфиценты снизу
    // 3 - большой
    public int mutagenWeigh = 1;

    public Sprite smallMutagenSprite;
    public Sprite mediumMutagenSprite;
    public Sprite largeMutagenSprite;

    public float mediumMutagenChance = 0.2f;
    public float largeMutagenChance = 0.05f;


    public float mutagenScaleHP = 0.2f;
    public float mutagenScaleDamage = 0.01f;
    public float mutagenScaleRate = 0.01f;
    //public float mutagenScaleSpeed = 0.01f;

    [HideInInspector]
    public int mutagenType = -1;

    public float mutantScale = 0.01f;

    private Player localPlayerScript;
    // Start is called before the first frame update
    private void Awake()
    {
        float randFloat = Random.Range(0f, 1f);
        if (randFloat <= largeMutagenChance)
        {
            mutagenWeigh = 3;
        }
        else if (randFloat <= mediumMutagenChance)
        {
            mutagenWeigh = 2;
        }
        else
        {
            mutagenWeigh = 1;
        }
        localPlayerScript = MainManager.Instance.mainPlayer.GetComponent<Player>();
        switch (mutagenWeigh)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = smallMutagenSprite;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = mediumMutagenSprite;
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = largeMutagenSprite;
                break;
        }

        //GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().bounds.size;
        //GetComponent<BoxCollider2D>().offset = GetComponent<SpriteRenderer>().bounds.center;
    }

    void Start()
    {
        mutagenScaleHP *= mutagenWeigh;
        mutagenScaleDamage *= mutagenWeigh;
        mutagenScaleRate *= mutagenWeigh;
        //mutagenScaleSpeed *= mutagenWeigh;

        //transform.localScale = new Vector2(transform.localScale.x * (mutagenWeigh / 4), transform.localScale.y * (mutagenWeigh / 4));

        if (mutagenType == -1)
        {
            mutagenType = Random.Range(0, 3);
        }

        Color newColor;

        /*switch (mutagenType)
        {
            case 0: // хп ап
                ColorUtility.TryParseHtmlString("#0A3A0F", out newColor);
                GetComponent<SpriteRenderer>().color = newColor;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#6A2A13", out newColor);
                GetComponent<SpriteRenderer>().color = newColor;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#172A61", out newColor);
                GetComponent<SpriteRenderer>().color = newColor;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#205550", out newColor);
                GetComponent<SpriteRenderer>().color = newColor;
                break;
        }*/
    }


    void MutantScale()
    {
        localPlayerScript.mutantScale += mutantScale;
    }

    public void MutagenHP()
    { 
        localPlayerScript.healthCup += mutagenScaleHP;
        localPlayerScript.AfterGettingHpCup();
        MutantScale();
    }

    public void MutagenDamage()
    {
        localPlayerScript.damageKf += mutagenScaleDamage;
        MutantScale();
    }

    public void MutagenRate()
    {
        localPlayerScript.rateKf += mutagenScaleRate;
        MutantScale();
    }

    public void MutagenSpeed()
    {
        //localPlayerScript.speedKf += mutagenScaleSpeed;
        MutantScale();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
