using UnityEngine;

public class sfxManager : MonoBehaviour
{
    [SerializeField] private AudioClip minionDeathSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip hpDownSound;
    [SerializeField] private AudioClip minionAttackSound;
    [SerializeField] private AudioClip enemyAttackSound;
    [SerializeField] private AudioClip bossDeathSound;
    [SerializeField] private AudioClip bossVictorySound;
    [SerializeField] private AudioClip bossAttackSound;
    [SerializeField] private AudioClip clickUiSound;
    
    [SerializeField] private AudioSource audioSource;
    
    public static sfxManager Instance { get; private set; }
    
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // enforce single instance
                return;
            }
    
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional
        }

        public void PlayMinionDeath() => audioSource.PlayOneShot(minionDeathSound);
        public void PlayEnemyDeath() => audioSource.PlayOneShot(enemyDeathSound);
        public void PlayDefeat() => audioSource.PlayOneShot(defeatSound);
        public void PlayVictory() => audioSource.PlayOneShot(victorySound);
        public void PlayHpDown() => audioSource.PlayOneShot(hpDownSound);
        public void PlayMinionAttack() => audioSource.PlayOneShot(minionAttackSound);
        public void PlayEnemyAttack() => audioSource.PlayOneShot(enemyAttackSound);
        public void PlayBossDeath() => audioSource.PlayOneShot(bossDeathSound);
        public void PlayBossVictory() => audioSource.PlayOneShot(bossVictorySound);
        public void PlayBossAttack() => audioSource.PlayOneShot(bossAttackSound);
        public void PlayClickUI() => audioSource.PlayOneShot(clickUiSound);
        
        
}
