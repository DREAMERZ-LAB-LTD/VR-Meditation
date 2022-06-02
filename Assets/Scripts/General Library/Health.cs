using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralLibrary
{
    public class Health : Debuggable
    {
        public interface IHealth
        {
            public void OnTakeDamage(GameObject damager);
            public void OnDeath(GameObject damager);
        }

        [Header("Health Setup")]
        [SerializeField] private bool isDeath = false;
        [SerializeField] private int maxHealth = 10;
        private int health;

        [Header("Health status bar")]
        [SerializeField] private UIBehaviour healthBar;
        private Slider healthBarSlider;
        private Image healthBarImage;

        [Header("Callbeck Events")]
        [Tooltip("Invoke per damaging")]
        public UnityEvent OnDamaging;
        [Tooltip("Invoke, when adding extra health")]
        public UnityEvent OnAdding;
        [Tooltip("Invoke, when health lessthen minmum health")]
        public UnityEvent OnDeath;
        private IHealth[] damageableResponses = { };

        private void Awake() => Initialize();


        /// <summary>
        /// Reset the health status as new
        /// </summary>
        public void Initialize()
        {
            if (healthBar)
            {
                healthBarSlider = healthBar.GetComponent<Slider>();
                if (healthBarSlider)
                {
                    healthBarSlider.minValue = 0;
                    healthBarSlider.maxValue = 1;
                }

                healthBarImage = healthBar.GetComponent<Image>();
                if (healthBarImage)
                {
                    healthBarImage.type = Image.Type.Filled;
                    healthBarImage.fillAmount = 1;
                }
            }

            health = maxHealth;
            UpdateHealthBar();

            damageableResponses = GetComponents<IHealth>();
        }

        /// <summary>
        /// Take damage on current health
        /// </summary>
        /// <param name="damager">Which one kill him</param>
        /// <param name="damageRate">How much damage take it per fream</param>
        public void TakeDamage(int damageRate, GameObject damager)
        {
            if (isDeath || damager == null) return;

            if (damager)
                ShowMessage("Take Damage from " + damager.name);

            health -= damageRate;
            isDeath = health <= 0;
            UpdateHealthBar();

            ShowMessage("Damage rate =  " + damageRate + " New health = " + health);
            ShowMessage(" Is death = " + isDeath);

            //event firing
            if (isDeath)
            {
                foreach (var damageResponse in damageableResponses)
                    if (damageResponse != null)
                        damageResponse.OnDeath(damager);

                OnDeath.Invoke();
            }
            else
            {
                foreach (var damageResponse in damageableResponses)
                    if (damageResponse != null)
                        damageResponse.OnTakeDamage(damager);

                OnDamaging.Invoke();
            }
        }

        /// <summary>
        /// Make it instant death 
        /// </summary>
        /// <param name="damager">Which one kill him</param>
        public void ForceDeath(GameObject damager)
        {
            if (isDeath || !enabled) return;

            health = 0;
            isDeath = health <= 0;
            UpdateHealthBar();

            ShowMessage("Force Death Activated");
            ShowMessage(" Is death = " + isDeath);

            foreach (var damageResponse in damageableResponses)
                if (damageResponse != null)
                    damageResponse.OnDeath(damager);

            if (isDeath)
                OnDeath.Invoke();
        }



        /// <summary>
        /// Add health amount with current health amount
        /// </summary>
        /// <param name="amount"></param>
        public void AddHealth(int amount)
        {
            health = Mathf.Clamp(health + amount, 0, maxHealth);
            ShowMessage("Health added = " + amount);
            OnAdding.Invoke();
        }


        /// <summary>
        /// Show health bar status on UI Slider Component
        /// </summary>
        private void UpdateHealthBar()
        {
            float t = health / (float)maxHealth;
            if (healthBarSlider)
                healthBarSlider.value = t;

            if (healthBarImage)
                healthBarImage.fillAmount = t;
        }
    }
}