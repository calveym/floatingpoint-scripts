using UnityEngine;
using System.Collections;
 
public class Music : MonoBehaviour {
 
    Object[] myMusic; // declare this as Object array
	public AudioSource[] audio;
	public AudioSource track1;
	public AudioSource track2;
	public AudioSource track3;
	public AudioSource track4;


    void Awake () {
       myMusic = Resources.LoadAll("Music",typeof(AudioClip));
//       audio.clip = myMusic[0] as AudioClip;
    }
    
    void Start (){
		audio = new AudioSource[4] { track1, track2, track3, track4 };
//  		audio.Play(); 
    }
 
    // Update is called once per frame
//    void Update () {
//       if(!audio.isPlaying)
//         playRandomMusic();
//    }
//    
//    void playRandomMusic() {
//       audio.clip = myMusic[Random.Range(0,myMusic.Length)] as AudioClip;
//       audio.Play();
//    }
}