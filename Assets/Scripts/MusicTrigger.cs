// 2022-04-18   Sean Hall       Created script - attach to trigger objects to change music

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    //private MusicManager musicM;
    public AudioClip newTrack;

    // Start is called before the first frame update
    void Start()
    {
        //musicM = GetComponent<MusicManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (newTrack != null && newTrack != MusicManager.current.music.clip) // Music does not update if same track or nonexistent
            {
                MusicManager.current.UpdateMusic(newTrack);
                //MusicManager.current.NewZone(newTrack);
            }
        }
    }
}
