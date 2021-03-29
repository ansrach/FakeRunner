using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
	//SyncHP
	public Slider healthPrefab;
	public Transform healthPos;
	Slider health;
	//[SyncVar(hook = "OnChangeHealth")]
	public int healthValue = 100;
	NetworkStartPosition[] spawnPlayerPos;
	public GameObject explosionPrefab;

	//playerName
	public Text namePrefab;
	public Transform namePos;
	private Text nameLable;
	[SyncVar(hook = "OnChangeName")] public string pName = "player";

	//ChatUI
	Text chatTxt;
	InputField inputText;

	//SyncAnimation
	Animator animator;
	[SyncVar(hook = "OnChangeAnimation")]
	public string animState = "idle";	

	void OnChangeName(string n)
	{
		pName = n;
		nameLable.text = pName;
	}
	[Command]
	public void CmdChangeName(string newName)
	{
		pName = newName;
		nameLable.text = pName;
	}
	
	[ClientRpc]
	public void RpcChangeHelth(int n)
	{
		if (isServer) return;
		healthValue = n;
		health.value = healthValue;
	}
	[Command]
	public void CmdChangeHealth(int hitValue)
	{
		healthValue = healthValue + hitValue;
		health.value = healthValue;
		RpcChangeHelth(healthValue);
		if (healthValue <= 0)
		{							
			healthValue = 100;
			health.value = healthValue;			
        }
	}	
    private void OnCollisionEnter(Collision collision)
	{
		if (isLocalPlayer && collision.gameObject.tag == "Bullet")
		{
			CmdChangeHealth(-20);
		}
	}
    //private void OnTriggerEnter(Collision collision)
    //{
    //    if (isLocalPlayer && collision.gameObject.tag == "goal")
    //    {
    //        print(pName + " win");
    //    }
    //}
	

    //SyncAnimation
    void OnChangeAnimation(string aS)
	{
		if (isLocalPlayer) return;
		UpdateAnimationState(aS);
	}

	[Command]
	public void CmdChangeAnimation(string aS)
	{
		UpdateAnimationState(aS);
	}

	void UpdateAnimationState(string aS)
	{
		if (animState == aS) return;
		animState = aS;
		if (animState == "idle")
			animator.SetBool("Idling", true);
		else if (animState == "run")
			animator.SetBool("Idling", false);
	}

	private void Awake()
	{
		GameObject canvas = GameObject.FindWithTag("MainCanvas");
		health = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity) as Slider;
		health.transform.SetParent(canvas.transform);

		nameLable = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as Text;
		nameLable.transform.SetParent(canvas.transform);

		//spawnPlayerPos = FindObjectsOfType<NetworkStartPosition>();
	}
	void Start()
	{
		if (isLocalPlayer)
		{
			GetComponent<MyPlayerController>().enabled = true;
			CameraFollow360.player = this.gameObject.transform;
		}
		else
		{
			GetComponent<MyPlayerController>().enabled = false;
		}

		chatTxt = GameObject.Find("chatTxt").GetComponent<Text>();
		inputText = GameObject.Find("inputText").GetComponent<InputField>();

		animator = GetComponentInChildren<Animator>();
		animator.SetBool("Idling", true);
	}

	private void Update()
	{
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
			screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		if (onScreen)
		{
			Vector3 healthLablePos = Camera.main.WorldToScreenPoint(healthPos.position);
			health.transform.position = healthLablePos;

			Vector3 nameLablePos = Camera.main.WorldToScreenPoint(namePos.position);
			nameLable.transform.position = nameLablePos;
		}
		else
		{
			health.transform.position = new Vector3(-1000, -1000, 0);
			nameLable.transform.position = new Vector3(-1000, -1000, 0);
		}

		//Chat UI
		if (!isLocalPlayer)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			string Massage = inputText.text;
			inputText.text = "";

			CmdSend(Massage);
		}
	}
	[Command]
	void CmdSend(string massagee)
	{
		RpcRecieve(massagee);
	}
	[ClientRpc]
	public void RpcRecieve(string massage)
	{
		chatTxt.text += ">>" + massage + "\n";
	}
	public void OnDestroy()
	{
		if (health != null)
		{

		}
	}
}
