using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollect : MonoBehaviour
{
    public int ItemsCollected, ItemsInLevel;
    public TMP_Text itemHUD;

    // Start is called before the first frame update
    void Start()
    {
        itemHUD.text = $"{ItemsCollected}/{ItemsInLevel}";
    }

    public void itemCollect()
    {
        ItemsCollected++;
        itemHUD.text = $"{ItemsCollected}/{ItemsInLevel}";

        if (ItemsCollected >= ItemsInLevel)
        {
            //unlock boss door
        }
    }
}
