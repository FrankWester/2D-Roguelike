using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    [SerializeField] RectTransform _leftPanel;

    public GameObject levelImage;
    public Text levelText;

    [Header("Images")]
    [SerializeField] Image _foodImage;
    [SerializeField] Image _timerImage;
    [SerializeField] Image _activeWeaponImage;
    [SerializeField] Image _ammoImage;
    [SerializeField] Image _trapsImage;

    [Header("Sprites")]
    [SerializeField] Sprite _food;
    [SerializeField] Sprite _melee; 
    [SerializeField] Sprite _gun; 
    [SerializeField] Sprite _trap;
    [SerializeField] Sprite _clock;
    [SerializeField] Sprite _noWeapon;
    [SerializeField] Sprite _ammoSprite;

    [Header("Text")]
    [SerializeField] Text _foodText;
    [SerializeField] Text _activeWeapon;
    [SerializeField] Text _ammo; 
    [SerializeField] Text _timer;
    [SerializeField] Text _trapsLeft;

    public static UiController instance = null;	//Static instance of GameManager which allows it to be accessed by any other script.


    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _leftPanel.sizeDelta = new Vector2(Screen.width / 4, Screen.height);

        //Setting images for the start
        _foodImage.sprite = _food;
        _timerImage.sprite = _clock;
        _activeWeaponImage.sprite = _noWeapon; ;
        _ammoImage.sprite = _ammoSprite;
        _trapsImage.sprite = _trap;

        //Preserving all aspects
        _foodImage.preserveAspect = true;
        _timerImage.preserveAspect = true;
        _activeWeaponImage.preserveAspect = true;
        _ammoImage.preserveAspect = true;
        _trapsImage.preserveAspect = true;
    }

    public void SetFoodText(int food)
    {
        _foodText.text = food.ToString();
    }

    public void SetAmmoText(int ammo)
    {
        _ammo.text = ammo.ToString();
    }

    public void SetTimerText(float timer, float totalTime)
    {
        _timer.text = timer.ToString("F1");

        if(timer <= totalTime/3)
        {
            _timer.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            _timer.color = new Color32(94, 94, 94, 255);
        }
    }

    public void SetActiveWeapon(string weaponName)
    {
        _activeWeapon.text = weaponName;

        if(weaponName == "Melee Weapon")
        {
            _activeWeaponImage.sprite = _melee;
        }
        else if (weaponName == "Shotgun")
        {
            _activeWeaponImage.sprite = _gun;
        }
        else
        {
            _activeWeaponImage.sprite = _noWeapon;

        }
    }

    public void SetTrapsLeft(int trapsLeft)
    {
        _trapsLeft.text = trapsLeft.ToString();
    }
}
