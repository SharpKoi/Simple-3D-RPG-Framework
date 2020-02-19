using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTag : Tag {
    public string data;

    public StringTag() : base() {
            
    }

    public StringTag(string name) : base(name) {
            
    }

    public StringTag(string name, string value) {
        this.name = name;
        this.data = value;
    }
}
