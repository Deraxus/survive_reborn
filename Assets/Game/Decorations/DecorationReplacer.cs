using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationReplacer : MonoBehaviour
{
    public List<Sprite> commonDecorationList = new List<Sprite>();
    public List<Sprite> rareDecorationList = new List<Sprite>();

    [SerializeField] float spawnDecorationChance = 0.2f;
    [SerializeField] float rareDecorationChance = 0.1f;
    Utils.RareTypes rareType;
    void Start()
    {
        rareType = Utils.GetRandomRareType(spawnDecorationChance, rareDecorationChance);
        switch (rareType)
        {
            case Utils.RareTypes.rare:
                SpawnDecoration(commonDecorationList);
                break;
            case Utils.RareTypes.mythic:
                SpawnDecoration(rareDecorationList);
                break;
            default:
                SpawnDecoration(null);
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool SpawnDecoration(List<Sprite> spriteList) {
        if (spriteList == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            return false;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = Utils.SelectRandomSprite(spriteList);
        return true;
    }

}
