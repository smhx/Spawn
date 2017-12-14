using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : Initializer<PlayerItems> {

	public Animator animator;

	SceneLoader sceneLoader;

	[SerializeField] GameObject menu;
	[SerializeField] GameObject store;
	[SerializeField] GameObject scrollView;
	[SerializeField] GameObject bcScrollView;

	[SerializeField] List<GameObject> stores;
	[SerializeField] GameObject bcStore;

	[SerializeField] List<Button> brickButtons;
	[SerializeField] List<Button> paddleButtons;
	[SerializeField] List<Button> ballButtons;
	[SerializeField] List<Button> backgroundImageButtons;
	[SerializeField] List<Button> backgroundCircleButtons;
	List<List<Button>> buttons = new List<List<Button>>();

	[SerializeField] Text titleText;
	[SerializeField] Text moneyText;

	static readonly string[] typeNames = new string[4] { "Bricks", "Paddle", "Ball", "Background" };

	[SerializeField] Sprite emptyCircle;
	[SerializeField] Sprite filledCircle;
	[SerializeField] Sprite emptyThickCircle;

	[SerializeField] float regularButtonHeight;
	[SerializeField] float dropdownTime;
	[SerializeField] float heightPerLine;
	[SerializeField] int charsPerLine;
	[SerializeField] float buttonDropPadding;

	int currentlyViewing; //0 for bricks, 1 for paddle, 2 for ball, 3 for background
	BackgroundImage currentBackgroundImage;
	List<Background> displayedBackgrounds = new List<Background>();

	bool isExpanding = false;
	bool isShrinking = false;
	Button buttonToChangeHeight;
	float expandTime;
	float shrinkTime;
	List<Button> expandedButtons = new List<Button>();


	void Awake() {
		sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
		buttons.Add(brickButtons);
		buttons.Add(paddleButtons);
		buttons.Add(ballButtons);
		buttons.Add(backgroundImageButtons);
	}

	// Functions for drop down height as a function of description length
	float DropDownForStrLen(int len) {
		int lines = len / charsPerLine + 1;
//		Debug.Log ("len = " + len.ToString () + "lines = " + lines.ToString ());
		return heightPerLine * lines + buttonDropPadding + regularButtonHeight;
	}
	float DropDownForButton(Button b) {
		return DropDownForStrLen(b.gameObject.transform.Find ("Description").GetComponent<Text>().text.Length);
	}

	void Update() {
		if (isExpanding) {
			if (expandTime <= dropdownTime) {
                buttonToChangeHeight.GetComponent<LayoutElement>().minHeight = Mathf.Lerp(regularButtonHeight, DropDownForButton(buttonToChangeHeight), expandTime / dropdownTime);
				expandTime += Time.deltaTime;
			}
			else {
				isExpanding = false;
                buttonToChangeHeight.GetComponent<LayoutElement> ().minHeight = DropDownForButton(buttonToChangeHeight);
                buttonToChangeHeight.transform.Find("Description").gameObject.SetActive(true);
                buttonToChangeHeight.gameObject.transform.Find("Drop Down Icon").gameObject.SetActive(true);
                buttonToChangeHeight.gameObject.transform.Find("Drop Down Icon").GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 90f);
            }
		}
		if (isShrinking) {
            if (shrinkTime <= dropdownTime) {
                buttonToChangeHeight.GetComponent<LayoutElement>().minHeight = Mathf.Lerp(DropDownForButton(buttonToChangeHeight), regularButtonHeight, shrinkTime / dropdownTime);
				shrinkTime += Time.deltaTime;
			}
			else {
				isShrinking = false;
                buttonToChangeHeight.GetComponent<LayoutElement>().minHeight = regularButtonHeight;
                buttonToChangeHeight.gameObject.transform.Find ("Drop Down Icon").gameObject.SetActive (true);
                buttonToChangeHeight.gameObject.transform.Find("Drop Down Icon").GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -90f);
            }
		}
	}

	protected override void ConfigureSettings(PlayerItems dat) {
		moneyText.text = dat.money.ToString();
		DisplayButtons();	
	}



	public void DisplayMenu() {
		animator.SetTrigger ("display_menu");
	}

	//displays the store that corresponds to x
	public void DisplayStore(int x) {
		animator.SetTrigger ("display_store");
		currentlyViewing = x;
		expandedButtons = new List<Button>(); //resets which buttons are expanded when store type switched
		if (x == 3) {
			bcScrollView.SetActive(true);
			scrollView.GetComponent<ScrollRect>().horizontal = true;
			scrollView.GetComponent<ScrollRect>().vertical = false;
			Store store = setting as Store;
			currentBackgroundImage = Store.backgroundImages[store.GetActiveBackground().image];
			bcStore.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
			titleText.fontSize = 120;
            titleText.GetComponent<RectTransform>().offsetMin = new Vector2(-100, 0);
		}
		else {
			titleText.fontSize = 150;
            titleText.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            bcScrollView.SetActive(false);
			scrollView.GetComponent<ScrollRect>().horizontal = false;
			scrollView.GetComponent<ScrollRect>().vertical = true;
		}
		for (int i=0; i<stores.Count; i++) {
			if (i == x) {
				stores[i].SetActive(true);
				DisplayButtons();
				scrollView.GetComponent<ScrollRect>().content = stores[i].GetComponent<RectTransform>();
				titleText.text = typeNames[i];
			}
			else
				stores[i].SetActive(false);
		}
	}


	//displays the buttons of the item that is currently being viewed
	void DisplayButtons() {
		if (currentlyViewing == 0)
			DisplayBrickButtons();
		else if (currentlyViewing == 1)
			DisplayPaddleButtons();
		else if (currentlyViewing == 2)
			DisplayBallButtons();
		else if (currentlyViewing == 3) {
			DisplayBackgroundImageButtons();
			DisplayBackgroundCircleButtons();
		}
	}

	void DisplayBrickButtons() {
		for (int i = 0; i < Store.bricks.Count; i++) {
			if (brickButtons[i]==null) {
				Debug.LogError ("StoreManager::DisplayBrickButtons brickButtons[i]==null, i="+i);
				return;
			}
			Brick brick = Store.bricks[(BrickTypes)i];
			Button b = buttons[0][i];
			Ability a = Store.abilities[brick.brickAbility];

			Image brickImage = b.transform.Find("Brick Image").gameObject.GetComponent<Image>();
			Text nameText = b.transform.Find("Name Text").gameObject.GetComponent<Text>();
			Text message = b.transform.Find("Buy Button").GetChild(0).GetComponent<Text>();
			Text price = b.transform.Find("Buy Button").GetChild(1).GetComponent<Text>();
			Image moneyIcon = b.transform.Find("Buy Button").GetChild(2).GetComponent<Image>();
			GameObject levelDisplay = b.transform.Find("Level Display").gameObject;
			Text description = b.transform.Find("Description").gameObject.GetComponent<Text>();

			brickImage.sprite = brick.sprite;
			brickImage.color = brick.storeColor;
			nameText.text = brick.brickName;
			description.text = a.description[a.level];

			if (a.level == a.maxLevel) {
				message.text = "Max Level";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else {
				message.text = "";
				price.text = a.cost[a.level + 1].ToString();
				moneyIcon.enabled = true;
			}
			for (int j=1; j<=levelDisplay.transform.childCount; j++) {
				Image circle = b.transform.Find("Level Display").GetChild(j - 1).gameObject.GetComponent<Image>();
				if (j <= a.level) {
					circle.enabled = true;
					circle.sprite = filledCircle;
				}
				else if (j <= a.maxLevel) {
					circle.sprite = emptyThickCircle;
					circle.enabled = true;
				}
				else
					circle.enabled = false;
			}
		}
	}

	void DisplayPaddleButtons() {
		Store store = setting as Store;
		for (int i = 0; i < Store.paddles.Count; i++) {
			Paddle p = Store.paddles[(PaddleTypes)i];
			Button b = buttons[1][i];

			Image paddleImage = b.transform.Find("Paddle Image").gameObject.GetComponent<Image>();
			Text nameText = b.transform.Find("Name Text").gameObject.GetComponent<Text>();
			Text message = b.transform.Find("Buy Button").GetChild(0).GetComponent<Text>();
			Text price = b.transform.Find("Buy Button").GetChild(1).GetComponent<Text>();
			Image moneyIcon = b.transform.Find("Buy Button").GetChild(2).GetComponent<Image>();
			Text description = b.transform.Find("Description").gameObject.GetComponent<Text>();

			paddleImage.color = p.color;
			nameText.text = p.paddleName;
			description.text = p.paddleDescription;

			if (store.IsActive((PaddleTypes)i)) {
				message.text = "Selected";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else if (store.IsUnlocked((PaddleTypes)i)) {
				message.text = "Select";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else {
				message.text = "";
				price.text = p.price.ToString();
				moneyIcon.enabled = true;
			}
		}
	}

	void DisplayBallButtons() {
		Store store = setting as Store;
		for (int i = 0; i < Store.balls.Count; i++) {
			Ball ball = Store.balls[(BallTypes)i];
			Button b = buttons[2][i];

			Image ballImage = b.transform.Find("Ball Image").gameObject.GetComponent<Image>();
			Text nameText = b.transform.Find("Name Text").gameObject.GetComponent<Text>();
			Text message = b.transform.Find("Buy Button").GetChild(0).GetComponent<Text>();
			Text price = b.transform.Find("Buy Button").GetChild(1).GetComponent<Text>();
			Image moneyIcon = b.transform.Find("Buy Button").GetChild(2).GetComponent<Image>();

			ballImage.color = ball.storeColor;
			nameText.text = ball.ballName;

			if (store.IsActive((BallTypes)i)) {
				message.text = "Selected";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else if (store.IsUnlocked((BallTypes)i)) {
				message.text = "Select";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else {
				message.text = "";
				price.text = ball.price.ToString();
				moneyIcon.enabled = true;
			}
		}
	}

	void DisplayBackgroundImageButtons() {

		Store store = setting as Store;
		for (int i = 0; i < backgroundImageButtons.Count; i++) {
			
			BackgroundImage bi = Store.backgroundImages[(BackgroundImageTypes)i];
			Button b = buttons[3][i];

			Image backgroundImage = b.transform.Find("Background Image").gameObject.GetComponent<Image>();
			Text message = b.transform.Find("Buy Button").GetChild(0).GetComponent<Text>();
			Text price = b.transform.Find("Buy Button").GetChild(1).GetComponent<Text>();
			Image moneyIcon = b.transform.Find("Buy Button").GetChild(2).GetComponent<Image>();

			backgroundImage.sprite = bi.sprite;
			backgroundImage.color = bi.color;

			if (store.IsUnlocked((BackgroundImageTypes)i)) {
				message.text = "Bought";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else {
				message.text = "";
				price.text = bi.price.ToString();
				moneyIcon.enabled = true;
			}
		}
	}

	void DisplayBackgroundCircleButtons() {
		Store store = setting as Store;

		displayedBackgrounds.Clear();
		int buttonIndex = 0;
		foreach (BackgroundCircleTypes bct in Enum.GetValues(typeof(BackgroundCircleTypes))) {
			if (currentBackgroundImage.incompatibleCircles.Contains(bct))
				continue;
			if (buttonIndex >= Enum.GetValues(typeof(BackgroundCircleTypes)).Length) {
				Debug.LogWarning("StoreManager::DisplayBackgroundCircleButtons: Not enough buttons to display all background circles");
				break;
			}
			displayedBackgrounds.Add(new Background(currentBackgroundImage.backgroundImageType, bct));

			Button b = backgroundCircleButtons[buttonIndex++];
			b.gameObject.SetActive(true);

			BackgroundCircle bc = Store.backgroundCircles[bct];

			Image backgroundImage = b.transform.Find("Background Image").gameObject.GetComponent<Image>();
			Image circleImage = b.transform.Find("Circle Image").gameObject.GetComponent<Image>();
			Text message = b.transform.Find("Buy Button").GetChild(0).GetComponent<Text>();
			Text price = b.transform.Find("Buy Button").GetChild(1).GetComponent<Text>();
			Image moneyIcon = b.transform.Find("Buy Button").GetChild(2).GetComponent<Image>();

			backgroundImage.sprite = currentBackgroundImage.sprite;
			backgroundImage.color = currentBackgroundImage.color;
			if (bc.isEmpty)
				circleImage.sprite = emptyCircle;
			else
				circleImage.sprite = filledCircle;
			circleImage.color = bc.color;

			Background background = new Background(currentBackgroundImage.backgroundImageType, bct);

			if (store.IsActive(background)) {
				message.text = "Selected";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else if (store.IsUnlocked(background)) {
				message.text = "Select";
				price.text = "";
				moneyIcon.enabled = false;
			}
			else {
				message.text = "";
				price.text = bc.price.ToString();
				moneyIcon.enabled = true;
			}
		}
		for (int i=buttonIndex; i < backgroundCircleButtons.Count; i++)
			backgroundCircleButtons[i].gameObject.SetActive(false);
	}




	public void ExpandButton(int index) {

		Button b = buttons[currentlyViewing][index];
		if (!isExpanding && !isShrinking) {
			if (!expandedButtons.Contains(b)) {  //expand button
				isExpanding = true;
                buttonToChangeHeight = b;
				expandTime = 0;
				expandedButtons.Add(b);
                b.gameObject.transform.Find("Drop Down Icon").gameObject.SetActive(false);
            }
			else {  //shrink button
				isShrinking = true;
                buttonToChangeHeight = b;
				shrinkTime = 0;
				expandedButtons.Remove(b);
				b.transform.Find("Description").gameObject.SetActive(false);
                buttonToChangeHeight.gameObject.transform.Find("Drop Down Icon").gameObject.SetActive(false);
            }
		}
	}

	public void BackgroundImageButtonClicked(int index) {

		currentBackgroundImage = Store.backgroundImages[(BackgroundImageTypes)index];
		DisplayBackgroundCircleButtons();
	}

	public void Buy(int i) {

		Store store = setting as Store;
		if (currentlyViewing == 0)
		{
			AbilityTypes at = Store.bricks[(BrickTypes)i].brickAbility;
			if (store.CanUpgrade(at)) {
				store.UpgradeAbility(at);
				Audio.Instance.PlayBought();
			}
			else
				Audio.Instance.PlayError();
		}
		if (currentlyViewing == 1) {
			if (store.IsUnlocked((PaddleTypes)i) && !store.IsActive((PaddleTypes)i)) {
				store.SetActive((PaddleTypes)i);
			}
			else if (store.CanBuy((PaddleTypes)i)) {
				store.Buy((PaddleTypes)i);
				Audio.Instance.PlayBought();
			}
			else
				Audio.Instance.PlayError();
		}
		if (currentlyViewing == 2) {
			if (store.IsUnlocked((BallTypes)i) && !store.IsActive((BallTypes)i)) {
				store.SetActive((BallTypes)i);
			}
			else if (store.CanBuy((BallTypes)i)) {
				store.Buy((BallTypes)i);
				Audio.Instance.PlayBought();
			}
			else
				Audio.Instance.PlayError();
		}
		if (currentlyViewing == 3) {
			if (store.CanBuy((BackgroundImageTypes)i)) {
				currentBackgroundImage = Store.backgroundImages[(BackgroundImageTypes)i];
				store.Buy((BackgroundImageTypes)i);
				Audio.Instance.PlayBought();
			}
			else
				Audio.Instance.PlayError();
		}
	}

	public void BuyBackground(int i) {

		Store store = setting as Store;

		Background b = displayedBackgrounds[i];
		if (store.IsUnlocked(b) && !store.IsActive(b)) {
			store.SetActive(b);
		}
		else if (store.CanBuy(b)) {
			store.Buy(b);
			Audio.Instance.PlayBought();
		}
		else
			Audio.Instance.PlayError();
	}

	IEnumerator GoBack() {
		animator.SetTrigger ("exit");
		yield return new WaitForSeconds (0.2f);
		string previous = sceneLoader.getLastClosedScene();
		sceneLoader.LoadScene (previous);
		sceneLoader.UnloadScene("Store");
	}

	public void BackButtonClicked() {StartCoroutine (GoBack ());}

	public void GoToScene(string sceneName) {
		sceneLoader.LoadScene(sceneName);
		sceneLoader.UnloadScene("Store");
	}
}

/*
 * 
 * Flowchart of which functions are called (this might not be up to date)
 * 
 * Store opened -> Awake
 *          and -> OnEnable -> DisplayMenu
 *              
 * Button in menu clicked -> DisplayStore -> DisplayButtons -> DisplayBrickButtons
 *                                                       or -> DisplayPaddleButtons
 *                                                       or -> DisplayBallButtons
 *                                                       or -> DisplayBackgroundButtons
 * 
 * Click button in store -> StoreButtonClicked -> Update (expand or shrink)
 * 
 * Buy button clicked -> Buy -> store.Buy -> ConfigureSettings -> DisplayButtons -> DisplayBrickButtons
 *                                                                          or -> DisplayPaddleButtons
 *                                                                          or -> DisplayBallButtons
 *                                                                          or -> DisplayBackgroundButtons
 *                        or -> store.SetActivePaddle -> ConfigureSettings -> DisplayButtons -> DisplayBrickButtons
 *                                                                                      or -> DisplayPaddleButtons
 *                                                                                      or -> DisplayBallButtons
 *                                                                                      or -> DisplayBackgroundButtons
 *                                                  
 * Back button -> DisplayMenu
 * 
 * Main menu button -> GoToScene(MainMenu)
 * 
 */