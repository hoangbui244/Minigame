using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public static class AddressablesUtility
{
    public static async void DownloadAssetsAsync<T>(AssetLabelReference key, Action<List<object>> completed = null)
    {
        List<object> values = new();
        var dl = Addressables.LoadAssetsAsync<T>(key, (item) =>
        {
            values.Add(item);
        });
        dl.Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                completed?.Invoke(values);
                completed = null;
                Debug.Log("Load completed");
            }
            else
            {
                Debug.Log("Failed to load");
            }
        };
        try
        {
            while (!dl.IsDone)
            {
                Debug.Log("Downloading Asset: " + dl.GetDownloadStatus().Percent);
                await Task.Yield();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Asset error: " + ex.Message);
        }
    }
}
