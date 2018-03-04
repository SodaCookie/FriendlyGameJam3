using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState {
	public Command command;
	public int[] collisions;
	public List<GameObject>[] colliders;
	public int aerial;
}

public enum Command {
	None,
	Jump,
	Left,
	Right,
	LeftPunch,
	RightPunch,
	UpPunch,
	LeftBend,
	RightBend,
	UpBend
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour {
	
	public float upPunchStrength = 10;
	public float moveSpeed = 1;
	public int maxAerials = 2;
	public Vector2 punchVelocity = new Vector2(10, 0);
	public float punchDuration = 0.5f;
	[HideInInspector]
	public InputState input;

	private StateNode previous;
	private StateNode current;
	private StateMachine state;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		// Initialize a state machine
		// Build the different states
		StateNode grounded = new StateNode ("grounded");
		StateNode move = new StateNode ("move");
		StateNode air = new StateNode ("air");
		StateNode jump = new StateNode ("jump");
		StateNode leftpunch = new StateNode ("leftpunch"); 
		StateNode rightpunch = new StateNode ("rightpunch"); 
		StateNode uppunch = new StateNode ("uppunch"); 
		StateNode leftbend = new StateNode ("leftbend"); 
		StateNode rightbend = new StateNode ("rightbend"); 
		StateNode upbend = new StateNode ("upbend"); 

		// Add transitions
		jump.transition = JumpTransition;
		air.transition = AirTransition;
		grounded.transition = GroundedTransition;
		move.transition = MoveTransition;
		leftpunch.transition = LeftPunchTransition;
		rightpunch.transition = RightPunchTransition;
		uppunch.transition = UpPunchTransition;
		leftbend.transition = LeftBendTransition;
		rightbend.transition = RightBendTransition;
		upbend.transition = UpBendTransition;

		// Add to StateMachine
		state = new StateMachine(air);
		state.AddNode (grounded);
		state.AddNode (move);
		state.AddNode (air);
		state.AddNode (jump);
		state.AddNode (leftpunch);
		state.AddNode (rightpunch);
		state.AddNode (uppunch);
		state.AddNode (leftbend);
		state.AddNode (rightbend);
		state.AddNode (upbend);

		// Get useful components
		rb = GetComponent<Rigidbody2D>();
		input = new InputState ();
		input.command = Command.None;
		input.collisions = new int[4];
		input.colliders = new List<GameObject>[4];
		for (int i = 0; i < 4; i++) {
			input.collisions [i] = 0;
			input.colliders [i] = new List<GameObject>();
		}
		input.aerial = maxAerials;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Process the inputs and set a new input frame 
		previous = state.CurrentState();
		state.Process (input);
		current = state.CurrentState();
		if (current.name == "jump") {
			rb.velocity = new Vector2 (rb.velocity.x, upPunchStrength);
		} else if (current.name == "move") {
			if (input.command == Command.Left) {
				rb.velocity = new Vector2 (-moveSpeed, rb.velocity.y);
			} else {
				rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
			}
		} else if (current.name == "leftpunch") {
			StartCoroutine(Dash(new Vector2(-punchVelocity.x, punchVelocity.y)));
		} else if (current.name == "rightpunch") {
			StartCoroutine(Dash(new Vector2(punchVelocity.x, punchVelocity.y)));
		} else if (current.name == "uppunch") {
			rb.velocity = new Vector2 (0, upPunchStrength);
		} else if (current.name == "leftbend") {
			input.colliders [3][0].GetComponent<Interactable> ().Bend (gameObject, Direction.Left);
		} else if (current.name == "rightbend") {
			input.colliders [2][0].GetComponent<Interactable> ().Bend (gameObject, Direction.Right);
		} else if (current.name == "upbend") {
			input.colliders [1][0].GetComponent<Interactable> ().Bend (gameObject, Direction.Up);
		} 

		// Transitions
		if (previous.name == "move" && (current.name == "air" || current.name == "grounded")) {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		} else if (previous.name == "air" && current.name == "grounded") {
			rb.velocity = new Vector2 (0, rb.velocity.y);
			input.aerial = maxAerials;  // Reset aerials
		} else if (previous.name == "air" && (current.name == "leftpunch" || current.name == "rightpunch" || current.name == "uppunch")) {
			input.aerial--;
		}

		input.command = Command.None;
	}

