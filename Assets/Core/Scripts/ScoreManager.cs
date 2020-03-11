using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    TextMeshProUGUI textMeshPro;
    private int score;
    public int Score { get => score; set => score = value; }

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text =  "Score " + score.ToString();
    }
}
