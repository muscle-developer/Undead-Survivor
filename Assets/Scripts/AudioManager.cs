 using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    // 어디에서도 사용할 수 있게
    public static AudioManager instance;
    
    [Header("BGM")]
    // BGM과 관련된 클립, 볼륨값, 오디오소스
    public AudioClip bgmClicp;
    public float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("SFX")]
    // 효과음과 관련된 클립, 값, 오디오소스
    public AudioClip[] sfxClicps;
    public float sfxVolume;
    private AudioSource[] sfxPlayers;
    
    // 각각의 다른 효과음을 위한 변수
    public int channels;
    // 현재 채널
    private int lastPlayChannelIndex;

    // 쉬운 구별을 위해 효과음을 열거형으로 관리
    public enum SFX { DEAD, HIT, LEVELUP = 3, LOSE, MELEE, RANGE = 7, SELECT, WIN}
    
    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 초기화
        GameObject bgmObject = new GameObject("BgmObject");
        bgmObject.transform.parent = this.transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        
        bgmPlayer.playOnAwake = false; 
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClicp;

        // 효과음 초기화
        GameObject sfxObject = new GameObject("SfxObject");
        sfxObject.transform.parent = this.transform;
        // 채널의 갯수만큼 생성
        sfxPlayers = new AudioSource[channels];

        // 각각의 효과음 컴포넌트 추가 및 초기화
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    // 효과음 실행
    public void PlaySFX(SFX sfx)
    {
        // 채널의 갯수만큼(오디오 클립 갯수) 반복문을 통해 체크한후 맞는것을 실행
        for(int i = 0; i < channels; i++)
        {
            // index가 초과했을 떄 오류 방지하기 위해 나머지를 사용
            int currentChannelIndex = (i +  lastPlayChannelIndex) % channels;

            // 이미 재생하고 있는 효과음이 있을 때 다음 효과음이 실행되도록 
            if(sfxPlayers[currentChannelIndex].isPlaying)
                continue;

            // 중복되는 효과음이 있을경우 랜덤으로 실행되게
            int randomIndex = 0;    
            if(sfx == SFX.HIT || sfx == SFX.MELEE)
                randomIndex = Random.Range(0, 2);

            // 마지막 플레이한 인덱스를 현재 인덱스로 초기화 시켜주기
            lastPlayChannelIndex = currentChannelIndex;
            // 해당 클립에 맞는 클립으로 초기화 후 실행
            sfxPlayers[lastPlayChannelIndex].clip = sfxClicps[(int)sfx + randomIndex];
            sfxPlayers[lastPlayChannelIndex].Play();
            break;
        }   
    }

    // 배경음 실행
    public void PlayBGM(bool isPlay)
    {
        // 플레이 중일때만 배경음 실행
        if(isPlay)
            bgmPlayer.Play();
        else 
            bgmPlayer.Stop();
    }

    // 배경음 일시정지
    public void PauseBGM()
    {
        bgmPlayer.Pause();   
    }

    // 배경음 정지 후 이어서 실행
    public void UnPause()
    {
        bgmPlayer.UnPause();
    }
}
