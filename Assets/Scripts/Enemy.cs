using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int maxMp;
    public int hp;
    public int mp;

    void Start()
    {
        hp = maxHp;
        mp = maxMp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Disappear() {
        gameObject.SetActive(false);
    }
}
