
using UnityEngine;


namespace GeneralLibrary
{
    public class RewardSource : MonoBehaviour
    {
        [SerializeField] private int givenScore = 1;
        private ScoreManager scoreManager;

        protected void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        public void GiveScore()
        {
            if (enabled)
            {
                scoreManager.UpdateScore(givenScore);
            }
        }
    }
}