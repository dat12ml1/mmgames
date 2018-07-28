using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BUDGIE_SOUNDER_CONTROLLER;
namespace BUDGIE_SOUNDER_CONTROLLER
{
    public enum BUDGIE_SOUND { LEFT_LEG, RIGHT_LEG, JUMP, ATTACK, HIT, GOT_HIT, WATER_SPLASH };
}

public class BudgieSoundController : MonoBehaviour {

    
    public AudioClip leftLegSound;
    public AudioClip rightLegSound;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip gotHitSound;
    public AudioClip waterSplashSound;

    private double freeTimeSlot = 0;

    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playClip(BUDGIE_SOUND sound){

        
        // problem probably causes by clip being changed before scheduled play
        float prev_clip_length = audioSource.clip.length;
        switch (sound)
        {
            case BUDGIE_SOUND.LEFT_LEG:
                scheduleSound(leftLegSound,false);
                break;
            case BUDGIE_SOUND.RIGHT_LEG:
                scheduleSound(rightLegSound,false);
                break;
            case BUDGIE_SOUND.JUMP:
                scheduleSound(jumpSound,true);
                break;
            case BUDGIE_SOUND.ATTACK:
                scheduleSound(attackSound, true);
                break;
            case BUDGIE_SOUND.HIT:
                scheduleSound(hitSound, true);
                break;
            case BUDGIE_SOUND.GOT_HIT:
                scheduleSound(gotHitSound, true);
                break;
            case BUDGIE_SOUND.WATER_SPLASH:
                scheduleSound(waterSplashSound, true);
                break;
        }
    }

    private double waitTime = 0.025d;
    private void scheduleSound(AudioClip clip, bool replaceOldClip)
    {
        // free to play the clip now
        if (AudioSettings.dspTime > freeTimeSlot)
        {
            double playtime = AudioSettings.dspTime + waitTime;
            audioSource.clip = clip;
            audioSource.PlayScheduled(playtime);
            freeTimeSlot = playtime + audioSource.clip.length;
        } else if(replaceOldClip)
        {
            audioSource.clip = clip;
            double playtime = freeTimeSlot + waitTime;
            audioSource.PlayScheduled(playtime);
            freeTimeSlot = playtime + audioSource.clip.length;
        }

            
    }

}
