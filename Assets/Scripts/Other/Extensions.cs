// This class adds functions to built-in types.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;

public static class Extensions
{
    // transform.Find only finds direct children, no grandchildren etc.
    public static Transform FindRecursively(this Transform transform, string name) {
        return Array.Find(transform.GetComponentsInChildren<Transform>(true), t => t.name == name);
    }

    /// <summary>
    /// UI SetListener extension that removes previous and then adds new listener without params
    /// </summary>
    /// <param name="uEvent"></param>
    /// <param name="call"></param>
    public static void SetListener(this UnityEvent uEvent, UnityAction call) {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    /// <summary>
    /// Generic UI SetListener extension that removes previous and then adds new listener with T params
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uEvent"></param>
    /// <param name="call"></param>
    public static void SetListener<T>(this UnityEvent<T> uEvent, UnityAction<T> call) {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static T[] Append<T>(this T[] array, T item)
    {
        List<T> list = new List<T>(array)
        {
            item
        };
        return list.ToArray();
    }

    public static void DestroyChildren(this Transform parentTransform)
    {
        // Iterate through each child of the parent transform
        foreach (Transform child in parentTransform)
        {
            // Destroy the child GameObject
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        // Check if the component exists on the GameObject
        var component = gameObject.GetComponent<T>();
        if (component != null)
        {
            // Use Destroy to remove the component
            UnityEngine.Object.Destroy(component);
        }
    }
}
