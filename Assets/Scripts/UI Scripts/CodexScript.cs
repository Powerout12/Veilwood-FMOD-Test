using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodexScript : MonoBehaviour
{
    CodexEntries[] CreatureEntries;
    int creatureEntriesLength;
    public GameObject codex;
    public TextMeshProUGUI nameText, descriptionText;
    public int currentCreatureEntry = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreatureEntries = Resources.LoadAll<CodexEntries>("Codex/Creatures/");
        creatureEntriesLength = CreatureEntries.Length;
        print(creatureEntriesLength);
        nameText.text = CreatureEntries[0].entryName;
        descriptionText.text = CreatureEntries[0].description;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("c"))
        {
            codex.SetActive(!codex.activeInHierarchy);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdatePage(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdatePage(1);
        }
    }

    void UpdatePage(int page)
    {
        currentCreatureEntry = currentCreatureEntry + page;
        currentCreatureEntry = Mathf.Clamp(currentCreatureEntry,0,creatureEntriesLength - 1);
        //print(CreatureEntries[currentCreatureEntry]);
        nameText.text = CreatureEntries[currentCreatureEntry].entryName;
        descriptionText.text = CreatureEntries[currentCreatureEntry].description;
    }
}
