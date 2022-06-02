using System.Collections.Generic;
using UnityEngine;


namespace GeneralLibrary
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Dlab/Player/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [System.Serializable]
        public class DataField
        {

            [SerializeField]
            private string m_Name;
            public Vector2 limit;
            [SerializeField]
            private float m_Value = 1;

            public delegate void FieldChanged(DataField field);
            public FieldChanged OnFieldChanged;

            public float Value
            {
                get
                {
                    if (limit == Vector2.zero)
                        return m_Value;
                    else
                        return Mathf.Clamp(m_Value, limit.x, limit.y);
                }
                set
                {
                    if (limit == Vector2.zero)
                        m_Value = value;
                    else
                        m_Value = Mathf.Clamp(value, limit.x, limit.y);

                    if (OnFieldChanged != null)
                        OnFieldChanged.Invoke(this);
                }
            }
        }

        public delegate void UpdateSpeed(float runSpeed, float turnSpeed);
        public delegate void ScoreUpdate(int newScore);
        public ScoreUpdate OnScoreChanged;


        [Header("Upgraded Data")] public int score;
        public List<DataField> dataFields = new List<DataField>();


        public bool AddScore(int deltaScore)
        {
            if (score <= 0 && deltaScore < 0)
                return false;

            score += deltaScore;
            score = (int)Mathf.Clamp(score, 0, Mathf.Infinity);
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(score);

            return true;
        }

        public void SetScore(int newScore)
        {
            score = newScore;
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(score);
        }
    }
}