﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int pointsPerFood = 10;				//Number of points to add to player food points when picking up a food object.
		public int pointsPerSoda = 20;				//Number of points to add to player food points when picking up a soda object.
		public int baseDamage = 1;					//How much damage a player does to a wall when chopping it.
		public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;					//1 of 2 Audio clips to play when player collects a food object.
		public AudioClip eatSound2;					//2 of 2 Audio clips to play when player collects a food object.
		public AudioClip drinkSound1;				//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;				//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
		
		private Animator animator;					//Used to store a reference to the Player's animator component.
		private int food;                           //Used to store player food points total during level.
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif

        UiController _uiController;
        float setupTimer = 7;
        float countdownTimer;
        [SerializeField] Text timerText;
        [SerializeField] int playerAttackHealthPenalty = 5;

        Weapon _activeWeapon;
        public Weapon activeWeapon { get { return _activeWeapon; } }
        int trapsInInventory = 0;
        public int stepInTrapPenalty = 30;

        //Start overrides the Start function of MovingObject
        protected override void Start ()
		{
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();

            //Get UI Controller
            _uiController = FindObjectOfType<UiController>();

            //Get the current food point total stored in GameManager.instance between levels.
            food = GameManager.instance.playerFoodPoints;

            //Get the current weapon stored in GameManager.instance between levels.
            _activeWeapon = GameManager.instance.playerWeapon;

            //Set the foodText to reflect the current player food total.
            _uiController.SetFoodText(food);
			
			//Call the Start function of the MovingObject base class.
			base.Start ();

            //(Re)set timer
            countdownTimer = setupTimer + 2; //The +2 is compensation for the black screen in the beginning.
        }
		
		
		//This function is called when the behaviour becomes disabled or inactive.
		private void OnDisable ()
		{
			//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
			GameManager.instance.playerFoodPoints = food;
            GameManager.instance.playerWeapon = _activeWeapon;
        }


        private void FixedUpdate ()
		{
			//If it's not the player's turn, exit the function.
			if(!GameManager.instance.playersTurn) return;

            //Count down
            countdownTimer -= Time.deltaTime;

            _uiController.SetTimerText(countdownTimer, setupTimer);


            if (countdownTimer <= 0)
            {
                PlayerDidntMove();
                return;
            }
			
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;		//Used to store the vertical move direction.
			
			//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER
			
			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));
			
			//Check if moving horizontally, if so set vertical to zero.
			if(horizontal != 0)
			{
				vertical = 0;
			}
			//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif //End of mobile platform dependendent compilation section started above with #elif
			//Check if we have a non-zero value for horizontal or vertical
			if(horizontal != 0 || vertical != 0)
			{
				//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
				//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
				AttemptMove<Wall> (horizontal, vertical);
			}

            if (Input.GetKeyDown("space") && trapsInInventory > 0)
            {
                //Place trap
                Instantiate(GameManager.instance.trap,transform.position, transform.rotation);

                //Take one from the inventory
                trapsInInventory--;

                _uiController.SetTrapsLeft(trapsInInventory);
                Debug.Log("Traps Left: " + trapsInInventory);
            }
        }
		
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
            //(Re)set timer
            countdownTimer = setupTimer;

            //Every time player moves, subtract from food points total.
            food--;
			
			//Update food text display to reflect current score.
            _uiController.SetFoodText(food);

            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove <T> (xDir, yDir);
			
			//Hit allows us to reference the result of the Linecast done in Move.
			RaycastHit2D hit;
			
			//If Move returns true, meaning Player was able to move into an empty space.
			if (Move (xDir, yDir, out hit)) 
			{
				//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
			
			//Since the player has moved and lost food points, check if the game has ended.
			CheckIfGameOver ();
			
			//Set the playersTurn boolean of GameManager to false now that players turn is over.
			GameManager.instance.playersTurn = false;
        }
		
		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
			//Set hitWall to equal the component passed in as a parameter.
			Wall hitWall = component as Wall;
			
			//Call the DamageWall function of the Wall we are hitting.
            if(_activeWeapon != null)
            {
                hitWall.DamageWall(baseDamage + _activeWeapon.wallDamage);
                PlayerUsesWeapon();
            }
            else
            {
                hitWall.DamageWall(baseDamage);
            }

            //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
            animator.SetTrigger ("playerChop");
		}

        /// <summary>
        /// Called when the player attacks a zombie, it retracts health based on the current weapon the player is wielding. Also retracts one durability from the weapon.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        protected override void AttackEnemy <T> (T component)
        {
            int x = playerAttackHealthPenalty;
            LoseFood(x);

            GameObject loseHealthFx = Instantiate(GameManager.instance.loseHealthFx, transform.position, transform.rotation);
            loseHealthFx.GetComponent<LoseHealthFx>().SetText(x);

            if (_activeWeapon != null)
            {
                PlayerUsesWeapon();
            }
        }

        //Lowers the attacks left and checks if the weapon has any attacks left, if not, the weapon will be destroyed.
        void PlayerUsesWeapon()
        {
            _activeWeapon.PlayerAttacked();
            _uiController.SetAmmoText(_activeWeapon.AttacksLeft());


            if (_activeWeapon.AttacksLeft() == 0)
            {
                _activeWeapon = null;
                _uiController.SetActiveWeapon(" - ");
            }
        }

        // PlayerDidntMove is called when it is the player's turn and he hasn't moved yet.
        void PlayerDidntMove()
        {
            food -= pointsPerFood;

            //Update food text display to reflect current score.
            _uiController.SetFoodText(food);

            //Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver();

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.instance.playersTurn = false;

            //Resets the timer
            int i = GameManager.instance.getLevel;
            switch (i)
            {
                case 1:
                    setupTimer = 7;
                    break;
                case 5:
                    setupTimer = 5;
                    break;
                case 10:
                    setupTimer = 3;
                    break;
                case 15:
                    setupTimer = 1;
                    break;
                default:
                    //Do Nothing
                    break;
            }
            countdownTimer = setupTimer;
        }
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);
				
				//Disable the player object since level is over.
				enabled = false;
			}
			
			//Check if the tag of the trigger collided with is Food.
			else if(other.tag == "Food")
			{
				//Add pointsPerFood to the players current food total.
				food += pointsPerFood;

                //Update foodText to represent current total and notify player that they gained points
                _uiController.SetFoodText(food);

                //Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
                SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				
				//Disable the food object the player collided with.
				other.gameObject.SetActive (false);
			}
			
			//Check if the tag of the trigger collided with is Soda.
			else if(other.tag == "Soda")
			{
				//Add pointsPerSoda to players food points total
				food += pointsPerSoda;

                //Update foodText to represent current total and notify player that they gained points
                _uiController.SetFoodText(food);

                //Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
                SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				//Disable the soda object the player collided with.
				other.gameObject.SetActive (false);
			}

            else if (other.tag == "Trap")
            {
                LoseFood(stepInTrapPenalty);
                GameObject loseHealthFx = Instantiate(GameManager.instance.loseHealthFx, transform.position, transform.rotation);
                loseHealthFx.GetComponent<LoseHealthFx>().SetText(stepInTrapPenalty);
                Destroy(other.gameObject);
            }

            else if (other.tag == "WeaponCrate")
            {
                GiveRandomWeapon();
                other.gameObject.SetActive(false);
            }
        }


        //Restart reloads the scene when called.
        private void Restart ()
		{
			//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
		
		
		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseFood (int loss)
		{
			//Set the trigger for the player animator to transition to the playerHit animation.
			animator.SetTrigger ("playerHit");
			
			//Subtract lost food points from the players total.
			food -= loss;

            //Update the food display with the new total.
            _uiController.SetFoodText(food);

            //Check to see if game has ended.
            CheckIfGameOver();

            //Show blood
            Instantiate(GameManager.instance.blood, transform.position, transform.rotation);

            GameObject loseHealthFx = Instantiate(GameManager.instance.loseHealthFx, transform.position, transform.rotation);
            loseHealthFx.GetComponent<LoseHealthFx>().SetText(loss);
        }


        //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
        private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (food <= 0) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}

        public int DamageCalculatorAgainstZombies()
        {
            int damage = 0;

            if(_activeWeapon != null)
            {
                damage = _activeWeapon.damage;
                Debug.Log(_activeWeapon.damage);
            }
            else
            {
                damage = baseDamage;
            }
            return damage;
        }

        void GiveRandomWeapon()
        {
            //Resets the timer
            int i = Random.Range(0, 3);
            string weapon = "";

            switch (i)
            {
                case 0:
                    _activeWeapon = new Shotgun();
                    weapon = "Shotgun";
                    _uiController.SetActiveWeapon(weapon);
                    break;

                case 1:
                    trapsInInventory += 2;
                    weapon = "Traps";
                    _uiController.SetTrapsLeft(trapsInInventory);
                    break;

                case 2:
                    _activeWeapon = new MeleeWeapon();
                    weapon = "Melee Weapon";
                    _uiController.SetActiveWeapon(weapon);
                    break;

                default:
                    //Do Nothing
                    Debug.Log("Something went wrong!");
                    break;
            }
            Debug.Log("New weapon acquired: " + weapon);
        }
    }
}

