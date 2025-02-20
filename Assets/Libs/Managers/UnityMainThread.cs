using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThread : MonoBehaviour
{
    public static UnityMainThread instance;
    Queue<Action> jobs = new Queue<Action>();
    public bool isHasNet = true;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        while (jobs.Count > 0)
        {
            jobs.Dequeue().Invoke();
        }
    }

    public void LateUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (WebSocketManager.getInstance().connectionStatus == Globals.ConnectionStatus.CONNECTED || !WebSocketManager.getInstance().IsAlive())
            {
                isHasNet = false;
                Globals.Logging.Log("Error. Check internet connection!");
                WebSocketManager.getInstance().connectionStatus = Globals.ConnectionStatus.DISCONNECTED;
                UIManager.instance.showLoginScreen(false);
                return;
            }
            else if (isHasNet)
            {
                Globals.Logging.Log("vao day roi");
                isHasNet = false;
                StartCoroutine(delayBox());
                return;
            }
        }
        else
        {
            if (!isHasNet && !LoadConfig.instance.isLoadedConfig)
            {

                Globals.Config.isErrorNet = false;
                LoadConfig.instance.getConfigInfo();
            }
            isHasNet = true;
        }

    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }

    IEnumerator delayBox()
    {
        yield return new WaitForSeconds(1);
        if (Globals.Config.isErrorNet) yield break;
        Globals.Config.isErrorNet = true;
        UIManager.instance.showMessageBox(Globals.Config.getTextConfig("err_network"));
        UIManager.instance.hideWatting();
    }
}
