using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolTag : Tag {
    public bool data;

    public BoolTag() : base() {
            
    }

    public BoolTag(string name) : base(name) {
            
    }

    public BoolTag(string name, bool value) {
        this.name = name;
        this.data = value;
    }  
}
