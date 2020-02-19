using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntTag : Tag {

    public int data;

    public IntTag() : base() {
            
    }

    public IntTag(string name) : base(name) {
            
    }

    public IntTag(string name, int value) {
        this.name = name;
        this.data = value;
    }
}
