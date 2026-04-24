using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public List<GameObject> hairs;
    public List<GameObject> faces;
    public List<GameObject> bodyes;
    public List<GameObject> hands;
    public List<GameObject> legs;

    private GameObject character_hair;
    private GameObject character_face;
    private GameObject character_body;
    private GameObject character_hands;
    private GameObject character_legs;
    void Start()
    {
        GenerateCharecter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCharecter() { 
        int randIndexHair = Random.Range(0, hairs.Count);
        int randIndexFace = Random.Range(0, faces.Count);
        int randIndexBody = Random.Range(0, bodyes.Count);
        int randIndexHands = Random.Range(0, hands.Count);
        int randIndexLegs = Random.Range(0, legs.Count);

        if (hairs.Count > 0)
        {
            for (int i = 0; i < hairs.Count; i++)
            {
                if (i != randIndexHair)
                {
                    hairs[i].SetActive(false);
                }
            }
        }

        if (faces.Count > 0) 
        {
            for (int i = 0; i < faces.Count; i++)
            {
                if (i != randIndexFace)
                {
                    faces[i].SetActive(false);
                }
            }
        }

        if (bodyes.Count > 0)
        {
            for (int i = 0; i < bodyes.Count; i++)
            {
                if (i != randIndexBody)
                {
                    bodyes[i].SetActive(false);
                }
            }
        }

        if (hands.Count > 0)
        {
            for (int i = 0; i < hands.Count; i++)
            {
                if (i != randIndexHands)
                {
                    hands[i].SetActive(false);
                }
            }
        }

        if (legs.Count > 0) 
        {
            for (int i = 0; i < legs.Count; i++)
            {
                if (i != randIndexLegs)
                {
                    legs[i].SetActive(false);
                }
            }
        }
    }

}
