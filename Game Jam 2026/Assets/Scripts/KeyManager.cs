using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<Key> keys = new List<Key>();

    public void AddKey(Key key)
    {
        keys.Add(key);
        Debug.Log($"Key collected! Total: {keys.Count}");
    }

    public bool HasKey(int keyID)
    {
        foreach (Key key in keys)
        {
            if (key.keyID == keyID)
                return true;
        }
        return false;
    }

    public bool UseKey(int keyID)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].keyID == keyID)
            {
                Destroy(keys[i].gameObject);
                keys.RemoveAt(i);
                Debug.Log($"Key used! Remaining: {keys.Count}");
                return true;
            }
        }
        return false;
    }

    public bool UseKeys(int keyID, int amount)
    {
        int foundCount = 0;
        List<int> indexesToRemove = new List<int>();

        // Find the required number of keys
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].keyID == keyID)
            {
                indexesToRemove.Add(i);
                foundCount++;
                if (foundCount >= amount) break;
            }
        }

        // If we have enough keys, remove them
        if (foundCount >= amount)
        {
            // Remove from highest index to lowest to avoid shifting issues
            indexesToRemove.Sort((a, b) => b.CompareTo(a));
            foreach (int index in indexesToRemove)
            {
                Destroy(keys[index].gameObject);
                keys.RemoveAt(index);
            }
            Debug.Log($"Used {amount} keys! Remaining: {keys.Count}");
            return true;
        }

        return false;
    }
}
