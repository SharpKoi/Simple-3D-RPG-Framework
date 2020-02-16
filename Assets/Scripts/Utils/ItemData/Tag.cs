using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag {
    public string name;

    public Tag() {
        name = "";
    }

    public Tag(string name) {
        if(name == null) name = "";
        else this.name = name;
    }
}
