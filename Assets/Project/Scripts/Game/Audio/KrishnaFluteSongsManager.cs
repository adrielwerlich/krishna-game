using UnityEngine;

public class KrishnaFluteSongsManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private AudioClip[] songs;
    
    [SerializeField] private float _maxDistance = 60f;
    [SerializeField] private float _minVolume = 0.1f;
    [SerializeField] private float _maxVolume = 1f;

    [SerializeField] private ParticleSystem _soundsParticles;

    private AudioSource _audioSource;
    private bool shouldPlay = false;



    public float volume = .5f;
    public static bool PlayerInTemple = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Helper.isMobile())
        {
            //_soundsParticles.gameObject.SetActive(false);
        } 
        else
        {
        }
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Stop();

            PlayFlute();

        //Player.PlayerInArea += IsTempleArea;
        //KrishnaInteractionManager.PlayMantra += PlayTheMantra;
    }

    //private void PlayTheMantra(Helper.MantraIndex mantraIndex)
    //{
    //    _audioSource.Stop();
    //    _audioSource.clip = Helper.getMantras()[((int)mantraIndex)];
    //    _audioSource.Play();
    //}

    private void IsTempleArea(string area)
    {
        if (area == "KrishnaTemple")
        {
            PlayFlute();
        }
        else if (area == "OutsideKrishnaTemple")
        {
            PlayerInTemple = shouldPlay = false;
            _audioSource.Stop();
        }
    }

    private void PlayFlute()
    {
        PlayerInTemple = shouldPlay = true;
        GetNextSong();
    }

    // Update is called once per frame
    void Update()
    {
        GetNextSong();
        if (player != null)
        {
            // Calculate the distance between the audio source and the target
            float distance = Helper.GetDistanceBetweenTwoObjects(
                transform.position, 
                player.transform.position
            );

            // Calculate the volume based on the distance
            float volume = Mathf.Lerp(_maxVolume, _minVolume, distance / _maxDistance);

            // Set the volume of the audio source
            _audioSource.volume = volume;
            //Debug.Log("_audioSource.volume => " + _audioSource.volume);
        }
    }

    private void GetNextSong()
    {
        if (_audioSource != null && !_audioSource.isPlaying && shouldPlay)
        {
            //_audioSource.volume = volume;
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    private void ChangeSong(int indexToPlay)
    {
        _audioSource.clip = songs[indexToPlay];
        _audioSource.Play();
    }

}
