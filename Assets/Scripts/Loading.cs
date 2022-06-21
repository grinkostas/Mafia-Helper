using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private const string DefaultCardUrl = "https://pastebin.com/raw/EUCB9it0";

    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        if (System.IO.File.Exists(SaveSystem.DefaultPath) == false)
            yield return DownloadDefaultCards();
        if (System.IO.File.Exists(SaveSystem.Path) == false)
            System.IO.File.Create(SaveSystem.Path);
        SceneManager.LoadScene("Game");
    }
    
    private IEnumerator DownloadDefaultCards()
    {
        using UnityWebRequest request = UnityWebRequest.Get(DefaultCardUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            throw new Exception();
        }
        else
        {       
            System.IO.File.WriteAllText(SaveSystem.DefaultPath, request.downloadHandler.text);
        }
    }
}