	IEnumerator Dash(Vector2 velocity) {
		float startTime = Time.time;
		while (Time.time - startTime < punchDuration) {
			rb.velocity = velocity;
			yield return null;
		}
		rb.velocity = new Vector2 ();
		yield return null;
	}

	IEnumerator CreateHurtBox(GameObject colliderPrefab, float duration) {
		float startTime = Time.time;
		GameObject go = Instantiate (colliderPrefab, transform);
		yield return new WaitForSeconds (duration);
		Destroy (go);
		yield return null;
	}

	string JumpTransition(InputState input) {
		return "air";
	}

	string LeftPunchTransition(InputState input) {
		if (input.collisions [1] > 0) {
			return "grounded";
		}
		return "air";
	}

	string RightPunchTransition(InputState input) {
		if (input.collisions [1] > 0) {
			return "grounded";
		}
		return "air";
	}

	string UpPunchTransition(InputState input) {
		return "air";
	}

	string LeftBendTransition(InputState input) {
		if (input.collisions [1] > 0) {
			return "grounded";
		}
		return "air";
	}

	string RightBendTransition(InputState input) {
		if (input.collisions [1] > 0) {
			return "grounded";
		}
		return "air";
	}

	string UpBendTransition(InputState input) {
		return "air";
	}

	string AirTransition(InputState input) {
		if (input.collisions [1] > 0) {
			return "grounded";
		} else if (input.command == Command.LeftPunch && input.aerial > 0) {
			return "leftpunch";
		} else if (input.command == Command.RightPunch && input.aerial > 0) {
			return "rightpunch";
		} else if (input.command == Command.UpPunch && input.aerial > 0) {
			return "uppunch";
		}
		return null;
	}

	string MoveTransition(InputState input) {
		if (input.command == Command.Jump && input.collisions[1] > 0) {
			return "jump";
		} else if (input.command == Command.LeftPunch && input.collisions [1] > 0) {
			return "leftpunch";
		} else if (input.command == Command.RightPunch && input.collisions [1] > 0) {
			return "rightpunch";
		} else if (input.command == Command.UpPunch && input.collisions [1] > 0) {
			return "uppunch";
		} else if (input.command == Command.LeftBend && input.colliders[3].Count > 0 && input.collisions [1] > 0) {
			return "leftbend";
		} else if (input.command == Command.RightBend && input.colliders[2].Count > 0 && input.collisions [1] > 0) {
			return "rightbend";
		} else if (input.command == Command.UpBend && input.colliders[1].Count > 0 && input.collisions [1] > 0) {
			return "upbend";
		} else if (input.command != Command.Left && input.command != Command.Right) {
			if (input.collisions [1] > 0) {
				return "grounded";
			}
			return "grounded";
		}
		return null;
	}

	string GroundedTransition(InputState input) {
		if (input.command == Command.Jump) {
			return "jump";
		} else if (!(input.collisions [1] > 0)) {
			return "air";
		} else if (input.command == Command.Left || input.command == Command.Right) {
			return "move";
		} else if (input.command == Command.LeftPunch) {
			return "leftpunch";
		} else if (input.command == Command.RightPunch) {
			return "rightpunch";
		} else if (input.command == Command.UpPunch) {
			return "uppunch";
		} else if (input.command == Command.LeftBend && input.colliders[3].Count > 0) {
			return "leftbend";
		} else if (input.command == Command.RightBend && input.colliders[2].Count > 0) {
			return "rightbend";
		} else if (input.command == Command.UpBend && input.colliders[1].Count > 0) {
			foreach (var o in input.colliders[1]) {
				print (o);
			}
			return "upbend";
		}
		return null;
	}
}
