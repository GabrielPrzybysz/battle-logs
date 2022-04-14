using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LogListener : MonoBehaviour
{
    private string _session = "";

    private const string URL = "https://battlelogs.domain.com.br";
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        StartCoroutine(HandleLogRoutine(logString));
    }

    private IEnumerator HandleLogRoutine(string logString)
    {
        if (string.IsNullOrEmpty(_session))
        {
            _session = Guid.NewGuid().ToString();
        }

        var logData = new LogData(logString, _session);
        string json = JsonUtility.ToJson(logData);

        using (UnityWebRequest request = new UnityWebRequest(URL))
        {
            request.method = "POST";
            request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));

            yield return request.SendWebRequest();
        }
    }
}

public class LogData
{
    public LogData(string log, string session)
    {
        _log = log;
        _session = session;
    }

    private string _session;
    private string _log;
}
