using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> clipList;
    public int clipCount;
    public int maxClipCount;
    public GameObject playButton;
    public GameObject pauseButton;
    public TMP_Text debug;
    public string folderPath;



    private void Awake()
    {
        folderPath = Application.persistentDataPath + "/Music/";
        audioSource = GetComponent<AudioSource>();
        Directory.CreateDirectory(folderPath);


    }

    private void Start()
    {
         
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");
        for (int i = 0; i < clips.Length; i++)
        {
            clipList.Add(clips[i]);
            maxClipCount++;
        }
        maxClipCount -= 1;
        UpdateClipList();

        

    }

    private void Update()
    {
        //debug.text = clipList.Count.ToString() + "\n" + clipCount;
    }

    public void PlayMusic()
    {
        if (clipCount >= 0 && clipCount <= maxClipCount)
        {
            audioSource.clip = clipList[clipCount];
            audioSource.Play();
            playButton.SetActive(false);
            pauseButton.SetActive(true);
        }
        else if (clipCount < 0)
        {
            clipCount = maxClipCount;
            PlayMusic();
        }
        else if (clipCount > maxClipCount)
        {
            clipCount = 0;
            PlayMusic();
        }
        else
        {
            clipCount = 0;
            print("播放错误, 没有音频");
        }
    }

    public void PauseMusic()
    {
        audioSource.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void UpdateMaxClipCount()
    {
        maxClipCount = clipList.Count - 1;
    }

    public void NextMusic()
    {
        clipCount++;
        PlayMusic();
    }

    public void LastMusic()
    {
        clipCount--;
        PlayMusic();
    }


    public void UpdateClipList()
    {
#if UNITY_EDITOR
        DirectoryInfo folder = new DirectoryInfo(folderPath);

#elif PLATFORM_ANDROID
        DirectoryInfo folder = new DirectoryInfo(folderPath);

#endif


        //LoadAB();
        var files = folder.GetFiles("*.mp3");
        Debug.Log("files count :" + files.Length);
        //debug.text = (folderPath);
        for (int i = 0; i < files.Length; i++)
        {
            StartCoroutine(LoadMusic(folder + files[i].Name));
        }
    }

    public IEnumerator LoadMusic(string filePath)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                clip.name = DownloadHandlerAudioClip.GetContent(uwr).name;
                clipList.Add(clip);
                UpdateMaxClipCount();
            }
        }

    }



}
