using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer mediaPlayer;
    private string _path=string.Empty;
    [SerializeField]
    private GameObject pageMain;
    bool isTouch = false;
    private Timer timer=null;
    float maxtimer = 0;
    public Sprite[] spbtn;
    // Start is called before the first frame update
    void Start()
    {
        _path = Application.streamingAssetsPath;
        if (!System.IO.Directory.Exists(_path))
            System.IO.Directory.CreateDirectory(_path);

        var timer=ReadConfigFile(_path,"SetTimer.json");
        maxtimer=float.Parse(timer);
        pageMain.transform.GetComponent<Image>().sprite= Utils.GetTexture2d(string.Format("{0}/{1}.png", _path, "二级界面"));
        pageMain.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(SendMsgToClient);
        SetPlayerVideoUrl("点击查询");
    }
    /// <summary>
    /// 发送消息到客户端
    /// </summary>
    private void SendMsgToClient()
    { 
        bool isplay = pageMain.transform.GetChild(0).GetComponent<Image>().sprite == spbtn[0] ? true : false;
        pageMain.transform.GetChild(0).GetComponent<Image>().sprite = isplay ? spbtn[1] : spbtn[0];
        GetComponent<BroadCast>().SendMsg(isplay);
    }

    public void SetPlayerVideoUrl(string urlname,bool isLoop=true) 
    {
        mediaPlayer.url = string.Format("{0}/{1}.mp4", _path, urlname);
        mediaPlayer.Play();
        mediaPlayer.playbackSpeed = 1;
        mediaPlayer.isLooping = isLoop;
    }
    public void PauseVideo() 
    {
        mediaPlayer.Pause(); 
        pageMain.SetActive(true);
        CreatTimer();
    }
    public void PlayVideo() {
        mediaPlayer.Play();
    }
    private void CreatTimer() 
    {
        if (timer == null)
            timer = Timer.createTimer();
        timer.startTiming(maxtimer, OnConpleted); 
    }

    private void OnConpleted()
    {
        timer = null;
        pageMain.SetActive(false);
        PlayVideo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)||Input.touchCount>0)
        {
            isTouch = true; 
        }
        if (isTouch)
        {
            PauseVideo();
            isTouch = false; 
        }
    }
    //Json数据写入
    public void WriteConfigFile(string path, string fileName, string configInfo)
    {

        string file = System.IO.Path.Combine(path, fileName);
        if (!File.Exists(file))
        {
            File.CreateText(file).Dispose();
        } 
        string data = configInfo;
        Debug.Log("数据写入 data=  " + data);
        File.WriteAllText(file, data);

    }  //读取JSON内容
    public string ReadConfigFile(string path, string fileName)
    {
        string file = System.IO.Path.Combine(path, fileName);
        string ConfigInfo ="";
        if (File.Exists(file))
        {
            string data = File.ReadAllText(file);
            ConfigInfo = data;
        }
        return ConfigInfo;
    }
}

