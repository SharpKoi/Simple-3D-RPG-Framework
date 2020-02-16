using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTag : Tag {
    public float data;

    public FloatTag() : base() {
            
    }

    public FloatTag(string name) : base(name) {
            
    }

    public FloatTag(string name, float value) {
        this.name = name;
        this.data = value;
    }   
}
