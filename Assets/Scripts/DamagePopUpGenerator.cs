using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpGenerator : MonoBehaviour
{

    public static DamagePopUpGenerator current;
    public GameObject prefab;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {

    }

    public void CreatePopUp(Vector3 position, string text)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        Destroy(popup, 1f);
    }
}
