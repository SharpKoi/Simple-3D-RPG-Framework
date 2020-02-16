using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class CompoundTag : Tag {
        Dictionary<string, Tag> tagCollection = new Dictionary<string, Tag>();

        public CompoundTag() : base() {
            
        }

        public CompoundTag(string name) : base(name) {
            
        }

        public CompoundTag put(string key, Tag tag) {
            tagCollection.Add(key, tag);
            return this;
        }

        public CompoundTag putInt(string key, int value) {
            return put(key, new IntTag(key, value));
        }

        public CompoundTag putFloat(string key, float value) {
            return put(key, new FloatTag(key, value));
        }

        public CompoundTag putString(string key, string value) {
            return put(key, new StringTag(key, value));
        }

        public Tag GetTag(string key) {
            Tag tag = null;
            tagCollection.TryGetValue(key, out tag);
            return tag;
        }

        public Dictionary<string, Tag>.ValueCollection GetAllTags() {
            return tagCollection.Values;
        }

        public CompoundTag RemoveTag(string key) {
            tagCollection.Remove(key);
            return this;
        }

        public CompoundTag Clear() {
            tagCollection.Clear();
            return this;
        }

        public bool IsEmpty() {
            return tagCollection.Count == 0;
        }
    }
}
