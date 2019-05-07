using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text descriptionText;
    public Text tribeText;

    public Image artwork;

    public Text attack;
    public Text health;
    public Text movement;

    public GridLayoutGroup costContainer;
    public Dictionary<Mana, Text> costs;

    void Start()
    {
        nameText.text = card.name;
        descriptionText.text = card.description;
        tribeText.text = card.tribes;

        artwork.sprite = card.artwork;

        attack.text = card.attack.ToString();
        health.text = card.health.ToString();
        movement.text = card.movement.ToString();

        costs = new Dictionary<Mana, Text>();
        foreach(Mana mana in card.ManasNeeded())
        {
            GameObject backgroundGO = new GameObject();
            GameObject textGO = new GameObject();
            backgroundGO.transform.SetParent(costContainer.transform);
            Debug.Log(backgroundGO.transform.ToString());
            textGO.transform.SetParent(backgroundGO.transform);
            backgroundGO.AddComponent<Image>();
            backgroundGO.GetComponent<Image>().sprite = Resources.Load<Sprite>("BlackManaBG");
            textGO.AddComponent<Text>();
            Text text = textGO.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.black;
            text.text = card.CostIn(mana).ToString();
        }
    }
}
